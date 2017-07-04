using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace app_pesquisa.model
{
    [Table("tb_pesquisa03")]
    public class CE_Pesquisa03 : INotifyPropertyChanged
    {
        [PrimaryKey, Column("idpesquisa03")]
        public Int32 idpesquisa03 { get; set; }
        public String retornopesquisa { get; set; }
        public Int32 ordem { get; set; }
		public Int32 campotipooutros { get; set; }

        [ForeignKey(typeof(CE_Pesquisa02))]
        public Int32 idpesquisa02 { get; set; }

        [Ignore]
        public CE_Pesquisa02 pesquisa02 { get; set; }

		private String desc;

		public String descricao 
		{ 
			get 
			{
				return desc;
			}
			set 
			{
				if (value != desc)
				{
					desc = value;
					OnPropertyChanged("descricao");
				}
			} 
		}

		private Boolean selecionado = false;

		[Ignore]
		public Boolean IsSelecionado 
		{ 
			get 
			{
				return selecionado;
			}
			set
			{
				if (value != selecionado)
                {
                    selecionado = value;
                    OnPropertyChanged("IsSelecionado");
                }
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
        
        public override String ToString()
        {
            return descricao;
        }

		private void OnPropertyChanged(String nome)
		{
			if (PropertyChanged != null)
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nome));
		}
    }
}
