using app_pesquisa.model;
using app_pesquisa.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class ItemArvoreFormulario : StackLayout
    {
        private Boolean isExpanded;
        public ImageButtonItemArvore Botao { get; set; }
        public CE_Pesquisa04 Pesquisa04 { get; set; }
        public CE_Pesquisa06 Pesquisa06 { get; set; }
        public CE_Formulario Formulario { get; set; }
        public int count;
        
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }

            set
            {
                isExpanded = value;
            }
        }

        public void Initialize(int nivel, bool temFilhos) 
        {
            Spacing = 0;

            StackLayout node = new StackLayout();
            node.Spacing = 2;

            if (temFilhos)
                node.Padding = new Thickness(10 + (5 * nivel), 4, 0, 4);
            else
                node.Padding = new Thickness(30 + (10 * nivel), 4, 0, 4);
            node.Orientation = StackOrientation.Horizontal;

            if (count % 2 == 0)
                node.BackgroundColor = Color.FromHex("#DEDEDE");
            else
                node.BackgroundColor = Color.FromHex("#FFFFFF");

            StackLayout layoutImage = new StackLayout();
            Botao = new ImageButtonItemArvore()
            {
                Source = "minus.png"
            };

            Botao.SetBinding(ImageButtonItemArvore.CommandProperty, new Binding("CmdExpand", BindingMode.OneWay));
                        
            layoutImage.Children.Add(Botao);

            StackLayout layoutLabel = new StackLayout();
            layoutLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
            layoutLabel.Padding = new Thickness(5, 2, 0, 0);

            Label label = new Label()
            {
                Text = Pesquisa04.descricao,
                FontSize = 22,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#212121")
            };

            if (temFilhos)
                label.FontAttributes = FontAttributes.Bold;

            layoutLabel.Children.Add(label);

            if (temFilhos)
                node.Children.Add(layoutImage);

            if (Pesquisa04.pesquisa02 == null)
                node.Children.Add(layoutLabel);
            else
            {
                LabelItemArvore labelItem = new LabelItemArvore(Pesquisa04, temFilhos);
                labelItem.SetBinding(LabelItemArvore.CommandProperty, new Binding("CmdShowDialogResposta", BindingMode.OneWay));
                labelItem.BindingContext = this.BindingContext;
                
                node.Children.Add(labelItem);

                Image imgRespondido = new Image();
                imgRespondido.Source = "check.png";
                imgRespondido.SetBinding(Image.IsVisibleProperty, new Binding("IsRespondido", BindingMode.OneWay));

                node.Children.Add(imgRespondido);
            }

            IsExpanded = true;

            Children.Add(node);
        }

        public ItemArvoreFormulario(CE_Pesquisa06 pesquisa06, CE_Pesquisa04 pesquisa04, int nivel, bool temFilhos, ContentPage page, CE_Formulario formulario, int count)
        {
            this.Pesquisa04 = pesquisa04;
            this.Pesquisa06 = pesquisa06;
            this.Formulario = formulario;
            this.count = count;
            ItemArvoreFormularioViewModel viewModel = new ItemArvoreFormularioViewModel(this, page);
            this.BindingContext = viewModel;
            Initialize(nivel, temFilhos);
            viewModel.IsRespondido = Pesquisa04.IsRespondido(formulario.codigoformulario);
        }
    }
}
