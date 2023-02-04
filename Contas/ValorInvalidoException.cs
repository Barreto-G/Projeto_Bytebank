using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Bytebank.Contas
{
    public class ValorInvalidoException : Exception
    {
        public ValorInvalidoException() { }
        public ValorInvalidoException(string mensagem) : base(mensagem) { }
        public ValorInvalidoException(string mensagem, Exception inner) : base(mensagem, inner) { }


    }
}
