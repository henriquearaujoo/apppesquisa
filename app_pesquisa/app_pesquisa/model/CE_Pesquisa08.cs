using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    [Table("tb_pesquisa08")]
    public class CE_Pesquisa08
    {
        [PrimaryKey, Column("idpesquisador")]
        public Int32 idpesquisador { get; set; }
        public String nome { get; set; }
        public String senha { get; set; }
        public Int32 logado { get; set; }
        public Int32 idcliente { get; set; }
        public String razaosocial { get; set; }
    }

    public class Pesquisador
    {
        public CE_Pesquisa08 pesquisador { get; set; }
    }
}
