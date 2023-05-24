using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWProgram.Classes
{
    public class Fluent
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public virtual bool IsNegation { get { return false; } }

        public override string ToString()
        {
            return Name;
        }

        public virtual (int, bool) ToLogic()
        {
            return (Index, true);
        }
    }

    public class NegatedFluent : Fluent
    {
        public Fluent Original { get; set; }
        public override bool IsNegation { get { return true; } }

        public override string ToString()
        {
            return "not " + Name;
        }
        public override (int, bool) ToLogic()
        {
            return (Index, false);
        }
    }
}
