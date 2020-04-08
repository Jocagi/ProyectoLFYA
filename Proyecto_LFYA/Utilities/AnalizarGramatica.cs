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
        private static string expresionSET = " *[A-Z]+ *= *((('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\)))(..(('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\))))?)+ *#";
        private static string expresionTOKEN = "( *TOKEN *[0-9]+ *= *(([A-Z]+)|('([Simbolo]|[A-Z]|[a-z]|[0-9]) *' *)|(\\(( *([A-Z]|[Simbolo]) *)+\\))| |\\?|\\||\\*|\\+|({ *[A-Z]+\\(\\) *}))+ *)+#";
        private static string expresionACTIONSYERROR = "( *ACTIONS +RESERVADAS *\\( *\\) *{( *[0-9]+ *= *'[A-Z]+')+ *} *([A-Z]+ *\\( *\\) *{( *[0-9]+ *= *'[A-Z]+')+ *})*)( *[A-Z]+ *= *[0-9]+)+ *#";

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
                if (!String.IsNullOrWhiteSpace(item) && !String.IsNullOrEmpty(item))
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
                if (!String.IsNullOrWhiteSpace(item) && !String.IsNullOrEmpty(item))
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

        //todo implement set reader
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
                    if (!String.IsNullOrEmpty(i))
                    {
                        Limits.Add(i.Trim());
                    }   
                }
                
                if (Limits.Count == 2)
                {
                    int lowerLimit = formatToken(Limits[0]);
                    int upperLimit = formatToken(Limits[1]); ;
                    
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
                    int character = formatToken(Limits[0]);

                    asciiValues.Add(character);
                }
            }

            sets.Add(setName, asciiValues.ToArray());
        }

        private static int formatToken(string token)
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

        //todo implement token reader
        private static void AddNewTOKEN(ref List<int> tokenNumbers, ref string tokens, string text )
        {
        }
    }
}
