using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace app_pesquisa.util
{
    public class WSUtil
    {
        //public const String URL = "http://192.168.56.1:8080/webapi/services/";
        //public const String URL = "http://10.54.0.29:8080/ws_pesquisa/webapi/services/";
        //public const String URL = "http://10.54.0.17:8080/webapi/services/";
        //public const String URL = "http://iconsti.com/ws_pesquisa/webapi/services/";
        //public const String URL = "http://pesquisaam.com/ws_pesquisa/webapi/services/";
		public const String URL = "http://pesquisa.iconsti.com/ws_pesquisa/webapi/services/";
    	//public const String URL = "http://10.54.0.18:8080/ws_pesquisa/webapi/services/";

        private static WSUtil instance;
        private HttpClient client;

        public WSUtil()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 5000000;
        }

        public static WSUtil Instance
        {
            get
            {
                if (instance == null)
                    instance = new WSUtil();

                return instance;
            }
        }
        
        public async Task<HttpResponseMessage> Post(String metodo, Object objeto)
        {
            var uri = new Uri(URL + metodo);

            var json = JsonConvert.SerializeObject(objeto);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            //String resp = await response.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<String> Get(String metodo)
        {
            var uri = new Uri(URL + metodo);

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                String content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
                return null;
        }
    }
}
