using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var gerenciador = new GerenciadorProcessoSeletivo();
            string caminhoEntrada = @"C:\Users\RiKBaladinha\Desktop\TP_AED_Manha (1)\aeds trabalho\ConsoleApp1\ConsoleApp1\bin\Debug\entrada.txt";
            string caminhoSaida = "saida.txt";

            gerenciador.LerArquivoEntrada(caminhoEntrada);
            gerenciador.ProcessarSelecao();
            gerenciador.GerarArquivoSaida(caminhoSaida);

            Console.WriteLine("Processamento concluído. Arquivo de saída gerado.");
        }
    }
}
