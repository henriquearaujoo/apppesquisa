using app_pesquisa.componentes;
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
    public class ConfiguracoesPageViewModel : INotifyPropertyChanged
    {
        private ContentPage page;
        private List<Configuracao> itensConfiguracao;
        public ICommand CmdRestaurarConf { get; protected set; }

        public List<Configuracao> ItensConfiguracao
        {
            get
            {
                return itensConfiguracao;
            }

            set
            {
                if (value != itensConfiguracao)
                {
                    itensConfiguracao = value;
                    OnPropertyChanged("ItensConfiguracao");
                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CarregarConfiguracoes()
        {
            Configuracao conf = DependencyService.Get<IUtils>().ObterConfiguracao();

            ItensConfiguracao = new List<Configuracao>();

            ItensConfiguracao.Add(new Configuracao("Endereço do servidor", "endereco_servidor", "Str", conf.EnderecoServidor, "ic_weather_cloudy_grey600_36dp"));
        }

        public ConfiguracoesPageViewModel(ContentPage page)
        {
            this.page = page;

            //CmdRestaurarConf = new Command(() =>
            //{
            //    RestaurarConfiguracoesIniciais();
            //});

            CarregarConfiguracoes();
        }

        private async void RestaurarConfiguracoesIniciais()
        {
            bool confirmacao = await this.page.DisplayAlert("Confirmação", "Deseja realmente restaurar as configurações iniciais?", "Sim", "Não");

            if (confirmacao)
            {
                DependencyService.Get<IUtils>().InserirConfiguracaoInicial(false);
                CarregarConfiguracoes();
            }
        }

        public async void ShowModalConfiguracao(object sender)
        {
            ModalConfiguracao modal = new ModalConfiguracao(((Configuracao)((ListView)sender).SelectedItem));
            ModalConfiguracaoViewModel model = new ModalConfiguracaoViewModel(this.page, ((Configuracao)((ListView)sender).SelectedItem), this);
            modal.BindingContext = model;
            await this.page.Navigation.PushModalAsync(modal);
            model.SetarValor();
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ShowModalConfiguracao(sender);
        }

        private void OnPropertyChanged(String nome)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nome));
        }
    }
}
