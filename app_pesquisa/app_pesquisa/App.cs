using app_pesquisa.interfaces;
using app_pesquisa.util;
using app_pesquisa.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace app_pesquisa
{
    public class App : Application
    {
        public App()
        {
            DependencyService.Get<IUtils>().InserirConfiguracaoInicial(true);

            // The root page of your application
            //MainPage = new NavigationPage(new PesquisaPage());
            var pesquisador = Utils.ObterPesquisadorLogado();
            if (pesquisador == null)
                MainPage = new NavigationPage(new LoginPage());
            else
            {
				//EventoPage page = new EventoPage();
				PesquisaPage page = new PesquisaPage();
                MainPage = new NavigationPage(page);
                page.Navigation.InsertPageBefore(new LoginPage(), page);

            }

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
