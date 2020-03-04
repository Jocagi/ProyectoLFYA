
using System.Text.RegularExpressions;

namespace Proyecto_LFYA.Utilities
{
    class RegEx:ExpressionCharacters
    {
        //Regular Expression
        DFA automaton;

        public RegEx(string expression)
        {
            //Create an expression tree
            //Create and set Table
            expression = replaceAbreviations(expression);
            automaton = new DFA(new TransitionTable(new ExpressionTree(expression)));
        }

        private string replaceAbreviations(string expression)
        {
            expression = expression.Replace(AbrevLetrasMayusculaRegex, LetrasMayusculaRegex.ToString());
            expression = expression.Replace(AbrevLetrasMinusculaRegex, LetrasMinusculaRegex.ToString());
            expression = expression.Replace(AbrevNumerosRegex, NumerosRegex.ToString());
            expression = expression.Replace(AbrevSimbolosRegex, SimbolosRegex.ToString());

            return expression;
        }

        public string ValidateString(string text)
        {
            //Go through list veryfing each state met
            //Note: Only use error message if result is false

            string message = "";
            string originalText = text;
            int characters = 0;
            int lines = 1;


            bool isvalid = automaton.isValidString(text, ref message, ref characters);

            //if (!isvalid)
            //{
            //    int i = 0;
            //    while (characters > 0)
            //    {
            //        if (originalText[i] == '\n')
            //        {
            //            lines++;
            //        }

            //        i++;
            //        characters--;
            //    }

            //    message = $"Error en linea:{lines}\n{message}";
            //}

            message = message.Replace(LetrasMayusculaRegex.ToString(), AbrevLetrasMayusculaRegex);
            message = message.Replace(LetrasMinusculaRegex.ToString(), AbrevLetrasMinusculaRegex);
            message = message.Replace(NumerosRegex.ToString(), AbrevNumerosRegex);
            message = message.Replace(SimbolosRegex.ToString(), AbrevSimbolosRegex);

            return message;
        }
    }
}
