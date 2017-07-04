using app_pesquisa.componentes;
using app_pesquisa.dao;
using app_pesquisa.model;
using app_pesquisa.util;
using app_pesquisa.view;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_pesquisa.viewmodel
{
    public class FormularioPageViewModel : INotifyPropertyChanged
    {
        private ContentPage page;
        private List<CE_Pesquisa04> itensFormulario;
        private CE_Pesquisa06 pesquisa06;
        private CE_Pesquisa08 pesquisador;
        private CE_Formulario formulario;
        private ArvoreFormulario arvoreFormulario;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CmdFinalizarFormulario { get; protected set; }
        public ICommand CmdEnviar { get; protected set; }
        public ICommand CmdVoltar { get; protected set; }

        private bool isRunning = false;

        private String title;

        private String subtitle;

        private String contador;

        private DAO_Pesquisa04 dao04;
        private DAO_Pesquisa02 dao02;
        private DAO_Pesquisa03 dao03;
        private DAO_Pesquisa07 dao07;
        private DAO_Formulario daoForm;
        
        public FormularioPageViewModel(ContentPage page, CE_Pesquisa06 pesquisa06)
        {
            this.page = page;
            this.pesquisa06 = pesquisa06;

            IsRunning = true;

            dao02 = DAO_Pesquisa02.Instance;
            dao03 = DAO_Pesquisa03.Instance;
            dao04 = DAO_Pesquisa04.Instance;
            dao07 = DAO_Pesquisa07.Instance;
            daoForm = DAO_Formulario.Instance;

            pesquisador = Utils.ObterPesquisadorLogado();

            formulario = daoForm.ObterUltimoFormulario(pesquisa06.pesquisa01.idpesquisa01);

            if (formulario == null)
                CriarFormulario();

            AdicionarControles();
            
            Title = pesquisador.razaosocial;
            SubTitle = pesquisador.nome;

            ObterContadores();

            CmdVoltar = new Command(() => {
                this.page.Navigation.PopAsync();
            });

            CmdFinalizarFormulario = new Command(() => {
                FinalizarFormulario();
            });

            CmdEnviar = new Command(() => {
                EnviarDados();
            });

            ObterItensFormulario();
            
            IsRunning = false;
        }

        private void ObterContadores()
        {
            int totalFormRespondidos = dao07.ObterTotalFormRespondidos(pesquisa06.idpesquisa06);
            Contador = totalFormRespondidos + "/" + pesquisa06.qtamostraporpesquisador + " - " + String.Format("{0:dd/MM/yy}", DateTime.ParseExact(pesquisa06.dtfimpesquisa, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
        }

        private void VerificarObrigatorias()
        {
            List<CE_Pesquisa04> obrigatorias = ItensFormulario.Where(o => o.obrigatoria == 1).ToList();

            foreach (var item in obrigatorias)
            {
                if (!item.IsRespondido(formulario.codigoformulario))
                    throw new Exception("Por favor, responda as questões obrigatórias.");
            }
        }

        private async void FinalizarFormulario()
        {
            try
            {
                if (pesquisa06.IsDentroDoPrazo())
                {
                    VerificarObrigatorias();

                    bool confirmacao = await this.page.DisplayAlert("Confirmação", "Confirma a finalização desse formulário?", "Sim", "Não");

                    if (confirmacao)
                    {
                        IsRunning = true;

                        await Task.Delay(1000);

                        AtualizarStatusFormulario();

                        CriarFormulario();

                        arvoreFormulario.Formulario = formulario;

                        ObterItensFormulario();

                        ObterContadores();

                        MessagingCenter.Send("", "AtualizarContador", formulario);

                        IsRunning = false;
                    }
                }
                else
                {
                    await this.page.DisplayAlert("Aviso", "Pesquisa fora do prazo, baixe novas pesquisas.", "Ok");
                    await this.page.Navigation.PopAsync();
                }
                
            }
            catch (Exception ex)
            {
                await this.page.DisplayAlert("Aviso", ex.Message, "Ok");
            }
           
        }

        public List<CE_Pesquisa04> ItensFormulario
        {
            get
            {
                return itensFormulario;
            }

            set
            {
                if (value != itensFormulario)
                {
                    itensFormulario = value;
                    OnPropertyChanged("ItensFormulario");
                }

            }
        }

        public string Contador
        {
            get
            {
                return contador;
            }

            set
            {
                if (value != contador)
                { 
                    contador = value;
                    OnPropertyChanged("Contador");
                }
            }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (value != isRunning)
                {
                    isRunning = value;
                    OnPropertyChanged("IsRunning");
                }
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (title == value)
                    return;

                title = value;
                OnPropertyChanged("Title");
            }
        }

        public string SubTitle
        {
            get { return subtitle; }
            set
            {
                if (subtitle == value)
                    return;

                subtitle = value;
                OnPropertyChanged("Subtitle");
            }
        }

        public void AdicionarControles()
        {
            StackLayout layoutPrincipal = new StackLayout();
            layoutPrincipal.Spacing = 0;
            layoutPrincipal.BackgroundColor = Color.FromHex("#FFFFFF");
            layoutPrincipal.Children.Add(new ToobarFormulario());

            layoutPrincipal.Children.Add(new Contadores(formulario));

            ScrollView view = new ScrollView();
            view.BackgroundColor = Color.FromHex("#FFFFFF");
            view.SetBinding(ScrollView.IsVisibleProperty, new Binding("IsRunning", BindingMode.TwoWay, new NegateBooleanConverter()));
            StackLayout layout = new StackLayout();
            arvoreFormulario = new ArvoreFormulario(this.page, pesquisa06);
            arvoreFormulario.Formulario = formulario;
            layout.Children.Add(arvoreFormulario);
            view.Content = layout;

            layoutPrincipal.Children.Add(view);
            
            layoutPrincipal.Children.Add(new ActivityIndicatorRunning());

            StackLayout layoutFooter = new StackLayout();
            layoutFooter.Spacing = 0;
            layoutFooter.Orientation = StackOrientation.Vertical;
            layoutFooter.BackgroundColor = Color.FromHex("#3F51B5");
            layoutFooter.HorizontalOptions = LayoutOptions.FillAndExpand;
            layoutFooter.VerticalOptions = LayoutOptions.EndAndExpand;

            StackLayout layoutFooterBotao = new StackLayout();
            layoutFooterBotao.Orientation = StackOrientation.Horizontal;
            layoutFooterBotao.BackgroundColor = Color.FromHex("#3F51B5");
            layoutFooterBotao.HorizontalOptions = LayoutOptions.FillAndExpand;
            layoutFooterBotao.Spacing = 0;

            Button btnConfirmar = new Button()
            {
                Image = "confirmar_formulario.png",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#3F51B5"), 
                Text = "  Finalizar",
                TextColor = Color.FromHex("#FFFFFF")
            };
            
            btnConfirmar.SetBinding(Button.CommandProperty, new Binding("CmdFinalizarFormulario", BindingMode.OneWay));

            layoutFooterBotao.Children.Add(btnConfirmar);

            Button btnEnviar = new Button()
            {
                Image = "enviar.png",
                Text = "Enviar",
                TextColor = Color.FromHex("#FFFFFF"),
                BackgroundColor = Color.FromHex("#3F51B5"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            btnEnviar.SetBinding(Button.CommandProperty, new Binding("CmdEnviar", BindingMode.OneWay));

            layoutFooterBotao.Children.Add(btnEnviar);

            StackLayout layoutLabel = new StackLayout();
            layoutLabel.Orientation = StackOrientation.Horizontal;
            layoutLabel.BackgroundColor = Color.FromHex("#3F51B5");
            layoutLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
            layoutLabel.VerticalOptions = LayoutOptions.End;

            Label lblIcon = new Label()
            {
                Text = "EZQUEST 1.7 - Powered by ICON",
                TextColor = Color.Yellow,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.End,
                FontAttributes = FontAttributes.Bold,
            };

            layoutLabel.Children.Add(lblIcon);

            layoutFooter.Children.Add(layoutFooterBotao);
            layoutFooter.Children.Add(layoutLabel);

            layoutPrincipal.Children.Add(layoutFooter);

            this.page.Content = layoutPrincipal;
        }

        private async void EnviarDados()
        {
            try
            {
                bool isOnline = Utils.IsOnline();

                if (!isOnline)
                    throw new Exception("Não há conexão disponível.");

                IsRunning = true;

                int registros = await new DadosPesquisaUtil().Upload();

                if (registros > 0)
                    await this.page.DisplayAlert("Sucesso", registros + " registros enviados.", "Ok");
                else
                    await this.page.DisplayAlert("Aviso", "Não há registros para enviar.", "Ok");

            }
            catch (Exception ex)
            {
                await this.page.DisplayAlert("Aviso", ex.Message, "Ok");
            }
            finally
            {
                IsRunning = false;
            }
        }

        private void AtualizarStatusFormulario()
        {
            if (formulario != null)
            {
                formulario.finalizado = 1;
                daoForm.AtualizarFormulario(formulario);
            }
        }
        private void CriarFormulario()
        {
            try
            {
                formulario = new CE_Formulario();
                formulario.idpesquisa01 = pesquisa06.pesquisa01.idpesquisa01;
                formulario.idpesquisa06 = pesquisa06.idpesquisa06;
                //formulario.codigoformulario = Utils.ObterCodigoFormulario();
                formulario.codigoformulario = String.Format("{0:00000}", pesquisador.idpesquisador) + String.Format("{0:000000}", pesquisa06.idpesquisa06) + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                formulario.finalizado = 0;

                daoForm.InserirFormulario(formulario);
                
            }
            catch (Exception)
            {
                this.page.DisplayAlert("Erro", "Erro ao criar um novo formulário.", "Ok");
            }
        }

        private void ObterItensFormulario()
        {
            try
            {
                ItensFormulario = dao04.ObterPerguntas(pesquisa06.pesquisa01.idpesquisa01);

                foreach (var pergunta in ItensFormulario)
                {
                    pergunta.pesquisa02 = dao02.ObterTipo(pergunta.idpesquisa02);
                    if (pergunta.pesquisa02 != null)
                        pergunta.Opcoes = dao03.ObterValores(pergunta.pesquisa02.idpesquisa02);

					if (pergunta.idpesquisa02outros != 0)
					{
						pergunta.pesquisa02outros = dao02.ObterTipo(pergunta.idpesquisa02outros);
						pergunta.OpcoesOutros = dao03.ObterValores(pergunta.pesquisa02outros.idpesquisa02);
					}
                }

                arvoreFormulario.Itens = ItensFormulario;
                arvoreFormulario.Initialize();
            }
            catch (Exception)
            {
                this.page.DisplayAlert("Erro", "Erro ao obter perguntas do formulário.", "Ok");
            }
            
        }
        
        private void OnPropertyChanged(String nome)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nome));
        }
    }
}
