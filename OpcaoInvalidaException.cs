using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Bytebank
{
    public class OpcaoInvalidaException : Exception
    {
        public OpcaoInvalidaException() { }
        public OpcaoInvalidaException(string mensagem): base(mensagem) { }
        public OpcaoInvalidaException(string mensagem, Exception inner): base(mensagem, inner) { }

    }
}
