using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class ToobarPesquisa : StackLayout
    {
        public void Initialize()
        {
            Orientation = StackOrientation.Horizontal;
            BackgroundColor = Color.FromHex("#3F51B5");

            StackLayout layout = new StackLayout();
            layout.Orientation = StackOrientation.Vertical;
            layout.VerticalOptions = LayoutOptions.CenterAndExpand;
            layout.Padding = new Thickness(16, 5, 0, 10);
            
            Label lblTitle = new Label()
            {
                Text = "{Binding Title}",
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#FFFFFF"),
                FontAttributes = FontAttributes.Bold
            };

            lblTitle.SetBinding(Label.TextProperty, new Binding("Title", BindingMode.OneWay));

            Label lblSubTitle = new Label()
            {
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#FFFFFF")
            };

            lblSubTitle.SetBinding(Label.TextProperty, new Binding("SubTitle", BindingMode.OneWay));
            
            layout.Children.Add(lblTitle);
            layout.Children.Add(lblSubTitle);

            StackLayout layoutBotoes = new StackLayout();
            layoutBotoes.Orientation = StackOrientation.Horizontal;
            layoutBotoes.Padding = new Thickness(0, 0, 15, 0);
            layoutBotoes.HorizontalOptions = LayoutOptions.EndAndExpand;
            layoutBotoes.VerticalOptions = LayoutOptions.CenterAndExpand;
            layoutBotoes.Spacing = 25;

            ImageButton btnConfiguracoes = new ImageButton()
            {
                Source = "ic_settings_white_36dp.png",
                WidthRequest = 26,
                HeightRequest = 26
            };

            btnConfiguracoes.SetBinding(ImageButton.CommandProperty, new Binding("CmdConfiguracoes", BindingMode.OneWay));

            layoutBotoes.Children.Add(btnConfiguracoes);

            Children.Add(layout);
            Children.Add(layoutBotoes);
        }

        public ToobarPesquisa()
        {
            Initialize();
        }
    }
}
