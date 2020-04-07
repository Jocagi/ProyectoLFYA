
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Proyecto_LFYA.Utilities
{
    class RegEx:ExpressionCharacters
    {
        //Regular Expression
        readonly DFA automaton;

        public RegEx(string expression)
        {
            //Create an expression tree
            //Create and set Table
            expression = replaceAbreviations(expression);
            automaton = new DFA(new ExpressionTree(expression));
        }

        public RegEx(string expression, Dictionary<string, int[]> sets)
        {
            //Create an expression tree considering special sets
            //Create and set Table
            expression = replaceAbreviations(expression);
            automaton = new DFA(new ExpressionTree(expression), sets);
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
            int characters = 0;
            
            bool isValid = automaton.isValidString(text, ref message, ref characters);
            
            message = message.Replace(LetrasMayusculaRegex.ToString(), AbrevLetrasMayusculaRegex);
            message = message.Replace(LetrasMinusculaRegex.ToString(), AbrevLetrasMinusculaRegex);
            message = message.Replace(NumerosRegex.ToString(), AbrevNumerosRegex);
            message = message.Replace(SimbolosRegex.ToString(), AbrevSimbolosRegex);

            return message;
        }
    }
}
