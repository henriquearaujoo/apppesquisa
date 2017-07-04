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
    public class DAO_Cliente01
    {
        private SQLiteConnection conn;
        private static DAO_Cliente01 instance;

        public DAO_Cliente01()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Cliente01>();
            conn.CreateTable<CE_Cliente01>();
        }

        public static DAO_Cliente01 Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Cliente01();

                return instance;
            }
        }

        public CE_Cliente01 ObterCliente(Int32 idcliente)
        {
            return conn.Query<CE_Cliente01>("SELECT * FROM [tb_cliente01] WHERE [idcliente] = " + idcliente).FirstOrDefault();
        }

        public void InserirCliente(CE_Cliente01 cliente)
        {
            conn.Insert(cliente);
        }

        public void AtualizarCliente(CE_Cliente01 cliente)
        {
            conn.Update(cliente);
        }

        public void SalvarPesquisa(CE_Cliente01 cliente)
        {
            if (cliente.idcliente == 0)
                InserirCliente(cliente);
            else
                AtualizarCliente(cliente);
        }

        public Int32 DeleteCliente(Int32 id)
        {
            return conn.Delete<CE_Cliente01>(id);
        }

        public void DeleteAll()
        {
            conn.DeleteAll<CE_Cliente01>();
        }
    }
}
