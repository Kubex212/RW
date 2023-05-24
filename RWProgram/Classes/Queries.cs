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

    public abstract class QueryWithGammaAndPi : Query
    {
        public State Gamma { get; set; }
        public State Pi { get; set; }

        public QueryWithGammaAndPi(State Gamma, State Pi)
        {
            this.Gamma = Gamma;
            this.Pi = Pi;
        }
    }

    public abstract class QueryWithW : Query
    {
        public Actor W { get; set; }
    }

    public class AlwaysExecutable : Query
    {
        public override string ToString()
        {
            return "Is program always executable";
        }
    }

    public class EverExecutable : Query
    {
        public override string ToString()
        {
            return "Is program ever executable";
        }
    }

    public class AlwaysAccesibleYFromPi : QueryWithGammaAndPi
    {
        public AlwaysAccesibleYFromPi(State Gamma, State Pi) : base(Gamma, Pi) { }
        public override string ToString()
        {
            var str = $"Is {Gamma.ToString()} always accessible";
            if (!string.IsNullOrEmpty(Pi.ToString()))
                str = str + $" from {Pi.ToString()}";
            return str;
        }
    }

    public class EverAccesibleYFromPi : QueryWithGammaAndPi
    {
        public EverAccesibleYFromPi(State Gamma, State Pi) : base(Gamma, Pi) { }

        public override string ToString()
        {
            var str = $"Is {Gamma.ToString()} ever accessible";
            if (!string.IsNullOrEmpty(Pi.ToString()))
                str = str + $" from {Pi.ToString()}";
            return str;
        }
    }

    public class TypicallyAccesibleYFromPi : QueryWithGammaAndPi
    {
        public TypicallyAccesibleYFromPi(State Gamma, State Pi) : base(Gamma, Pi) { }

        public override string ToString()
        {
            var str = $"Is {Gamma.ToString()} typically accessible";
            if (!string.IsNullOrEmpty(Pi.ToString()))
                str = str + $" from {Pi.ToString()}";
            return str;
        }
    }

    public class AlwaysWInvolved : QueryWithW
    {
        public override string ToString()
        {
            return $"Always {W.ToString()} involved";
        }
    }

    public class EverWInvolved : QueryWithW
    {
        public override string ToString()
        {
            return $"Ever {W.ToString()} involved";
        }
    }
}
