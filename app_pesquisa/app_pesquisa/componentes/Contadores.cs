using app_pesquisa.dao;
using app_pesquisa.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class Contadores : StackLayout
    {
        private Label lblQtdTotal;
        private void Initialize(CE_Formulario formulario)
        {
            lblQtdTotal = new Label();
            lblQtdTotal.TextColor = Color.White;
            lblQtdTotal.Text = "0/0 (0%)";
            lblQtdTotal.FontSize = 16;
            lblQtdTotal.HorizontalOptions = LayoutOptions.CenterAndExpand;
            
            Orientation = StackOrientation.Horizontal;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Gray;

            Children.Add(lblQtdTotal);

            MessagingCenter.Subscribe<String, CE_Formulario>(this, "AtualizarContador", (s, arg) =>
            {
                AtualizarContador(arg);
            });

            MessagingCenter.Send("", "AtualizarContador", formulario);
            
        }

        public void AtualizarContador(CE_Formulario formulario)
        {
            Device.BeginInvokeOnMainThread(() => {
                
                DAO_Pesquisa04 dao04 = DAO_Pesquisa04.Instance;
                DAO_Pesquisa07 dao07 = DAO_Pesquisa07.Instance;

                int totalPerguntas = dao04.ObterTotalPerguntas(formulario.idpesquisa01);
                int totalRespondido = dao07.ObterTotalRespondidoPorPesquisa(formulario.idpesquisa06, formulario.codigoformulario);

                double percTotal = 0;

                if (totalPerguntas > 0)
                    percTotal = ((double)totalRespondido / (double)totalPerguntas) * 100;

                lblQtdTotal.Text = totalRespondido + " de " + totalPerguntas + " (" + String.Format("{0:n1}", percTotal) + "%)";
            });
            
        }

        public Contadores(CE_Formulario formulario)
        {
            Initialize(formulario);
        }
    }
}
