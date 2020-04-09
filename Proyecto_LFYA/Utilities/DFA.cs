using System;
using System.Collections.Generic;

namespace Proyecto_LFYA.Utilities
{
    class DFA
    {
        FollowTable states;

        public DFA(ExpressionTree tree)
        {
            states = new FollowTable(tree);
            TransitionTable transitionTable = new TransitionTable(states);
        }

        //todo DFA with sets
        public DFA(ExpressionTree tree, Dictionary<string, int[]> sets)
        {
            states = new FollowTable(tree);
            TransitionTable transitionTable = new TransitionTable(states);
        }

        public bool isValidString(string text, ref string message, ref int countCharacters)
        {
            //Evaluate character in table position
            //Go through all possible next positions and find a match
            //count number of times that was validated until error
            //if any of the possible follows is match return suggested char of the one with the longest chain

            bool result = false;
            Stack<char> characters = new Stack<char>();
            for (int i = text.Length - 1; i >= 0; i--)
            {
                characters.Push(text[i]);
            }

            message = traverseTable(characters, ref result, ref countCharacters);

            return result;
        }
        
        private string traverseTable(Stack<char> expression, ref bool isCorrect, ref int characters)
        {
            string expectedValue = "";
            int logestPath = 0;
            bool isCorrectString = false;

            int count = pathCount(expression, 0, ref expectedValue, ref logestPath, ref isCorrectString);

            characters = count;

            if (!isCorrectString)
            {
                if (expectedValue != "")
                {
                    expectedValue = "Se esperaba " + expectedValue; 
                }

                isCorrect = false;
                return $"Error de formato: {expectedValue}";
            }

            isCorrect = true;
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

                //Do not show # as an expected value
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
                bool jumpFound = nextCharacter == '\n'; //Ya no se que hacer :(

                if (jumpFound)
                {
                    nextCharacter = ' ';
                }

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
                    
                    if (states.nodes[item].character ==  nextCharacter.ToString() || states.nodes[item].isAcceptanceStatus ||
                        (states.nodes[item].character == ExpressionCharacters.LetrasMayusculaRegex && char.IsUpper(nextCharacter))
                        || (states.nodes[item].character == ExpressionCharacters.LetrasMinusculaRegex && char.IsLower(nextCharacter))
                        || (states.nodes[item].character == ExpressionCharacters.NumerosRegex && char.IsDigit(nextCharacter))
                        || (states.nodes[item].character == ExpressionCharacters.SimbolosRegex && (char.IsSymbol(nextCharacter) || char.IsPunctuation(nextCharacter))))
                    {
                        //Mark flag for later use
                        foundNextValue = true;
                        
                        //Check if this is not the correct path
                        int tmp = pathCount(expression, item, ref tmpExpectedValue,
                            ref tmpLongestPath, ref isCorrect);

                        if (tmp == 5) //If final status is reached but there were still char in stack
                        {
                            tmpExpectedValue += states.nodes[position].character;
                        }

                        if (tmp >= longestPath)
                        {
                            expectedValue = tmpExpectedValue;
                            longestPath = tmp;
                        }
                        
                    }

                    if (isCorrect || (jumpFound&&foundNextValue))
                    {
                        break;
                    }
                }

                //Analyze results for recursion
                if (isCorrect)
                {
                    return 0;
                }
                else if (foundNextValue)
                {
                    expression.Push(nextCharacter); //Return item for next iteration
                    return longestPath + 1;
                }
                else
                {
                    expression.Push(nextCharacter); //Return item for next iteration
                    expectedValue = thisNodeFollowValues;
                    return 1;
                }
            }
        }
    }
}
