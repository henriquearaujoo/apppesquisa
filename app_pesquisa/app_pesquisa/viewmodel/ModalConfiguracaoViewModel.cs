using app_pesquisa.interfaces;
using app_pesquisa.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_pesquisa.viewmodel
{
    public class ModalConfiguracaoViewModel : INotifyPropertyChanged
    {
        private ContentPage page;
        private String txtResposta;
        private Configuracao configuracao;
        private ConfiguracoesPageViewModel configuracoesViewModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CmdCancelar { get; protected set; }
        public ICommand CmdConfirmar { get; protected set; }

        public string TxtResposta
        {
            get
            {
                return txtResposta;
            }

            set
            {
                if (value != txtResposta)
                {
                    txtResposta = value;
                    OnPropertyChanged("TxtResposta");
                }
            }
        }

        public void SetarValor()
        {
            switch (configuracao.Tipo)
            {
                case "Lng":
                case "Int":
                case "Flt":
                case "Str":
                    TxtResposta = configuracao.Valor.ToString();
                    break;
                case "Bool":

                    break;
                default:
                    break;
            }
        }

        public ModalConfiguracaoViewModel(ContentPage page, Configuracao configuracao, ConfiguracoesPageViewModel configuracoesViewModel)
        {
            this.page = page;
            this.configuracao = configuracao;
            this.configuracoesViewModel = configuracoesViewModel;

            CmdCancelar = new Command(() =>
            {
                this.page.Navigation.PopModalAsync();
            });

            CmdConfirmar = new Command(() =>
            {
                DefinirConfiguracao();
            });
        }

        public async Task DefinirConfiguracao()
        {
            try
            {
                if (String.IsNullOrEmpty(TxtResposta))
                {
                    await this.page.DisplayAlert("Aviso", "Defina um valor antes de confirmar.", "Ok");
                    return;
                }

                switch (configuracao.Tipo)
                {
                    case "Str":
                        configuracao.Valor = TxtResposta;
                        break;
                    case "Flt":
                        configuracao.Valor = float.Parse(TxtResposta);
                        break;
                    case "Bool":

                        break;
                    case "Int":
                        configuracao.Valor = Int32.Parse(TxtResposta);
                        break;
                    case "Lng":
                        configuracao.Valor = long.Parse(TxtResposta);
                        break;
                    default:
                        break;
                }

                DependencyService.Get<IUtils>().SalvarConfiguracao(configuracao);
                configuracoesViewModel.CarregarConfiguracoes();
                this.page.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await this.page.DisplayAlert("Aviso", ex.Message, "Ok");
            }
        }

        private void OnPropertyChanged(String nome)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nome));
        }
    }
}
