using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicExpressionsParser;

namespace RWProgram.Classes
{
    public abstract class Statement
    {
        public override string ToString() { return string.Empty; }

        public abstract object ToLogic();

        public virtual T Clone<T>() where T: Statement { return (T)MemberwiseClone(); }
    }

    public abstract class ConditionStatement : Statement
    {

        public State Pi { get; set; }

        public ConditionStatement(State Pi)
        {
            this.Pi = Pi;
        }
    }

    public abstract class ConditionActionStatement : ConditionStatement
    {
        public Action Action { get; set; }

        public int Cost { get; set; } = 1;


        public ConditionActionStatement(Action Action, State Pi, int Cost) : base(Pi)
        {
            this.Action = Action;
            this.Cost = Cost;
        }
    }

    public abstract class StatementAfterActionByUser : Statement
    {
        public List<Action> Actions { get; set; }

        public StatementAfterActionByUser(List<Action> actions)
        {
            this.Actions = actions == null ? new List<Action>() :
                    actions.Where(a => a != null).ToList();
        }

    }

    //Same as FluentAfterActionbyActor but actors and action empty
    public class InitiallyFluent : FluentAfterActionbyActor
    {
        public InitiallyFluent(State Alpha) : base(Alpha, new List<Action>())
        { }

        public override string ToString()
        {
            return $"initially {Alpha.ToString()}";
        }

        public override object ToLogic()
        {
            return new RWLogic.Initially(Alpha.ToLogic());
        }
    }

    public class FluentAfterActionbyActor : StatementAfterActionByUser
    {
        public State Alpha { get; set; }

        public FluentAfterActionbyActor(State Alpha, List<Action> actions) : base(actions) 
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var str = $"{Alpha.ToString()} after";
            for (var i = 0; i < Actions.Count; i++)
            {
                var comma = i != 0 ? "," : "";
                str = str + $"{comma} {Actions[i]}";
            }
            return str;
        }

        public override object ToLogic()
        {
            return new RWLogic.After(Actions.Select(action => action.Index).ToList(), Alpha.ToLogic());
        }
    }

    public class FluentTypicallyAfterAction : StatementAfterActionByUser
    {
        public State Alpha { get; set; }

        public FluentTypicallyAfterAction(State Alpha, List<Action> actions) : base(actions)
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var str = $"{Alpha} typically after";
            for (var i = 0; i < Actions.Count; i++)
            {
                var comma = i != 0 ? "," : "";
                str = str + $"{comma} {Actions[i]}";
            }
            return str;
        }

        public override object ToLogic()
        {
            return new RWLogic.TypicallyAfter(Actions.Select(action => action.Index).ToList(), Alpha.ToLogic());
        }
    }

    public class ObservableFluentAfterAction : StatementAfterActionByUser
    {
        public State Alpha { get; set; }

        public ObservableFluentAfterAction(State Alpha, List<Action> actions) : base(actions)
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var str = $"observable {Alpha.ToString()} after";
            for (var i = 0; i < Actions.Count; i++)
            {
                var comma = i != 0 ? "," : "";
                str = str + $"{comma} {Actions[i]}";
            }
            return str;
        }

        public override object ToLogic()
        {
            return new RWLogic.ObservableAfter(Actions.Select(action => action.Index).ToList(), Alpha.ToLogic());
        }
    }

    public class ActionCausesAlphaIfFluents : ConditionActionStatement
    {
        public State Alpha { get; set; }


        public ActionCausesAlphaIfFluents(State Alpha, Action Action, State Pi, int Cost) : base(Action, Pi, Cost)
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var conditionStr = string.IsNullOrEmpty(Pi.ToString()?.Trim()) ? string.Empty : $"if { Pi.ToString()}";
            return $"{Action} casues {Alpha.ToString()} {conditionStr} cost {Cost}";
        }

        public override object ToLogic()
        {
            return new RWLogic.Causes(Action.Index, Alpha.ToLogic(), Pi.ToLogic(), Cost);
        }
    }

    public class ActionReleasesFluent1IfFluents : ConditionActionStatement
    {
        public Fluent F { get; set; }

        public ActionReleasesFluent1IfFluents(Fluent F, Action Action, State Pi, int Cost) : base(Action, Pi, Cost)
        {
            this.F = F;
        }

        public override string ToString()
        {
            var conditionStr = string.IsNullOrEmpty(Pi?.ToString().Trim()) ? string.Empty : $"if {Pi}";
            return $"{Action} releases {F} {conditionStr} cost {Cost}";
        }

        public override object ToLogic()
        {
            return new RWLogic.Releases(Action.Index, F.Index, Pi.ToLogic(), Cost);
        }
    }

    public class ImpossibleActionIfFluents : ActionCausesAlphaIfFluents
    {
        public ImpossibleActionIfFluents(Action Action, State Pi, int Cost) : base(new State(), Action, Pi, Cost) { }

        public override string ToString()
        {
            var conditionStr = string.IsNullOrEmpty(Pi?.ToString().Trim()) ? string.Empty : $"if { Pi.ToString()}";
            return $"impossible {Action} {conditionStr}";
        }

        public override object ToLogic()
        {
            return new RWLogic.Causes(Action.Index, new Formula(), Pi.ToLogic(), Cost);
        }
    }

    public class AlwaysPi : ConditionStatement
    {
        public AlwaysPi(State Pi) : base(Pi) { }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Pi.ToString())) return string.Empty;

            return $"always {Pi.ToString()}";
        }
        public override object ToLogic()
        {
            return new RWLogic.Always(Pi.ToLogic());
        }
    }

    public class NoninertialFluent : Statement
    {
        public Fluent Fluent { get; set; }

        public NoninertialFluent(Fluent Fluent)
        {
            this.Fluent = Fluent;
        }

        public override string ToString()
        {
            return $"noninertial {Fluent}";
        }

        public override object ToLogic()
        {
            return new RWLogic.Noninertial(Fluent.Index);
        }
    }
}
