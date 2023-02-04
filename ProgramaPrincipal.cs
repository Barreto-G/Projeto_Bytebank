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
    public class ProgramaPrincipal
    {
        public static List<ContaCorrente> _ListaDeContas = new List<ContaCorrente>();
        public void Exec()
        {
            try
            {
                char opcao = '0';
                do
                {
                    Console.Clear();
                    Console.WriteLine(  $"======= Boas Vindas ao ByteBank ========\n" +
                                        $"== Digite o numero da opcao desejada: ==\n" +
                                        $"== 1. Cadastrar Conta ==================\n" +
                                        $"== 2. Listar Contas ====================\n" +
                                        $"== 3. Remover Conta ====================\n" +
                                        $"== 4. Saque ============================\n" +
                                        $"== 5. Deposito =========================\n" +
                                        $"== 6. Tranferencia =====================\n" +
                                        $"== 7. Ordenar Contas ===================\n" +
                                        $"== 8. Sair do Sistema ==================\n");
                    opcao = Console.ReadLine()[0];
                    if (opcao < '1' || opcao > '8')
                    {
                        throw new OpcaoInvalidaException("A opcao eh invalida e o programa encerrara");
                    }
                    else if (opcao != '8')
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

                } while (opcao != '8');
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
                case '7':
                    this.Ordenar();
                    break;
                default:
                    Console.WriteLine("Digite uma opcao valida!");
                    Console.ReadLine();
                    Console.Clear();
                    break;
            }
        }

        private int ProcurarConta()
        {
            string num_conta;
            do
            {
                Console.Write("Digite um numero de conta valido: ");
                num_conta = Console.ReadLine();
                Console.Clear();

            } while (!Regex.IsMatch(num_conta, @"[0-9]{3}[-][A-Z]{1}"));

            for (int i = 0; i < _ListaDeContas.Count; i++)
            {
                if (num_conta == _ListaDeContas[i].Conta)
                {
                    return i;
                }
            }
            return -1;
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
                _ListaDeContas.Add(conta_auxiliar);

            }
            catch (ArgumentException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            Console.WriteLine("Conta Adicionada com Sucesso!");
            Console.ReadLine();
        }

        private void ListarContas()
        {
            if (_ListaDeContas == null || _ListaDeContas.Count == 0)
            {
                Console.WriteLine("Ainda nao ha nenhuma conta cadastrada");
                Console.ReadLine();
            }
            else
            {
                foreach (var lista in _ListaDeContas)
                {
                    Console.WriteLine(lista.ToString() + "\n");
                    Console.ReadLine();
                }
                Console.WriteLine("----------------");
                Console.ReadLine();
            }

        }

        private void RemoverConta()
        {
            int auxiliar = this.ProcurarConta();
            if (auxiliar != -1)
            {
                _ListaDeContas.RemoveAt(auxiliar);
                Console.WriteLine("Conta Excluida com sucesso");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Conta nao encontrada!");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private void Saque()
        {
            int num_conta = this.ProcurarConta();
            if (num_conta != -1)
            {
                try
                {
                    Console.Write("Digite o valor que quer sacar: ");
                    double valor = double.Parse(Console.ReadLine());
                    _ListaDeContas[num_conta].Sacar(valor);
                    Console.WriteLine($"Valor de {valor} reais sacado com sucesso\n" +
                                      $"O novo Saldo da conta eh {_ListaDeContas[num_conta].Saldo}");
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
        }

        private void Deposito()
        {
            int num_conta = this.ProcurarConta();
            if (num_conta != -1)
            {
                try
                {
                    Console.Write("Digite o valor que quer Depositar: ");
                    double valor = double.Parse(Console.ReadLine());
                    _ListaDeContas[num_conta].Deposito(valor);
                    Console.WriteLine($"Valor de {valor} reais depositado com sucesso\n" +
                                      $"O novo Saldo da conta eh {_ListaDeContas[num_conta].Saldo}");
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
                int numContaEnvia = this.ProcurarConta();
                Console.Write("Digite o valor a ser enviado: ");
                double valorEnviado = double.Parse(Console.ReadLine());
                Console.WriteLine("Agora a conta a quem sera enviado: ");
                int numContaRecebe = this.ProcurarConta();
                _ListaDeContas[numContaEnvia].Transferir(_ListaDeContas[numContaRecebe], valorEnviado);
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

        private void Ordenar()
        {
            try
            {
                _ListaDeContas.Sort();
                Console.WriteLine("Contas Ordenadas com Sucesso");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




    }
}

