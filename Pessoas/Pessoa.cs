using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projeto_Bytebank.Pessoas
{
    public class Pessoa
    {
        public Pessoa(string cpf, string rg, string nome)
        {
            RG = rg;
            CPF = cpf;
            Nome = nome;
        }

        private string cpf = "";
        public string RG {get; private set;}
        public string CPF 
        { 
            get { return cpf; }
            private set 
            {
                if (Regex.IsMatch(value, @"([0-9]{9}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})")) 
                {
                    cpf = value;
                }
                else
                {
                    throw new ArgumentException("CPF digitado eh invalido");
                }
            } 
        }
        public string Nome {get; set; }

        public override string ToString()
        {
            return $"Nome: {Nome}\n" +
                   $"CPF = {CPF}\n" +
                   $"RG = {RG}";
        }

    }
}
