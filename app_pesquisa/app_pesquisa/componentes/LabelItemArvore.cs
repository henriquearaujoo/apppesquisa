using app_pesquisa.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class LabelItemArvore : StackLayout
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create<LabelItemArvore, ICommand>(p => p.Command, null);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create<LabelItemArvore, object>(p => p.CommandParameter, null);
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        private ICommand TransitionCommand
        {
            get
            {
                return new Command(()=>
                {
                    //this.AnchorX = 0.70;
                    //this.AnchorY = 0.70;
                    //await this.ScaleTo(0.9, 50, Easing.Linear);
                    //await Task.Delay(50);
                    //await this.ScaleTo(1, 50, Easing.Linear);
                    if (Command != null)
                    {
                        Command.Execute(CommandParameter);
                    }
                });
            }
        }

        public void Initialize(CE_Pesquisa04 pesquisa04, bool temFilhos)
        {
            Label label = new Label()
            {
                FontSize = 20,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#212121")
            };

            if (pesquisa04.obrigatoria == 1)
            {
                label.Text = "* " + pesquisa04.descricao;
            }
            else
            {
                label.Text = pesquisa04.descricao;
            }

            if (temFilhos)
                label.FontAttributes = FontAttributes.Bold;

            Padding = new Thickness(0, 5, 0, 0);
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.CenterAndExpand;

            Children.Add(label);

            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = TransitionCommand
            });
        }

        public LabelItemArvore(CE_Pesquisa04 pesquisa04, bool temFilhos)
        {
            Initialize(pesquisa04, temFilhos);
        }
    }
}
