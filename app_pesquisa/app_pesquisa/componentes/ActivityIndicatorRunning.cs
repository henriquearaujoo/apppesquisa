using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class ActivityIndicatorRunning : StackLayout
    {
        private void Initialize()
        {
            ActivityIndicator ai = new ActivityIndicator();
            ai.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsRunning", BindingMode.TwoWay));
            ai.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsRunning", BindingMode.TwoWay));
            ai.Color = Color.Gray;
            ai.VerticalOptions = LayoutOptions.CenterAndExpand;
            ai.HorizontalOptions = LayoutOptions.CenterAndExpand;

            Children.Add(ai);
        }

        public ActivityIndicatorRunning()
        {
            Initialize();
        }
    }
}
