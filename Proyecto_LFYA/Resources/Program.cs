using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Scanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<char> Input = new Stack<char>();

            Inicio:
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine(" __                                 ");
            Console.WriteLine("/ _\\ ___ __ _ _ __  _ __   ___ _ __ ");
            Console.WriteLine("\\ \\ / __/ _` | '_ \\| '_ \\ / _ \\ '__|");
            Console.WriteLine("_\\ \\ (_| (_| | | | | | | |  __/ |   ");
            Console.WriteLine("\\__/\\___\\__,_|_| |_|_| |_|\\___|_|   ");

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("\nEscriba la cadena para analizar: ");
            Input = getStack(Console.ReadLine());

            Console.Write("\nAnalizando");
            Thread.Sleep(300);
            Console.Write(".");
            Thread.Sleep(300);
            Console.Write(".");
            Thread.Sleep(800);
            Console.Write(".\n");

            AnalizarTexto(Input);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nPresiona cualquier tecla para continuar.");
            Console.ReadKey();
            Console.Clear();
            goto Inicio;
        }

        static Stack<char> getStack(string input)
        {
            Stack<char> output = new Stack<char>();

            for (int i = input.Length - 1; i >= 0; i--)
            {
                output.Push(input[i]);
            }

            return output;
        }

        static void Error()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nError: La cadena introducida no es valida");
        }

        static void Valido(ref string token)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + token + " = " + getTokenNumber(token));
            token = "";
        }

        static int getTokenNumber(string text)
        {
            char firstCharacterFromText = text[0];
            char lastCharacterFromText = text[text.Length - 1];
            
            Dictionary<int, List<char>> Character_Token_First = new Dictionary<int, List<char>>
            {
                {1, new List<char> {'a', 'b', 'c'}}
            };

            Dictionary<List<char>, int> Character_Token_Last = new Dictionary<List<char>, int>
            {
                {new List<char> {'a', 'b', 'c'}, 1}
            };

            Dictionary<string, int> ReservadasValues = new Dictionary<string, int>
            {
                {"bcccc", 14},
                {"bccccc", 15}
            };
             
            List<int> TokensConReferencia = new List<int>
            {
                1, 99
            };
            
            foreach (var Pair in Character_Token_Last)
            {
                if (Pair.Key.Contains(lastCharacterFromText) && 
                    Character_Token_First[Pair.Value].Contains(firstCharacterFromText))
                {
                    if (TokensConReferencia.Contains(Pair.Value))
                    {
                        if (ReservadasValues.Keys.Contains(text))
                        {
                            return ReservadasValues[text];
                        }
                    }
                    return Pair.Value;
                }   
            }

            return 0;
        }

        static void AnalizarTexto(Stack<char> Input)
        {
            Inicio:

            int Estado = 0;
            string actualText = "";

            while (Input.Count > 0)
            {
                actualText = actualText.Trim();
                char actualChar = Input.Pop();

                if (actualChar != ' ')
                {
                    switch (Estado)
                    {
                        //States and transitions of DFA
                        case 0:
                            if (actualChar == 'a')
                            {
                                Estado = 2;
                            }
                            else if (actualChar == 'b')
                            {
                                Estado = 1;
                            }
                            else if (actualChar == 'c')
                            {
                                Estado = 0;
                            }
                            else
                            {
                                goto Error;
                            }
                            break;
                        case 1:
                            if (actualChar == 'c')
                            {
                                Estado = 1;
                            }
                            else
                            {
                                goto Valido;
                            }
                            break;
                        case 2:
                            if (actualChar == 'c')
                            {
                                Estado = 0;
                            }
                            else if(actualChar == 'd')
                            {
                                Estado = 0;
                            }
                            else
                            {
                                goto Valido;
                            }
                            break;

                        case -1:
                            Valido:
                            Valido(ref actualText);
                            Estado = 0;
                            Input.Push(actualChar);
                            actualChar = ' ';
                            break;
                        default:
                            Error:
                            Error();
                            return;
                    }

                    actualText += actualChar;

                }
                else
                {
                    goto CheckForAcceptedStates;       
                }
            }
            
            if (actualText.Length > 0)
            {
                goto CheckForAcceptedStates;    
            }

            return;

            CheckForAcceptedStates:
            if (Estado == 2 || Estado == 1) //Estados de aceptacion
            {
                Valido(ref actualText);
            }
            else
            {
                Error();
            }
            goto Inicio;
        }
    }
}
