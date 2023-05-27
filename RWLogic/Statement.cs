using System.Collections.Generic;
using LogicExpressionsParser;

namespace RWLogic
{
    public class Always
    {
        public Formula condition { get; }

        public Always(Formula condition)
        {
            this.condition = condition;
        }
    }

    public class Noninertial
    {
        public int fluent { get; }

        public Noninertial(int fluent)
        {
            this.fluent = fluent;
        }
    }

    public interface CausesOrTypicallyCauses
    {
        int action { get; } //id akcji
        Formula condition { get; } 
        Formula effect { get; } 
    }

    public class Causes : CausesOrTypicallyCauses
    {
        public int action { get; } //id akcji
        public Formula condition { get; } 
        public Formula effect { get; } 

        public Causes(int action, Formula effect, Formula condition)
        {
            this.action = action;
            this.condition = condition;
            this.effect = effect;
        }

        public Causes(int action, Formula effect)
        {
            this.action = action;
            this.condition = new Formula();
            this.effect = effect;
        }
    }

    public class TypicallyCauses : CausesOrTypicallyCauses
    {
        public int action { get; } //id akcji
        public Formula condition { get; } 
        public Formula effect { get; } 

        public TypicallyCauses(int action, Formula effect, Formula condition)
        {
            this.action = action;
            this.condition = condition;
            this.effect = effect;
        }

        public TypicallyCauses(int action, Formula effect)
        {
            this.action = action;
            this.condition = new Formula();
            this.effect = effect;
        }
    }

    public interface ReleasesOrTypicallyReleases
    {
        int action { get; } //id akcji
        Formula condition { get; }
        int fluent { get; }
    }

    public class Releases : ReleasesOrTypicallyReleases
    {
        public int action { get; } //id akcji
        public Formula condition { get; }
        public int fluent { get; }

        public Releases(int action, int fluent, Formula condition)
        {
            this.action = action;
            this.condition = condition;
            this.fluent = fluent;
        }

        public Releases(int action, int fluent)
        {
            this.action = action;
            this.condition = new Formula();
            this.fluent = fluent;
        }

    }

    public class TypicallyReleases : ReleasesOrTypicallyReleases
    {
        public int action { get; } //id akcji
        public Formula condition { get; } 
        public int fluent { get; }

        public TypicallyReleases(int action, int fluent, Formula condition)
        {
            this.action = action;
            this.condition = condition;
            this.fluent = fluent;
        }

        public TypicallyReleases(int action, int fluent)
        {
            this.action = action;
            this.condition = new Formula();
            this.fluent = fluent;
        }

    }

    public class Initially
    {
        public Formula condition { get; }

        public Initially(Formula condition)
        {
            this.condition = condition;
        }
    }

    public class After
    {
        public List<int> activity { get; }
        public Formula effect { get; }

        public After(List<int> activity, Formula effect)
        {
            this.activity = activity;
            this.effect = effect;
        }

    }

    public class TypicallyAfter
    {
        public List<int> activity { get; }
        public Formula effect { get; }

        public TypicallyAfter(List<int> activity, Formula effect)
        {
            this.activity = activity;
            this.effect = effect;
        }

    }

    public class ObservableAfter
    {
        public List<int> activity { get; }
        public Formula effect { get; }

        public ObservableAfter(List<int> activity, Formula effect)
        {
            this.activity = activity;
            this.effect = effect;
        }
    }

    // dalej syntaktyka kwerend

    public class Query_ExecutableAlways
    {
        public List<int> program { get; } //program dzialan

        public Query_ExecutableAlways(List<int> program)
        {
            this.program = program;
        }
    }
    
    public class Query_ExecutableEver
    {
        public List<int> program { get; } //program dzialan

        public Query_ExecutableEver(List<int> program)
        {
            this.program = program;
        }
    }

    public class Query_AccessibleAlways
    {
        public List<int> program { get; } //program dzialan
        public Formula initialCondition { get; }
        public Formula endCondition { get; }

        public Query_AccessibleAlways(List<int> program, Formula initialCondition, Formula endCondition)
        {
            this.program = program;
            this.initialCondition = initialCondition;
            this.endCondition = endCondition;
        }
    }

    public class Query_AccessibleTypically
    {
        public List<int> program { get; } //program dzialan
        public Formula initialCondition { get; }
        public Formula endCondition { get; }

        public Query_AccessibleTypically(List<int> program, Formula initialCondition, Formula endCondition)
        {
            this.program = program;
            this.initialCondition = initialCondition;
            this.endCondition = endCondition;
        }
    }

    public class Query_AccessibleEver
    {
        public List<int> program { get; } //program dzialan
        public Formula initialCondition { get; }
        public Formula endCondition { get; }

        public Query_AccessibleEver(List<int> program, Formula initialCondition, Formula endCondition)
        {
            this.program = program;
            this.initialCondition = initialCondition;
            this.endCondition = endCondition;
        }
    }

    public class Query_InvolvedAlways
    {
        public List<int> program { get; } //program dzialan
        public int agent { get; } //id agenta

        public Query_InvolvedAlways(List<int> program, int agent)
        {
            this.program = program;
            this.agent = agent;
        }
    }

    public class Query_InvolvedEver
    {
        public List<int> program { get; } //program dzialan
        public int agent { get; } //id agenta

        public Query_InvolvedEver(List<int> program, int agent)
        {
            this.program = program;
            this.agent = agent;
        }
    }
}
