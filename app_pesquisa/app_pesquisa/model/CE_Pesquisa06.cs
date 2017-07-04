using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    [Table("tb_pesquisa06")]
    public class CE_Pesquisa06
    {
        [PrimaryKey, Column("idpesquisa06")]
        public Int32 idpesquisa06 { get; set; }
        public Int32 idcliente { get; set; }
        public Int32 qtamostra { get; set; }
        public String dtiniciopesquisa { get; set; }
        public String dtfimpesquisa { get; set; }
        public Int32 qtamostraporpesquisador { get; set; }

        [ForeignKey(typeof(CE_Pesquisa01))]
        public Int32 idpesquisa01 { get; set; }

        [Ignore]
        public CE_Pesquisa01 pesquisa01 { get; set; }

        public bool IsDentroDoPrazo()
        {
            return DateTime.Now <= DateTime.ParseExact(this.dtfimpesquisa, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
    
    public class ListPesquisas
    {
        public List<CE_Pesquisa06> pesquisas { get; set; }
    }
}
