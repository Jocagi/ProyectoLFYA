using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_LFYA.Utilities
{
    public class TransitionTable
    {
        /// <summary>
        /// Listado de todos los simbolos de esta expresion regular
        /// </summary>
        public List<string> symbolsList = new List<string>();

        /// <summary>
        /// Conjunto de estados y transiciones. Estas son las filas de la tabla.
        /// Key = indice del estado
        /// Value = Listado de transiciones(Incluye el simbolo y el listado de follows)
        /// </summary>
        public Dictionary<int, List<Transition>> transitions = new Dictionary<int, List<Transition>>();

        /// <summary>
        /// Diccionario que hace referencia a la posicion que representa un conjunto.
        /// </summary>
        public List<List<int>> states = new List<List<int>>();

        private readonly FollowTable _followTable;

        public TransitionTable(FollowTable _followTable)
        {
            this._followTable = _followTable;
            getSymbolsList();
            generateTransitions();
        }

        private void getSymbolsList()
        {
            foreach (var element in _followTable.nodes)
            {
                if (!symbolsList.Contains(element.character) 
                    && element.character != ExpressionCharacters.EndCharacter)
                {
                    symbolsList.Add(element.character);   
                }
            }

            symbolsList.Remove(ExpressionCharacters.Epsilon);
            symbolsList.Sort();
        }
        
        private void generateTransitions()
        {
            //The first state is always the root of the tree
            states.Add(_followTable.nodes[0].follows);

            generateTransitionOfSingleState(0); 
        }

        private void generateTransitionOfSingleState(int position)
        {
            transitions.Add(position, new List<Transition>());
            List<int> newStates = new List<int>();

            //get current transitions from symbols
            foreach (var item in symbolsList)
            {
                Transition newTransition = getTransition(states[position], item);
                transitions[position].Add(newTransition);

                //Add new state
                if (!StateContainsNodes(newTransition.nodes) && newTransition.nodes.Count > 0)
                {
                    states.Add(newTransition.nodes);
                    newStates.Add(states.Count - 1); //Pointer of the new element added
                }
            }

            //generate new transitions from new states
            if (newStates.Count > 0)
            {
                foreach (var item in newStates)
                {
                    //Get transition from new state
                    generateTransitionOfSingleState(item);
                }
            }
        }

        /// <summary>
        /// Obtener la transicion para un simbolo de un estado.
        /// Recorre el listado de nodos, evalua el simbolo y agrega sus follows.
        /// </summary>
        /// <param name="state">Conjunto de nodos que conforma el estado</param>
        /// <param name="symbol">Simbolo</param>
        private Transition getTransition(List<int> state, string symbol)
        {
            List<int> follows = new List<int>();
            
            foreach (var item in state)
            {
                if (_followTable.nodes[item].character == symbol)
                {
                    follows = follows.Count > 0 ? 
                        follows.Union(_followTable.nodes[item].follows).ToList() : 
                                                                _followTable.nodes[item].follows;
                }
            }
            follows.Sort();

            return new Transition(symbol, follows);
        }

        /// <summary>
        /// Check if List of states already contains the new states
        /// </summary>
        private bool StateContainsNodes(List<int> nodes)
        {
            foreach (var state in states)
            {
                if (state.All(nodes.Contains) && state.Count == nodes.Count)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
