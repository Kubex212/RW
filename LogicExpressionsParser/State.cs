using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicExpressionsParser
{
    public class State
    {
        public bool forbidden = false; // czy stan jest niedozwolony (nie spelnia always costam)

        public readonly bool[] fluents; // jak ustawione sa fluenty w danym stanie
        public List<State>[,] possibleEffects; // lista wszystkich możliwych bezpośrednich efektow danej akcji danego agenta w tym stanie
        public List<State>[,] typicalEffects; // lista typowych efektów bezpośrednich
        public List<State>[,] abnormalEffects; // lista efektów nietypowych

        public State(bool[] fluents, int agents, int actions)
        {
            this.fluents = new bool[fluents.Length];
            for (int i = 0; i < fluents.Length; i++) this.fluents[i] = fluents[i];
            possibleEffects = new List<State>[agents, actions];
            typicalEffects = new List<State>[agents, actions];
            abnormalEffects = new List<State>[agents, actions];

            for (int i = 0; i < agents; i++)
            {
                for (int j = 0; j < actions; j++)
                {
                    possibleEffects[i, j] = new List<State>();
                    typicalEffects[i, j] = new List<State>();
                    abnormalEffects[i, j] = new List<State>();
                }
            }
        }

        public bool SatisfiesCondition(Formula condition) // czy stan spelnia warunek
        {
            return condition.CheckCondition(this);
            //if (conditions[0] == 2) return false;
            //for (int i = 0; i < fluents.Length; i++)
            //{
            //    if (conditions[i] == -1) continue;
            //    if (conditions[i] == 1 && fluents[i] == false) return false;
            //    if (conditions[i] == 0 && fluents[i] == true) return false;
            //}
            //return true;
        }

        public string Print()
        {
            string result = "(";
            for (int i = 0; i < fluents.Length; i++)
            {
                if (fluents[i]) result += 1 + " ";
                else result += 0 + " ";
            }
            result += ")\n";
            return result;
        }

    }
}
