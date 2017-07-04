using app_pesquisa.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace app_pesquisa.view
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            LoginPageViewModel model = new LoginPageViewModel(this);
            BindingContext = model;

            NavigationPage.SetHasNavigationBar(this, false);
            
            InitializeComponent();
        }
        
    }
}
