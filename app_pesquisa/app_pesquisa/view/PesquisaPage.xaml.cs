using app_pesquisa.componentes;
using app_pesquisa.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace app_pesquisa.view
{
    public partial class PesquisaPage : ContentPage
    {
        private PesquisaPageViewModel viewModel;

        public PesquisaPage()
        {
            StackLayout layout = new StackLayout();
            layout.Spacing = 0;

            layout.Children.Add(new ToobarPesquisa());
            layout.Children.Add(new ListViewPesquisas(this));
            layout.Children.Add(new ActivityIndicatorRunning());
            layout.Children.Add(new ToobarBotoesPesquisa());
            //layout.Children.Add(new LabelIcon());

            Content = layout;

            viewModel = new PesquisaPageViewModel(this);
            BindingContext = viewModel;

            NavigationPage.SetHasNavigationBar(this, false);

            //InitializeComponent();
        }

        public async void PesquisaForaDoPrazo()
        {
            await DisplayAlert("Aviso", "Pesquisa fora do prazo, baixe novas pesquisas.", "Ok");

            viewModel.ObterPesquisas();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
