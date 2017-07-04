using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Behaviors;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class TextChangedBehavior : Xamarin.Behaviors.Behavior<Entry>
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create<TextChangedBehavior, string>(p => p.Text, null, propertyChanged: OnTextChanged);

        private static void OnTextChanged(BindableObject bindable, string oldvalue, string newvalue)
        {
            (bindable as TextChangedBehavior).AssociatedObject.Text = newvalue;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        protected override void OnAttach()
        {
            this.AssociatedObject.TextChanged += this.OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Text = e.NewTextValue;

            if (e.NewTextValue != null && (e.NewTextValue.Contains(",") || e.NewTextValue.Contains(".")))
                this.Text = e.OldTextValue;
        }

        protected override void OnDetach()
        {
            this.AssociatedObject.TextChanged -= this.OnTextChanged;
        }
    }
}
