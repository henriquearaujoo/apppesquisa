using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    [Table("tb_cliente01")]
    public class CE_Cliente01
    {
        [PrimaryKey, Column("idcliente")]
        public Int32 idcliente { get; set; }

        public String razaosocial { get; set; }
    }
}
