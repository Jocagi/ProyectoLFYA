using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Proyecto_LFYA.Utilities.DFA_Procedures;
using Action = Proyecto_LFYA.Utilities.DFA_Procedures.Action;

namespace Proyecto_LFYA.Utilities
{
    public class ExpressionTree:ExpressionCharacters
    {
        /// <summary>
        /// Initial node of the tree
        /// </summary>
        public Node root;
        
        /// <summary>
        /// Dictionary with definitions of sets
        /// </summary>
        public Dictionary<string, string[]> sets = new Dictionary<string, string[]>();

        /// <summary>
        /// List with token number and expeted final values.
        /// </summary>
        public List<Token> tokens = new List<Token>();
        
        /// <summary>
        /// List with definitions of actions
        /// </summary>
        public List<Action> actions = new List<Action>();
        
        /// <summary>
        /// Indicates a reference from a token to an action.
        ///
        /// Key=TokenNumber
        /// Value=ActionName
        /// </summary>
        public Dictionary<int, string> actionReference = new Dictionary<int, string>();

        
        /// <summary>
        /// Regular expression that defines this tree
        /// </summary>
        public string expression;

        public ExpressionTree()
        {
            root = null;
        }

        //[Deprecated]
        public ExpressionTree(string expression)
        {
            this.expression = expression;

            checkForEndCharacter(ref expression);

            //Shunting yard algorithm to generate tree
            Queue<string> Tokens = getTokensFromExpression(expression);
            shuntingYard(Tokens);

            setNumberInNodes();
            setNullableNodes();
            setFirstPos();
            setLastPos();
        }

        ////[Deprecated]
        public ExpressionTree(string expression, Dictionary<string, string[]> sets)
        {
            this.sets = sets;
            this.expression = expression;

            checkForEndCharacter(ref expression);
            Queue<string> Tokens = getTokensFromGrammarExpression(expression, sets);
            shuntingYard(Tokens);

            setNumberInNodes();
            setNullableNodes();
            setFirstPos();
            setLastPos();
        }

        public ExpressionTree(string expression, Dictionary<string, string[]> sets,
                                List<Action> actions, List<int> tokenNumbers, Dictionary<int, string> reference)
        {
            this.sets = sets;
            this.expression = expression;

            checkForEndCharacter(ref expression);
            Queue<string> Tokens = getTokensFromGrammarExpression(expression, sets);
            shuntingYard(Tokens);

            setNumberInNodes();
            setNullableNodes();
            setFirstPos();
            setLastPos();
        }

        private void checkForEndCharacter(ref string expression)
        {
            if (expression[expression.Length-1].ToString() != EndCharacter)
            {
                expression = $"({expression}){EndCharacter}";
            }
        }

        /// <summary>
        /// Adds each character from the string to a Queque including 
        /// concat (.) when the are two characters next to each other.
        /// </summary>
        /// <param name="expression">Regular Expression</param>
        private Queue<string> getTokensFromExpression(string expression)
        {
            Queue<string> tokens = new Queue<string>();
            int lenght = expression.Length;

            for (int i = 0; i < lenght; i++)
            {
                if (expression[i].ToString() == EndCharacter)
                {
                    tokens.Enqueue(expression[i].ToString());
                    break;
                }
                if (expression[i].ToString() == Escape)
                {
                    tokens.Enqueue(expression[i].ToString());
                    tokens.Enqueue(expression[i + 1].ToString());
                    //Prevent a concatenation with an operation
                    if (expression[i + 2].ToString() != Grouping_Close && !isAnOperationChar(expression[i + 2].ToString())) 
                    {
                        tokens.Enqueue(Concatenation);
                    }
                    i++;
                }
                else if (isABinaryOperationChar(expression[i].ToString()) || expression[i].ToString() == Grouping_Open || 
                            expression[i].ToString() == EndCharacter || isAnOperationChar(expression[i + 1].ToString()) ||
                            expression[i + 1].ToString() == Grouping_Close)
                {
                    tokens.Enqueue(expression[i].ToString());
                }
                else //it must be a valid char tha can be concatenated ( + * ? a..z etc
                {
                    tokens.Enqueue(expression[i].ToString());
                    tokens.Enqueue(Concatenation);
                }
            }

            return tokens;
        }

        /// <summary>
        /// Adds each character and custom values defined in sets from the string to a Queque 
        /// </summary>
        private Queue<string> getTokensFromGrammarExpression(string expression, Dictionary<string, string[]> sets)
        {
            expression = removeExtraSpacesFromString(expression);

            Queue<string> tokens = new Queue<string>();
            int lenght = expression.Length;

            for (int i = 0; i < lenght; i++)
            {
                string item = expression[i].ToString();

                if (item == EndCharacter)
                {
                    tokens.Enqueue(expression[i].ToString());
                    break;
                }

                if (item == Char_Separator) //if it begins with '
                {
                    string itemAhead = expression[i + 2].ToString();

                    if (itemAhead == Char_Separator) //if it ends with '
                    {

                        tokens.Enqueue(Escape);
                        tokens.Enqueue(expression[i + 1].ToString());

                        //Prevent a concatenation with an operation
                        if (expression[i + 3].ToString() != Grouping_Close &&
                            !isAnOperationChar(expression[i + 3].ToString()))
                        {
                            tokens.Enqueue(Concatenation);
                        }
                        i += 2;
                    }
                    else if (itemAhead != " ") //ignore blank spaces
                    {
                        throw new Exception("Formato invalido, se esperaba '");
                    }
                }
                else if ((isABinaryOperationChar(item) || item == Grouping_Open ||
                         item == EndCharacter || isAnOperationChar(expression[i + 1].ToString()) ||
                         expression[i + 1].ToString() == Grouping_Close))
                {
                    tokens.Enqueue(expression[i].ToString());
                }
                else //it must be a valid char that can be concatenated ) + * ? a..z etc
                {
                    if (item == Grouping_Close | item == KleenePlus|
                             item == KleeneStar| item == QuestionMark)
                    {
                        tokens.Enqueue(item);

                        //Prevent a concatenation with an operation
                        if (expression[i + 1].ToString() != Grouping_Close && !isAnOperationChar(expression[i + 1].ToString()))
                        {
                            tokens.Enqueue(Concatenation);
                        }
                    }
                    else if (!string.IsNullOrEmpty(item) && !string.IsNullOrWhiteSpace(item))
                        //it is a special token (defined in set)
                    {
                        string value = "";
                        char token = expression[i];
                        int counter = 0;

                        while (token != ' ' && token.ToString() != Char_Separator && token.ToString() != Grouping_Close &&
                               lenght > i + counter  && !isAnOperationChar(expression[i + counter].ToString()))
                        {
                            value += token;
                            counter++;
                            token = expression[i + counter];
                        }

                        if (sets.ContainsKey(value))
                        {
                            tokens.Enqueue(value);

                            //Prevent a concatenation with an operation
                            if (expression[i + counter].ToString() != Grouping_Close && !isAnOperationChar(expression[i + counter].ToString()))
                            {
                                tokens.Enqueue(Concatenation);
                            }

                            i += counter - 1;
                        }
                        else
                        {
                            throw new Exception($"No existe una definicion del SET {value}");
                        }
                    }
                }
            }

            return tokens;
        }

        private string removeExtraSpacesFromString(string input)
        {
            string result = "";

            for (int i = 0; i < input.Length; i++)
            {
                char item = input[i];

                if (item != ' ')
                {
                    if (item == '\'')
                    {
                        result += $"'{input[i + 1]}'";
                        i+=2;
                    }
                    else
                    {
                        result += item;
                    }
                }
                //if last item added was not a blankspace or the bext is a parentesis
                else if ((result[result.Length -1] != ' ' && 
                         !isAnOperationChar(input[i + 1].ToString()) &&
                         result[result.Length - 1] != '\'') &&
                         (input[i + 1].ToString() != Grouping_Close) &&
                         (input[i + 1] != ' ')) 
                {
                    result += item;
                }
            }

            return result;
        }

        private bool isSpecialCharacter(string item)
        {
            return isAnOperationChar(item) | item == Grouping_Close | item == Grouping_Open | item == Escape;
        }

        private bool isASingleOperationChar(string item)
        {
            string[] SpecialCharacters = { KleeneStar, KleenePlus, QuestionMark };

            if (SpecialCharacters.Contains(item))
            {
                return true;
            }

            return false;
        }

        private bool isABinaryOperationChar(string item)
        {
            string[] SpecialCharacters = { Alternation, Concatenation };

            if (SpecialCharacters.Contains(item))
            {
                return true;
            }

            return false;
        }

        private bool isAnOperationChar(string item)
        {
            if (isABinaryOperationChar(item) || isASingleOperationChar(item))
            {
                return true;
            }

            return false;
        }

        private bool isATerminalCharacter(string item)
        {
            //Characters that represent operations
            string[] SpecialCharacters = { Escape, Grouping_Open, Grouping_Close };

            if (SpecialCharacters.Contains(item) || isAnOperationChar(item))
            {
                return false;
            }

            return true;
        }

        private void shuntingYard(Queue<string> regularExpression)
        {
            Stack<string> T = new Stack<string>(); //Stack of tokens
            Stack<Node> S = new Stack<Node>(); //Stack of trees
            
            //Step 1
            while (regularExpression.Count > 0)
            {
                //Step 2
                string token = regularExpression.Dequeue();

                //Step 3
                if (token == Escape)
                {
                    if (regularExpression.Count > 0)
                    {
                        token = regularExpression.Dequeue();
                        S.Push(new Node(token, false));
                    }
                    else
                    {
                        throw new Exception("Se esperaban mas tokens");
                    }
                }
                //Step 4
                else if (isATerminalCharacter(token))
                {
                    S.Push(new Node(token, true));
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

                        Node temp = new Node(T.Pop());
                        temp.right = S.Pop();
                        temp.left = S.Pop();
                        S.Push(temp);
                    }
                    string tmp = T.Pop();
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
                root = S.Pop();
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
                        i.nullable = i.left.nullable;
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
            root.firstPos.Sort();
        }
        private void setFirstPos(ref Node i)
        {
            if (i.isLeaf())
            {
                if (i.element != Epsilon)
                {
                    i.firstPos.Add(i.number);
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
                    i.lastPos.Add(i.number);
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


        //Draw tree

        public Image Draw()
        {
            GC.Collect();// collects the unreffered locations on the memory
            int temp;
            return root == null ? null : root.Draw(out temp);
        }
        
        //Get list of values of the node
        public List<string[]> getValuesOfNodes()
        {
            //Simbolo, First, Last, Nullable
            List<string[]> cells = new List<string[]>();
            int j = 0;

            getValuesOfNodes(root, ref cells, ref j);

            return cells;
        }

        private void getValuesOfNodes(Node i, ref List<string[]> cells, ref int j)
        {
            if (i.left != null)
            {
                j++;
                getValuesOfNodes(i.left, ref cells, ref j);
            }
            if (i.right != null)
            {
                j++;
                getValuesOfNodes(i.right, ref cells, ref j);
            }

            cells.Add(new[]
            {i.element, string.Join( ",", i.firstPos),
                string.Join( ",", i.lastPos), i.nullable.ToString()});

        }

        private int countNodes()
        {
            return countNodes(root);
        }
        private int countNodes(Node i)
        {
            if (i != null)
            {
                return 1 + countNodes(i.left) + countNodes(i.right);
            }
            else
            {
                return 0;
            }
        }
    }
}
