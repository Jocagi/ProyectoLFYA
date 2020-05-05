using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_LFYA.Utilities.DFA_Procedures;
using Action = Proyecto_LFYA.Utilities.DFA_Procedures.Action;

namespace Proyecto_LFYA.Utilities
{
    class AnalizarGramatica
    {
        private static string expresionSET 
            = " *[A-Z]+ *= *((('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\)))( *. *. *(('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\))))?)+ *" +
              "( *\\+ *((('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\)))( *. *. *(('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\))))?))* *#";
        private static string expresionTOKEN 
            = "( *TOKEN *[0-9]+ *= *((([A-Z]+)|('([Simbolo]|[A-Z]|[a-z]|[0-9]) *' *)|(\\(+( *([A-Z]|[Simbolo]) *)+\\))| |\\?|\\(|\\)|\\||\\*|\\+|({ *[A-Z]+\\(\\) *}))|( *\\( *((([A-Z]+)|('([Simbolo]|[A-Z]|[a-z]|[0-9]) *' *)|(\\(( *([A-Z]|[Simbolo]) *)+\\))| |\\?|\\(|\\)|\\||\\*|\\+|({ *[A-Z]+\\(\\) *})))+ *\\)+ *))+ *)+#";
        private static string expresionACTIONSYERROR 
            = "( *ACTIONS +RESERVADAS *\\( *\\) *{( *[0-9]+ *= *'([A-Z]|[a-z]|[0-9]|[Simbolo])+')+ *} *([A-Z]+ *\\( *\\) *{( *[0-9]+ *= *'([A-Z]|[a-z]|[0-9]|[Simbolo])+')+ *})*)( *[A-Z]+ *= *[0-9]+)+ *#";

        public static Dictionary<int, string> actionReference = new Dictionary<int, string>();


        public static string analizarAchivoGramatica(string text, ref int linea)
        {
            text = text.Replace('\r', ' ');
            text = text.Replace('\t', ' ');
            
            text = text.TrimStart();
            text = text.TrimEnd();

            //RegEx reg = new RegEx(expresionRegular);
            RegEx regSET = new RegEx(expresionSET);
            RegEx regTOKEN = new RegEx(expresionTOKEN);
            RegEx regOTRO = new RegEx(expresionACTIONSYERROR);

            string mensaje;
            string actions = "";

            bool inicio = true;
            bool setActive = false;
            bool tokenActive = false;
            bool actionActive = false;

            int tokenCount = 0;
            int setCount = 0;

            string[] lineas = text.Split('\n');
            int count = 0;

            foreach (var item in lineas)
            {
                count++;
                if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrEmpty(item))
                {
                    if (inicio)
                    {
                        inicio = false;
                        if (item.Contains("SETS"))
                        {
                            setActive = true;
                        }
                        else if (item.Contains("TOKENS"))
                        {
                            tokenActive = true;
                        }
                        else
                        {
                            linea = 1;
                            return "Error en linea 1: Se esperaba SETS|TOKENS";
                        }
                    }
                    else if (setActive)
                    {
                        if (item.Contains("TOKENS"))
                        {
                            if (setCount < 1)
                            {
                                linea = count;
                                return "Error: Se esperaba almenos un SET";
                            }
                            setActive = false;
                            tokenActive = true;
                        }
                        else
                        {
                            mensaje = regSET.ValidateString(item);
                            if (mensaje.Contains("Error"))
                            {
                                linea = count;
                                mensaje = mensaje.Replace(ExpressionCharacters.Epsilon, " TOKENS");
                                return $"Error en linea: {count} \n{mensaje}";
                            }
                            if (!areParenthesisPaired(item))
                            {
                                linea = count;
                                return $"Error en linea: {count} \nSe esperaba ()";
                            }
                            setCount++;
                        }
                    }
                    else if (tokenActive)
                    {
                        if (item.Contains("ACTIONS"))
                        {
                            if (tokenCount < 1)
                            {
                                linea = count;
                                return "Error: Se esperaba almenos un TOKEN";
                            }
                            actions = "ACTIONS";
                            tokenActive = false;
                            actionActive = true;
                        }
                        else
                        {
                            mensaje = regTOKEN.ValidateString(item);
                            if (mensaje.Contains("Error"))
                            {
                                linea = count;
                                return $"Error en linea: {count} \n{mensaje}";
                            }
                            tokenCount++;
                        }
                    }
                    else if (actionActive)
                    {
                        actions += " " + item;
                    }
                }
            }

            mensaje = regOTRO.ValidateString(actions);
            
            linea = count;
            return mensaje;
        }

        public static ExpressionTree obtenerArbolDeGramatica(string text)
        {
            Dictionary<string, string[]> sets = new Dictionary<string, string[]>();
            List<Action> actionsList = new List<Action>();

            //List with the number of the tokens
            List<int> tokensList = new List<int>();

            string token = ""; //Each token will be concatenated
            string actions = ""; //All actions are concatenated

            text = text.Replace('\r', ' ');
            text = text.Replace('\t', ' ');

            text = text.TrimStart();
            text = text.TrimEnd();

            //Position in the file
            bool inicio = true;
            bool setActive = false;
            bool tokenActive = false;
            bool actionActive = false;
            
            string[] lineas = text.Split('\n');
            
            //Traverse the file
            foreach (var item in lineas)
            {
                if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrEmpty(item))
                {
                    if (inicio)
                    {
                        inicio = false;
                        if (item.Contains("SETS"))
                        {
                            setActive = true;
                        }
                        else if (item.Contains("TOKENS"))
                        {
                            tokenActive = true;
                        }
                        else
                        {
                            throw new Exception("Formato incorrecto.");
                        }
                    }
                    else if (setActive)
                    {
                        if (item.Contains("TOKENS")) //End of section 'SETS'
                        {
                            setActive = false;
                            tokenActive = true;
                        }
                        else //There are still sets in the file 
                        {
                            AddNewSET(ref sets, item);
                        }
                    }
                    else if (tokenActive)
                    {
                        if (item.Contains("ACTIONS")) //End of section 'TOKENS'
                        {
                            tokenActive = false;
                            actionActive = true;
                        }
                        else
                        {
                            AddNewTOKEN(ref tokensList, ref token, item);
                        }
                    }
                    else if (actionActive)
                    {
                        if (item.Contains("ERROR") && item.Contains("="))
                        {
                            AddActions(actions, ref actionsList);
                            break;//Exit procedure
                        }
                        else
                        {
                            actions += item;
                        }
                    }
                }
            }

            //Check for repeated token numbers
            CheckForRepeatedTokens(tokensList, actionsList);

            //Reset references
            Dictionary<int, string> references = actionReference;
            actionReference = new Dictionary<int, string>();

            CheckReferences(references, actionsList);

            //Create tree
            if (token != "")
            {
                return new ExpressionTree(token, sets, actionsList, tokensList, references);
            }
            else
            {
                throw new Exception("Se esperaba mas tokens");
            }
        }

        private static bool areParenthesisPaired(string expression)
        {
            if (expression.Contains('(') || expression.Contains(')'))
            {
                int Open = 0;
                int Close = 0;

                expression = expression.Replace(" ", "");

                for (int i = 0; i < expression.Length; i++)
                {
                    if (expression[i] == '\'')
                    {
                        i += 2;
                    }
                    else if (expression[i] == '(')
                    {
                        Open++;
                    }
                    else if (expression[i] == ')')
                    {
                        Close++;
                    }
                }

                if (Open == Close)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        private static void CheckForRepeatedTokens(List<int> tokens, List<Action> actionsList)
        {
            List<int> repeated = new List<int>();

            foreach (var action in actionsList)
            {
                foreach (var item in action.ActionValues.Keys)
                {
                    if (repeated.Contains(item) || tokens.Contains(item))
                    {
                        throw new Exception($"Error: El token {item} aparece más de una vez");
                    }
                    else
                    {
                        repeated.Add(item);
                    }
                }
            }
            //If it ends, it is correct
        }

        private static void CheckReferences(Dictionary<int, string> references, List<Action> actionsList)
        {
            foreach (var item in references.Values)
            {
                bool notFound = true;

                foreach (var action in actionsList)
                {
                    if (action.ActionName == item)
                    {
                        notFound = false;
                        break;
                    }
                }

                if (notFound)
                {
                    throw new Exception($"No existe el ACTION {item}");
                }
            }
        }

        //SET reader
        private static void AddNewSET(ref Dictionary<string, string[]> sets, string text)
        {
            List<string> asciiValues = new List<string>();
            string setName = "";

            string[] line = text.Split('=');

            setName = line[0].Trim();//this is the set name
            line[1] = line[1].Replace(" ", "");//this are the values

            string[] values = line[1].Split('+');

            foreach (var item in values)
            {
                string[] tmpLimits = item.Split('.');

                List<string> Limits = new List<string>();

                //format
                foreach (var i in tmpLimits)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        Limits.Add(i);
                    }   
                }
                
                if (Limits.Count == 2)
                {
                    int lowerLimit = formatSET(Limits[0]);
                    int upperLimit = formatSET(Limits[1]); ;
                    
                    //Add range of values
                    asciiValues.Add($"{lowerLimit},{upperLimit}");
                }
                else if (Limits.Count == 1)
                {
                    int character = formatSET(Limits[0]);

                    asciiValues.Add(character.ToString());
                }
            }

            if (setName.Length > 1)
            {
                sets.Add(setName, asciiValues.ToArray());
            }
            else
            {
                throw new Exception($"El nombre del SET {setName} debe ser mas largo.");
            }

        }

        private static int formatSET(string token)
        {
            int result;

            if (token.Contains("CHR")) //Ex. CHR(90)
            {
                string value = token.Replace("CHR", "");
                value = value.Replace("(", "");
                value = value.Replace(")", "");
                value = value.Replace(" ", "");

                result = Convert.ToInt16(value);
            }
            else //Ex. 'A'
            {
                result = Encoding.ASCII.GetBytes(token)[1];
            }

            return result;
        }

        //TOKEN reader
        private static void AddNewTOKEN(ref List<int> tokenNumbers, ref string tokens, string text )
        {
            text = text.Replace("TOKEN", "");
            text = text.Trim();
            int tokenNumber = 0;

            string[] line = SplitToken(text);

            //Validate token number
            if (int.TryParse(line[0].Trim(), out tokenNumber))
            {
                if (!tokenNumbers.Contains(tokenNumber))
                {
                    tokenNumbers.Add(tokenNumber);
                }
                else
                {
                    throw new Exception($"El TOKEN {tokenNumber} aparece mas de una vez.");
                }
            }
            else
            {
              throw new Exception($"El nombre del TOKEN {line[0]} no es valido.");
            }
            
            string newToken = line[1].Trim();//this are the values

            if (string.IsNullOrEmpty(tokens) | string.IsNullOrWhiteSpace(tokens))
            {
                tokens = $"({newToken})";
            }
            else
            {
                tokens += $"|({newToken})";
            }
        }

        private static string[] SplitToken(string expression)
        {
            string functionName = "";//When Token Contains { function() }
            string[] token = {"", ""};

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] != '=')
                {
                    token[0] += expression[i];
                }
                else
                {
                    string tmp = "";

                    for (int j = i + 1; j < expression.Length; j++)
                    {
                        tmp += expression[j];
                    }

                    token[1] = removeActionsFromExpression(tmp, ref functionName);
                    token[1] = token[1].Trim();
                    break;
                }
            }
            
            //Validate token number (just for the reference)
            if (!string.IsNullOrEmpty(functionName))
            {
                if (int.TryParse(token[0].Trim(), out int tokenNumber))
                {
                    actionReference.Add(tokenNumber, functionName.Trim());
                }
                else
                {
                    throw new Exception($"El nombre del TOKEN {token[0]} no es valido.");
                }

            }
            
            return token;
        }

        private static string removeActionsFromExpression(string text, ref string functionName)
        {
            //Remove everything contained within {}
            string result = "";
            
            if (text.Contains('{') && text.Contains('}'))
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '\'')
                    {
                        result += $"'{text[i + 1]}'";
                        i += 2;
                    }
                    else if (text[i] == '{')
                    {
                        int counter = 0;
                        char? actualChar = text[i];

                        while (actualChar != '}')
                        {
                            counter++;
                            actualChar = text[i + counter];
                            functionName += actualChar;
                        }

                        functionName = functionName.Replace("}", "");
                        functionName = functionName.Replace(" ", "");

                        i += counter;
                    }
                    else
                    {
                        result += text[i];
                    }
                }
                return result;
            }

            return text;
        }

        //ACTIONS reader
        private static void AddActions(string text, ref List<Action> actions)
        {
            string[] IndividualActions = text.Split('}');

            foreach (var Actions in IndividualActions)
            {
                if (!string.IsNullOrEmpty(Actions))
                {
                    Action newAction = new Action();

                    string[] SeparatedValues = Actions.Split('{');

                    string name = SeparatedValues[0];
                    Dictionary<int, string> tokens = SplitActions(SeparatedValues[1]);

                    newAction.ActionName = name.Replace(" ", "");
                    newAction.ActionValues = tokens;

                    actions.Add(newAction);
                }
            }
        }
        
        private static Dictionary<int, string> SplitActions(string setOfTokens)
        {
            Dictionary<int, string> ActionValues = new Dictionary<int, string>();

            //Separar actions y numeros. Sus numeros seran pares y el valor sera impar.
            string newText = setOfTokens.Replace("=", " ");
            newText = newText.Replace("'", " ");
            newText = Regex.Replace(newText, @"\s+", " ");
            newText = newText.Trim();

            string[] tokens = newText.Split();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (int.TryParse(tokens[i], out int tokenNumber))
                {
                    ActionValues.Add(tokenNumber, tokens[i+1].Replace("'", ""));

                    i++;
                }
                else
                {
                    throw new Exception("Error al leer ACTIONS");
                }
            }

            return ActionValues;
        }
    }
}
