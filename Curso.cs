using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    public class Curso
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public int Vagas { get; set; }
        public List<Candidato> Selecionados { get; set; } = new List<Candidato>();
        public FilaLinear FilaDeEspera { get; set; } = new FilaLinear(10);

        public double NotaDeCorte => Selecionados.Any() ? Selecionados.Min(c => c.CalcularMedia()) : 0.0;
    }
}

