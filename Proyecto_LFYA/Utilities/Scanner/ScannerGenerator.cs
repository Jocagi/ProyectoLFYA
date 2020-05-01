using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_LFYA.Utilities.DFA_Procedures;
using Action = System.Action;

namespace Proyecto_LFYA.Utilities.Scanner
{
    class ScannerGenerator
    {
        public static string GetSourceCode(ExpressionTree tree)
        {
            DFA automata = new DFA(tree);
            TransitionTable transitions = new TransitionTable(automata.states);
            
            string sourceCode = Resources.Resource1.Program;
            
            //GetValues
            string TitleColor = getColor();
            string Character_Token_First = getFistPositionsWithCharacters(tree.tokens);
            string Character_Token_Last = getLastPositionsWithCharacters(tree.tokens);
            string Reservadas_Values = getReservedValues(tree.actions); 
            string TokensConReferencia = getReferences(tree.actionReference);
            string Estados_Aceptacion = getAcceptedStates(transitions);
            string States = "\r\n //States and transitions of DFA\r\n  case 0:\r\n   if (actualChar == 'a')\r\n                            {\r\n                                Estado = 2;\r\n                            }\r\n                            else if (actualChar == 'b')\r\n                            {\r\n                                Estado = 1;\r\n                            }\r\n                            else if (actualChar == 'c')\r\n                            {\r\n                                Estado = 0;\r\n                            }\r\n                            else\r\n                            {\r\n                                goto Error;\r\n                            }\r\n                            break;\r\n                        case 1:\r\n                            if (actualChar == 'c')\r\n                            {\r\n                                Estado = 1;\r\n                            }\r\n                            else\r\n                            {\r\n                                goto Valido;\r\n                            }\r\n                            break;\r\n                        case 2:\r\n                            if (actualChar == 'c')\r\n                            {\r\n                                Estado = 0;\r\n                            }\r\n                            else if(actualChar == 'd')\r\n                            {\r\n                                Estado = 0;\r\n                            }\r\n                            else\r\n                            {\r\n                                goto Valido;\r\n                            }\r\n                            break;";
                
            //Replace Values
            sourceCode = sourceCode.Replace("</TitleColor>", TitleColor);
            sourceCode = sourceCode.Replace("</FirstPos>", Character_Token_First);
            sourceCode = sourceCode.Replace("</LastPos>", Character_Token_Last);
            sourceCode = sourceCode.Replace("</Reservadas>", Reservadas_Values);
            sourceCode = sourceCode.Replace("</Referencias>", TokensConReferencia);
            sourceCode = sourceCode.Replace("</States>", States);
            sourceCode = sourceCode.Replace("</Aceptacion>", Estados_Aceptacion);

            return sourceCode;
        }

        private static string getColor()
        {
            List<string> colors = new List<string>
                { "ConsoleColor.Magenta" , "ConsoleColor.Cyan",
                    "ConsoleColor.Yellow", "ConsoleColor.Green"};

            var random = new Random();
            int index = random.Next(colors.Count);

            return colors[index];
        }

        private static string getFistPositionsWithCharacters(List<Token> tokens)
        {
            string result = "";
            foreach (var token in tokens)
            {
                //if it is the first element
                if (result != "")
                {
                    result += " , ";
                }
                //Concatenate firstPositions
                result += 
                $"{{ {token.TokenNumber}, new List<char> {{ {getFormatedCharactersFromList(token.FirstPositions)} }} }}";
            }

            return result;
        }

        private static string getLastPositionsWithCharacters(List<Token> tokens)
        {
            string result = "";
            foreach (var token in tokens)
            {
                //if it is the first element
                if (result != "")
                {
                    result += " , ";
                }
                //Concatenate firstPositions
                result +=
                    $"{{ new List<char> {{ {getFormatedCharactersFromList(token.LastPositions)} }}, {token.TokenNumber} }}";
            }

            return result;
        }

        private static string getFormatedCharactersFromList(List<char> list)
        {
            string output = "";

            for (int i = 0; i < list.Count; i++)
            {
                output += $"'{list[i]}'";

                if (i != list.Count - 1)
                {
                    output += ",";
                }
            }

            return output;
        }

        private static string getAcceptedStates(TransitionTable transitions)
        {
            string output = "";

            //Rows (States)
            for (int i = 0; i < transitions.states.Count; i++)
            {
                int Estado = i;
                
                //If it is Accepted status
                if (transitions.states[i].Contains(transitions._followTable.nodes.Count - 1))
                {
                    
                    output += (output == "") ? $"Estado=={Estado}" : $" || Estado=={Estado}";
                }
            }

            return output;
        }

        private static string getReservedValues(List<DFA_Procedures.Action> words)
        {
            string output = "";

            foreach (var item in words)
            {
                foreach (var token in item.ActionValues)
                {
                    output += (output == "") ? $"{{ \"{token.Value}\" , {token.Key} }}" 
                                             : $" , {{ \"{token.Value}\" , {token.Key} }}";
                }
            }

            return output;
        }

        private static string getReferences(Dictionary<int, string> references)
        {
            string output = "";

            foreach (var item in references)
            {
                output += (output == "") ? $"{item.Key}" : $",{item.Key}";
            }

            return output;
        }
    }
}
