using Projeto_Bytebank.Contas;
using Projeto_Bytebank.Pessoas;
using System.Numerics;

namespace Projeto_Bytebank
{
    public partial class ProgramaPrincipal
    {
        public void SalvarEmArquivo(ContaCorrente conta)    //Salva os dados de um objeto ContaCorrente no arquivo
        {
            string auxiliar = TransformaEmDados(conta);

            using (FileStream FluxoDoArquivo = new FileStream("contas.csv", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            using (StreamWriter escritor = new StreamWriter(FluxoDoArquivo))
            {
                FluxoDoArquivo.Position = FluxoDoArquivo.Length;
                escritor.WriteLine(auxiliar);
            }
        }

        ContaCorrente TransformaEmContaCorrente(string dados)   //Transforma uma linha dados em um objeto ContaCorrente
        {
            string[] dadosSeparados = dados.Split(',');
            Pessoa titular = new Pessoa(dadosSeparados[3], dadosSeparados[4], dadosSeparados[5]);
            ContaCorrente contaAuxiliar = new ContaCorrente(titular, dadosSeparados[1], dadosSeparados[0]);
            contaAuxiliar.Deposito(double.Parse(dadosSeparados[2]));
            return contaAuxiliar;
        }

        string TransformaEmDados(ContaCorrente conta)   //Transforma um objeto ContaCorrente em uma linha de dados
        {
            //Padrao usado: numDaConta, Agencia, Saldo, Cpf, Rg, nome
            string auxiliar = $"{conta.Conta},{conta.Agencia},{conta.Saldo},{conta.Titular.CPF},{conta.Titular.RG},{conta.Titular.Nome}";
            return auxiliar;
        }
        public ContaCorrente ProcurarEmArquivo(string pesquisa) //Retorna um objeto ContaCorrente a partir de uma linha de dados passada
        {
            bool found = false;
            using (FileStream FluxoDoArquivo = new FileStream("contas.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader leitor = new StreamReader(FluxoDoArquivo))
            {
                string auxiliar;
                while(!found && !leitor.EndOfStream)
                {
                    auxiliar = leitor.ReadLine();
                    string numContaLido = auxiliar.Substring(0, 6);
                    if(numContaLido == pesquisa)
                    {
                        ContaCorrente contaAuxiliar = TransformaEmContaCorrente(auxiliar);
                        return contaAuxiliar;
                    }

                }
            }
            throw new Exception("Conta nao encontrada");
        }

        public bool RemoveFromFile(string num_conta)    //Remove a conta do arquivo
        {
            bool found = false;
            using (FileStream FluxoDoArquivo = new FileStream("contas.csv", FileMode.OpenOrCreate))
            using (FileStream FluxoSecundario = new FileStream("arquivoauxiliar.csv", FileMode.OpenOrCreate))
            {
                string conteudo;
                using (StreamReader leitor = new StreamReader(FluxoDoArquivo))
                using (StreamWriter escritor = new StreamWriter(FluxoSecundario))
                {
                    while (!leitor.EndOfStream)
                    {
                        conteudo = leitor.ReadLine();
                        string contaLinha = conteudo.Substring(0, 6);
                        if(contaLinha == num_conta)
                        {
                            found = true;
                            //Pula a linha com a conta a ser excluida, fazendo com que ela seja deletada do arquivo
                        }
                        else
                        {
                            escritor.WriteLine(conteudo);
                        }
                        
                    }
                }

            }
            File.Delete("contas.csv");
            File.Copy("arquivoauxiliar.csv", "contas.csv");
            File.Delete("arquivoauxiliar.csv");
            return found;
        }

        public void AlterarArquivo(string dados)    //Substitui os dados da conta passada
        {
            
            string num_conta = dados.Substring(0,6);
            using (FileStream FluxoDoArquivo = new FileStream("contas.csv", FileMode.OpenOrCreate))
            using (FileStream FluxoSecundario = new FileStream("arquivoauxiliar.csv", FileMode.OpenOrCreate))
            {
                string conteudo;
                using (StreamReader leitor = new StreamReader(FluxoDoArquivo))
                using (StreamWriter escritor = new StreamWriter(FluxoSecundario))
                {
                    while (!leitor.EndOfStream)
                    {
                        conteudo = leitor.ReadLine();
                        string contaLinha = conteudo.Substring(0, 6);
                        if (contaLinha == num_conta)
                        {
                            
                            escritor.WriteLine(dados);
                            //Substitui a linha com os dados a serem alterados
                        }
                        else
                        {
                            escritor.WriteLine(conteudo);
                        }

                    }
                }

            }
            File.Delete("contas.csv");
            File.Copy("arquivoauxiliar.csv", "contas.csv");
            File.Delete("arquivoauxiliar.csv");

        }
    }
}