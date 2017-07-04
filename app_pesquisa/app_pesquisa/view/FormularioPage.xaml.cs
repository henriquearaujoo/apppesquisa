using app_pesquisa.model;
using app_pesquisa.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace app_pesquisa.view
{
    public partial class FormularioPage : ContentPage
    {
        public FormularioPage(CE_Pesquisa06 pesquisa06)
        {
            FormularioPageViewModel viewModel = new FormularioPageViewModel(this, pesquisa06);
            BindingContext = viewModel;

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
        
    }
}
