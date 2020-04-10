using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_LFYA.Utilities
{
    class AnalizarGramatica
    {
        //private static string expresionRegular = "(( *SETS *)([A-Z]+ *= *(('[Simbolo]')|('([A-Z]|[a-z]|[0-9])+'(..'([A-Z]|[a-z]|[0-9])+')?)|(CHR\\([0-9]\\)(..CHR\\([0-9]\\))?)|(CHR\\([0-9]+\\))((..CHR\\([0-9]+\\))?))(( *\\+ *)(('[Simbolo]')|('([A-Z]|[a-z]|[0-9])+'(..'([A-Z]|[a-z]|[0-9])+')?)|(CHR\\([0-9]\\)(..CHR\\([0-9]\\))?)|(CHR\\([0-9]+\\))((..CHR\\([0-9]+\\))?)))* *)+)?(( *TOKENS *)(TOKEN *[0-9]+ *= *(([A-Z]+)|(\\( *[A-Z] *)\\)|('([Simbolo]|[A-Z]|[a-z]|[0-9])')| |\\?|\\||\\*|\\+|\\(|\\)|({ *[A-Z]+\\(\\) *}))+ *)+)( *ACTIONS +RESERVADAS *\\( *\\) *{( *[0-9]+ *= *'[A-Z]+')+ *}([A-Z]+ *\\( *\\) *{( *[0-9]+ *= *'[A-Z]+')+ *})*)( *[A-Z]+ *= *[0-9]+)+ *#";
        private static string expresionSET 
            = " *[A-Z]+ *= *((('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\)))(..(('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\))))?)+ *#";
        private static string expresionTOKEN 
            = "( *TOKEN *[0-9]+ *= *((([A-Z]+)|('([Simbolo]|[A-Z]|[a-z]|[0-9]) *' *)|(\\(( *([A-Z]|[Simbolo]) *)+\\))| |\\?|\\||\\*|\\+|({ *[A-Z]+\\(\\) *}))|( *\\( *((([A-Z]+)|('([Simbolo]|[A-Z]|[a-z]|[0-9]) *' *)|(\\(( *([A-Z]|[Simbolo]) *)+\\))| |\\?|\\||\\*|\\+|({ *[A-Z]+\\(\\) *})))+ *\\) *))+ *)+#";
        private static string expresionACTIONSYERROR 
            = "( *ACTIONS +RESERVADAS *\\( *\\) *{( *[0-9]+ *= *'[A-Z]+')+ *} *([A-Z]+ *\\( *\\) *{( *[0-9]+ *= *'[A-Z]+')+ *})*)( *[A-Z]+ *= *[0-9]+)+ *#";

        public static string analizarAchivoGramatica(string text)
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
                            return "Error: Se esperaba SETS|TOKENS";
                        }
                    }
                    else if (setActive)
                    {
                        if (item.Contains("TOKENS"))
                        {
                            if (setCount < 1)
                            {
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
                                return $"Error en linea:{count}\n{mensaje}";
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
                                return $"Error en linea:{count}\n{mensaje}";
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

            return mensaje;
        }

        public static ExpressionTree obtenerArbolDeGramatica(string text)
        {
            Dictionary<string, int[]> sets = new Dictionary<string, int[]>();
            string token = ""; //Each token will be concatenated

            text = text.Replace('\r', ' ');
            text = text.Replace('\t', ' ');

            text = text.TrimStart();
            text = text.TrimEnd();

            //Position in the file
            bool inicio = true;
            bool setActive = false;
            bool tokenActive = false;
            bool actionActive = false;

            //List with the number of the tokens
            List<int> tokensList = new List<int>();
            
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

                    //todo add ACTIONS and ERROR reader. (Fase 3)
                }
            }

            //Create tree
            if (token != "")
            {
                return new ExpressionTree(token, sets);
            }
            else
            {
                throw new Exception("Se esperaba mas tokens");
            }
        }

        //SET reader
        private static void AddNewSET(ref Dictionary<string, int[]> sets, string text)
        {
            List<int> asciiValues = new List<int>();
            string setName = "";

            string[] line = text.Split('=');

            setName = line[0].Trim();//this is the set name
            line[1] = line[1].Trim();//this are the values

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
                        Limits.Add(i.Trim());
                    }   
                }
                
                if (Limits.Count == 2)
                {
                    int lowerLimit = formatSET(Limits[0]);
                    int upperLimit = formatSET(Limits[1]); ;
                    
                    //get all ascii values
                    for (int i = lowerLimit; i <= upperLimit; i--)
                    {
                        //Add to values
                        asciiValues.Add(i);

                        //Increment i, to fit all ascii values (256)
                        i = (i + 2) % 257;
                    }
                }
                else if (Limits.Count == 1)
                {
                    int character = formatSET(Limits[0]);

                    asciiValues.Add(character);
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
            string[] token = {"", ""};

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] != '=')
                {
                    token[0] += expression[i];
                }
                else
                {
                    //todo Implement ask what to do with Reservadas()

                    string tmp = "";

                    for (int j = i + 1; j < expression.Length; j++)
                    {
                        tmp += expression[j];
                    }

                    token[1] = removeActionsFromExpression(tmp);
                    token[1] = token[1].Trim();
                    break;
                }
            }

            return token;
        }

        private static string removeActionsFromExpression(string text)
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
                        }

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
    }
}
