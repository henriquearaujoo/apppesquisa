using app_pesquisa.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class ListItemArvore : StackLayout
    {
        private void Initialize(int nivel)
        {
            StackLayout layoutLabel = new StackLayout();
            layoutLabel.Orientation = StackOrientation.Vertical;
            layoutLabel.VerticalOptions = LayoutOptions.Center;
            Label label = new Label()
            {
                FontSize = 20,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#212121")
            };

            label.SetBinding(Label.TextProperty, new Binding("descricao", BindingMode.OneWay));

            layoutLabel.Children.Add(label);

            StackLayout layouImage = new StackLayout();
            layouImage.Orientation = StackOrientation.Vertical;
            layouImage.VerticalOptions = LayoutOptions.Center;
            Image imgRespondido = new Image();
            imgRespondido.Source = "check.png";
            imgRespondido.SetBinding(Image.IsVisibleProperty, new Binding("IsRespondido", BindingMode.OneWay));
            
            layouImage.Children.Add(imgRespondido);

            Orientation = StackOrientation.Horizontal;
            Padding = new Thickness(15 + (10 * nivel), 0, 0, 5);

            Children.Add(layoutLabel);
            Children.Add(imgRespondido);
        }

        public ListItemArvore(int nivel)
        {
            Initialize(nivel);
        }
    }
}
