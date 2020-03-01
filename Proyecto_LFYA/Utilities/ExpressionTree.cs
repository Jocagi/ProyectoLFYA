using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    class ExpressionTree:ExpressionCharacters
    {
        public Node root;
        
        private ExpressionTree()
        {
            this.root = null;
        }

        public ExpressionTree(string expression)
        {
            //Shunting yard algorithm to generate tree
            Queue<char> Tokens = getTokensFromExpression(expression);
            shuntingYard(Tokens);

            setNumberInNodes();
            setNullableNodes();
            setFirstPos();
            setLastPos();
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

                if (expression[i] == EndCharacter)
                {
                    tokens.Enqueue(expression[i]);
                    break;
                }
                else if (expression[i] == Escape)
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
            char[] SpecialCharacters = { KleeneStar, KleenePlus, QuestionMark };

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
                    else if (T.Count > 0 && T.Peek() != Grouping_Open && 
                                ((T.Peek() == Concatenation && token == Alternation) ||
                                    (T.Peek() == token)))
                    {
                        // Operation order:
                        // aleternation(|) has less precedence than concat(.)

                        Node temp = new Node(T.Pop());

                        if (S.Count >= 2)
                        {
                            temp.right = S.Pop();
                            temp.left = S.Pop();

                            S.Push(temp);
                            T.Push(token);
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

        //Follow Functions

        private void setNumberInNodes()
        {
            int number = 1;
            setNumberInNodes(ref root, ref number);
        }
        private void setNumberInNodes(ref Node i, ref int number)
        {
            if (i.isLeaf())
            {
                if (i.element != Epsilon)
                {
                    i.number = number;
                    number++;
                }
            }
            else
            {
                if (i.left != null)
                {
                    Node k = i.left;
                    setNumberInNodes(ref k, ref number);
                    i.left = k;
                }
                if (i.right != null)
                {
                    Node k = i.right;
                    setNumberInNodes(ref k, ref number);
                    i.right = k;
                }
            }
        }

        private void setNullableNodes()
        {
            setNullableNodes(ref root);
        }
        private void setNullableNodes(ref Node i)
        {
            if (i.isLeaf())
            {
                i.nullable = i.element == Epsilon;
            }
            else
            {
                //First, get nullable states from child. (Postorder)
                if (i.left != null)
                {
                    Node k = i.left;
                    setNullableNodes(ref k);
                    i.left = k;
                }
                if (i.right != null)
                {
                    Node k = i.right;
                    setNullableNodes(ref k);
                    i.right = k;
                }

                //Rules
                switch (i.element)
                {
                    case Alternation: //nullable(c1) or nullable(c2)
                        i.nullable = i.right.nullable || i.left.nullable; 
                        break;
                    case Concatenation: //nullable(c1) and nullable(c2)
                        i.nullable = i.right.nullable && i.left.nullable;
                        break;
                    case KleeneStar:
                        i.nullable = true;
                        break;
                    case KleenePlus:
                        i.nullable = false;
                        break;
                    case QuestionMark:
                        i.nullable = true;
                        break;
                    default:
                        throw new Exception("Operacion no reconocida.");
                }
            }
        }

        private void setFirstPos()
        {
            setFirstPos(ref root);
        }
        private void setFirstPos(ref Node i)
        {
            if (i.isLeaf())
            {
                if (i.element != Epsilon)
                {
                    i.firstPos.Add(i.element);
                }
            }
            else
            {
                //(Postorder)
                if (i.left != null)
                {
                    Node k = i.left;
                    setFirstPos(ref k);
                    i.left = k;
                }
                if (i.right != null)
                {
                    Node k = i.right;
                    setFirstPos(ref k);
                    i.right = k;
                }

                //Rules
                switch (i.element)
                {
                    case Alternation: //fistpos(c1) U fistpos(c2)
                        i.firstPos = i.left.firstPos.Concat(i.right.firstPos).ToList();
                        break;

                    case Concatenation:
                        if (i.left.nullable) //fistpos(c1) U fistpos(c2)
                        {
                            i.firstPos = i.left.firstPos.Concat(i.right.firstPos).ToList();
                        }
                        else //fistpos(c1)
                        {
                            i.firstPos = i.left.firstPos;
                        }
                        break;

                    case KleeneStar: //fistpos(c1)
                        i.firstPos = i.left.firstPos;
                        break;

                    case KleenePlus: //fistpos(c1)
                        i.firstPos = i.left.firstPos;
                        break;

                    case QuestionMark: //fistpos(c1)
                        i.firstPos = i.left.firstPos;
                        break;

                    default:
                        throw new Exception("Operacion no reconocida.");
                }
            }
        }

        private void setLastPos()
        {
            setLastPos(ref root);
        }
        private void setLastPos(ref Node i)
        {
            if (i.isLeaf())
            {
                if (i.element != Epsilon)
                {
                    i.lastPos.Add(i.element);
                }
            }
            else
            {
                //(Postorder)
                if (i.left != null)
                {
                    Node k = i.left;
                    setLastPos(ref k);
                    i.left = k;
                }
                if (i.right != null)
                {
                    Node k = i.right;
                    setLastPos(ref k);
                    i.right = k;
                }

                //Rules
                switch (i.element)
                {
                    case Alternation: //lastpos(c1) U lastpos(c2)
                        i.lastPos = i.left.lastPos.Concat(i.right.lastPos).ToList();
                        break;

                    case Concatenation:
                        if (i.right.nullable) //lastpos(c1) U lastpos(c2)
                        {
                            i.lastPos = i.left.lastPos.Concat(i.right.lastPos).ToList();
                        }
                        else //lastpos(c2)
                        {
                            i.lastPos = i.right.lastPos;
                        }
                        break;

                    case KleeneStar: //lastpos(c1)
                        i.lastPos = i.left.lastPos;
                        break;

                    case KleenePlus: //lastpos(c1)
                        i.lastPos = i.left.lastPos;
                        break;

                    case QuestionMark: //lastpos(c1)
                        i.lastPos = i.left.lastPos;
                        break;

                    default:
                        throw new Exception("Operacion no reconocida.");
                }
            }
        }
    }
}
