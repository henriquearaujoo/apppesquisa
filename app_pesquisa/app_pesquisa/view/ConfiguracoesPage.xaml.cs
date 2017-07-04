using app_pesquisa.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace app_pesquisa.view
{
    public partial class ConfiguracoesPage : ContentPage
    {
        private ConfiguracoesPageViewModel model;

        public ConfiguracoesPage()
        {
            model = new ConfiguracoesPageViewModel(this);
            BindingContext = model;

            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            model.OnItemTapped(sender, e);

            ((ListView)sender).SelectedItem = null;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }
    }
}
