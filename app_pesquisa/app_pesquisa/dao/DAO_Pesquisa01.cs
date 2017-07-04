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
    public class DAO_Pesquisa01
    {
        private SQLiteConnection conn;
        private static DAO_Pesquisa01 instance;

        public DAO_Pesquisa01()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Pesquisa01>();
            conn.CreateTable<CE_Pesquisa01>();
        }

        public static DAO_Pesquisa01 Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Pesquisa01();

                return instance;
            }
        }

        public List<CE_Pesquisa01> ObterPesquisas()
        {
            return conn.Query<CE_Pesquisa01>("SELECT * FROM [tb_pesquisa01]");
        }

        public CE_Pesquisa01 ObterPesquisa(Int32 idpesquisa01)
        {
            return conn.Query<CE_Pesquisa01>("SELECT * FROM [tb_pesquisa01] WHERE [idpesquisa01] = " + idpesquisa01).FirstOrDefault();
        }
        
        public void InserirPesquisa(CE_Pesquisa01 pesquisa)
        {
            conn.Insert(pesquisa);
        }

        public void AtualizarPesquisa(CE_Pesquisa01 pesquisa)
        {
            conn.Update(pesquisa);
        }

        public void SalvarPesquisa(CE_Pesquisa01 pesquisa)
        {
            if (pesquisa.idpesquisa01 == 0)
                InserirPesquisa(pesquisa);
            else
                AtualizarPesquisa(pesquisa);
        }
        
        public Int32 DeletePesquisa(Int32 id)
        {
            return conn.Delete<CE_Pesquisa01>(id);
        }

        public void DeleteAll()
        {
            conn.DeleteAll<CE_Pesquisa01>();
        }
    }
}
