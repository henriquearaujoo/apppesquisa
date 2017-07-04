using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace app_pesquisa
{
	public partial class CadastroPartipantePage : ContentPage
	{
		public CadastroPartipantePage()
		{
			CadastroParticipanteViewModel model = new CadastroParticipanteViewModel(this);
			BindingContext = model;

			NavigationPage.SetHasNavigationBar(this, false);

			InitializeComponent();
		}
	}
}
