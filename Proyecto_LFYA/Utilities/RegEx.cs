
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
            expression = expression.Replace(AbrevLetrasMayusculaRegex, LetrasMayusculaRegex);
            expression = expression.Replace(AbrevLetrasMinusculaRegex, LetrasMinusculaRegex);
            expression = expression.Replace(AbrevNumerosRegex, NumerosRegex);
            expression = expression.Replace(AbrevSimbolosRegex, SimbolosRegex);

            return expression;
        }

        public void ValidateString(string text, ref string message)
        {
            //Go through list veryfing each state met
            //Note: Only use error message if result is false
            automaton.isValidString(text, ref message);
        }
    }
}
