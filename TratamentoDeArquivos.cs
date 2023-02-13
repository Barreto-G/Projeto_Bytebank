using Projeto_Bytebank.Contas;
using Projeto_Bytebank.Pessoas;
using System.Numerics;

namespace Projeto_Bytebank
{
    public partial class ProgramaPrincipal
    {
        public void SalvarEmArquivo(ContaCorrente conta)
        {
            //Padrao usado: numDaConta, Agencia, Saldo, Cpf, Rg, nome
            string auxiliar = $"{conta.Conta},{conta.Agencia},{conta.Saldo},{conta.Titular.CPF},{conta.Titular.RG},{conta.Titular.Nome}";

            using (FileStream FluxoDoArquivo = new FileStream("contas.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamWriter escritor = new StreamWriter(FluxoDoArquivo))
            {
                FluxoDoArquivo.Position = FluxoDoArquivo.Length;
                escritor.WriteLine(auxiliar);
            }
        }

        ContaCorrente TransformaEmContaCorrente(string dados)
        {
            string[] dadosSeparados = dados.Split(',');
            Pessoa titular = new Pessoa(dadosSeparados[3], dadosSeparados[4], dadosSeparados[5]);
            ContaCorrente contaAuxiliar = new ContaCorrente(titular, dadosSeparados[1], dadosSeparados[0]);
            return contaAuxiliar;
        }

        public ContaCorrente ProcurarEmArquivo(string pesquisa)
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

        public bool overwrite(string num_conta)
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

    }
}