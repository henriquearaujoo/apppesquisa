using System;
using System.ComponentModel;
using System.Windows.Input;
using app_pesquisa.interfaces;
using app_pesquisa.util;
using Xamarin.Forms;

namespace app_pesquisa
{
	public class CadastroParticipanteViewModel : INotifyPropertyChanged
	{
		private String txtNome;
		private String txtEmail;
		private String txtTelefone;
		private String txtEmpresa;
		private String txtInfoAdicional;
		private bool isRunning = false;

		public ICommand CmdCancelar { get; protected set; }
		public ICommand CmdSalvar { get; protected set; }

		private ContentPage page;

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

		public string TxtNome
		{
			get { return txtNome; }
			set
			{
				if (txtNome == value)
					return;

				txtNome = value;
				OnPropertyChanged("TxtNome");
			}
		}

		public string TxtEmail
		{
			get { return txtEmail; }
			set
			{
				if (txtEmail == value)
					return;

				txtEmail = value;
				OnPropertyChanged("TxtEmail");
			}
		}

		public string TxtTelefone
		{
			get { return txtTelefone; }
			set
			{
				if (txtTelefone == value)
					return;

				txtTelefone = value;
				OnPropertyChanged("TxtTelefone");
			}
		}

		public string TxtEmpresa
		{
			get { return txtEmpresa; }
			set
			{
				if (txtEmpresa == value)
					return;

				txtEmpresa = value;
				OnPropertyChanged("TxtEmpresa");
			}
		}

		public string TxtInfoAdicional
		{
			get { return txtInfoAdicional; }
			set
			{
				if (txtInfoAdicional == value)
					return;

				txtInfoAdicional = value;
				OnPropertyChanged("TxtInfoAdicional");
			}
		}

		public CadastroParticipanteViewModel(ContentPage page)
		{
			this.page = page;

			CmdCancelar = new Command(() => {
				Cancelar();
            });

			CmdSalvar = new Command(() => {
				Salvar();
            });
		}

		private void Cancelar()
		{
			this.page.Navigation.PopAsync();
		}

		private void Salvar()
		{
			bool valido = true;

			if (String.IsNullOrEmpty(TxtNome))
				valido = false;

			if (String.IsNullOrEmpty(TxtEmail))
				valido = false;

			if (String.IsNullOrEmpty(TxtTelefone))
				valido = false;

			if (String.IsNullOrEmpty(TxtEmpresa))
				valido = false;

			if (!valido)
				this.page.DisplayAlert("Aviso", "Preencha todos os campos obrigatórios.", "Ok");
			else
				Enviar();
                
		}

		private async void Enviar()
		{
			try
			{
				bool isOnline = Utils.IsOnline();

            	if (!isOnline)
                	throw new Exception("Não há conexão disponível.");

				IsRunning = true;

				//String mensagem = await new DadosPesquisaUtil().EnviarParticipante(TxtNome, TxtEmail, TxtTelefone, TxtEmpresa, TxtInfoAdicional);
				String sql = "";

                if (!String.IsNullOrEmpty(TxtInfoAdicional))
                    sql = "insert into tb_participante01 (nome, email, telefone, empresa, infoadicional, qtqrcode) values ('" + TxtNome + "', '" + TxtEmail + "', '" + TxtTelefone + "', '" + TxtEmpresa + "', '" + TxtInfoAdicional + "', 1) returning idparticipante01";
                else
                    sql = "insert into tb_participante01 (nome, email, telefone, empresa, qtqrcode) values ('" + TxtNome + "', '" + TxtEmail + "', '" + TxtTelefone + "', '" + TxtEmpresa + "', 1) returning idparticipante01";

                int idparticipante = await new DadosPesquisaUtil().EnviarSQL(sql, 0);

                await this.page.DisplayAlert("Sucesso", "Participante cadastrado com sucesso.", "Ok");

				DependencyService.Get<IUtils>().CompartilharCode(idparticipante + ";" + TxtNome  + ";" + TxtEmail + ";" + TxtTelefone + ";" + TxtEmpresa);
			}
			catch (Exception ex)
			{
				await this.page.DisplayAlert("Erro", ex.Message, "Ok");
			}
			finally
			{
				IsRunning = false;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(String nome)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(nome));
		}
	}
}
