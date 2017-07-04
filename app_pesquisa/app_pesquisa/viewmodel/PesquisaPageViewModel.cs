using app_pesquisa.dao;
using app_pesquisa.interfaces;
using app_pesquisa.model;
using app_pesquisa.util;
using app_pesquisa.view;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_pesquisa.viewmodel
{
    public class PesquisaPageViewModel : INotifyPropertyChanged
    {
        private ContentPage page;
        private CE_Pesquisa08 pesquisador;
        private ObservableCollection<CE_Pesquisa06> pesquisas;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CmdAtualizar { get; protected set; }
        public ICommand CmdEnviar { get; protected set; }
        public ICommand CmdSair { get; protected set; }
        public ICommand CmdConfiguracoes { get; protected set; }

        private bool isRunning = false;
        
        private String title;

        private String subtitle;

        private DAO_Pesquisa06 dao06;
        private DAO_Pesquisa01 dao01;

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (value != isRunning)
                {
                    isRunning = value;
                    OnPropertyChanged("IsRunning");
                }
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (title == value)
                    return;

                title = value;
                OnPropertyChanged("Title");
            }
        }

        public string SubTitle
        {
            get { return subtitle; }
            set
            {
                if (subtitle == value)
                    return;

                subtitle = value;
                OnPropertyChanged("Subtitle");
            }
        }

        public PesquisaPageViewModel(ContentPage page)
        {
            IsRunning = true;

            this.page = page;

            dao01 = DAO_Pesquisa01.Instance;
            dao06 = DAO_Pesquisa06.Instance;

            pesquisador = Utils.ObterPesquisadorLogado();

            Title = pesquisador.razaosocial;
            SubTitle = pesquisador.nome;

            CmdConfiguracoes = new Command(() => {
                this.page.Navigation.PushAsync(new ConfiguracoesPage());
            });

            CmdAtualizar = new Command(() => {
                DownloadDados();
            });
            
            CmdEnviar = new Command(() => {
                EnviarDados();
            });

            CmdSair = new Command(() => {
                Sair();
            });

            ObterPesquisas();
            
            IsRunning = false;
        }

        public async void Sair()
        {
            bool confirmacao = await this.page.DisplayAlert("Confirmação", "Deseja realmente sair?", "Sim", "Não");

            if (confirmacao)
            {
                pesquisador.logado = 0;
                DAO_Pesquisa08 dao08 = DAO_Pesquisa08.Instance;
                dao08.AtualizarPesquisador(pesquisador);
                
                //this.page.Navigation.InsertPageBefore(new LoginPage(), this.page);
                await this.page.Navigation.PopAsync();
            }
            
        }

        private async void DownloadDados()
        {
            try
            {
                bool isOnline = Utils.IsOnline();

                if (!isOnline)
                    throw new Exception("Não há conexão disponível.");

                IsRunning = true;

                await new DadosPesquisaUtil().Download();

                await this.page.DisplayAlert("Sucesso", "Dados baixados com sucesso.", "Ok");
            }
            catch (Exception ex)
            {
                await this.page.DisplayAlert("Aviso", ex.Message, "Ok");
            }
            finally
            {
                ObterPesquisas();

                IsRunning = false;
            }
            
        }

        private async void EnviarDados()
        {
            try
            {
                bool isOnline = Utils.IsOnline();

                if (!isOnline)
                    throw new Exception("Não há conexão disponível.");

                IsRunning = true;

                int registros = await new DadosPesquisaUtil().Upload();

                if (registros > 0)
                    await this.page.DisplayAlert("Sucesso", registros + " registros enviados.", "Ok");
                else
                    await this.page.DisplayAlert("Aviso", "Não há registros para enviar.", "Ok");

            }
            catch (Exception ex)
            {
                await this.page.DisplayAlert("Aviso", ex.Message, "Ok");
            }
            finally
            {
                IsRunning = false;
            }
        }

        public ObservableCollection<CE_Pesquisa06> Pesquisas
        {
            get
            {
                return pesquisas;
            }

            set
            {
                if (value != pesquisas)
                {
                    pesquisas = value;
                    OnPropertyChanged("Pesquisas");
                }

            }
        }

        public void ObterPesquisas()
        {
            List<CE_Pesquisa06> listOndas = dao06.ObterOndas();

            foreach (var item in listOndas)
            {
                item.pesquisa01 = dao01.ObterPesquisa(item.idpesquisa01);
            }

            Pesquisas = new ObservableCollection<CE_Pesquisa06>();

            foreach (var item in listOndas)
            {
                Pesquisas.Add(item);
            }

            listOndas = null;
            
        }

        private void OnPropertyChanged(String nome)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nome));
        }
    }
}
