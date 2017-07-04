using app_pesquisa.interfaces;
using app_pesquisa.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.dao
{
    public class DAO_Pesquisa07
    {
        private SQLite.SQLiteConnection conn;
        private static DAO_Pesquisa07 instance;

        public DAO_Pesquisa07()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Pesquisa07>();
            conn.CreateTable<CE_Pesquisa07>();
        }

        public static DAO_Pesquisa07 Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Pesquisa07();

                return instance;
            }
        }

        public List<CE_Pesquisa07> ObterRespostas()
        {
            return (from i in conn.Table<CE_Pesquisa07>() select i).ToList();
        }

        public List<CE_Pesquisa07> ObterRespostaPorPergunta(Int32 idpesquisa04, String codigo)
        {
            return conn.Query<CE_Pesquisa07>("SELECT * FROM [tb_pesquisa07] WHERE [idpesquisa04] = " + idpesquisa04 + " AND [chavepesquisa] = '" + codigo + "'");
        }

        public List<CE_Pesquisa07> ObterRespostasPorEnvio(Int32 enviado)
        {
            return conn.Query<CE_Pesquisa07>("SELECT * FROM [tb_pesquisa07] WHERE [enviado] = " + enviado);
        }

        public List<CE_Pesquisa07> ObterRespostasPorFormulario(String codigo)
        {
            return conn.Query<CE_Pesquisa07>("SELECT * FROM [tb_pesquisa07] WHERE [enviado] = 0 AND [chavepesquisa] = '" + codigo + "'");
        }

        public bool IsRespondido(Int32 idpesquisa04, String codigo)
        {
            int count = conn.ExecuteScalar<int>("SELECT COUNT(idpesquisa07) FROM [tb_pesquisa07] where [idpesquisa04] = " + idpesquisa04 + " AND [chavepesquisa] = '" + codigo + "'");

            return count > 0;
        }

        public int ObterTotalRespondidoPorPesquisa(Int32 idpesquisa06, String codigo)
        {
            int count = conn.ExecuteScalar<int>("SELECT COUNT(idpesquisa07) FROM [tb_pesquisa07] WHERE [idpesquisa06]  = " + idpesquisa06 + " AND [chavepesquisa] = '" + codigo + "'");

            return count;
        }

        public int ObterTotalFormRespondidos(Int32 idpesquisa06)
        {
            int count = conn.ExecuteScalar<int>("SELECT COUNT(DISTINCT [chavepesquisa]) FROM [tb_pesquisa07] WHERE [idpesquisa06] =  " + idpesquisa06);

            return count;
        }

        public void SalvarResposta(CE_Pesquisa07 resposta)
        {
            if (resposta.idpesquisa07 == 0)
                conn.Insert(resposta);
            else
                conn.Update(resposta);
        }

        public CE_Pesquisa07 ObterResposta(Int32 id)
        {
            return conn.Table<CE_Pesquisa07>().FirstOrDefault(x => x.idpesquisa07 == id);
        }
        public Int32 DeleteResposta(Int32 id)
        {
            return conn.Delete<CE_Pesquisa07>(id);
        }
    }
}
