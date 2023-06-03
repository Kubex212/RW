using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWProgram.Classes
{
    public abstract class Query
    {

    }

    public class AlwaysAfter : Query
    {
        public State Alpha { get; set; }

        public State Pi { get; set; }

        public override string ToString()
        {
            return $"Always {Alpha} after... from {Pi}";
        }
    }

    public class PossiblyAfter : Query
    {
        public State Alpha { get; set; }

        public State Pi { get; set; }

        public override string ToString()
        {
            return $"Possibly {Alpha} after... from {Pi}";
        }
    }

    public abstract class QueryWithGammaAndPi : Query
    {
        public State Gamma { get; set; }
        public State Pi { get; set; }

        public int Cost { get; set; }

        public QueryWithGammaAndPi(State Gamma, State Pi, int Cost)
        {
            this.Gamma = Gamma;
            this.Pi = Pi;
            this.Cost = Cost;
        }
    }

    public class AlwaysExecutable : Query
    {
        public State Pi { get; set; }
        public int Cost { get; set; }
        public override string ToString()
        {
            return $"Is program always executable from {Pi} cost {Cost}";
        }
    }

    public class EverExecutable : Query
    {
        public State Pi { get; set; }
        public int Cost { get; set; }
        public override string ToString()
        {
            return $"Is program possibly executable from {Pi} cost {Cost}";
        }
    }

    public class AlwaysAccesibleYFromPi : QueryWithGammaAndPi
    {
        public AlwaysAccesibleYFromPi(State Gamma, State Pi, int Cost) : base(Gamma, Pi, Cost) { }
        public override string ToString()
        {
            var str = $"Is {Gamma.ToString()} always accessible";
            if (!string.IsNullOrEmpty(Pi.ToString()))
                str = str + $" from {Pi.ToString()} cost {Cost}";
            return str;
        }
    }

    public class EverAccesibleYFromPi : QueryWithGammaAndPi
    {
        public EverAccesibleYFromPi(State Gamma, State Pi, int Cost) : base(Gamma, Pi, Cost) { }

        public override string ToString()
        {
            var str = $"Is {Gamma.ToString()} possibly accessible";
            if (!string.IsNullOrEmpty(Pi.ToString()))
                str = str + $" from {Pi.ToString()} cost {Cost}";
            return str;
        }
    }

    public class TypicallyAccesibleYFromPi : QueryWithGammaAndPi
    {
        public TypicallyAccesibleYFromPi(State Gamma, State Pi, int Cost) : base(Gamma, Pi, Cost) { }

        public override string ToString()
        {
            var str = $"Is {Gamma.ToString()} typically accessible";
            if (!string.IsNullOrEmpty(Pi.ToString()))
                str = str + $" from {Pi.ToString()}";
            return str;
        }
    }
}
