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
        static async Task<int> Main(string[] args) {
            if(args.Length < 2)
            {
                Console.WriteLine("Execute o programa da seguinte forma: AlertaPrecoAtivo.exe COD_ACAO GAIN LOSS");
                return 1;
            }
            string symbol = args[0];
            double gain = Convert.ToDouble(args[1]);
            double loss = Convert.ToDouble(args[2]);

            var stock = new AlphaVantageAPI(symbol, gain, loss, "ZQ46C6IRILKNDBH7");
            await stock.GetPrice();

            return 0;
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
                var client = new RestClient("https://www.alphavantage.co");
                var request = new RestRequest($"query?function=GLOBAL_QUOTE&symbol={_symbol}&apikey={_myApiKey}", Method.Get);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    dynamic data = JObject.Parse(response.Content);
                    string price = data["Global Quote"]["05. price"];
                    this._price = Convert.ToDouble(price, CultureInfo.InvariantCulture);
                    Console.WriteLine(this._price);

                    //se for diferente atualiza o current_price
                    if (current_price != this._price)
                    {
                        current_price = this._price;
                        if(current_price >= this._gain)
                            Email.SendEmail(account, $"eduardorossi80@hotmail.com", "GAIN", "O preço da ação ultrapassou os " + _gain + " reais");

                        if (current_price <= this._loss)
                            Email.SendEmail(account, $"eduardorossi80@hotmail.com", "LOSS", "O preço da ação caiu os " + _loss + " reais");
                    }
                }
                //espera 1 min
                await Task.Delay(60000);
            }
        }
    }
}
