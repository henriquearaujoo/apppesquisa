using app_pesquisa.dao;
using app_pesquisa.interfaces;
using app_pesquisa.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

namespace app_pesquisa.util
{
    public class Utils
    {
        public static String ObterCodigoFormulario()
        {
            String imei = ObterImei();

            return imei + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        }

        public static String ObterImei()
        {
            String imei = DependencyService.Get<IUtils>().ObterIMEI();

            return imei;
        }

        public static bool IsOnline()
        {
            bool isOnline = DependencyService.Get<IUtils>().IsOnline();

            return isOnline;
        }

        public static CE_Pesquisa08 ObterPesquisadorLogado()
        {
            DAO_Pesquisa08 dao08 = DAO_Pesquisa08.Instance;
            return dao08.ObterPesquisadorLogado();
        }
                
    }
}
