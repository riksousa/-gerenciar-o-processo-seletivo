using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    using System.Globalization;
    using System.IO;

    public class GerenciadorProcessoSeletivo
    {
        private Dictionary<int, Curso> cursos = new Dictionary<int, Curso>();
        private List<Candidato> candidatos = new List<Candidato>();

        public void LerArquivoEntrada(string caminho)
        {
            if (!File.Exists(caminho))
            {
                Console.WriteLine("Erro: O arquivo especificado não foi encontrado.");
                return;
            }
         
            var linhas = File.ReadAllLines(caminho);
        
            if (linhas.Length == 0)
            {
                Console.WriteLine("Erro: O arquivo está vazio.");
                return;
            }
        
            var primeiraLinha = linhas[0].Split(';');
        
            if (primeiraLinha.Length < 2)
            {
                Console.WriteLine("Erro: A primeira linha do arquivo não contém os dados esperados.");
                return;
            }


            int qtdCursos = int.Parse(primeiraLinha[0]);
            int qtdCandidatos = int.Parse(primeiraLinha[1]);

            for (int i = 1; i <= qtdCursos; i++)
            {
                if (i >= linhas.Length)
                {
                    Console.WriteLine($"Erro: Linha do curso {i} não encontrada.");
                    break;
                }

                var dadosCurso = linhas[i].Split(';');
                if (dadosCurso.Length < 3)
                {
                    Console.WriteLine($"Erro: Linha {i + 1} do curso está mal formatada.");
                    continue;
                }

                try
                {
                    var curso = new Curso
                    {
                        Codigo = int.Parse(dadosCurso[0]),
                        Nome = dadosCurso[1],
                        Vagas = int.Parse(dadosCurso[2])
                    };
                    cursos.Add(curso.Codigo, curso);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar linha {i + 1} do curso: {ex.Message}");
                }
            }

            for (int i = qtdCursos + 1; i < linhas.Length; i++)
            {
                var dadosCandidato = linhas[i].Split(';');
                if (dadosCandidato.Length < 6)
                {
                    Console.WriteLine($"Erro: Linha {i + 1} do candidato está mal formatada.");
                    continue;
                }

                try
                {
                    var candidato = new Candidato
                    {
                        Nome = dadosCandidato[0],
                        NotaRedacao = double.Parse(dadosCandidato[1], CultureInfo.InvariantCulture),
                        NotaMatematica = double.Parse(dadosCandidato[2], CultureInfo.InvariantCulture),
                        NotaLinguagens = double.Parse(dadosCandidato[3], CultureInfo.InvariantCulture),
                        CodigoPrimeiraOpcao = int.Parse(dadosCandidato[4]),
                        CodigoSegundaOpcao = int.Parse(dadosCandidato[5])
                    };
                    candidatos.Add(candidato);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Erro de formatação na linha {i + 1}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro inesperado na linha {i + 1}: {ex.Message}");
                }
            }

            Console.WriteLine("Processamento de arquivo concluído.");
        }
    

    public void ProcessarSelecao()
        {
            candidatos = candidatos.OrderByDescending(c => c.CalcularMedia())
                                   .ThenByDescending(c => c.NotaRedacao)
                                   .ThenByDescending(c => c.NotaMatematica)
                                   .ToList();

            foreach (var candidato in candidatos)
            {
                if (TentarInserirNoCurso(candidato, candidato.CodigoPrimeiraOpcao))
                    continue;

                if (TentarInserirNoCurso(candidato, candidato.CodigoSegundaOpcao))
                {
                    cursos[candidato.CodigoPrimeiraOpcao].FilaDeEspera.Enfileirar(candidato);
                    continue;
                }

                cursos[candidato.CodigoPrimeiraOpcao].FilaDeEspera.Enfileirar(candidato);
                cursos[candidato.CodigoSegundaOpcao].FilaDeEspera.Enfileirar(candidato);
            }
        }

        private bool TentarInserirNoCurso(Candidato candidato, int codigoCurso)
        {
            if (cursos[codigoCurso].Selecionados.Count < cursos[codigoCurso].Vagas)
            {
                cursos[codigoCurso].Selecionados.Add(candidato);
                return true;
            }
            return false;
        }

        public void GerarArquivoSaida(string caminho)
        {
            using (var writer = new StreamWriter(caminho))
            {
                foreach (var curso in cursos.Values)
                {
                    writer.WriteLine($"{curso.Nome} {curso.NotaDeCorte:F2}");
                    writer.WriteLine("Selecionados");

                    foreach (var selecionado in curso.Selecionados.OrderByDescending(c => c.CalcularMedia())
                                                                  .ThenByDescending(c => c.NotaRedacao)
                                                                  .ThenByDescending(c => c.NotaMatematica))
                    {
                        writer.WriteLine($"{selecionado.Nome} {selecionado.CalcularMedia():F2} {selecionado.NotaRedacao} {selecionado.NotaMatematica} {selecionado.NotaLinguagens}");
                    }

                    writer.WriteLine("Fila de Espera");

                    foreach (var espera in curso.FilaDeEspera.ObterTodos().OrderByDescending(c => c.CalcularMedia())
                                                             .ThenByDescending(c => c.NotaRedacao)
                                                             .ThenByDescending(c => c.NotaMatematica))
                    {
                        writer.WriteLine($"{espera.Nome} {espera.CalcularMedia():F2} {espera.NotaRedacao} {espera.NotaMatematica} {espera.NotaLinguagens}");
                    }

                    writer.WriteLine();
                }
            }
        }
    }
}
