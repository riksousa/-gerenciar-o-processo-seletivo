using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    public class Candidato
    {
        public string Nome { get; set; }
        public double NotaRedacao { get; set; }
        public double NotaMatematica { get; set; }
        public double NotaLinguagens { get; set; }
        public int CodigoPrimeiraOpcao { get; set; }
        public int CodigoSegundaOpcao { get; set; }

        public double CalcularMedia()
        {
            return (NotaRedacao + NotaMatematica + NotaLinguagens) / 3.0;
        }
    }
}
