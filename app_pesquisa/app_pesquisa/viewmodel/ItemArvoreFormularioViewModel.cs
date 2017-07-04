using app_pesquisa.componentes;
using app_pesquisa.model;
using app_pesquisa.dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_pesquisa.viewmodel
{
    public class ItemArvoreFormularioViewModel : INotifyPropertyChanged
    {
        private ContentPage page;
        public ItemArvoreFormulario Item { get; set; }
        private Boolean isRespondido;
        public ICommand CmdExpand { get; protected set; }
		public ICommand CmdShowDialogResposta { get; protected set; }

        public Boolean IsRespondido
        {
            get
            {
                return isRespondido;
            }

            set
            {
                if (value != isRespondido)
                { 
                    isRespondido = value;
                    OnPropertyChanged("IsRespondido");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ItemArvoreFormularioViewModel(ItemArvoreFormulario item, ContentPage page)
        {
            this.Item = item;
            this.page = page;

            CmdExpand = new Command(() => {

                if (item.IsExpanded)
                {
                    foreach (var filho in item.Children)
                    {
                        if (filho is ItemArvoreFormulario)
                            filho.IsVisible = false;
                    }

                    item.IsExpanded = false;
                    item.Botao.Source = "plus.png";
                    
                }
                else
                {
                    foreach (var filho in item.Children)
                    {
                        if (filho is ItemArvoreFormulario)
                            filho.IsVisible = true;
                    }

                    item.IsExpanded = true;
                    item.Botao.Source = "minus.png";
                }
                
            });

            CmdShowDialogResposta = new Command(() =>
            {
                ShowDialogResposta();
            });
        }

        public async void ShowDialogResposta()
        {
            if (Item.Pesquisa06.IsDentroDoPrazo())
            {
				DAO_Pesquisa03 dao03 = DAO_Pesquisa03.Instance;
				DAO_Pesquisa02 dao02 = DAO_Pesquisa02.Instance;

				if (Item.Pesquisa04.pesquisa02 != null)
                    Item.Pesquisa04.Opcoes = dao03.ObterValores(Item.Pesquisa04.pesquisa02.idpesquisa02);

				if (Item.Pesquisa04.idpesquisa02outros != 0)
				{
					Item.Pesquisa04.pesquisa02outros = dao02.ObterTipo(Item.Pesquisa04.idpesquisa02outros);
					Item.Pesquisa04.OpcoesOutros = dao03.ObterValores(Item.Pesquisa04.pesquisa02outros.idpesquisa02);
				}

                ModalResposta modalResposta = new ModalResposta(Item.Pesquisa04, Item.Pesquisa06, Item.Pesquisa04.pesquisa02.tipodado, 0);
                modalResposta.CodigoFormulario = Item.Formulario.codigoformulario;
                ModalRespostaViewModel viewModel = new ModalRespostaViewModel(page, modalResposta, this, null, 0, Item.Pesquisa04.pesquisa02.tipodado, null);
                modalResposta.BindingContext = viewModel;
                await this.page.Navigation.PushModalAsync(modalResposta);
                viewModel.SetarValores();
            }
            else
            {
                await page.DisplayAlert("Aviso", "Pesquisa fora do prazo, baixe novas pesquisas.", "Ok");
                await page.Navigation.PopAsync();
            }
            
        }

        private void OnPropertyChanged(String nome)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nome));
        }
    }
}
