using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Models
{
    class Shabdkosh
    {
        public static List<string> GetSuggestions(string term)
        {
            //StringBuilder theWebAddress = new StringBuilder();
            //theWebAddress.Append("http://query.yahooapis.com/v1/public/yql?");
            //theWebAddress.Append("q=" + System.Web.HttpUtility.UrlEncode("select * from local.search where location='Nashville ,TN' and query='Fast Food'"));
            //theWebAddress.Append("&format=json");
            //theWebAddress.Append("&diagnostics=false");


            string shabdkoshUrl = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20html%20where%20url%3D%22http%3A%2F%2Fwww.shabdkosh.com%2Futils%2Fautocomplete.php%3Flc%3Dhi%26query%3D" + System.Web.HttpUtility.UrlEncode(term) +"%22%20&format=json";
                                
            string results = "";

            try
            {
                using (WebClient wc = new WebClient())
                {
                    results = wc.DownloadString(shabdkoshUrl);
                    var allSuggestions = JObject.Parse(JObject.Parse(results)["query"]["results"]["body"].ToString())["suggestions"]
                        .ToObject<List<string>>();

                    return allSuggestions.Where(s=> HindiProcessor.IsHindiWord(s))
                        .ToList();
                }
            }
            catch (Exception)
            {

                
            }

            
            return new List<string>();
        }

    }

         

}
