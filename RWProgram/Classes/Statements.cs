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

    public abstract class ConditionActionByActorStatement : ConditionStatement
    {
        public Action Action { get; set; }

        public Actor Actor { get; set; }

        public ConditionActionByActorStatement(Action Action, Actor Actor, State Pi) : base(Pi)
        {
            this.Action = Action;
            this.Actor = Actor;
        }
    }

    public abstract class StatementAfterActionByUser : Statement
    {
        public List<(Action action, Actor actor)> ActionsByActors { get; set; }

        public StatementAfterActionByUser(List<(Action action, Actor actor)> actionsByActors)
        {
            this.ActionsByActors = actionsByActors == null ? new List<(Action action, Actor actor)>() :
                    actionsByActors.Where(a => a.action != null && a.actor != null).ToList();
        }

    }

    //Same as FluentAfterActionbyActor but actors and action empty
    public class InitiallyFluent : FluentAfterActionbyActor
    {
        public InitiallyFluent(State Alpha) : base(Alpha, new List<(Action action, Actor actor)>())
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

        public FluentAfterActionbyActor(State Alpha, List<(Action action, Actor actor)> actionsByActors) : base(actionsByActors) 
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var str = $"{Alpha.ToString()} after";
            for (var i = 0; i < ActionsByActors.Count; i++)
            {
                var comma = i != 0 ? "," : "";
                str = str + $"{comma} {ActionsByActors[i].action} by {ActionsByActors[i].actor}";
            }
            return str;
        }

        public override object ToLogic()
        {
            return new RWLogic.After(ActionsByActors.Select(actionByActor => (actionByActor.actor.Index, actionByActor.action.Index)).ToList(), Alpha.ToLogic());
        }
    }

    public class FluentTypicallyAfterActionbyActor : StatementAfterActionByUser
    {
        public State Alpha { get; set; }

        public FluentTypicallyAfterActionbyActor(State Alpha, List<(Action action, Actor actor)> actionsByActors) : base(actionsByActors)
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var str = $"{Alpha} typically after";
            for (var i = 0; i < ActionsByActors.Count; i++)
            {
                var comma = i != 0 ? "," : "";
                str = str + $"{comma} {ActionsByActors[i].action} by {ActionsByActors[i].actor}";
            }
            return str;
        }

        public override object ToLogic()
        {
            return new RWLogic.TypicallyAfter(ActionsByActors.Select(actionByActor => (actionByActor.actor.Index, actionByActor.action.Index)).ToList(), Alpha.ToLogic());
        }
    }

    public class ObservableFluentAfterActionByActor : StatementAfterActionByUser
    {
        public State Alpha { get; set; }

        public ObservableFluentAfterActionByActor(State Alpha, List<(Action action, Actor actor)> actionsByActors) : base(actionsByActors)
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var str = $"observable {Alpha.ToString()} after";
            for (var i = 0; i < ActionsByActors.Count; i++)
            {
                var comma = i != 0 ? "," : "";
                str = str + $"{comma} {ActionsByActors[i].action} by {ActionsByActors[i].actor}";
            }
            return str;
        }

        public override object ToLogic()
        {
            return new RWLogic.ObservableAfter(ActionsByActors.Select(actionByActor => (actionByActor.actor.Index, actionByActor.action.Index)).ToList(), Alpha.ToLogic());
        }
    }

    public class ActionByActorCausesAlphaIfFluents : ConditionActionByActorStatement
    {
        public State Alpha { get; set; }


        public ActionByActorCausesAlphaIfFluents(State Alpha, Action Action, Actor Actor, State Pi) : base(Action ,Actor, Pi)
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var conditionStr = string.IsNullOrEmpty(Pi.ToString()?.Trim()) ? string.Empty : $"if { Pi.ToString()}";
            return $"{Action} by {Actor} casues {Alpha.ToString()} {conditionStr}";
        }

        public override object ToLogic()
        {
            return new RWLogic.Causes(Actor.Index, Action.Index, Alpha.ToLogic(), Pi.ToLogic());
        }
    }

    public class ActionByActorReleasesFluent1IfFluents : ConditionActionByActorStatement
    {
        public Fluent F { get; set; }

        public ActionByActorReleasesFluent1IfFluents(Fluent F, Action Action, Actor Actor, State Pi) : base(Action ,Actor, Pi)
        {
            this.F = F;
        }

        public override string ToString()
        {
            var conditionStr = string.IsNullOrEmpty(Pi?.ToString().Trim()) ? string.Empty : $"if {Pi}";
            return $"{Action} by {Actor} releases {F} {conditionStr}";
        }

        public override object ToLogic()
        {
            return new RWLogic.Releases(Actor.Index, Action.Index, F.Index, Pi.ToLogic());
        }
    }

    public class ActionByActorTypicallyCausesAlphaIfFluents : ConditionActionByActorStatement
    {
        public State Alpha { get; set; }

        public ActionByActorTypicallyCausesAlphaIfFluents(State Alpha, Action Action, Actor Actor, State Pi) : base(Action, Actor, Pi)
        {
            this.Alpha = Alpha;
        }

        public override string ToString()
        {
            var conditionStr = string.IsNullOrEmpty(Pi?.ToString().Trim()) ? string.Empty : $"if { Pi}";
            return $"{Action} by {Actor} typically casues {Alpha} {conditionStr}";
        }

        public override object ToLogic()
        {
            return new RWLogic.TypicallyCauses(Actor.Index, Action.Index, Alpha.ToLogic(), Pi.ToLogic());
        }
    }

    public class ActionByActorTypicallyReleasesFluent1IfFluents : ConditionActionByActorStatement
    {
        public Fluent F { get; set; }

        public ActionByActorTypicallyReleasesFluent1IfFluents(Fluent F, Action Action, Actor Actor, State Pi) : base(Action, Actor, Pi)
        {
            this.F = F;
        }

        public override string ToString()
        {
            var conditionStr = string.IsNullOrEmpty(Pi?.ToString().Trim()) ? string.Empty : $"if { Pi}";
            return $"{Action} by {Actor} typically releases {F} {conditionStr}";
        }

        public override object ToLogic()
        {
            return new RWLogic.TypicallyReleases(Actor.Index, Action.Index, F.Index, Pi.ToLogic());
        }
    }

    public class ImpossibleActionByActorIfFluents : ActionByActorCausesAlphaIfFluents
    {
        public ImpossibleActionByActorIfFluents(Action Action, Actor Actor, State Pi) : base(new State(), Action, Actor, Pi) { }

        public override string ToString()
        {
            var conditionStr = string.IsNullOrEmpty(Pi?.ToString().Trim()) ? string.Empty : $"if { Pi.ToString()}";
            return $"impossible {Action} by {Actor} {conditionStr}";
        }

        public override object ToLogic()
        {
            return new RWLogic.Causes(Actor.Index, Action.Index, new Formula(), Pi.ToLogic());
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
