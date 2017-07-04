using app_pesquisa.dao;
using app_pesquisa.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.util
{
	public class DadosPesquisaUtil
	{
		private DAO_Pesquisa06 dao06;
		private DAO_Pesquisa01 dao01;
		private DAO_Pesquisa04 dao04;
		private DAO_Pesquisa02 dao02;
		private DAO_Pesquisa03 dao03;
		private DAO_Pesquisa07 dao07;
		private DAO_Formulario daoForm;

		private List<CE_Pesquisa06> listPesquisas;
		private List<CE_Pesquisa04> listPerguntas;

		private WSUtil ws;

		public DadosPesquisaUtil()
		{
			dao01 = DAO_Pesquisa01.Instance;
			dao06 = DAO_Pesquisa06.Instance;
			dao02 = DAO_Pesquisa02.Instance;
			dao03 = DAO_Pesquisa03.Instance;
			dao04 = DAO_Pesquisa04.Instance;
			dao07 = DAO_Pesquisa07.Instance;
			daoForm = DAO_Formulario.Instance;

			listPesquisas = new List<CE_Pesquisa06>();
			listPerguntas = new List<CE_Pesquisa04>();

			ws = WSUtil.Instance;
		}

		public async Task ObterDadosPerguntas(Int32 idpesquisa01)
		{
			WSUtil ws = WSUtil.Instance;

			JObject obj = new JObject();
			obj["idpesquisa01"] = idpesquisa01;
			obj["data"] = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);

			HttpResponseMessage resposta = await ws.Post("obterItensFormulario", obj);

			String message = await resposta.Content.ReadAsStringAsync();

			if (resposta.IsSuccessStatusCode)
			{
				ListPesquisa04 listPesquisa04 = JsonConvert.DeserializeObject<ListPesquisa04>(message);

				this.listPerguntas.AddRange(listPesquisa04.itensFormulario);

				listPesquisa04 = null;
			}
			else
			{
				throw new Exception(message);
			}

		}

		public async Task Download()
		{
			try
			{
				JObject obj = new JObject();
				obj["data"] = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);

				HttpResponseMessage resposta = await ws.Post("obterPesquisas", obj);

				String message = await resposta.Content.ReadAsStringAsync();

				if (resposta.IsSuccessStatusCode)
				{
					ListPesquisas listPesquisas = JsonConvert.DeserializeObject<ListPesquisas>(message);

					this.listPesquisas.AddRange(listPesquisas.pesquisas);

					var groupPesquisas = listPesquisas.pesquisas.GroupBy(o => o.pesquisa01.idpesquisa01);

					foreach (var grpPesq in groupPesquisas)
					{
						foreach (var pesq in grpPesq)
						{
							await ObterDadosPerguntas(pesq.pesquisa01.idpesquisa01);
						}
					}

					dao06.DeleteAll();
					dao01.DeleteAll();
					dao03.DeleteAll();
					dao04.DeleteAll();
					dao02.DeleteAll();

					var group01 = this.listPesquisas.GroupBy(o => o.pesquisa01.idpesquisa01);

					foreach (var grp1 in group01)
					{
						var pq01 = this.listPesquisas.FirstOrDefault(o => o.pesquisa01.idpesquisa01 == grp1.Key).pesquisa01;
						dao01.InserirPesquisa(pq01);
					}

					var group06 = listPesquisas.pesquisas.GroupBy(o => o.idpesquisa06);

					foreach (var grp6 in group06)
					{
						var onda = this.listPesquisas.FirstOrDefault(o => o.idpesquisa06 == grp6.Key);
						onda.idpesquisa01 = onda.pesquisa01.idpesquisa01;
						dao06.InserirOnda(onda);
					}

					var group02 = this.listPerguntas.Where(w => w.pesquisa02.idpesquisa02 != 0).GroupBy(o => o.pesquisa02.idpesquisa02);

					foreach (var grp2 in group02)
					{
						var tipo = this.listPerguntas.Where(w => w.pesquisa02.idpesquisa02 != 0).FirstOrDefault(o => o.pesquisa02.idpesquisa02 == grp2.Key).pesquisa02;
						dao02.InserirTipo(tipo);
					}

					var group02outros = this.listPerguntas.Where(w => w.pesquisa02outros != null && w.pesquisa02outros.idpesquisa02 != 0).GroupBy(o => o.pesquisa02outros.idpesquisa02);

					foreach (var grp2outros in group02outros)
					{
						var tipo = this.listPerguntas.Where(w => w.pesquisa02outros != null && w.pesquisa02outros.idpesquisa02 != 0).FirstOrDefault(o => o.pesquisa02outros.idpesquisa02 == grp2outros.Key).pesquisa02outros;

						var tipooutros = group02.FirstOrDefault(o => o.Key == grp2outros.Key);

						if (tipooutros == null)
							dao02.InserirTipo(tipo);
					}

					var group03 = this.listPerguntas.Where(w => w.pesquisa02.idpesquisa02 != 0 && w.pesquisa03.idpesquisa03 != 0).GroupBy(o => o.pesquisa03.idpesquisa03);

					foreach (var grp3 in group03)
					{
						var valor = this.listPerguntas.Where(w => w.pesquisa02.idpesquisa02 != 0 && w.pesquisa03.idpesquisa03 != 0).FirstOrDefault(o => o.pesquisa03.idpesquisa03 == grp3.Key);
						valor.pesquisa03.idpesquisa02 = valor.pesquisa03.pesquisa02.idpesquisa02;
						dao03.InserirValor(valor.pesquisa03);
					}

					/*var group03outros = this.listPerguntas.Where(w => w.pesquisa02.idpesquisa02 != 0 && w.pesquisa02outros != null && w.pesquisa02outros.idpesquisa02 != 0 && w.pesquisa03.idpesquisa03 != 0).GroupBy(o => o.pesquisa03.idpesquisa03);

					foreach (var grp3outros in group03outros)
					{
						
						var valor = this.listPerguntas.Where(w => w.pesquisa02.idpesquisa02 != 0 && w.pesquisa02outros != null && w.pesquisa02outros.idpesquisa02 != 0 && w.pesquisa03.idpesquisa03 != 0).FirstOrDefault(o => o.pesquisa03.idpesquisa03 == grp3outros.Key);
						valor.pesquisa03.idpesquisa02 = valor.pesquisa02outros.idpesquisa02;

						var groupoutros = group03.FirstOrDefault(o => o.Key == grp3outros.Key);

						if (groupoutros == null)
							dao03.InserirValor(valor.pesquisa03);

					}*/

					var group04 = this.listPerguntas.Where(w => w.idpesquisa04 != 0).GroupBy(o => o.idpesquisa04);

					foreach (var grp4 in group04)
					{
						var pergunta = this.listPerguntas.FirstOrDefault(o => o.idpesquisa04 == grp4.Key);
						pergunta.idpesquisa02 = pergunta.pesquisa02.idpesquisa02;
						if (pergunta.pesquisa02outros != null)
							pergunta.idpesquisa02outros = pergunta.pesquisa02outros.idpesquisa02;
						dao04.InserirPergunta(pergunta);
					}

					listPesquisas = null;
					this.listPesquisas = null;
					this.listPerguntas = null;
				}
				else
				{
					throw new Exception(message);
				}

			}
			catch (Exception e)
			{
				throw new Exception("Não foi possível efetuar o download dos dados.");
			}

		}

		public async Task<Int32> EnviarSQL(String sql, int tipo)
		{
			JObject jSQL = new JObject();
			jSQL["sql"] = sql;
			jSQL["tipo"] = tipo;

			HttpResponseMessage resposta = await ws.Post("executarSQL", jSQL);

			String message = await resposta.Content.ReadAsStringAsync();

			if (!resposta.IsSuccessStatusCode)
			{
				throw new Exception(message);
			}
			else
			{
                JObject obj = JsonConvert.DeserializeObject<JObject>(message);
				return Convert.ToInt32(obj["retorno"].ToString());
			}
		}

		public async Task<String> EnviarParticipante(String nome, String email, String telefone, String empresa, String infoAdicional)
		{
			JObject jParticipante = new JObject();
			jParticipante["nome"] = nome;
			jParticipante["email"] = email;
			jParticipante["telefone"] = telefone;
			jParticipante["empresa"] = empresa;

			if (!String.IsNullOrEmpty(infoAdicional))
				jParticipante["infoadicional"] = infoAdicional;

			HttpResponseMessage resposta = await ws.Post("salvarParticipante", jParticipante);

			String message = await resposta.Content.ReadAsStringAsync();
                    
            if (resposta.IsSuccessStatusCode)
            {
				return "Participante salvo com sucesso.";
            }
            else
                throw new Exception("Não foi possível salvar o participante.");
		}

        public async Task<Int32> Upload()
        {
            try
            {
                List<CE_Formulario> formularios = daoForm.ObterFormulariosFinalizados();

                List<CE_Pesquisa07> respostas = new List<CE_Pesquisa07>();

                foreach (var formulario in formularios)
                {
                    respostas.AddRange(dao07.ObterRespostasPorFormulario(formulario.codigoformulario));
                }
                
                if (respostas.Count > 0)
                {
                    JObject objEnvio = new JObject();
                    JArray jRespostas = new JArray();

                    foreach (var item in respostas)
                    {
                        JObject jItem = new JObject();
                        jItem["idpesquisa06"] = item.idpesquisa06;
                        jItem["idpesquisa04"] = item.idpesquisa04;
                        jItem["vlresposta"] = item.vlresposta;
						jItem["vlrespostaoutros"] = item.vlrespostaoutros;
                        if (item.txresposta != null)
                            jItem["txresposta"] = item.txresposta;
						if (item.txrespostaoutros != null)
							jItem["txrespostaoutros"] = item.txrespostaoutros;
                        jItem["chavepesquisa"] = item.chavepesquisa;
                        jItem["idpesquisador"] = item.idpesquisador;
                        jItem["idcliente"] = item.idcliente;
						jItem["idpesquisa03"] = item.idpesquisa03;
                        jRespostas.Add(jItem);
                        
                    }

                    objEnvio["respostas"] = jRespostas;

                    HttpResponseMessage resposta = await ws.Post("enviarRespostas", objEnvio);

					String message = await resposta.Content.ReadAsStringAsync();
                    
                    if (resposta.IsSuccessStatusCode)
                    {                        
                        foreach (var r in respostas)
                        {
                            r.enviado = 1;
                            dao07.SalvarResposta(r);
                        }
                    }
                    else
                        throw new Exception(message);
                }

                return respostas.Count;
            }
            catch (Exception ex)
            {
				throw new Exception("Erro ao preparar para enviar os dados: " + ex.Message);
            }
        }
    }
}
