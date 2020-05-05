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
            string States = getTransitions(transitions, tree.sets);
            
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
            {
                "ConsoleColor.Magenta", "ConsoleColor.Cyan",
                "ConsoleColor.Yellow", "ConsoleColor.Green"
            };

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
                if (list[i] != '\'')
                {
                    output += $"'{list[i]}'";
                }
                else
                {
                    output += "(char) 39";
                }


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

        private static Dictionary<int, bool> getAcceptedStatesDictionary(TransitionTable transitions)
        {
            Dictionary<int, bool> acceptedStates = new Dictionary<int, bool>();

            //(States)
            for (int i = 0; i < transitions.states.Count; i++)
            {
                //If it is Accepted status
                acceptedStates.Add(i, transitions.states[i].Contains(transitions._followTable.nodes.Count - 1));
            }

            return acceptedStates;
        }

        private static string getReservedValues(List<DFA_Procedures.Action> words)
        {
            string output = "";

            foreach (var item in words)
            {
                foreach (var token in item.ActionValues)
                {
                    output += (output == "")
                        ? $"{{ \"{token.Value.ToUpper()}\" , {token.Key} }}"
                        : $" , {{ \"{token.Value.ToUpper()}\" , {token.Key} }}";
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

        private static Dictionary<string, int> getDictionaryOfStates(List<List<int>> states)
        {
            Dictionary<string, int> statesDictionary = new Dictionary<string, int>();

            for (int i = 0; i < states.Count; i++)
            {
                statesDictionary.Add(getStringList(states[i]), i);
            }

            return statesDictionary;
        }

        private static bool listsAreEqual<T>(ICollection<T> a, ICollection<T> b)
        {
            // 1
            // Require that the counts are equal
            if (a.Count != b.Count)
            {
                return false;
            }

            // 2
            // Initialize new Dictionary of the type
            Dictionary<T, int> d = new Dictionary<T, int>();
            // 3
            // Add each key's frequency from collection A to the Dictionary
            foreach (T item in a)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    d[item] = c + 1;
                }
                else
                {
                    d.Add(item, 1);
                }
            }

            // 4
            // Add each key's frequency from collection B to the Dictionary
            // Return early if we detect a mismatch
            foreach (T item in b)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    if (c == 0)
                    {
                        return false;
                    }
                    else
                    {
                        d[item] = c - 1;
                    }
                }
                else
                {
                    // Not in dictionary
                    return false;
                }
            }

            // 5
            // Verify that all frequencies are zero
            foreach (int v in d.Values)
            {
                if (v != 0)
                {
                    return false;
                }
            }

            // 6
            // We know the collections are equal
            return true;
        }

        private static string getStringList(List<int> list)
        {
            list.Sort();
            return string.Join(",", list);
        }

        private static string createIFstatement
            (Transition transition, Dictionary<string, int> states, Dictionary<string, string[]> sets)
        {
            if (transition.nodes.Count > 0)
            {
                return $"if({getTransitionBoolStatement(transition, sets)})\r\n{{\r\nEstado = {getStateNumber(transition.nodes, states)};\r\n}}\r\n";
            }
            else
            {
                return "";
            }
        }

        private static string getStateNumber(List<int> actualState, Dictionary<string, int> states)
        {
            return states[getStringList(actualState)].ToString();
        }

        private static string getTransitionBoolStatement(Transition transition, Dictionary<string, string[]> sets)
        {
            if (transition.symbol.Length > 1) //It is a set
            {
                string[] values = sets[transition.symbol];
                string output = "";

                foreach (var item in values)
                {
                    output += (output == "")? "" : "||"; //Add or operator

                    if (item.Contains(',')) //If it is a range of values
                    {
                        string[] limits = item.Split(',');
                        string lowerLimit = limits[0].Trim();
                        string upperLimit = limits[1].Trim();
                        
                        output += $" (actualChar >= {lowerLimit} && actualChar <= {upperLimit}) ";
                    }
                    else //If it is a single char
                    {
                        string value = item.Trim();
                        
                        output += $" actualChar == {value} ";
                    }
                }

                return output;
            }
            else if (transition.symbol.Length == 1)//It is a single char
            {
                if (transition.symbol != "'")
                {
                    return $"actualChar == '{transition.symbol}'";
                }
                else
                {
                    return "actualChar == 39";
                }
            }
            else
            {
                throw new Exception("Se ha intentado leer un set sin valor.");
            }
        }

        private static string getTransitions(TransitionTable transitions, Dictionary<string, string[]> sets)
        {
            string output = "";

            Dictionary<string, int> states = getDictionaryOfStates(transitions.states);
            Dictionary<int, bool> isAcceptedState = getAcceptedStatesDictionary(transitions);

            //Get actual state
            for (int i = 0; i < transitions.states.Count; i++)
            {
                bool firstIf = true;

                //Case's names
                string actualCase = $"\r\ncase {i}:\r\n";
                
                //Get transitions from actual state
                foreach (var item in transitions.transitions[i])
                {
                    if (item.nodes.Count > 0)
                    {
                        actualCase += firstIf ? "" : "else ";

                        actualCase += createIFstatement(item, states, sets);

                        firstIf = false;
                    }
                }

                //if it is accepted State
                if (actualCase.Contains("if"))
                {
                    if (isAcceptedState[i] && i != 0) //Prevent the first state to be accepted
                    {
                        actualCase += "else\r\n{\r\ngoto Valido;\r\n}\r\n";
                    }
                    else
                    {
                        actualCase += "else\r\n{\r\ngoto Error;\r\n}\r\n";
                    }
                }
                else
                {
                    if (isAcceptedState[i])
                    {
                        actualCase += "\r\ngoto Valido;\r\n";
                    }
                    else
                    {
                        actualCase += "\r\ngoto Error;\r\n";
                    }
                }

                //break actual state
                actualCase += $"break;\r\n";

                //Add spaces to look nice
                actualCase = actualCase.Replace("\r\n", "\r\n                        ");

                output += actualCase;
            }

            return output;
        }
    }
}
