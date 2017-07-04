using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.model
{
    public class Configuracao
    {
        public String Descricao { get; set; }
        public String Key { get; set; }
        public String Tipo { get; set; }
        public Object Valor { get; set; }
        public String Img { get; set; }
        public String EnderecoServidor { get; set; }
        public float PercentualMaximoGrafico { get; set; }

        public Configuracao()
        {

        }

        public Configuracao(String descricao, String key, String tipo, Object valor, String img)
        {
            this.Descricao = descricao;
            this.Key = key;
            this.Tipo = tipo;
            this.Valor = valor;
            this.Img = img;
        }
    }
}
