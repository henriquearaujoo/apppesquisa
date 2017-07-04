using app_pesquisa.dao;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    [Table("tb_pesquisa04")]
    public class CE_Pesquisa04
    {
        [PrimaryKey, Column("idpesquisa04")]
        public Int32 idpesquisa04 { get; set; }
        public Int32 numeropesquisa { get; set; }
        public String descricao { get; set; }
        public Int32 idpesquisa04pai { get; set; }
        public Int32 numeroperguntapai { get; set; }
        public Int32 tamanhoresposta { get; set; }
        public Int32 qtdecimais { get; set; }
        public String vldefault { get; set; }
        public Int32 qtrespostas { get; set; }
		public Int32 qtminrespostas { get; set; }
        public Int32 qtminobrigatoria { get; set; }
        public String dtinicio { get; set; }
        public String dtfim { get; set; }
        public Int32 ordempergunta { get; set; }
        public Int32 obrigatoria { get; set; }

        [ForeignKey(typeof(CE_Pesquisa02))]
        public Int32 idpesquisa02 { get; set; }

        [ForeignKey(typeof(CE_Pesquisa01))]
        public Int32 idpesquisa01 { get; set; }

		[ForeignKey(typeof(CE_Pesquisa02))]
		public Int32 idpesquisa02outros { get; set; }

        [Ignore]
        public CE_Pesquisa02 pesquisa02 { get; set; }

		[Ignore]
		public CE_Pesquisa02 pesquisa02outros { get; set; }

        [Ignore]
        public CE_Pesquisa03 pesquisa03 { get; set; }

        [Ignore]
        public List<CE_Pesquisa03> Opcoes { get; set; }

		[Ignore]
		public List<CE_Pesquisa03> OpcoesOutros { get; set; }
        
        public Boolean IsRespondido(String codigo)
        {
            DAO_Pesquisa07 dao = DAO_Pesquisa07.Instance;
            return dao.IsRespondido(idpesquisa04, codigo);
        }
    }

    public class ListPesquisa04
    {
        public List<CE_Pesquisa04> itensFormulario { get; set; }
    }
}
