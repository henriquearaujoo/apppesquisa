using app_pesquisa.interfaces;
using app_pesquisa.model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;

namespace app_pesquisa.dao
{
    public class DAO_Pesquisa06
    {
        private SQLiteConnection conn;
        private static DAO_Pesquisa06 instance;

        public DAO_Pesquisa06()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Pesquisa06>();
            conn.CreateTable<CE_Pesquisa06>();
        }

        public static DAO_Pesquisa06 Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Pesquisa06();

                return instance;
            }
        }

        public List<CE_Pesquisa06> ObterOndas()
        {
            return conn.Query<CE_Pesquisa06>("SELECT * FROM [tb_pesquisa06]").Where(o => DateTime.Now >= DateTime.ParseExact(o.dtiniciopesquisa, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture) && DateTime.Now <= DateTime.ParseExact(o.dtfimpesquisa, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
        }

        public void InserirOnda(CE_Pesquisa06 onda)
        {
            conn.Insert(onda);
        }

        public void AtualizarOnda(CE_Pesquisa06 onda)
        {
            conn.Update(onda);
        }

        public void SalvarOnda(CE_Pesquisa06 onda)
        {
            if (onda.idpesquisa06 == 0)
                conn.Insert(onda);
            else
                conn.Update(onda);
        }

        public Int32 DeleteOnda(Int32 id)
        {
            return conn.Delete<CE_Pesquisa06>(id);
        }

        public void DeleteAll()
        {
            conn.DeleteAll<CE_Pesquisa06>();
        }
    }
}
