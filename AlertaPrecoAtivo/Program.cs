using System;
using System.Threading.Tasks;
using AV_api;

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
            string symbol = args[0];
            try
            {
                double compra = Convert.ToDouble(args[1]);
                double venda = Convert.ToDouble(args[2]);
                var stock = new AlphaVantageAPI(symbol, compra, venda, "ZQ46C6IRILKNDBH7");
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
