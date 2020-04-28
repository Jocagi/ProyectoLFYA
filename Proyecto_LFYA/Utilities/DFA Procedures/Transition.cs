using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    public class Transition
    {
        /// <summary>
        /// Simbolo necesario para acceder a este estado
        /// </summary>
        public string symbol { get; }
        
        /// <summary>
        /// Conjunto de follows que conforman este estado (Siguiente estado)
        /// </summary>
        public List<int> nodes { get; }

        public bool isAcceptanceStatus { get; }

        /// <summary>
        /// Crea una nueva instancia de la transicion
        /// </summary>
        public Transition(string simbolo, List<int> nodos)
        {
            symbol = simbolo;
            nodes = nodos;
            isAcceptanceStatus = nodos.Contains(ExpressionCharacters.EndCharacter.ToCharArray()[0]); 
        }
    }
}
