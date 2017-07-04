using System;
using System.Collections.Generic;
using app_pesquisa.componentes;
using Xamarin.Forms;

namespace app_pesquisa
{
	public partial class EventoPage : ContentPage
	{
		public EventoPage()
		{
			StackLayout layout = new StackLayout();
			layout.Spacing = 0;

			StackLayout layoutFuncionalidades = new StackLayout();
			layoutFuncionalidades.VerticalOptions = LayoutOptions.CenterAndExpand;
			layoutFuncionalidades.HorizontalOptions = LayoutOptions.CenterAndExpand;
			//layoutFuncionalidades.BackgroundColor = Color.FromHex("#3F51B5");

			StackLayout layoutBotoes = new StackLayout();
			layoutBotoes.VerticalOptions = LayoutOptions.CenterAndExpand;
			layoutBotoes.HorizontalOptions = LayoutOptions.CenterAndExpand;

			Button btInformarParticipante = new Button();
			btInformarParticipante.Text = "Informar Participante";
			btInformarParticipante.WidthRequest = 350;
			btInformarParticipante.TextColor = Color.FromHex("#FFFFFF");
			btInformarParticipante.SetBinding(Button.CommandProperty, new Binding("CmdInformarParticipante", BindingMode.OneWay));
			btInformarParticipante.SetBinding(Button.IsEnabledProperty, new Binding("IsRunning", BindingMode.TwoWay, new NegateBooleanConverter()));
			btInformarParticipante.BackgroundColor = Color.FromHex("#3F51B5");

			Button btCadastrarParticipante = new Button();
			btCadastrarParticipante.Text = "Cadastrar Participante";
			btCadastrarParticipante.WidthRequest = 350;
			btCadastrarParticipante.TextColor = Color.FromHex("#FFFFFF");
			btCadastrarParticipante.SetBinding(Button.CommandProperty, new Binding("CmdCadastrarParticipante", BindingMode.OneWay));
			btCadastrarParticipante.SetBinding(Button.IsEnabledProperty, new Binding("IsRunning", BindingMode.TwoWay, new NegateBooleanConverter()));
			btCadastrarParticipante.BackgroundColor = Color.FromHex("#3F51B5");

			layoutBotoes.Children.Add(btCadastrarParticipante);
			layoutBotoes.Children.Add(btInformarParticipante);

			StackLayout layoutInfo = new StackLayout();
			layoutInfo.VerticalOptions = LayoutOptions.CenterAndExpand;
			layoutInfo.HorizontalOptions = LayoutOptions.StartAndExpand;
			layoutInfo.Orientation = StackOrientation.Horizontal;
			layoutInfo.Padding = new Thickness(0, 20, 0, 0);

			StackLayout layoutInfo1 = new StackLayout();
			layoutInfo1.Orientation = StackOrientation.Vertical;
			Label lblNome = new Label();
			lblNome.Text = "Nome:";
			lblNome.FontSize = 19;
			lblNome.FontAttributes = FontAttributes.Bold;
			Label lblEmail = new Label();
			lblEmail.Text = "Email:";
			lblEmail.FontSize = 19;
			lblEmail.FontAttributes = FontAttributes.Bold;
			Label lblTel = new Label();
			lblTel.Text = "Telefone:";
			lblTel.FontSize = 19;
			lblTel.FontAttributes = FontAttributes.Bold;
			Label lblEmpresa = new Label();
			lblEmpresa.Text = "Empresa:";
			lblEmpresa.FontSize = 19;
			lblEmpresa.FontAttributes = FontAttributes.Bold;

			layoutInfo1.Children.Add(lblNome);
			layoutInfo1.Children.Add(lblEmail);
			layoutInfo1.Children.Add(lblTel);
			layoutInfo1.Children.Add(lblEmpresa);

			layoutInfo.Children.Add(layoutInfo1);

			StackLayout layoutInfo2 = new StackLayout();
			layoutInfo2.Orientation = StackOrientation.Vertical;
			Label lblNomeParticipante = new Label();
			lblNomeParticipante.SetBinding(Label.TextProperty, new Binding("NomeParticipante", BindingMode.OneWay));
			lblNomeParticipante.FontSize = 19;
			Label lblEmailParticipante = new Label();
			lblEmailParticipante.SetBinding(Label.TextProperty, new Binding("EmailParticipante", BindingMode.OneWay));
			lblEmailParticipante.FontSize = 19;
			Label lblTelParticipante = new Label();
			lblTelParticipante.SetBinding(Label.TextProperty, new Binding("TelParticipante", BindingMode.OneWay));
			lblTelParticipante.FontSize = 19;
			Label lblEmpresaParticipante = new Label();
			lblEmpresaParticipante.SetBinding(Label.TextProperty, new Binding("EmpresaParticipante", BindingMode.OneWay));
			lblEmpresaParticipante.FontSize = 19;

			layoutInfo2.Children.Add(lblNomeParticipante);
			layoutInfo2.Children.Add(lblEmailParticipante);
			layoutInfo2.Children.Add(lblTelParticipante);
			layoutInfo2.Children.Add(lblEmpresaParticipante);

			layoutInfo.Children.Add(layoutInfo2);


			layoutBotoes.Children.Add(layoutInfo);

			layoutFuncionalidades.Children.Add(layoutBotoes);


			layout.Children.Add(new ToobarPesquisa());
			layout.Children.Add(layoutFuncionalidades);
            layout.Children.Add(new ActivityIndicatorRunning());
            layout.Children.Add(new ToobarBotoesPesquisa());

            Content = layout;

			EventoPageViewModel model = new EventoPageViewModel(this);
			BindingContext = model;

			NavigationPage.SetHasNavigationBar(this, false);

			//InitializeComponent();
		}
	}
}
