using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    class ExpressionCharacters
    {
        //Characters of expression
        public const char Concatenation = '.';
        public const char Alternation = '|';
        public const char Escape = '\\';
        public const char KleeneStar = '*';
        public const char KleenePlus = '+';
        public const char QuestionMark = '?';
        public const char Grouping_Open = '(';
        public const char Grouping_Close = ')';
        public const char EndCharacter = '#';
        public const char Epsilon = 'ε';
    }
}
