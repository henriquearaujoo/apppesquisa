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
    public class DAO_Pesquisa08
    {
        private SQLiteConnection conn;
        private static DAO_Pesquisa08 instance;

        public DAO_Pesquisa08()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Pesquisa08>();
            conn.CreateTable<CE_Pesquisa08>();
        }

        public static DAO_Pesquisa08 Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Pesquisa08();

                return instance;
            }
        }
        
        public CE_Pesquisa08 ObterPesquisadorLogado()
        {
            return conn.Query<CE_Pesquisa08>("SELECT * FROM [tb_pesquisa08] WHERE [logado] = 1").FirstOrDefault();
        }

        public CE_Pesquisa08 ObterPesquisador(Int32 idpesquisador, String senha)
        {
            return conn.Query<CE_Pesquisa08>("SELECT * FROM [tb_pesquisa08] WHERE [idpesquisador] = " + idpesquisador + " AND [senha] = '" + senha + "'").FirstOrDefault();
        }

        public CE_Pesquisa08 ObterPesquisador(Int32 idpesquisador)
        {
            return conn.Find<CE_Pesquisa08>(idpesquisador);
        }

        public void InserirPesquisador(CE_Pesquisa08 pesquisador)
        {
            conn.Insert(pesquisador);
        }

        public void AtualizarPesquisador(CE_Pesquisa08 pesquisador)
        {
            conn.Update(pesquisador);
        }

        public void SalvarPesquisa(CE_Pesquisa08 pesquisador)
        {
            if (pesquisador.idpesquisador == 0)
                InserirPesquisador(pesquisador);
            else
                AtualizarPesquisador(pesquisador);
        }

        public Int32 DeletePesquisador(Int32 id)
        {
            return conn.Delete<CE_Pesquisa08>(id);
        }

        public void DeleteAll()
        {
            conn.DeleteAll<CE_Pesquisa08>();
        }
    }
}
