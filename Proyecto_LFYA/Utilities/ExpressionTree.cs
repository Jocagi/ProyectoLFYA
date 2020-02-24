using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    class ExpressionTree
    {
        public Node root;

        #region Characters
        //Characters of expression
        private readonly char Concatenation = '.';
        private readonly char Alternation = '|';
        private readonly char Escape = '\\';
        private readonly char Asterisk = '*';
        private readonly char Plus = '+';
        private readonly char QuestionMark = '?';
        private readonly char Grouping_Open = '(';
        private readonly char Grouping_Close = ')';
        private readonly char EndCharacter = '#';

        #endregion

        private ExpressionTree()
        {
            this.root = null;
        }

        public ExpressionTree(string expression)
        {
            //Shunting yard algorithm to generate tree
            Queue<char> Tokens = getTokensFromExpression(expression);
            shuntingYard(Tokens);
        }
        
        private Queue<char> getTokensFromExpression(string expression)
        {
            //    Adds each character from the string to a Queque including 
            //    concat (.) when the are two characters next to each other.

            Queue<char> tokens = new Queue<char>();
            int lenght = expression.Length;

            for (int i = 0; i < lenght; i++)
            {
                int itemsLeft = lenght - i;
               
                if (expression[i] == Escape)
                {
                    tokens.Enqueue(expression[i]);
                    tokens.Enqueue(expression[i + 1]);
                    i++;
                }
                else if (isABinaryOperationChar(expression[i]) || expression[i] == Grouping_Open || 
                            expression[i] == EndCharacter || isAnOperationChar(expression[i + 1]) || expression[i + 1] == Grouping_Close)
                {
                    tokens.Enqueue(expression[i]);
                }
                else //it must be a valid char tha can be concatenated ( + * ? a..z etc
                {
                    tokens.Enqueue(expression[i]);
                    tokens.Enqueue(Concatenation);
                }
            }

            return tokens;
        }
        
        private bool isASingleOperationChar(char item)
        {
            char[] SpecialCharacters = { Asterisk, Plus, QuestionMark };

            if (SpecialCharacters.Contains(item))
            {
                return true;
            }

            return false;
        }

        private bool isABinaryOperationChar(char item)
        {
            char[] SpecialCharacters = { Alternation, Concatenation };

            if (SpecialCharacters.Contains(item))
            {
                return true;
            }

            return false;
        }

        private bool isAnOperationChar(char item)
        {
            if (isABinaryOperationChar(item) || isASingleOperationChar(item))
            {
                return true;
            }

            return false;
        }

        private bool isATerminalCharacter(char item)
        {
            //Characters that represent operations
            char[] SpecialCharacters = { Escape, Grouping_Open, Grouping_Close };

            if (SpecialCharacters.Contains(item) || isAnOperationChar(item))
            {
                return false;
            }

            return true;
        }

        private void shuntingYard(Queue<char> regularExpression)
        {
            Stack<char> T = new Stack<char>(); //Stack of tokens
            Stack<Node> S = new Stack<Node>(); //Stack of trees
            
            //Step 1
            while (regularExpression.Count > 0)
            {
                //Step 2
                char token = regularExpression.Dequeue();

                //Step 3
                if (token == Escape)
                {
                    if (regularExpression.Count > 0)
                    {
                        token = regularExpression.Dequeue();
                        S.Push(new Node(token));
                    }
                    else
                    {
                        throw new Exception("Se esperaban mas tokens");
                    }
                }
                //Step 4
                else if (isATerminalCharacter(token))
                {
                    S.Push(new Node(token));
                }
                //Step 5
                else if (token == Grouping_Open)
                {
                    T.Push(token);
                }
                //Step 6
                else if (token == Grouping_Close)
                {
                    while (T.Count > 0 && T.Peek() != Grouping_Open)
                    {
                        if (T.Count == 0 || S.Count < 2)
                        {
                            throw new Exception("Faltan operandos");
                        }
                        else
                        {
                            Node temp = new Node(T.Pop());
                            temp.right = S.Pop();
                            temp.left = S.Pop();
                            S.Push(temp);
                        }
                    }
                    char tmp = T.Pop();
                }
                //Step 7
                else if (isAnOperationChar(token))
                {
                    if (isASingleOperationChar(token))
                    {
                        Node op = new Node(token);

                        if (S.Count >= 0)
                        {
                            op.left = S.Pop();
                            S.Push(op);
                        }
                        else
                        {
                            throw new Exception("Faltan operandos");
                        }
                    }
                    else if (T.Count > 0 && T.Peek() != Grouping_Open && ((T.Peek() == Concatenation && token == Alternation)))
                    {
                        // Operation order:
                        // aleternation(|) has less precedence than concat(.)

                        Node temp = new Node(T.Pop());

                        if (S.Count >= 2)
                        {
                            temp.right = S.Pop();
                            temp.left = S.Pop();

                            S.Push(temp);
                        }
                        else
                        {
                            throw new Exception("Faltan operandos");
                        }
                    }
                    else
                    {
                        T.Push(token);
                    }
                }
                //Step 8
                else
                {
                    throw new Exception("No es un token reconocido" +
                        "");
                }
            }
            //Step 9 -> return to step 2 if there are still tokens in expression
            //Step 10
            while (T.Count > 0)
            {
                Node temp = new Node(T.Pop());
                if (temp.element != Grouping_Open && S.Count >= 2)
                {
                    temp.right = S.Pop();
                    temp.left = S.Pop();
                    S.Push(temp);
                }
                else
                {
                    throw new Exception("Faltan operandos");
                }
            }
            //Step 11 -> return to step 10 if there are still tokens in T
            //Step 12
            if (S.Count == 1)
            {
                //Step 13
                this.root = S.Pop();
            }
            else
            {
                throw new Exception("Faltan operandos"); 
            }
        }
    }
}
