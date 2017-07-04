using app_pesquisa.componentes;
using app_pesquisa.dao;
using app_pesquisa.model;
using app_pesquisa.util;
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
    public class ModalRespostaViewModel : INotifyPropertyChanged
    {
        private ContentPage page;
        private ModalResposta modalResposta;
        private ItemArvoreFormularioViewModel itemViewModel;
        private String txtResposta;
        private CE_Pesquisa03 opcaoSelecionada;
        private DateTime dataSelecionada;
        private TimeSpan horaSelecionada;
        private CE_Pesquisa07 resposta;
        private DAO_Pesquisa07 dao;
        private CE_Pesquisa08 pesquisador;
        public ICommand CmdCancelar { get; protected set; }
        public ICommand CmdConfirmar { get; protected set; }
		public int NPage { get; set; }
		public String TipoDado { get; set; }
		public List<CE_Pesquisa07> ListaRespostas { get; set; }

		public ModalRespostaViewModel(ContentPage page, ModalResposta modalResposta, ItemArvoreFormularioViewModel itemViewModel, CE_Pesquisa07 resposta, int npage, String tipoDado, List<CE_Pesquisa07> listaRespostas)
		{
			this.page = page;
			this.modalResposta = modalResposta;
			this.itemViewModel = itemViewModel;
			this.resposta = resposta;
			NPage = npage;
			TipoDado = tipoDado;
			ListaRespostas = listaRespostas;

			dao = DAO_Pesquisa07.Instance;

			pesquisador = Utils.ObterPesquisadorLogado();

			CmdCancelar = new Command(() =>
			{
				Cancelar();
				this.page.Navigation.PopModalAsync();
			});

			CmdConfirmar = new Command(() =>
			{
				DefinirResposta();
			});

			if (NPage == 0 && modalResposta.Item.qtrespostas > 1)
			{
				modalResposta.ListView.ItemTapped += ListView_ItemTapped;
			}

		}

		private void Cancelar()
		{
			if (NPage > 0)
			{
				if (ListaRespostas != null && ListaRespostas.Count > 0 && resposta != null)
					ListaRespostas.Remove(resposta);
			}
		}

        public string TxtResposta
        {
            get
            {
                return txtResposta;
            }

            set
            {
                if (value != txtResposta)
                {
                    txtResposta = value;
                    OnPropertyChanged("TxtResposta");
                }
            }
        }

        public CE_Pesquisa03 OpcaoSelecionada
        {
            get
            {
                return opcaoSelecionada;
            }

            set
            {
                if (value != opcaoSelecionada)
                {
                    opcaoSelecionada = value;
                    OnPropertyChanged("OpcaoSelecionada");
					TratarRespostaLista(opcaoSelecionada);
                }
            }
        }

        public DateTime DataSelecionada
        {
            get
            {
                return dataSelecionada;
            }

            set
            {
                if (value != DataSelecionada)
                {
                    dataSelecionada = value;
                    OnPropertyChanged("DataSelecionada");
                }
            }
        }

        public TimeSpan HoraSelecionada
        {
            get
            {
                return horaSelecionada;
            }

            set
            {
                if (value != horaSelecionada)
                {
                    horaSelecionada = value;
                    OnPropertyChanged("HoraSelecionada");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public String ObterValorTxt()
        {
            if (itemViewModel.IsRespondido)
            {
				if (NPage == 0)
                	resposta = dao.ObterRespostaPorPergunta(modalResposta.Item.idpesquisa04, itemViewModel.Item.Formulario.codigoformulario).FirstOrDefault();
				
                String tipodado = NPage == 0 ? modalResposta.Item.pesquisa02.tipodado : modalResposta.Item.pesquisa02outros.tipodado;

                switch (tipodado)
                {
                    case "Int":
                    case "Dbl":
						if (NPage == 0)
                        	return resposta.vlresposta.ToString().Replace(",", ".");
						else
							return resposta.vlrespostaoutros.ToString().Replace(",", ".");
                    case "Txt":
                    case "Lista":
                    case "Date":
                    case "MesAno":
                    case "Mes":
                    case "Hora":
						if (NPage == 0)
                        	return resposta.txresposta;
						else
							return resposta.txrespostaoutros;
                    default:
                        return null;
                }
            }

            return null;
        }

        public void SetarValores()
        {
            switch (TipoDado)
            {
                case "Int":
                case "Dbl":
                case "Txt":
                    SetarValorTxt();
                    break;
                case "Lista":
					if (NPage == 0)
					{
						if (modalResposta.Item.qtrespostas == 1)
							SetarValorLista();
						else
							SetarValorListaMulti();
					}
					else
						SetarValorLista();
                    break;
                case "Date":
                case "MesAno":
                case "Mes":
                    SetarValorData();
                    break;
                case "Hora":
                    SetarValorHora();
                    break;
                default:
                    break;
            }
        }

		public void SetarValorTxt()
        {
            TxtResposta = ObterValorTxt();
        }

        public void SetarValorLista()
        {
            try
            {
				String txresposta = ObterValorTxt();

				if (NPage == 0)
				{
					if (ListaRespostas != null)
						ListaRespostas.Clear();
					
					OpcaoSelecionada = modalResposta.Item.Opcoes.FirstOrDefault(o => o.descricao == txresposta);

				}
				else
				{
					if (txresposta != null)
						OpcaoSelecionada = modalResposta.Item.OpcoesOutros.FirstOrDefault(o => o.descricao == txresposta);

					if (ListaRespostas != null && ListaRespostas.Count > 0)
					{
						List<CE_Pesquisa03> opcoes = new List<CE_Pesquisa03>();
						opcoes.AddRange(modalResposta.Item.OpcoesOutros);

						foreach (var item in ListaRespostas)
						{
							var opcao = modalResposta.Item.OpcoesOutros.FirstOrDefault(o => o.idpesquisa03 == item.idpesquisa03outros);

							if (opcao != null)
								opcoes.Remove(opcao);
						}

						modalResposta.ListView.ItemsSource = opcoes;
					}
					
				}

				if (OpcaoSelecionada.campotipooutros == 1)
					OpcaoSelecionada.descricao = OpcaoSelecionada.descricao + " - " + resposta.txrespostaoutros; 

            }
            catch (Exception)
            {
                OpcaoSelecionada = null;
            }
        }

		public void SetarValorListaMulti()
		{
			if (itemViewModel.IsRespondido)
			{
				DAO_Pesquisa07 dao = DAO_Pesquisa07.Instance;

                List<CE_Pesquisa07> respostas = dao.ObterRespostaPorPergunta(modalResposta.Item.idpesquisa04, itemViewModel.Item.Formulario.codigoformulario);

				resposta = respostas.FirstOrDefault();

				foreach (var item in respostas)
				{
					/*var opcoes = modalResposta.Item.Opcoes.Where(o => o.descricao == item.txresposta).ToList();
					if (opcoes.Count > 0)
						opcoes[0].Selecionado = true;*/

					foreach (var opcao in modalResposta.Item.Opcoes)
					{
						if (opcao.descricao == item.txresposta)
						{
							opcao.IsSelecionado = true;

							if (opcao.campotipooutros == 1)
							{
								if (resposta.tipodadooutros == "Int" || resposta.tipodadooutros == "Dbl")
									opcao.descricao = opcao.descricao + " - " + item.vlrespostaoutros.ToString().Replace(",", ".");
								else
									opcao.descricao = opcao.descricao + " - " + item.txrespostaoutros;

							}

							if (ListaRespostas == null)
								ListaRespostas = new List<CE_Pesquisa07>();
							
							item.pesquisa03 = opcao;
							ListaRespostas.Add(item);
							break;
						}
					}
				}
			}	
		}

        public void SetarValorData()
        {
            String txt = ObterValorTxt();

            if (String.IsNullOrEmpty(txt))
                DataSelecionada = DateTime.Now;
            else
            {
                switch (TipoDado)
                {
                    case "Date":
                        DataSelecionada = DateTime.ParseExact(txt, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "MesAno":
                        DataSelecionada = DateTime.ParseExact(txt, "MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "Mes":
                        DataSelecionada = DateTime.ParseExact(txt, "MM", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    default:
                        break;
                }
            }

        }

        public void SetarValorHora()
        {
            String txt = ObterValorTxt();

            if (String.IsNullOrEmpty(txt))
                HoraSelecionada = TimeSpan.Parse(String.Format("{0:HH:mm}", DateTime.Now));
            else
                HoraSelecionada = TimeSpan.Parse(ObterValorTxt());
        }

        public void DefinirRespostaTxt()
        {
            try
            {
                if (String.IsNullOrEmpty(TxtResposta))
                {
                    this.page.DisplayAlert("Aviso", "Defina uma resposta antes de confirmar.", "Ok");
                    return;
                }

				if (NPage == 0)
				{
					switch (TipoDado)
					{
						case "Int":
						case "Dbl":
							resposta.vlresposta = Decimal.Parse(TxtResposta.Replace(".", ","));
							break;
						case "Txt":
							resposta.txresposta = TxtResposta;
							break;
						default:
							break;
					}

					resposta.idpesquisa03 = 0;
					resposta.chavepesquisa = itemViewModel.Item.Formulario.codigoformulario;
					dao.SalvarResposta(resposta);

					itemViewModel.IsRespondido = true;
				}
				else
				{
					switch (TipoDado)
					{
						case "Int":
						case "Dbl":
							resposta.vlrespostaoutros = Decimal.Parse(TxtResposta.Replace(".", ","));
							resposta.pesquisa03.descricao = resposta.pesquisa03.descricao + " - " + resposta.vlrespostaoutros;
							break;
						case "Txt":
							resposta.txresposta = resposta.pesquisa03.descricao;
							resposta.txrespostaoutros = TxtResposta;
							resposta.pesquisa03.descricao = resposta.pesquisa03.descricao + " - " + resposta.txrespostaoutros;
							break;
						default:
							break;
					}

					resposta.tipodadooutros = TipoDado;
					resposta.pesquisa03.IsSelecionado = true;
				}

                this.page.Navigation.PopModalAsync();
            }
            catch (Exception)
            {
                itemViewModel.IsRespondido = false;
                this.page.DisplayAlert("Erro", "Não foi possível salvar a resposta.", "Ok");
            }

        }

        public void DefinirRespostaLista()
        {
            try
            {
                if (OpcaoSelecionada == null)
                {
                    this.page.DisplayAlert("Aviso", "Selecione uma das opções antes de confirmar.", "Ok");
                    return;
                }

				if (NPage == 0)
				{
					if (ListaRespostas == null || ListaRespostas.Count == 0)
					{
						resposta.chavepesquisa = itemViewModel.Item.Formulario.codigoformulario;
						resposta.idpesquisa03 = OpcaoSelecionada.idpesquisa03;

						if (OpcaoSelecionada.retornopesquisa != null)
							resposta.vlresposta = Decimal.Parse(OpcaoSelecionada.retornopesquisa);

						if (OpcaoSelecionada.campotipooutros == 0)
						{
							resposta.vlrespostaoutros = 0;
							resposta.txrespostaoutros = null;
						}

						dao.SalvarResposta(resposta);
					}
					else
					{
						DeletarRespostas();

						foreach (var item in ListaRespostas)
						{
							if (item.pesquisa03.campotipooutros == 0)
							{
								item.vlrespostaoutros = 0;
								item.txrespostaoutros = null;
							}

			                dao.SalvarResposta(item);
						}
					}

					itemViewModel.IsRespondido = true;
				}
				else
				{
					if (OpcaoSelecionada.retornopesquisa != null)
						resposta.vlrespostaoutros = Decimal.Parse(OpcaoSelecionada.retornopesquisa);

					resposta.txrespostaoutros = OpcaoSelecionada.descricao;

					resposta.pesquisa03.descricao = resposta.pesquisa03.descricao + " - " + resposta.txrespostaoutros;

					resposta.idpesquisa03outros = OpcaoSelecionada.idpesquisa03;

					resposta.pesquisa03outros = OpcaoSelecionada;

					resposta.tipodadooutros = TipoDado;

					resposta.pesquisa03.IsSelecionado = true;
				}

                this.page.Navigation.PopModalAsync();
            }
            catch (Exception)
            {
                itemViewModel.IsRespondido = false;
                this.page.DisplayAlert("Erro", "Não foi possível salvar a resposta.", "Ok");
            }
        }

		private void DeletarRespostas()
		{
			DAO_Pesquisa07 dao = DAO_Pesquisa07.Instance;

			List<CE_Pesquisa07> respostas = dao.ObterRespostaPorPergunta(modalResposta.Item.idpesquisa04, itemViewModel.Item.Formulario.codigoformulario);

			foreach (var item in respostas)
			{
				dao.DeleteResposta(item.idpesquisa07);
			}
		}

		public void DefinirRespostaListaMulti()
		{
			try
			{
				//var selecionados = modalResposta.Item.Opcoes.Where(o => o.IsSelecionado).ToList();

				if (ListaRespostas == null || ListaRespostas.Count == 0)
				{
                    this.page.DisplayAlert("Aviso", "Selecione pelo menos uma das opções antes de confirmar.", "Ok");
                    return;
				}

				if (ListaRespostas.Count < modalResposta.Item.qtminrespostas && modalResposta.Item.qtminobrigatoria == 1)
				{
					this.page.DisplayAlert("Aviso", "Para esta pergunta deve ser informado no mínimo " + modalResposta.Item.qtminrespostas + " resposta(s).", "Ok");
                    return;
				}

				DeletarRespostas();

				foreach (var item in ListaRespostas)
				{
					if (item.pesquisa03.campotipooutros == 0)
					{
						item.vlrespostaoutros = 0;
						item.txrespostaoutros = null;
					}

					item.idpesquisa07 = 0;
	                dao.SalvarResposta(item);
				}

                ListaRespostas.Clear();

				itemViewModel.IsRespondido = true;
                this.page.Navigation.PopModalAsync();
			}
			catch (Exception)
			{
				itemViewModel.IsRespondido = false;
                this.page.DisplayAlert("Erro", "Não foi possível salvar a resposta.", "Ok");
			}
		}

        public void DefinirRespostaData()
        {
            try
            {
                if (DataSelecionada == null)
                {
                    this.page.DisplayAlert("Aviso", "Selecione uma data antes de confirmar.", "Ok");
                    return;
                }

                resposta.chavepesquisa = itemViewModel.Item.Formulario.codigoformulario;
                switch (TipoDado)
                {
                    case "Date":
                        resposta.txresposta = String.Format("{0:dd/MM/yyyy}", DataSelecionada);
                        break;
                    case "MesAno":
                        resposta.txresposta = String.Format("{0:MM/yyyy}", DataSelecionada);
                        break;
                    case "Mes":
                        resposta.txresposta = String.Format("{0:MM}", DataSelecionada);
                        break;
                    default:
                        break;
                }

                dao.SalvarResposta(resposta);

                itemViewModel.IsRespondido = true;
                this.page.Navigation.PopModalAsync();
            }
            catch (Exception)
            {
                itemViewModel.IsRespondido = false;
                this.page.DisplayAlert("Erro", "Não foi possível salvar a resposta.", "Fechar");
            }
        }

        public void DefinirRespostaHora()
        {
            try
            {
                if (HoraSelecionada == null)
                {
                    this.page.DisplayAlert("Aviso", "Selecione uma hora antes de confirmar.", "Ok");
                    return;
                }

                resposta.chavepesquisa = itemViewModel.Item.Formulario.codigoformulario;
                resposta.txresposta = HoraSelecionada.Hours + ":" + HoraSelecionada.Minutes;
                dao.SalvarResposta(resposta);

                itemViewModel.IsRespondido = true;
                this.page.Navigation.PopModalAsync();
            }
            catch (Exception)
            {
                itemViewModel.IsRespondido = false;
                this.page.DisplayAlert("Erro", "Não foi possível salvar a resposta.", "Fechar");
            }
        }

        public void DefinirResposta()
        {
			if (resposta == null)
			{
				resposta = new CE_Pesquisa07();
				resposta.idpesquisa04 = modalResposta.Item.idpesquisa04;
				resposta.idpesquisa06 = modalResposta.Pesquisa06.idpesquisa06;
				resposta.idpesquisador = pesquisador.idpesquisador;
				resposta.idcliente = pesquisador.idcliente;
			}

            switch (TipoDado)
            {
                case "Int":
                case "Dbl":
                case "Txt":
                    DefinirRespostaTxt();
                    break;
                case "Lista":
					if (NPage == 0)
					{
						if (modalResposta.Item.qtrespostas == 1)
							DefinirRespostaLista();
						else
							DefinirRespostaListaMulti();
					}
					else
						DefinirRespostaLista();
                    break;
                case "Date":
                case "MesAno":
                case "Mes":
                    DefinirRespostaData();
                    break;
                case "Hora":
                    DefinirRespostaHora();
                    break;
                default:
                    break;
            }

            MessagingCenter.Send("", "AtualizarContador", itemViewModel.Item.Formulario);
        }

		private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var list = sender as ListView;

			CE_Pesquisa03 item = list.SelectedItem as CE_Pesquisa03;

			List<CE_Pesquisa03> selecionados = modalResposta.Item.Opcoes.Where(o => o.IsSelecionado).ToList();

			var selecionado = selecionados.Where(o => o.idpesquisa03 == item.idpesquisa03).FirstOrDefault();

			if (selecionado == null)
			{
				if (item.campotipooutros == 0)
					item.IsSelecionado = true;
				
				TratarRespostaLista(item);
			}
			else
			{
				selecionado.IsSelecionado = !selecionado.IsSelecionado;

				if (selecionado.IsSelecionado)
				{
					if (selecionado.IsSelecionado && selecionado.campotipooutros == 1)
						selecionado.IsSelecionado = false;

					TratarRespostaLista(selecionado);
				}
				else
				{
					var resp = ListaRespostas.FirstOrDefault(o => o.idpesquisa03 == selecionado.idpesquisa03);
					selecionado.descricao = selecionado.descricao.Replace(" - " + resp.txresposta, "").Replace(" - " + resp.txrespostaoutros, "").Replace(" - " + resp.vlresposta.ToString(), "").Replace(" - " + resp.vlrespostaoutros.ToString(), "");
					ListaRespostas.Remove(resp);
				}
			}

			if (selecionados.Count >= modalResposta.Item.qtrespostas)
			{
				if (selecionado == null){
					selecionados.Last().IsSelecionado = false;
					var resp = ListaRespostas.FirstOrDefault(o => o.idpesquisa03 == selecionados.Last().idpesquisa03);
					selecionados.Last().descricao = selecionados.Last().descricao.Replace(" - " + resp.txresposta, "").Replace(" - " + resp.txrespostaoutros, "").Replace(" - " + resp.vlresposta.ToString(), "").Replace(" - " + resp.vlrespostaoutros.ToString(), "");
					ListaRespostas.Remove(resp);
				}
				else
				{
					if (selecionado.idpesquisa03 != item.idpesquisa03)
					{
						selecionados.Last().IsSelecionado = false;
						var resp = ListaRespostas.FirstOrDefault(o => o.idpesquisa03 == selecionados.Last().idpesquisa03);
						selecionados.Last().descricao = selecionados.Last().descricao.Replace(" - " + resp.txresposta, "").Replace(" - " + resp.txrespostaoutros, "").Replace(" - " + resp.vlresposta.ToString(), "").Replace(" - " + resp.vlrespostaoutros.ToString(), "");
						ListaRespostas.Remove(resp);
					}
				}
			}
		}

		private void CriarResposta(CE_Pesquisa03 opcao)
		{
			resposta = new CE_Pesquisa07();
			resposta.idpesquisa04 = modalResposta.Item.idpesquisa04;
			resposta.idpesquisa06 = modalResposta.Pesquisa06.idpesquisa06;
			resposta.idpesquisador = pesquisador.idpesquisador;
			resposta.idcliente = pesquisador.idcliente;
			resposta.idpesquisa03 = opcao.idpesquisa03;
			resposta.pesquisa03 = opcao;
			resposta.txresposta = opcao.descricao;
			resposta.chavepesquisa = itemViewModel.Item.Formulario.codigoformulario;
		}

		public async void TratarRespostaLista(CE_Pesquisa03 opcao)
		{
			//var selecionado = modalResposta.Item.Opcoes.Where(o => o.IsSelecionado).ToList().FirstOrDefault(b => b.idpesquisa03 == OpcaoSelecionada.idpesquisa03);

			if (modalResposta.Item.pesquisa02outros != null && opcao.campotipooutros == 1) //&& selecionado == null)
			{
				if (itemViewModel.IsRespondido && modalResposta.Item.qtrespostas == 1)
				{
					resposta = dao.ObterRespostaPorPergunta(modalResposta.Item.idpesquisa04, itemViewModel.Item.Formulario.codigoformulario).FirstOrDefault();
					resposta.pesquisa03 = opcao;

					if (resposta.idpesquisa03 != opcao.idpesquisa03)
						CriarResposta(opcao);

					if (ListaRespostas != null)
						ListaRespostas.Clear();
					else
						ListaRespostas = new List<CE_Pesquisa07>();

					ListaRespostas.Add(resposta);
				}
				else
				{
					CriarResposta(opcao);

					if (opcao.retornopesquisa != null)
						resposta.vlresposta = Decimal.Parse(opcao.retornopesquisa);

					if (ListaRespostas == null)
						ListaRespostas = new List<CE_Pesquisa07>();

                    ListaRespostas.Add(resposta);
				}

				ModalResposta modalRespostaOutros = new ModalResposta(modalResposta.Item, modalResposta.Pesquisa06, modalResposta.Item.pesquisa02outros.tipodado, 1);
				modalRespostaOutros.CodigoFormulario = itemViewModel.Item.Formulario.codigoformulario;
				ModalRespostaViewModel viewModel = new ModalRespostaViewModel(this.page, modalRespostaOutros, itemViewModel, resposta, 1, modalResposta.Item.pesquisa02outros.tipodado, ListaRespostas);
				modalRespostaOutros.BindingContext = viewModel;
				await this.page.Navigation.PushModalAsync(modalRespostaOutros);
				viewModel.SetarValores();
			}
			else
			{
				if (NPage == 0)
				{
					//if (ListaRespostas != null)
					//	ListaRespostas.Clear();
					//else
					//	ListaRespostas = new List<CE_Pesquisa07>();

                    if (ListaRespostas == null)
                        ListaRespostas = new List<CE_Pesquisa07>();

                    CriarResposta(opcao);

					if (opcao.retornopesquisa != null)
						resposta.vlresposta = Decimal.Parse(opcao.retornopesquisa);

					ListaRespostas.Add(resposta);
				}
			}
			
		}

	    private void OnPropertyChanged(String nome)
	    {
	        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nome));
	    }
    }
}
