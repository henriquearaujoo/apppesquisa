using app_pesquisa.model;
using app_pesquisa.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.componentes
{
    public class ListViewPesquisas : StackLayout
    {
        private ContentPage page;
        private void Initialize()
        {
            SetBinding(StackLayout.IsVisibleProperty, new Binding("IsRunning", BindingMode.OneWay, new NegateBooleanConverter()));
            BackgroundColor = Color.FromHex("#FFFFFF");

            ListView listView = new ListView();
            listView.SeparatorColor = Color.FromHex("#B6B6B6");
            listView.RowHeight = 130;
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("Pesquisas", BindingMode.OneWay));
            listView.ItemTemplate = new DataTemplate(() =>
            {
                ViewCell cell = new ViewCell();
                StackLayout layoutList = new StackLayout();
                layoutList.Padding = new Thickness(16, 0, 0, 0);
                layoutList.Orientation = StackOrientation.Horizontal;

                StackLayout layoutImage = new StackLayout();
                layoutImage.Orientation = StackOrientation.Vertical;
                layoutImage.VerticalOptions = LayoutOptions.Center;

                Image img = new Image()
                {
                    Source = "pesquisa.png"
                };

                layoutImage.Children.Add(img);

                StackLayout layoutLabel = new StackLayout();
                layoutLabel.Padding = new Thickness(10, 0, 0, 0);
                layoutLabel.Orientation = StackOrientation.Vertical;
                layoutLabel.VerticalOptions = LayoutOptions.Center;

                Label lbl = new Label()
                {
                    FontSize = 19,
                    TextColor = Color.FromHex("#212121")
                };
                lbl.SetBinding(Label.TextProperty, new Binding("pesquisa01.nomepesquisa", BindingMode.OneWay));

                Label lbl2 = new Label()
                {
                    FontSize = 14,
                    TextColor = Color.FromHex("#212121")
                };

                lbl2.SetBinding(Label.TextProperty, new Binding("nome", BindingMode.OneWay));

                Label lbl3 = new Label()
                {
                    FontSize = 14,
                    TextColor = Color.FromHex("#212121")
                };

                lbl3.SetBinding(Label.TextProperty, new Binding("pesquisa01.DSDATACONSOLIDADA", BindingMode.OneWay));

                layoutLabel.Children.Add(lbl);
                layoutLabel.Children.Add(lbl2);
                layoutLabel.Children.Add(lbl3);

                layoutList.Children.Add(layoutImage);
                layoutList.Children.Add(layoutLabel);

                cell.View = layoutList;

                return cell;
            });

            listView.ItemTapped += ListView_ItemTapped;

            Children.Add(listView);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;

            CE_Pesquisa06 onda = list.SelectedItem as CE_Pesquisa06;

            if (onda.IsDentroDoPrazo())
                Navegar(onda);
            else
                ((PesquisaPage)page).PesquisaForaDoPrazo();
        }

        public async void Navegar(CE_Pesquisa06 pesquisa06)
        {
            await page.Navigation.PushAsync(new FormularioPage(pesquisa06));
        }

        public ListViewPesquisas(ContentPage page)
        {
            this.page = page;
            Initialize();
        }
    }
}
