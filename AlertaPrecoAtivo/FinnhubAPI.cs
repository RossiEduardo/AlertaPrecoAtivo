using System;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using EmailStuffs;
using System.Threading.Tasks;

namespace Finnhub_api
{
    public class FinnhubAPI{
        //propriedades
        public string _symbol;
        public string _myApiKey;
        private double _price;
        private double _valorVenda;
        private double _valorCompra;

        public FinnhubAPI(string symbol, double valorVenda, double valorCompra, string api_key){
            this._symbol = symbol;
            this._myApiKey = api_key;
            this._price = 0.00f;
            this._valorVenda = valorVenda;
            this._valorCompra = valorCompra;
        }

        public async Task GetPrice(){
            double current_price = 0.00f;
            //email
            //recuperando os valores do arquivo json
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("smtpconfig.json", false, true)
                .AddEnvironmentVariables();
            var config = builder.Build();
            var account = config.GetSection("EmailServerConfig").Get<EmailServerConfig>();

            while (true){
                var client = new RestClient("https://finnhub.io/api/v1/");
                var request = new RestRequest("quote", Method.Get);
                request.AddParameter("symbol", this._symbol);
                request.AddParameter("token", this._myApiKey);

                var response = client.Execute(request);

                if (response.IsSuccessful){
                    dynamic data = JObject.Parse(response.Content);
                  
                    //obtem o preço atual
                    this._price = Convert.ToDouble(data.c, CultureInfo.InvariantCulture);
                    Console.WriteLine($"{this._symbol} Current price: {this._price}");

                    //se for diferente atualiza o current_price
                    if (current_price != this._price){
                        current_price = this._price;
                        if (current_price >= this._valorVenda){
                            Console.WriteLine("O preço da acao esta acima do valor de venda");
                            Email.SendEmail(account, "valorVenda", "O preço da ação ultrapassou os " + _valorVenda + 
                                $" reais, o preço atual da ação está {current_price}, recomendamos a venda desta ação");
                        }
                        if (current_price <= this._valorCompra){
                            Console.WriteLine("O preço da acao esta abaixo do valor de compra");
                            Email.SendEmail(account, "valorCompra", "O preço da ação caiu dos " + _valorCompra + 
                                $" reais, o preço atual da ação está {current_price}, recomendamos a compra desta ação");

                        }
                    }
                }
                else {
                    Console.WriteLine("Falha para obter o preço da ação, fechando o programa...");
                    return;
                }
                //espera 1 min
                await Task.Delay(60000 * account.TimeUpdateMinutes);
            }
        }
    }
}
