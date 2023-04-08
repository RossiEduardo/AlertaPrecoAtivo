using System;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using EmailStuffs;
using System.Threading.Tasks;


namespace AlertaPrecoAtivo
{
    class Program {
        static async Task Main(string[] args) {
            Console.WriteLine("hello world");
            string symbol = Console.ReadLine();
            double gain = Convert.ToDouble(Console.ReadLine());
            double loss = Convert.ToDouble(Console.ReadLine());

            var stock = new AlphaVantageAPI(symbol, gain, loss, "ZQ46C6IRILKNDBH7");
            await stock.GetPrice();

           
        }
    }

    public class AlphaVantageAPI {
        //propriedades
        public string _symbol;
        public string _myApiKey;
        private double _price;
        private double _gain;
        private double _loss;

        public AlphaVantageAPI(string symbol, double gain, double loss, string api_key) {
            this._symbol = symbol;
            this._myApiKey = api_key;
            this._price = 0.00f;
            this._gain = gain;
            this._loss = loss;
        }

        public async Task GetPrice() {
            double current_price = 0.00f;
            //email
            //recuperando os valores do arquivo json
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("smtpconfig.json", false, true)
                .AddEnvironmentVariables();
            var config = builder.Build();
            var account = config.GetSection("EmailServerConfig").Get<EmailServerConfig>();

            while (true){
                Console.WriteLine("oi");
                var client = new RestClient("https://www.alphavantage.co");
                var request = new RestRequest($"query?function=GLOBAL_QUOTE&symbol={_symbol}&apikey={_myApiKey}", Method.Get);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    dynamic data = JObject.Parse(response.Content);
                    string price = data["Global Quote"]["05. price"];
                    this._price = Convert.ToDouble(price, CultureInfo.InvariantCulture);

                    //se for diferente atualiza o current_price
                    if (current_price != this._price)
                    {
                        current_price = this._price;
                        Console.WriteLine(current_price);

                        if(current_price >= this._gain)
                            Email.SendEmail(account, $"eduardorossi80@hotmail.com", "Gain", "O preço da ação ultrapassou" + _gain + "reais");

                        if (current_price <= this._loss)
                            Email.SendEmail(account, $"eduardorossi80@hotmail.com", "Gain", "O preço da ação ultrapassou" + _loss + "reais");
                    }
                }
                //espera 5 min
                await Task.Delay(300);
            }
        }
    }
}
