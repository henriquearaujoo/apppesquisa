using System;
using System.ComponentModel;
using System.Windows.Input;
using app_pesquisa.dao;
using app_pesquisa.model;
using app_pesquisa.util;
using Xamarin.Forms;

namespace app_pesquisa
{
	public class EventoPageViewModel : INotifyPropertyChanged
	{
		
		private CE_Pesquisa08 pesquisador;
		private bool isRunning = false;
		private WSUtil ws;
		private ContentPage page;

		private String title;
		private String subtitle;

		private String nomeParticipante;
		private String emailParticipante;
		private String telParticipante;
		private String empresaParticipante;

		public ICommand CmdAtualizar { get; protected set; }
		public ICommand CmdEnviar { get; protected set; }
		public ICommand CmdSair { get; protected set; }
		public ICommand CmdConfiguracoes { get; protected set; }
		public ICommand CmdInformarParticipante { get; protected set; }
		public ICommand CmdCadastrarParticipante { get; protected set; }

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

		public string NomeParticipante
		{
			get { return nomeParticipante; }
			set
			{
				if (nomeParticipante == value)
					return;

				nomeParticipante = value;
				OnPropertyChanged("NomeParticipante");
			}
		}

		public string EmailParticipante
		{
			get { return emailParticipante; }
			set
			{
				if (emailParticipante == value)
					return;

				emailParticipante = value;
				OnPropertyChanged("EmailParticipante");
			}
		}

		public string TelParticipante
		{
			get { return telParticipante; }
			set
			{
				if (telParticipante == value)
					return;

				telParticipante = value;
				OnPropertyChanged("TelParticipante");
			}
		}

		public string EmpresaParticipante
		{
			get { return empresaParticipante; }
			set
			{
				if (empresaParticipante == value)
					return;

				empresaParticipante = value;
				OnPropertyChanged("EmpresaParticipante");
			}
		}

		public EventoPageViewModel(ContentPage page)
		{
			this.page = page;
			ws = WSUtil.Instance;

			pesquisador = Utils.ObterPesquisadorLogado();

            Title = pesquisador.razaosocial;
            SubTitle = pesquisador.nome;

			IsRunning = false;

			CmdInformarParticipante = new Command(() => {
				InformarParticipante();
            });

			CmdCadastrarParticipante = new Command(() => {
				CadastrarParticipante();
            });
		}

		private async void InformarParticipante()
		{
			try
			{
				var scanner = new ZXing.Mobile.MobileBarcodeScanner();
				var result = await scanner.Scan();

				if (result != null)
				{
					String[] dados = result.ToString().Split(';');

					NomeParticipante = dados[1];
					EmailParticipante = dados[2];
					TelParticipante = dados[3];
					EmpresaParticipante = dados[4];

					IsRunning = true;

					String sql = "insert into tb_participante02 (idcliente01, idparticipante01, dtpresenca) values (" + pesquisador.idcliente + ", " + dados[0] + " ,'" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now) + "')";

					await new DadosPesquisaUtil().EnviarSQL(sql, 0);

					await this.page.DisplayAlert("Sucesso", "Participante registrado com sucesso.", "Ok");
				}
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

		private async void CadastrarParticipante()
		{
			await this.page.Navigation.PushAsync(new CadastroPartipantePage());
		}

		public async void Sair()
		{
			bool confirmacao = await this.page.DisplayAlert("Confirmação", "Deseja realmente sair?", "Sim", "Não");

			if (confirmacao)
			{
				pesquisador.logado = 0;
				DAO_Pesquisa08 dao08 = DAO_Pesquisa08.Instance;
				dao08.AtualizarPesquisador(pesquisador);

				//this.page.Navigation.InsertPageBefore(new LoginPage(), this.page);
				await this.page.Navigation.PopAsync();
			}

		}

		private async void DownloadDados()
		{
			try
			{
				bool isOnline = Utils.IsOnline();

				if (!isOnline)
					throw new Exception("Não há conexão disponível.");

				IsRunning = true;

				await new DadosPesquisaUtil().Download();

				await this.page.DisplayAlert("Sucesso", "Dados baixados com sucesso.", "Ok");
			}
			catch (Exception ex)
			{
				await this.page.DisplayAlert("Aviso", ex.Message, "Ok");
			}
			finally
			{
				//ObterPesquisas();

				IsRunning = false;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

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

		private void OnPropertyChanged(String nome)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(nome));
		}
	
	}
}
