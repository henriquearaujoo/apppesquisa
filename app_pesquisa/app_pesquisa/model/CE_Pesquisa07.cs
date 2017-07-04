using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    [Table("tb_pesquisa07")]
    public class CE_Pesquisa07
    {
        [PrimaryKey, AutoIncrement, Column("idpesquisa07")]
        public Int32 idpesquisa07 { get; set; }
        public Int32 idpesquisa06 { get; set; }
        public Int32 idpesquisa04 { get; set; }
		public Int32 idpesquisa03 { get; set; }
		public Int32 idpesquisa03outros { get; set; }
        public Decimal vlresposta { get; set; }
        public String txresposta { get; set; }
        public String chavepesquisa { get; set; }
        public Int32 enviado { get; set; }
        public Int32 idpesquisador { get; set; }
        public Int32 idcliente { get; set; }
		public Decimal vlrespostaoutros { get; set; }
		public String txrespostaoutros { get; set; }
		public String tipodadooutros { get; set; }

		[Ignore]
		public CE_Pesquisa03 pesquisa03 { get; set; }

		[Ignore]
		public CE_Pesquisa03 pesquisa03outros { get; set; }
    }
}
