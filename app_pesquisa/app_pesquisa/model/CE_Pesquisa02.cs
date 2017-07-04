using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    [Table("tb_pesquisa02")]
    public class CE_Pesquisa02
    {
        [PrimaryKey, Column("idpesquisa02")]
        public Int32 idpesquisa02 { get; set; }
		public Int32 idpesquisa02outros { get; set; }
        public String dscricao { get; set; }
        public String tipodado { get; set; }
    }
}
