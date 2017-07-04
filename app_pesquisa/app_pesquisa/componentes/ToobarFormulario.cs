using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{

    public class ToobarFormulario : StackLayout
    {
        public void Initialize()
        {
            Orientation = StackOrientation.Horizontal;
            BackgroundColor = Color.FromHex("#3F51B5");

            StackLayout layoutLabels = new StackLayout();
            layoutLabels.Orientation = StackOrientation.Vertical;
            layoutLabels.VerticalOptions = LayoutOptions.CenterAndExpand;
            layoutLabels.Padding = new Thickness(16, 5, 0, 10);

            StackLayout layoutImgBack = new StackLayout();
            layoutImgBack.Orientation = StackOrientation.Vertical;
            layoutImgBack.VerticalOptions = LayoutOptions.CenterAndExpand;
            layoutImgBack.Padding = new Thickness(5, 0, 0, 0);

            ImageButton btnVoltar = new ImageButton()
            {
                Source = "voltar.png",
                WidthRequest = 30,
                HeightRequest = 30
            };

            btnVoltar.SetBinding(ImageButton.CommandProperty, new Binding("CmdVoltar", BindingMode.OneWay));

            layoutImgBack.Children.Add(btnVoltar);

            Label lblTitle = new Label()
            {
                Text = "{Binding Title}",
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#FFFFFF"), 
                FontAttributes = FontAttributes.Bold
            };

            lblTitle.SetBinding(Label.TextProperty, new Binding("Title", BindingMode.OneWay));

            StackLayout layoutSubCont = new StackLayout();
            layoutSubCont.Orientation = StackOrientation.Horizontal;
            
            Label lblSubTitle = new Label()
            {
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#FFFFFF")
            };

            lblSubTitle.SetBinding(Label.TextProperty, new Binding("SubTitle", BindingMode.OneWay));

            StackLayout layoutCont = new StackLayout();
            layoutCont.VerticalOptions = LayoutOptions.EndAndExpand;
            layoutCont.HorizontalOptions = LayoutOptions.EndAndExpand;

            Label lblContador = new Label()
            {
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#FFFFFF"), 
                Text = "0/0"
            };

            lblContador.SetBinding(Label.TextProperty, new Binding("Contador", BindingMode.OneWay));

            layoutCont.Children.Add(lblContador);

            layoutSubCont.Children.Add(lblSubTitle);
            layoutSubCont.Children.Add(layoutCont);

            layoutLabels.Children.Add(lblTitle);
            layoutLabels.Children.Add(layoutSubCont);
            
            StackLayout layoutImgRefresh = new StackLayout();
            layoutImgRefresh.Orientation = StackOrientation.Vertical;
            layoutImgRefresh.Padding = new Thickness(0, 0, 10, 0);
            layoutImgRefresh.HorizontalOptions = LayoutOptions.EndAndExpand;
            layoutImgRefresh.VerticalOptions = LayoutOptions.CenterAndExpand;

            ImageButton btnConfirmar = new ImageButton()
            {
                Source = "confirmar_formulario.png",
                WidthRequest = 30,
                HeightRequest = 30
            };

            btnConfirmar.SetBinding(ImageButton.CommandProperty, new Binding("CmdFinalizarFormulario", BindingMode.OneWay));

            //layoutImgRefresh.Children.Add(btnConfirmar);

            //Children.Add(layoutImgBack);
            Children.Add(layoutLabels);
            Children.Add(layoutImgRefresh);
        }

        public ToobarFormulario()
        {
            Initialize();
        }
    }
}
