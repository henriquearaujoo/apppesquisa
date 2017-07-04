using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class LabelIcon : StackLayout
    {
        public LabelIcon()
        {
            Initialize();
        }

        private void Initialize()
        {
            Orientation = StackOrientation.Horizontal;
            BackgroundColor = Color.FromHex("#3F51B5");
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.End;
            Spacing = 0;

            Label lblIcon = new Label()
            {
                Text = "EZQUEST - Powered by ICON",
                TextColor = Color.Yellow,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.End,
                FontAttributes = FontAttributes.Bold,
            };

            Children.Add(lblIcon);
        }
    }
}