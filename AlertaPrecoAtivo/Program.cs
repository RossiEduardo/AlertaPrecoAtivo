using System;
using System.Threading.Tasks;
using AV_api;

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
}
