using app_pesquisa.dao;
using app_pesquisa.model;
using app_pesquisa.viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class ModalResposta : ContentPage
    {
        public CE_Pesquisa04 Item { get; set; }
        public CE_Pesquisa06 Pesquisa06 { get; set; }
        public ListView ListView { get; set; }
        public Entry TxtFiltro { get; set; }
        public String CodigoFormulario { get; set; }
        private CE_Pesquisa03 opcaoSelecionada;
		private String TipoDado { get; set; }
		public int NPage { get; set; }

		public ModalResposta(CE_Pesquisa04 item, CE_Pesquisa06 pesquisa06, String tipodado, int npage)
		{
			this.Item = item;
			this.Pesquisa06 = pesquisa06;
			this.TipoDado = tipodado;
			NPage = npage;

			switch (tipodado)
			{
				case "Int":
				case "Dbl":
				case "Txt":
					this.Content = ObterFormTxt();
					break;
				case "Lista":
					if (NPage == 0)
					{
						if (Item.qtrespostas == 1)
							this.Content = ObterFormLista();
						else
							this.Content = ObterFormListaMulti();
					}
					else
						this.Content = ObterFormLista();
					break;
				case "Date":
				case "MesAno":
				case "Mes":
					this.Content = ObterFormData();
					break;
				case "Hora":
					this.Content = ObterFormHora();
					break;
				default:
					break;
			}
		}

        private StackLayout ObterLayoutPrincipal()
        {
            StackLayout layoutPrincipal = new StackLayout();
            layoutPrincipal.HorizontalOptions = LayoutOptions.CenterAndExpand;
            layoutPrincipal.VerticalOptions = LayoutOptions.CenterAndExpand;
            layoutPrincipal.BackgroundColor = Color.FromHex("#FFFFFF");

            StackLayout layoutPergunta = new StackLayout();
            layoutPergunta.Orientation = StackOrientation.Horizontal;
            layoutPergunta.BackgroundColor = Color.FromHex("#3F51B5");

            StackLayout layoutLabel = new StackLayout();
            layoutLabel.Padding = new Thickness(16, 10, 0, 10);
            layoutLabel.HorizontalOptions = LayoutOptions.StartAndExpand;
            layoutLabel.VerticalOptions = LayoutOptions.CenterAndExpand;

            Label label = new Label()
            {
                Text = Item.descricao,
                FontSize = 17,
                TextColor = Color.FromHex("#FFFFFF"),
                FontAttributes = FontAttributes.Bold

            };

            layoutLabel.Children.Add(label);

            layoutPergunta.Children.Add(layoutLabel);

            layoutPrincipal.Children.Add(layoutPergunta);

            BackgroundColor = new Color(0, 0, 0, 0.5);
            

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

        public StackLayout ObterFormTxt()
        {
            StackLayout layoutPrincipal = ObterLayoutPrincipal();

            StackLayout layoutResposta = new StackLayout();
            layoutResposta.Orientation = StackOrientation.Horizontal;
            layoutResposta.Padding = new Thickness(0, 0, 0, 0);
            layoutResposta.HorizontalOptions = LayoutOptions.CenterAndExpand;

            Entry txtResposta = new Entry();
            txtResposta.PlaceholderColor = Color.FromHex("#212121");
            txtResposta.TextColor = Color.FromHex("#212121");
            txtResposta.WidthRequest = 400;
            txtResposta.FontSize = 17;
            if (TipoDado == "Txt")
            {
                txtResposta.Placeholder = "Digite um texto";
                txtResposta.Keyboard = Keyboard.Text;
				txtResposta.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 100 });
            }
            else
            {
                txtResposta.Placeholder = "Digite um valor";
                txtResposta.Keyboard = Keyboard.Numeric;
				txtResposta.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 15 });
				           
                if (TipoDado == "Int")
                    txtResposta.TextChanged += TxtResposta_TextChanged;
            }
            txtResposta.SetBinding(Entry.TextProperty, new Binding("TxtResposta", BindingMode.TwoWay));
            layoutResposta.Children.Add(txtResposta);

            layoutPrincipal.Children.Add(layoutResposta);
            layoutPrincipal.Children.Add(ObterLayoutBotoes());

            return layoutPrincipal;
        }

        private void TxtResposta_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtResposta = sender as Entry;

            if (e.NewTextValue != null && (e.NewTextValue.Contains(",") || e.NewTextValue.Contains(".")))
                txtResposta.Text = e.OldTextValue;
        }

        private StackLayout ObterFormLista()
        {
            StackLayout layoutPrincipal = ObterLayoutPrincipal();

            StackLayout layoutOpcoes = new StackLayout();
            layoutOpcoes.Padding = new Thickness(0, 0, 0, 0);
            layoutOpcoes.HorizontalOptions = LayoutOptions.CenterAndExpand;

            ListView = new ListView();
            ListView.HeightRequest = 200;
            ListView.BackgroundColor = Color.FromHex("#FFFFFF");
            ListView.SeparatorColor = Color.FromHex("#B6B6B6");
            DataTemplate cell = new DataTemplate(() =>
            {
                TextCell txtCell = new TextCell();
                txtCell.TextColor = Color.FromHex("#212121");

                return txtCell;
            });
            cell.SetBinding(TextCell.TextProperty, "descricao");
            ListView.ItemTemplate = cell;

			if (NPage == 0)
            	ListView.ItemsSource = Item.Opcoes;
			else
				ListView.ItemsSource = Item.OpcoesOutros;
			
            ListView.SetBinding(ListView.SelectedItemProperty, new Binding("OpcaoSelecionada", BindingMode.TwoWay));

            TxtFiltro = new Entry();
            TxtFiltro.PlaceholderColor = Color.FromHex("#212121");
            TxtFiltro.Placeholder = "Filtro";
            TxtFiltro.TextColor = Color.FromHex("#212121");
            TxtFiltro.WidthRequest = 260;
            TxtFiltro.FontSize = 17;
            TxtFiltro.TextChanged += TxtFiltro_TextChanged;

            layoutOpcoes.Children.Add(TxtFiltro);
            layoutOpcoes.Children.Add(ListView);

            //layoutPrincipal.HeightRequest = 300;
            layoutPrincipal.Children.Add(layoutOpcoes);
            layoutPrincipal.Children.Add(ObterLayoutBotoes());

            return layoutPrincipal;
        }


		private StackLayout ObterFormListaMulti()
		{
			StackLayout layoutPrincipal = ObterLayoutPrincipal();

			StackLayout layoutOpcoes = new StackLayout();
			layoutOpcoes.Padding = new Thickness(0, 0, 0, 0);
			layoutOpcoes.HorizontalOptions = LayoutOptions.CenterAndExpand;

			ListView = new ListView();
			ListView.HeightRequest = 200;
			ListView.BackgroundColor = Color.FromHex("#FFFFFF");
			ListView.SeparatorColor = Color.FromHex("#B6B6B6");
			/*DataTemplate cell = new DataTemplate(() =>
			{
				TextCell txtCell = new TextCell();
				txtCell.TextColor = Color.FromHex("#212121");

				return txtCell;
			});
			cell.SetBinding(TextCell.TextProperty, "descricao");
			ListView.ItemTemplate = cell;*/

			ListView.ItemTemplate = new DataTemplate(() =>
            {
                ViewCell cell = new ViewCell();
				StackLayout layoutList = new StackLayout();
				layoutList.Padding = new Thickness(16, 0, 0, 0);
				layoutList.Orientation = StackOrientation.Horizontal;

                StackLayout layoutImage = new StackLayout();
				layoutImage.Orientation = StackOrientation.Vertical;
                layoutImage.VerticalOptions = LayoutOptions.Center;

				Image img = new Image();
				img.Source = "check.png";
                img.SetBinding(Image.IsVisibleProperty, new Binding("IsSelecionado", BindingMode.TwoWay));

				layoutImage.Children.Add(img);

                StackLayout layoutLabel = new StackLayout();
				layoutLabel.Padding = new Thickness(10, 0, 0, 0);
				layoutLabel.Orientation = StackOrientation.Vertical;
                layoutLabel.VerticalOptions = LayoutOptions.Center;

                Label lbl = new Label()
				{
					FontSize = 17,
					TextColor = Color.FromHex("#212121")
				};
				lbl.SetBinding(Label.TextProperty, new Binding("descricao", BindingMode.OneWay));

                layoutLabel.Children.Add(lbl);

                layoutList.Children.Add(layoutImage);
                layoutList.Children.Add(layoutLabel);

                cell.View = layoutList;

                return cell;
            });

			ListView.ItemsSource = Item.Opcoes;

			TxtFiltro = new Entry();
			TxtFiltro.PlaceholderColor = Color.FromHex("#212121");
			TxtFiltro.Placeholder = "Filtro";
			TxtFiltro.TextColor = Color.FromHex("#212121");
			TxtFiltro.WidthRequest = 260;
			TxtFiltro.FontSize = 17;
			TxtFiltro.TextChanged += TxtFiltro_TextChanged;

			layoutOpcoes.Children.Add(TxtFiltro);
			layoutOpcoes.Children.Add(ListView);

			layoutPrincipal.Children.Add(layoutOpcoes);
			layoutPrincipal.Children.Add(ObterLayoutBotoes());

			return layoutPrincipal;
		}

        private void TxtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry txtFiltro = sender as Entry;

            if (String.IsNullOrEmpty(e.NewTextValue))
            {
                ListView.SelectedItem = OpcaoSelecionada;
                ListView.ItemsSource = Item.Opcoes;
            }
            else
            {
                List<CE_Pesquisa03> lista = Item.Opcoes.Where(o => o.descricao.ToLower().Contains(txtFiltro.Text.ToLower())).ToList();

                ListView.ItemsSource = lista;

                if (lista != null && lista.Count > 0)
                {
                    //Recurso técnico de Ivan Ortiz 31/08/2016
                    ListView.SelectedItem = lista.Last();
                    ListView.SelectedItem = lista.First();
                    //Gambiarra
                }

            }
        }

        public String ObterValorTxt()
        {
            DAO_Pesquisa07 dao = DAO_Pesquisa07.Instance;
            CE_Pesquisa07 resposta = dao.ObterRespostaPorPergunta(Item.idpesquisa04, CodigoFormulario).FirstOrDefault();

            if (resposta != null)
            {
                //String tipodado = Item.pesquisa02.tipodado;
                switch (TipoDado)
                {
                    case "Int":
                    case "Dbl":
                        return resposta.vlresposta.ToString().Replace(",", ".");
                    case "Txt":
                    case "Lista":
                    case "Date":
                    case "MesAno":
                    case "Mes":
                    case "Hora":
                        return resposta.txresposta;
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }

        }

        public CE_Pesquisa03 OpcaoSelecionada
        {
            get
            {
                String valor = ObterValorTxt();
                if (!String.IsNullOrEmpty(valor))
                    return Item.Opcoes.FirstOrDefault(o => o.descricao == valor);
                else
                    return null;
            }

            set
            {
                opcaoSelecionada = value;
            }
        }

        private StackLayout ObterFormData()
        {
            StackLayout layoutPrincipal = ObterLayoutPrincipal();

            StackLayout layoutData = new StackLayout();
            layoutData.Padding = new Thickness(0, 0, 0, 0);
            layoutData.HorizontalOptions = LayoutOptions.CenterAndExpand;

            DatePicker dtp = new DatePicker();
            dtp.Date = DateTime.Now;
            switch (TipoDado)
            {
                case "Date":
                    dtp.Format = "dd/MM/yyyy";
                    break;
                case "MesAno":
                    dtp.Format = "MM/yyyy";
                    break;
                case "Mes":
                    dtp.Format = "MM";
                    break;
                default:
                    break;
            }

            dtp.SetBinding(DatePicker.DateProperty, new Binding("DataSelecionada", BindingMode.TwoWay));

            layoutData.Children.Add(dtp);

            layoutPrincipal.Children.Add(layoutData);
            layoutPrincipal.Children.Add(ObterLayoutBotoes());

            return layoutPrincipal;
        }

        private StackLayout ObterFormHora()
        {
            StackLayout layoutPrincipal = ObterLayoutPrincipal();

            StackLayout layoutHora = new StackLayout();
            layoutHora.Padding = new Thickness(0, 0, 0, 0);
            layoutHora.HorizontalOptions = LayoutOptions.CenterAndExpand;

            TimePicker tp = new TimePicker();
            tp.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            tp.Format = "HH:mm";

            tp.SetBinding(TimePicker.TimeProperty, new Binding("HoraSelecionada", BindingMode.TwoWay));

            layoutHora.Children.Add(tp);

            layoutPrincipal.Children.Add(layoutHora);
            layoutPrincipal.Children.Add(ObterLayoutBotoes());

            return layoutPrincipal;
        }
    }
}