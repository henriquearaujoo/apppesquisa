using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class ToobarBotoesPesquisa : StackLayout
    {
        public ToobarBotoesPesquisa()
        {
            Initialize();
        }

        public void Initialize()
        {
            Spacing = 0;
            BackgroundColor = Color.FromHex("#3F51B5");
            VerticalOptions = LayoutOptions.EndAndExpand;

            StackLayout layoutBotoes = new StackLayout();

            layoutBotoes.Orientation = StackOrientation.Horizontal;
            layoutBotoes.BackgroundColor = Color.FromHex("#3F51B5");
            layoutBotoes.HorizontalOptions = LayoutOptions.FillAndExpand;
            layoutBotoes.VerticalOptions = LayoutOptions.EndAndExpand;
            layoutBotoes.Spacing = 0;

            Button btnBaixar = new Button()
            {
                Image = "download.png",
                Text = "Download",
                TextColor = Color.FromHex("#FFFFFF"),
                BackgroundColor = Color.FromHex("#3F51B5"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            btnBaixar.SetBinding(Button.CommandProperty, new Binding("CmdAtualizar", BindingMode.OneWay));
            layoutBotoes.Children.Add(btnBaixar);

            Button btnSair = new Button()
            {
                Image = "sair.png",
                Text = "Sair",
                TextColor = Color.FromHex("#FFFFFF"),
                BackgroundColor = Color.FromHex("#3F51B5"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            btnSair.SetBinding(Button.CommandProperty, new Binding("CmdSair", BindingMode.OneWay));
            layoutBotoes.Children.Add(btnSair);

            StackLayout layoutLabel = new StackLayout();

            layoutLabel.Orientation = StackOrientation.Horizontal;
            layoutLabel.BackgroundColor = Color.FromHex("#3F51B5");
            layoutLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
            layoutLabel.VerticalOptions = LayoutOptions.EndAndExpand;
            layoutLabel.Spacing = 0;

            Label lblIcon = new Label()
            {
                Text = "EZQUEST 1.7 - Powered by ICON",
                TextColor = Color.Yellow,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold
            };

            layoutLabel.Children.Add(lblIcon);

            Children.Add(layoutBotoes);
            Children.Add(layoutLabel);
        }
    }
}