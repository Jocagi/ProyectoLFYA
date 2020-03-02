
namespace Proyecto_LFYA.Utilities
{
    class RegEx
    {
        //Regular Expression
        DFA automaton;

        public RegEx(string expression)
        {
            //Create an expression tree
            //Create and set Table
            automaton = new DFA(new TransitionTable(new ExpressionTree(expression)));
        }

        public void ValidateString(string text, ref string message)
        {
            //Go through list veryfing each state met
            //Note: Only use error message if result is false
            automaton.isValidString(text, ref message);
        }
    }
}
