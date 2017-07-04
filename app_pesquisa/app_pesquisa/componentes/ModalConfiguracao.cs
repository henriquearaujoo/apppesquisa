using app_pesquisa.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class ModalConfiguracao : ContentPage
    {
        private StackLayout ObterLayoutPrincipal(String descricao)
        {
            StackLayout layoutPrincipal = new StackLayout();
            layoutPrincipal.HorizontalOptions = LayoutOptions.CenterAndExpand;
            layoutPrincipal.VerticalOptions = LayoutOptions.CenterAndExpand;
            //layoutPrincipal.HeightRequest = 180;
            layoutPrincipal.WidthRequest = 290;
            layoutPrincipal.BackgroundColor = Color.FromHex("#FFFFFF");

            StackLayout layoutPergunta = new StackLayout();
            layoutPergunta.Orientation = StackOrientation.Horizontal;
            layoutPergunta.HeightRequest = 50;
            layoutPergunta.BackgroundColor = Color.FromHex("#3F51B5");
            
            StackLayout layoutLabel = new StackLayout();
            layoutLabel.Padding = new Thickness(16, 0, 0, 0);
            layoutLabel.HorizontalOptions = LayoutOptions.StartAndExpand;
            layoutLabel.VerticalOptions = LayoutOptions.CenterAndExpand;

            Label label = new Label()
            {
                Text = descricao,
                FontSize = 17,
                //TextColor = Color.FromHex("#212121"),
                TextColor = Color.FromHex("#FFFFFF"),
                FontAttributes = FontAttributes.Bold

            };

            layoutLabel.Children.Add(label);

            //layoutPergunta.Children.Add(layoutIcon);
            layoutPergunta.Children.Add(layoutLabel);

            layoutPrincipal.Children.Add(layoutPergunta);

            BackgroundColor = new Color(0, 0, 0, 0.5);
            //BackgroundColor = Color.Transparent;

            return layoutPrincipal;
        }

        private StackLayout ObterLayoutBotoes()
        {
            StackLayout layoutBotoes = new StackLayout();
            layoutBotoes.Orientation = StackOrientation.Horizontal;
            layoutBotoes.Padding = new Thickness(5, 10, 5, 0);
            layoutBotoes.HorizontalOptions = LayoutOptions.End;
            layoutBotoes.Spacing = 1;

            Button btnCancelar = new Button();
            //btnCancelar.BackgroundColor = Color.FromHex("#FFFFFF");
            btnCancelar.BackgroundColor = Color.Transparent;
            btnCancelar.TextColor = Color.Green;
            btnCancelar.Text = "Cancelar";
            btnCancelar.SetBinding(Button.CommandProperty, new Binding("CmdCancelar", BindingMode.OneWay));

            Button btnConfirmar = new Button();
            //btnConfirmar.BackgroundColor = Color.FromHex("#FFFFFF");
            btnConfirmar.BackgroundColor = Color.Transparent;
            btnConfirmar.TextColor = Color.Green;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.SetBinding(Button.CommandProperty, new Binding("CmdConfirmar", BindingMode.OneWay));

            layoutBotoes.Children.Add(btnCancelar);
            layoutBotoes.Children.Add(btnConfirmar);

            return layoutBotoes;
        }

        public StackLayout ObterFormTxt(Configuracao configuracao)
        {
            StackLayout layoutPrincipal = ObterLayoutPrincipal(configuracao.Descricao);

            StackLayout layoutResposta = new StackLayout();
            layoutResposta.Orientation = StackOrientation.Horizontal;
            layoutResposta.Padding = new Thickness(5, 5, 5, 5);
            layoutResposta.HorizontalOptions = LayoutOptions.StartAndExpand;
            layoutResposta.VerticalOptions = LayoutOptions.CenterAndExpand;

            Entry txtConf = new Entry();
            txtConf.PlaceholderColor = Color.FromHex("#212121");
            txtConf.TextColor = Color.FromHex("#212121");
            txtConf.WidthRequest = 260;
            txtConf.FontSize = 17;
            if (configuracao.Tipo == "Str")
            {
                txtConf.Placeholder = "Digite um texto";
                txtConf.Keyboard = Keyboard.Text;
            }
            else
            {
                txtConf.Placeholder = "Digite um valor";
                txtConf.Keyboard = Keyboard.Numeric;

                if (configuracao.Tipo == "Int" || configuracao.Tipo == "Flt")
                    txtConf.TextChanged += TxtConf_TextChanged;
            }
            txtConf.SetBinding(Entry.TextProperty, new Binding("TxtResposta", BindingMode.TwoWay));
            layoutResposta.Children.Add(txtConf);

            layoutPrincipal.Children.Add(layoutResposta);
            layoutPrincipal.Children.Add(ObterLayoutBotoes());

            return layoutPrincipal;
        }

        private void TxtConf_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtResposta = sender as Entry;

            if (e.NewTextValue != null && (e.NewTextValue.Contains(",") || e.NewTextValue.Contains(".")))
                txtResposta.Text = e.OldTextValue;
        }

        public ModalConfiguracao(Configuracao configuracao)
        {
            switch (configuracao.Tipo)
            {
                case "Int":
                case "Flt":
                case "Str":
                    this.Content = ObterFormTxt(configuracao);
                    break;
                case "Lista":
                    
                    break;
                case "Date":
                case "MesAno":
                case "Mes":
                    
                    break;
                case "Hora":
                    
                    break;
                default:
                    break;
            }
        }
    }
}
