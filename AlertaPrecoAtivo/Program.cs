using System;
using System.Threading.Tasks;
using Finnhub_api;
using System.Globalization;

namespace AlertaPrecoAtivo
{
    class Program {
        static async Task<int> Main(string[] args) {
            if(args.Length < 3){
                Console.WriteLine("Execute o programa da seguinte forma:" +
                    " AlertaPrecoAtivo.exe <COD_ACAO> <VALOR_VENDA> <VALOR_COMPRA>");
                return 1;
            }

            //tratando erros de input
            string symbol = args[0].ToUpper();
            
            try
            {
                double compra = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);
                double venda = Convert.ToDouble(args[2], CultureInfo.InvariantCulture);
                if(compra == venda)
                {
                    Console.WriteLine("Os valores de compra e venda devem ser diferentes");
                    return 1;
                }
                        
                string myApiKey = "SUA_API_KEY_DA_FINNHUB";

                var stock = new FinnhubAPI(symbol, compra, venda, myApiKey);
                await stock.GetPrice();
            }
            catch{
                Console.WriteLine("O valor informado para compra e venda não estão no formato correto, digite um número");
                return 1;
            }

            return 0;
        }
    }
}
