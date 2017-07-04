using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    [Table("tb_pesquisa01")]
    public class CE_Pesquisa01
    {
        [PrimaryKey, Column("idpesquisa01")]
        public Int32 idpesquisa01 { get; set; }
        public String dtinicio { get; set; }
        public String dtfim { get; set; }
        public String nomepesquisa { get; set; }

        public String DSDATACONSOLIDADA
        {
            get { return this.dtinicio + " - " + this.dtfim; }
        }
    }
}
