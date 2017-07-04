using app_pesquisa.interfaces;
using app_pesquisa.model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_pesquisa.dao
{
    public class DAO_Pesquisa04
    {
        private SQLiteConnection conn;
        private static DAO_Pesquisa04 instance;

        public DAO_Pesquisa04()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Pesquisa04>();
            conn.CreateTable<CE_Pesquisa04>();
        }

        public static DAO_Pesquisa04 Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Pesquisa04();

                return instance;
            }
        }

        public List<CE_Pesquisa04> ObterPerguntas(Int32 idpesquisa01)
        {
            return conn.Query<CE_Pesquisa04>("SELECT * FROM [tb_pesquisa04] WHERE [idpesquisa01] = " + idpesquisa01);
        }

        public int ObterTotalPerguntas(Int32 idpesquisa01)
        {
            int count = conn.ExecuteScalar<int>("SELECT COUNT(idpesquisa04) FROM [tb_pesquisa04] WHERE [idpesquisa02] <> 0 AND [idpesquisa01] = " + idpesquisa01);

            return count;
        }

        public void InserirPergunta(CE_Pesquisa04 pergunta)
        {
            conn.Insert(pergunta);
        }

        public void AtualizarPergunta(CE_Pesquisa04 pergunta)
        {
            conn.Update(pergunta);
        }

        public void SalvarPergunta(CE_Pesquisa04 pergunta)
        {
            if (pergunta.idpesquisa04 == 0)
                conn.Insert(pergunta);
            else
                conn.Update(pergunta);
        }

        public Int32 DeletePergunta(Int32 id)
        {
            return conn.Delete<CE_Pesquisa04>(id);
        }

        public void DeleteAll()
        {
            conn.DeleteAll<CE_Pesquisa04>();
        }
    }
}
