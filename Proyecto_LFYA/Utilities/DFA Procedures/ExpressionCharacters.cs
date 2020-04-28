using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    public class ExpressionCharacters
    {
        //Characters of expression
        public const string Concatenation = "●";
        public const string Alternation = "|";
        public const string Escape = "\\";
        public const string KleeneStar = "*";
        public const string KleenePlus = "+";
        public const string QuestionMark = "?";
        public const string Grouping_Open = "(";
        public const string Grouping_Close = ")";
        public const string EndCharacter = "#";
        public const string Epsilon = "ε";

        //Division for tokens
        public const string Char_Separator = "'";

        public const string AbrevLetrasMinusculaRegex = "[a-z]";
        //public const string LetrasMinusculaRegex = "(a|b|c|d|e|f|g|h|i|j|k|l|m|n|ñ|o|p|q|r|s|t|u|v|w|x|y|z)";
        public const string LetrasMinusculaRegex = "©";

        public const string AbrevLetrasMayusculaRegex = "[A-Z]";
        //public const string LetrasMayusculaRegex = "(A|B|C|D|E|F|G|H|I|J|K|L|M|N|Ñ|O|P|Q|R|S|T|U|V|W|X|Y|Z)";
        public const string LetrasMayusculaRegex = "®";

        public const string AbrevNumerosRegex = "[0-9]";
        //public const string NumerosRegex = "(0|1|2|3|4|5|6|7|8|9)";
        public const string NumerosRegex = "Ø";

        public const string AbrevSimbolosRegex = "[Simbolo]";
        //public const string SimbolosRegex = "(\\#|[|]|{|}|\\(|\\)|\\\\|$|@|!|%|^|&|\\*|\\+|-|_|.|:|/|;|<|>|,|\"|"|`|~|\\||=)";
        public const string SimbolosRegex = "ƒ";
    }
}
