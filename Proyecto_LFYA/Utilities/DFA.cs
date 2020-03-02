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
            string thisNodeFollowValues = "";

            //If next is end node
            if (expression.Count == 0)
            {
                foreach (var item in states.nodes[position].follows)
                {
                    //Save possible next values if any of these appear 
                    if (thisNodeFollowValues != "")
                    {
                        thisNodeFollowValues = $"{thisNodeFollowValues} | {states.nodes[item].character}";
                    }
                    else
                    {
                        thisNodeFollowValues = $"{states.nodes[item].character}";
                    }

                    if (states.nodes[item].isAcceptanceStatus)
                    {
                        isCorrect = true;
                        return 1;
                    }
                }

                //Fixes bug that shows # as an expected value
                expectedValue = states.nodes[position].isAcceptanceStatus ? 
                                "menos caracteres" : thisNodeFollowValues;
                
                return 0;
            }
            //If end node and there still items in stack
            else if (states.nodes[position].isAcceptanceStatus)
            {
                return 5;
            }
            //if middle node
            else
            {
                //Character of this iteration
                char nextCharacter = expression.Pop();
                string tmpExpectedValue = "";
                int tmpLongestPath = 0;
                bool foundNextValue = false;

                //Traverse follow values
                foreach (var item in states.nodes[position].follows)
                {
                    //Save possible next values if any of these appear 
                    if (thisNodeFollowValues != "")
                    {
                        thisNodeFollowValues = $"{thisNodeFollowValues} | {states.nodes[item].character}";
                    }
                    else
                    {
                        thisNodeFollowValues = $"{states.nodes[item].character}";
                    }
                    
                    if (states.nodes[item].character == nextCharacter || states.nodes[item].isAcceptanceStatus)
                    {
                        //Mark flag for later use
                        foundNextValue = true;

                        //Check if this is not the correct path
                        int tmp = pathCount(expression, item, ref tmpExpectedValue,
                            ref tmpLongestPath, ref isCorrect);

                        //Not efficient nor logical, but it works
                        if (tmp == 5) //If final status is reached but there were still char in stack
                        {
                            tmpExpectedValue += states.nodes[position].character.ToString();
                        }

                        if (tmp >= longestPath)
                        {
                            expectedValue = tmpExpectedValue;
                            longestPath = tmp;
                        }
                    }
                }

                //Analyze results for recursion
                if (foundNextValue)
                {
                    return longestPath + 1;
                }
                else
                {
                    expectedValue = thisNodeFollowValues;
                    return 1;
                }
            }
        }
    }
}
