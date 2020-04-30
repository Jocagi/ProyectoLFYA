using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities.DFA_Procedures
{
    public class Token
    {
        /// <summary>
        /// Number defined in file
        /// </summary>
        public int TokenNumber;

        /// <summary>
        /// List of ascii codes for the last character in an individual token.
        /// </summary>
        public List<char> FirstPositions = new List<char>();

        /// <summary>
        /// List of ascii codes for the last character in an individual token.
        /// </summary>
        public List<char> LastPositions = new List<char>();

        public Token(int tokenNumber, List<char> FirstPosition, List<char> LastPosition)
        {
            this.TokenNumber = tokenNumber;
            this.FirstPositions = FirstPosition;
            this.LastPositions = LastPosition;
        }
    }
}
