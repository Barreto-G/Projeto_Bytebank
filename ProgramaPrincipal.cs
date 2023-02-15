using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Projeto_Bytebank.Contas;
using Projeto_Bytebank.Pessoas;

namespace Projeto_Bytebank
{
    public partial class ProgramaPrincipal //: IDisposable
    {
        public static List<ContaCorrente> _ListaDeContas = new List<ContaCorrente>();
        //public static FileStream FluxoDoArquivo = new FileStream("contas.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.ReadWrite);
        public void Exec()
        {
            try
            {
                char opcao = '0';
                do
                {
                    Console.Clear();
                    Console.WriteLine($"======= Boas Vindas ao ByteBank ========\n" +
                                        $"== Digite o numero da opcao desejada: ==\n" +
                                        $"== 1. Cadastrar Conta ==================\n" +
                                        $"== 2. Listar Contas ====================\n" +
                                        $"== 3. Remover Conta ====================\n" +
                                        $"== 4. Saque ============================\n" +
                                        $"== 5. Deposito =========================\n" +
                                        $"== 6. Tranferencia =====================\n" +
                                        $"== 7. Sair do Sistema ==================\n");
                    opcao = Console.ReadLine()[0];
                    if (opcao < '1' || opcao > '7')
                    {
                        throw new OpcaoInvalidaException("A opcao eh invalida e o programa encerrara");
                    }
                    else if (opcao != '7')
                    {
                        Console.Clear();
                        OpcaoSelecionada(opcao);
                    }
                    else
                    {
                        Console.WriteLine("\nObrigado por usar o Bytebank");
                        Console.WriteLine("Desenvolvido por: Gabriel Barreto");
                        Console.ReadLine();
                    }

                } while (opcao != '7');             
            }
            catch (OpcaoInvalidaException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            catch (Exception excessao)
            {
                Console.WriteLine("Excessao desconhecida");
                Console.WriteLine(excessao.Message);
                Console.ReadLine();
            } 
        }

        private void OpcaoSelecionada(char opcao)
        {
            switch (opcao)
            {
                case '1':
                    this.CadastrarConta();
                    break;
                case '2':
                    this.ListarContas();
                    break;
                case '3':
                    this.RemoverConta();
                    break;
                case '4':
                    this.Saque();
                    break;
                case '5':
                    this.Deposito();
                    break;
                case '6':
                    this.Transferencia();
                    break;
                default:
                    Console.WriteLine("Digite uma opcao valida!");
                    Console.ReadLine();
                    Console.Clear();
                    break;
            }
        }

        private ContaCorrente ProcurarConta()
        {
            string num_conta;
            do
            {
                Console.Write("Digite um numero de conta valido: ");
                num_conta = Console.ReadLine();
                Console.Clear();

            } while (!Regex.IsMatch(num_conta, @"[0-9]{3}[-][A-Z]{1}"));

            ContaCorrente auxiliar = ProcurarEmArquivo(num_conta);
            return auxiliar;
        }

        private void CadastrarConta()
        {
            try
            {
                Console.Write("Digite o nome: ");
                string nome = Console.ReadLine();
                Console.Write("Digite o cpf: ");
                string cpf = Console.ReadLine();
                Console.Write("Digite o rg: ");
                string rg = Console.ReadLine();
                Pessoa auxiliar = new Pessoa(cpf, rg, nome);

                Console.Write("Digite a agencia: ");
                string agencia = Console.ReadLine();
                Console.Write("Digite o numero da conta: ");
                string num_conta = Console.ReadLine();
                ContaCorrente conta_auxiliar = new ContaCorrente(auxiliar, agencia, num_conta);
                SalvarEmArquivo(conta_auxiliar);
                Console.WriteLine("Conta Adicionada com Sucesso!");
                Console.ReadLine();
            }
            catch (ArgumentException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        }

        private void ListarContas()
        {
            using (FileStream FluxoDoArquivo = new FileStream("contas.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                if (FluxoDoArquivo.Length == 0)
                {
                    Console.WriteLine("Ainda nao ha nenhuma conta cadastrada");
                    Console.ReadLine();
                }
                else
                {
                    using (StreamReader leitor = new StreamReader(FluxoDoArquivo))
                    {
                        FluxoDoArquivo.Position = 0;
                        while (!leitor.EndOfStream)
                        {
                            string linha = leitor.ReadLine();
                            if (linha == null)
                            {
                                break;
                            }
                            ContaCorrente auxiliar = TransformaEmContaCorrente(linha);
                            Console.WriteLine(auxiliar.ToString());
                            Console.WriteLine("----------------");
                            Console.ReadLine();
                        }
                    }
                }
            }
                
        }

        private void RemoverConta()
        {
            string num_conta;
            do
            {
                Console.Write("Digite um numero de conta valido: ");
                num_conta = Console.ReadLine();
                Console.Clear();

            } while (!Regex.IsMatch(num_conta, @"[0-9]{3}[-][A-Z]{1}"));

            if (RemoveFromFile(num_conta))
            {
                Console.WriteLine("Conta Excluida com sucesso");
            }
            else
            {
                Console.WriteLine("Conta nao encontrada");
            }
            
            Console.ReadLine();
            Console.Clear();
        }

        private void Saque()
        {
            ContaCorrente conta = this.ProcurarConta();
            try
            {
                Console.Write("Digite o valor que quer sacar: ");
                double valor = double.Parse(Console.ReadLine());
                conta.Sacar(valor);
                AlterarArquivo(TransformaEmDados(conta));
                Console.WriteLine($"Valor de {valor} reais sacado com sucesso\n" +
                                    $"O novo Saldo da conta eh {conta.Saldo}");
                Console.ReadLine();
                Console.Clear();
            }
            catch (ValorInvalidoException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine(); 
            }

        }

        private void Deposito()
        {
            ContaCorrente conta = this.ProcurarConta();
            if (conta != null)
            {
                try
                {
                    Console.Write("Digite o valor que quer Depositar: ");
                    double valor = double.Parse(Console.ReadLine());
                    conta.Deposito(valor);
                    AlterarArquivo(TransformaEmDados(conta));
                    Console.WriteLine($"Valor de {valor} reais depositado com sucesso\n" +
                                      $"O novo Saldo da conta eh {conta.Saldo}");

                    Console.ReadLine();
                }
                catch (ValorInvalidoException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }

        }

        private void Transferencia()
        {
            try
            {
                Console.WriteLine("Digite o numero da conta que enviara o dinheiro:");
                ContaCorrente ContaEnvia = this.ProcurarConta();
                Console.Write("Digite o valor a ser enviado: ");
                double valorEnviado = double.Parse(Console.ReadLine());
                Console.WriteLine("Agora a conta a quem sera enviado: ");
                ContaCorrente ContaRecebe = this.ProcurarConta();
                ContaEnvia.Transferir(ContaRecebe, valorEnviado);
                AlterarArquivo(TransformaEmDados(ContaEnvia));
                AlterarArquivo(TransformaEmDados(ContaRecebe));
                Console.WriteLine("Transferencia realizada com sucesso!");
                Console.ReadLine();

            }
            catch(ValorInvalidoException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }


        }

    }
}

