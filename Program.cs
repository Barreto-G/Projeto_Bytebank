using System;
using Projeto_Bytebank;
using Projeto_Bytebank.Pessoas;
using Projeto_Bytebank.Contas;

namespace MainProgram
{
    class MainProgram
    {
        static void Main()
        {
            ProgramaPrincipal programa = new ProgramaPrincipal();
            ContaCorrente conta = new ContaCorrente(new Pessoa("070.326.199-16", "12345", "Raffael Barreto"),"001","1234-G");
            try
            {
                //ContaCorrente conta1 = programa.ProcurarEmArquivo("1234-G");
                //Console.WriteLine(conta.ToString());
                programa.Exec();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


              
            


            

            


        

        }
    }
}