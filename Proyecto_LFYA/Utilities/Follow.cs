using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    class Follow
    {
        public char character { get; set; }
        public bool isAcceptanceStatus { get; set; }
        public List<int> follows { get; set; }

        public Follow(char character)
        {
            this.character = character;
            isAcceptanceStatus = character == ExpressionCharacters.EndCharacter;
            follows = new List<int>();
        }
    }
}
