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
        /// Code -1 means the token in nullable.
        /// </summary>
        public List<int> LastPositions =new List<int>();
    }
}
