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
    public class DAO_Formulario
    {
        private SQLiteConnection conn;
        private static DAO_Formulario instance;

        public DAO_Formulario()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CE_Formulario>();
            conn.CreateTable<CE_Formulario>();
        }

        public static DAO_Formulario Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO_Formulario();

                return instance;
            }
        }

        public List<CE_Formulario> ObterFormularios()
        {
            return conn.Query<CE_Formulario>("SELECT * FROM [tb_formulario]");
        }

        public List<CE_Formulario> ObterFormulariosFinalizados()
        {
            return conn.Query<CE_Formulario>("SELECT * FROM [tb_formulario] WHERE [finalizado] = 1");
        }

        public CE_Formulario ObterUltimoFormulario(Int32 idpesquisa01)
        {
            return conn.Query<CE_Formulario>("SELECT * FROM [tb_formulario] WHERE [finalizado] = 0 AND [idpesquisa01] = " + idpesquisa01).LastOrDefault();
        }

        public void InserirFormulario(CE_Formulario formulario)
        {
            conn.Insert(formulario);
        }

        public void AtualizarFormulario(CE_Formulario formulario)
        {
            conn.Update(formulario);
        }

        public void SalvarFormulario(CE_Formulario formulario)
        {
            if (formulario.idpesquisa01 == 0)
                InserirFormulario(formulario);
            else
                AtualizarFormulario(formulario);
        }

        public Int32 DeleteFormulario(Int32 id)
        {
            return conn.Delete<CE_Formulario>(id);
        }

        public void DeleteAll()
        {
            conn.DeleteAll<CE_Formulario>();
        }
    }
}
