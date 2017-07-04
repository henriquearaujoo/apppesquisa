using app_pesquisa.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.interfaces
{
    public interface IUtils
    {
        Boolean IsOnline();

        String ObterIMEI();

        Configuracao ObterConfiguracao();

        void InserirConfiguracaoInicial(bool verificarExiste);
        void SalvarConfiguracao(Configuracao configuracao);
		void CompartilharCode(String dados);
    }
}
