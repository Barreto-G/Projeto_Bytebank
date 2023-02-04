using Projeto_Bytebank.Pessoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projeto_Bytebank.Contas
{
    public class ContaCorrente : IComparable<ContaCorrente>
    {
        public ContaCorrente(Pessoa Titular, string agencia, string conta)
        {
            this.Titular = Titular;
            this.Agencia = agencia;
            this.Conta = conta;
            this.Saldo = 0;
        }

        private Pessoa titular;
        public Pessoa Titular 
        {
            get { return titular; }
            private set 
            {
                if(value!= null)
                {
                    titular = value;
                }
                else
                {
                    throw new ArgumentException("Titular passado nao eh valido");
                }
            }
        }
        private string agencia;
        public string Agencia 
        { 
            get { return agencia; }
            private set 
            {
                if (Regex.IsMatch(value, @"[0-9]{3}"))
                {
                    agencia = value;
                }
                else
                {
                    throw new ArgumentException("Numero da agencia invalida");
                }
            } 
        }

        private string conta = "";
        public string Conta 
        { 
            get 
            {
                return conta;
            } 
            private set 
            { 
                if(Regex.IsMatch(value, @"[0-9]{4}[-][A-Z]{1}"))
                {
                    conta = value;
                }
                else
                {
                    throw new ArgumentException("Numero da conta digitado fora do padrao");
                }
            } 
        }

        public double Saldo { get; private set; }

        public void Sacar(double valor)
        {
            if(valor > Saldo)
            {
                throw new ValorInvalidoException("Saldo insuficiente");   
            }
            else if(valor < 0)  
            {
                throw new ValorInvalidoException("Tentativa de sacar um valor negativo");    
            }
            else
            {
                Saldo -= valor;
            }
        }

        public void Deposito(double valor)
        {
            if (valor > 0)
            {
                this.Saldo += valor;
            }
            else
            {
                throw new ValorInvalidoException("Tentativa de depositar um valor negativo");
            }  
        }

        public void Transferir(ContaCorrente recebedor, double valor)   //Aqui nao eh necessario fazer verificacao pois ela ja eh feita nas funcoes sacar e Deposito
        {
            this.Sacar(valor);
            recebedor.Deposito(valor);
            Console.WriteLine($"Valor de {valor} enviado para a conta de numero {recebedor.Conta}.\n"+
                              $"Seu novo saldo eh de {this.Saldo} reais.");
        }
        public override string ToString()
        {
            return $"Conta: {Conta}\n" +
                   $"Agencia: {Agencia}\n" +
                   $"Saldo: {Saldo}\n"+
                   Titular.ToString();
        }

        public int CompareTo(ContaCorrente? other)
        {
            if(other == null)
            {
                return 1;
            }
            else
            {
                int thisNumericConverted = Int32.Parse(this.Conta.Substring(0, 4));
                int otherNumericConverted = Int32.Parse(other.Conta.Substring(0, 4));
                if (thisNumericConverted > otherNumericConverted)
                {
                    return 1;
                }
                else if(thisNumericConverted == otherNumericConverted)
                {
                    char thisChar = this.Conta[5];
                    char otherChar = other.Conta[5];
                    if(thisChar > otherChar)
                    {
                        return 1;
                    }
                    else if (thisChar < otherChar)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
