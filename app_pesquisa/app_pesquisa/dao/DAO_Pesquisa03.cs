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
    public class DAO_Pesquisa03
    {
        private SQLiteConnection conn;
        private static DAO_Pesquisa03 instance;

        public DAO_Pesquisa03()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Pesquisa06>();
            conn.CreateTable<CE_Pesquisa03>();
        }

        public static DAO_Pesquisa03 Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Pesquisa03();

                return instance;
            }
        }

        public List<CE_Pesquisa03> ObterValores(Int32 idpesquisa02)
        {
            return conn.Query<CE_Pesquisa03>("SELECT * FROM [tb_pesquisa03] WHERE [idpesquisa02] = " + idpesquisa02 + " ORDER BY [ordem]");
        }

        public void InserirValor(CE_Pesquisa03 valor)
        {
            conn.Insert(valor);
        }

        public void AtualizarValor(CE_Pesquisa03 valor)
        {
            conn.Update(valor);
        }

        public void SalvarValor(CE_Pesquisa03 valor)
        {
            if (valor.idpesquisa03 == 0)
                conn.Insert(valor);
            else
                conn.Update(valor);
        }

        public Int32 DeleteValor(Int32 id)
        {
            return conn.Delete<CE_Pesquisa03>(id);
        }

        public void DeleteAll()
        {
            conn.DeleteAll<CE_Pesquisa03>();
        }

        public List<CE_Pesquisa03> obterValoresFiltrados(String filtro, int idpesquisa02)
        {
            return conn.Query<CE_Pesquisa03>("SELECT * FROM [tb_pesquisa03] WHERE [idpesquisa02] = " + idpesquisa02 + " AND [descricao] LIKE '%" + filtro + "%' ORDER BY [ordem]");
        }
    }
}
