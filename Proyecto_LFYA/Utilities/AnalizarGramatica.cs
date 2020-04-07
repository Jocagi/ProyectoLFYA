using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        //public static ExpressionTree obtenerArbolDeGramatica(string text)
        //{
        //    text = text.Replace('\r', ' ');
        //    text = text.Replace('\t', ' ');

        //    text = text.TrimStart();
        //    text = text.TrimEnd();

        //    string[] lineas = text.Split('\n');

        //}
    }
}
