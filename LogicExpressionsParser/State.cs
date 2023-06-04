using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicExpressionsParser
{
    public class State
    {
        public List<string> fluentNames = new List<string>();
        public bool forbidden = false; // czy stan jest niedozwolony (nie spelnia always costam)

        public readonly bool[] fluents; // jak ustawione sa fluenty w danym stanie
        public List<State>[] possibleEffects; // lista wszystkich możliwych bezpośrednich efektow danej akcji danego agenta w tym stanie
        public List<int>[] costs; // koszty którymi można dostać się do odpowiadających stanów
        public List<State>[] typicalEffects; // lista typowych efektów bezpośrednich
        public List<State>[] abnormalEffects; // lista efektów nietypowych

        public State(bool[] fluents, int actions, List<string> fluentNames)
        {
            this.fluents = new bool[fluents.Length];
            for (int i = 0; i < fluents.Length; i++) this.fluents[i] = fluents[i];
            possibleEffects = new List<State>[actions];
            costs = new List<int>[actions];
            typicalEffects = new List<State>[actions];
            abnormalEffects = new List<State>[actions];
            this.fluentNames = fluentNames;

            for (int i = 0; i < actions; i++)
            {
                possibleEffects[i] = new List<State>();
                typicalEffects[i] = new List<State>();
                abnormalEffects[i] = new List<State>();
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
                result += fluentNames[i] + ": ";
                if (fluents[i]) result += 1 + " ";
                else result += 0 + " ";
            }
            result += ")\n";
            return result;
        }

    }

    public class StateEqualityComparer : IEqualityComparer<State>
    {
        public bool Equals(State x, State y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            if (x.fluents.Length != y.fluents.Length)
                return false; // shouldnt happen

            for (int i = 0; i < x.fluents.Length; i++)
            {
                if (x.fluents[i] != y.fluents[i])
                    return false;
            }

            return true;
        }

        public int GetHashCode(State obj)
        {
            if (ReferenceEquals(obj, null))
                return 0;

            int hash = 17;
            foreach (bool variable in obj.fluents)
            {
                hash = hash * 23 + variable.GetHashCode();
            }

            return hash;
        }
    }
}
