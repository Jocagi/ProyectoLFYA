using System;
using System.Collections.Generic;

namespace Proyecto_LFYA.Utilities
{
    class DFA
    {
        TransitionTable states;

        public DFA(TransitionTable table)
        {
            this.states = table;
        }

        public void isValidString(string text, ref string message)
        {
            //Evaluate character in table position
            //Go through all possible next positions and find a match
            //count number of times that was validated until error
            //if any of the possible follows is match return suggested char of the one with the longest chain

            Stack<char> characters = new Stack<char>();
            for (int i = text.Length - 1; i >= 0; i--)
            {
                characters.Push(text[i]);
            }

            message = traverseTable(characters, 0);
        }

        private string traverseTable(Stack<char> expression, int position)
        {
            string expectedValue = "";
            int logestPath = 0;
            bool isCorrectString = false;

            int count = pathCount(expression, 0, ref expectedValue, ref logestPath, ref isCorrectString);
            
            if (!isCorrectString)
            {
                if (expectedValue != "")
                {
                    expectedValue = "Se esperaba " + expectedValue; 
                }

                return $"Error de formato: {expectedValue}";
            }

            return "Formato Correcto";
        }

        private int pathCount(Stack<char> expression, int position, ref string expectedValue, 
            ref int longestPath, ref bool isCorrect)

        {
            string followValues = "";
            
            if (expression.Count == 0)
            {
                if (states.nodes[position].follows.Count == 0)
                {
                    if (states.nodes[position].isAcceptanceStatus)
                    {
                        isCorrect = true;
                        return 2;
                    }
                    else
                    {
                        expectedValue = states.nodes[position].character.ToString();
                        return 0;
                    }
                }
                else
                {
                    foreach (var item in states.nodes[position].follows)
                    {
                        followValues = $"{followValues} | {item}";

                        if (states.nodes[item].isAcceptanceStatus)
                        {
                            isCorrect = true;
                            return 2;
                        }
                    }

                    expectedValue = followValues;
                    return 0;
                }
            }
            
            //Character of this iteration
            char first = expression.Pop();
            string tmpExpectedValue = "";
            int tmpLongestPath = 0;
            bool foundNextValue = false;
            
            //Traverse follow values
            foreach (var item in states.nodes[0].follows)
            {
                followValues = $"{followValues} | {item}";
                
                if (states.nodes[item].character == first && expression.Count >= 0)
                {
                    //Mark flag
                    foundNextValue = true;

                    //Check if this is not the correct path
                    int tmp = pathCount(expression, item, ref tmpExpectedValue,
                        ref tmpLongestPath, ref isCorrect);

                    if (tmp > longestPath)
                    {
                        expectedValue = tmpExpectedValue;
                        longestPath = tmp;
                    }
                }
            }

            //Analyze results
            if (foundNextValue)
            {
                return longestPath + 1;
            }
            else
            {
                expectedValue = followValues;
                return 0;
            }
        }
    }
}
