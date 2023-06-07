using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWProgram.Classes
{
    public abstract class Query
    {
        public abstract string ToString(List<Action> program);
    }

    public class AlwaysAfter : Query
    {
        public State Alpha { get; set; }

        public State Pi { get; set; }

        public override string ToString()
        {
            return $"Always {Alpha} after... from {Pi}";
        }

        public override string ToString(List<Action> program)
        {
            var programString = string.Join(", ", program.Select(a => a.Name)) != "" ?
                string.Join(", ", program.Select(a => a.Name)) : "[empty program]";
            return $"Always {Alpha} after {programString} from {Pi}";
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

        public override string ToString(List<Action> program)
        {
            var programString = string.Join(", ", program.Select(a => a.Name)) != "" ?
                string.Join(", ", program.Select(a => a.Name)) : "[empty program]";
            return $"Possibly {Alpha} after {programString} from {Pi}";
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
        public AlwaysExecutable()
        {
            
        }
        public AlwaysExecutable(State pi, int cost)
        {
            Pi = pi;
            Cost = cost;
        }

        public State Pi { get; set; }
        public int Cost { get; set; }
        public override string ToString()
        {
            return $"Is program necessarily executable" + (!string.IsNullOrEmpty(Pi?.ToString()) ? $" from {Pi}" : string.Empty) + $" cost {Cost}";
        }

        public override string ToString(List<Action> program)
        {
            var programString = string.Join(", ", program.Select(a => a.Name)) != "" ?
                string.Join(", ", program.Select(a => a.Name)) : "[empty program]";
            return $"Is program {programString} necessarily executable" + (!string.IsNullOrEmpty(Pi?.ToString()) ? $" from {Pi}" : string.Empty) + $" cost {Cost}";
        }
    }

    public class EverExecutable : Query
    {
        public EverExecutable()
        {
            
        }
        public EverExecutable(State pi, int cost)
        {
            Pi = pi;
            Cost = cost;
        }

        public State Pi { get; set; }
        public int Cost { get; set; }
        public override string ToString()
        {
            return $"Is program possibly executable " + (!string.IsNullOrEmpty(Pi?.ToString()) ? $" from {Pi} " : string.Empty) + $"cost {Cost}";
        }

        public override string ToString(List<Action> program)
        {
            var programString = string.Join(", ", program.Select(a => a.Name)) != "" ?
                string.Join(", ", program.Select(a => a.Name)) : "[empty program]";
            return $"Is program {programString} possibly executable" + (!string.IsNullOrEmpty(Pi?.ToString()) ? $" from {Pi}" : string.Empty) + $" cost {Cost}";
        }
    }

    public class AlwaysAccesibleYFromPi : QueryWithGammaAndPi
    {
        public AlwaysAccesibleYFromPi(State Gamma, State Pi, int Cost) : base(Gamma, Pi, Cost) { }
        public override string ToString()
        {
            var str = $"Is {Gamma.ToString()} necessarily accessible";
            if (!string.IsNullOrEmpty(Pi.ToString()))
                str = str + $" from {Pi.ToString()}";
            str += $" cost {Cost}";
            return str;
        }

        public override string ToString(List<Action> program)
        {
            return ToString();
        }
    }

    public class EverAccesibleYFromPi : QueryWithGammaAndPi
    {
        public EverAccesibleYFromPi(State Gamma, State Pi, int Cost) : base(Gamma, Pi, Cost) { }

        public override string ToString()
        {
            var str = $"Is {Gamma.ToString()} possibly accessible";
            if (!string.IsNullOrEmpty(Pi.ToString()))
                str = str + $" from {Pi.ToString()}";
            str += $" cost {Cost}";
            return str;
        }

        public override string ToString(List<Action> program)
        {
            return ToString();
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

        public override string ToString(List<Action> program)
        {
            return ToString();
        }
    }
}
