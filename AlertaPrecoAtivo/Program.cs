using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Globalization;


namespace AlertaPrecoAtivo
{
    class Program{
        static void Main(string[] args){
            Console.WriteLine("hello world");
            string symbol = Console.ReadLine();
            var stock = new AlphaVantageAPI(symbol, "ZQ46C6IRILKNDBH7");
            Console.WriteLine(stock.getPrice());
        }
    }

    public class AlphaVantageAPI{
        //propriedades
        public string _symbol;
        public string _myApiKey;
        private float _price;

        public AlphaVantageAPI(string symbol, string api_key){
            this._symbol = symbol;
            this._myApiKey = api_key;
            this._price = 0.00f;
        }

        public float getPrice(){
            var client = new RestClient("https://www.alphavantage.co");
            var request = new RestRequest($"query?function=GLOBAL_QUOTE&symbol={_symbol}&apikey={_myApiKey}", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful){
                dynamic data = JObject.Parse(response.Content);
                string price = data["Global Quote"]["05. price"];
                Console.WriteLine($"O preço da ação {_symbol} é {price}");
                this._price = float.Parse(price, CultureInfo.InvariantCulture);
                this._price = (float)Math.Round(this._price, 2);

            }
            return this._price;
        }

    }
}
