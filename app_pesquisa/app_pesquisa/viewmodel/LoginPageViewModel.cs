using app_pesquisa.dao;
using app_pesquisa.interfaces;
using app_pesquisa.model;
using app_pesquisa.util;
using app_pesquisa.view;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_pesquisa.viewmodel
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private ContentPage page;
        private String txtId;
        private String txtSenha;
        private bool isRunning = false;
        public ICommand CmdEntrar { get; protected set; }
        
        private CE_Pesquisa08 pesquisador;

        private WSUtil ws;
        private DAO_Pesquisa08 dao08;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginPageViewModel(ContentPage page)
        {
            this.page = page;

            ws = WSUtil.Instance;

            IsRunning = false;

            dao08 = DAO_Pesquisa08.Instance;

            CmdEntrar = new Command(() => {
                Entrar();
            });
        }

        private async void Entrar()
        {
            try
            {
                bool isOnline = Utils.IsOnline();

                if (!isOnline)
                    throw new Exception("Não há conexão disponível.");
                if(string.IsNullOrEmpty(TxtId))
                    throw new Exception("O campo 'Id' não pode ficar vazio.");
                if (string.IsNullOrEmpty(TxtSenha))
                    throw new Exception("O campo 'Senha' não pode ficar vazio.");

                IsRunning = true;

                ws = WSUtil.Instance;

                JObject obj = new JObject();
                obj["idpesquisador"] = TxtId;
                obj["senha"] = TxtSenha;
                obj["imei"] = Utils.ObterImei();

                HttpResponseMessage resposta = await ws.Post("login", obj);

                String message = await resposta.Content.ReadAsStringAsync();

                if (resposta.IsSuccessStatusCode)
                {
                    Pesquisador pesquisadorWeb = JsonConvert.DeserializeObject<Pesquisador>(message);

                    pesquisador = dao08.ObterPesquisador(Int32.Parse(TxtId));

                    if (pesquisador == null)
                    {
                        pesquisadorWeb.pesquisador.idpesquisador = Int32.Parse(TxtId);
                        pesquisadorWeb.pesquisador.senha = TxtSenha;
                        pesquisadorWeb.pesquisador.logado = 1;
                        dao08.InserirPesquisador(pesquisadorWeb.pesquisador);
                    }
                    else
                    {
                        pesquisador.logado = 1;
                        pesquisador.nome = pesquisadorWeb.pesquisador.nome;
                        pesquisador.idcliente = pesquisadorWeb.pesquisador.idcliente;
                        pesquisador.razaosocial = pesquisadorWeb.pesquisador.razaosocial;
                        dao08.AtualizarPesquisador(pesquisador);
                    }

                    await this.page.Navigation.PushAsync(new PesquisaPage());
					//await this.page.Navigation.PushAsync(new EventoPage());
                }
                else
                {
                    throw new Exception(message);
                }
                
            }
            catch (Exception ex)
            {
                await this.page.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsRunning = false;
            }
            
        }
        
        public string TxtId
        {
            get
            {
                return txtId;
            }

            set
            {
                if (value != txtId)
                {
                    txtId = value;
                    OnPropertyChanged("TxtId");
                }
            }
        }

        public string TxtSenha
        {
            get
            {
                return txtSenha;
            }

            set
            {
                if (value != txtSenha)
                {
                    txtSenha = value;
                    OnPropertyChanged("TxtSenha");
                }

            }
        }

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

        private void OnPropertyChanged(String nome)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nome));
        }
    }
}
