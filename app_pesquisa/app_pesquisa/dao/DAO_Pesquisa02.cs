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

    public class DAO_Pesquisa02
    {
        private SQLiteConnection conn;
        private static DAO_Pesquisa02 instance;

        public DAO_Pesquisa02()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Pesquisa02>();
            conn.CreateTable<CE_Pesquisa02>();
        }

        public static DAO_Pesquisa02 Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Pesquisa02();

                return instance;
            }
        }

        public List<CE_Pesquisa02> ObterTipos()
        {
            return conn.Query<CE_Pesquisa02>("SELECT * FROM [tb_pesquisa02]");
        }

        public CE_Pesquisa02 ObterTipo(Int32 idpesquisa02)
        {
            return conn.Query<CE_Pesquisa02>("SELECT * FROM [tb_pesquisa02] WHERE [idpesquisa02] = " + idpesquisa02).FirstOrDefault();
        }

        public void InserirTipo(CE_Pesquisa02 tipo)
        {
            conn.Insert(tipo);
        }

        public void AtualizarTipo(CE_Pesquisa02 tipo)
        {
            conn.Update(tipo);
        }

        public void SalvarTipo(CE_Pesquisa02 tipo)
        {
            if (tipo.idpesquisa02 == 0)
                conn.Insert(tipo);
            else
                conn.Update(tipo);
        }

        public Int32 DeleteTipo(Int32 id)
        {
            return conn.Delete<CE_Pesquisa02>(id);
        }

        public void DeleteAll()
        {
            conn.DeleteAll<CE_Pesquisa02>();
        }
    }
}
