using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    [Table("tb_formulario")]
    public class CE_Formulario
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public Int32 id { get; set; }
        public Int32 idpesquisa01 { get; set; }
        public Int32 idpesquisa06 { get; set; }
        public String codigoformulario { get; set; }
        public Int32 finalizado { get; set; }
    }
}
