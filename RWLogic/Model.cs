using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicExpressionsParser;

namespace RWLogic
{
    public class Model
    {
        public string[] fluent { get; } // fluenty z nazwami
        public bool[] noninertial { get; private set; } // ktore fluenty sa nieinercjalne
        public State[] States { get; private set; } // tablica wszystkich mozliwych stanow
        public List<State> initial { get; private set; } // lista stanow poczatkowych
        public string[] action { get; } // akcje z nazwami
        public List<Causes> causes { get; private set; }
        public List<Releases> releases { get; private set; }

        // generujemy zbior wszystkich mozliwych stanow i takie tam
        public Model(List<string> fluent, List<string> action)
        {
            initial = new List<State>();
            int stateCount = (int)Math.Pow(2, fluent.Count);
            bool[] fluentValues = new bool[fluent.Count];
            noninertial = new bool[fluent.Count];
            int t = 0;
            States = new State[stateCount];
            for(int i = 0; i < stateCount; i++)
            {
                for(int j=0; j < fluent.Count; j++)
                {
                    t = i; //t = stateCount;
                    t %= ((int)Math.Pow(2, j+1));
                    t /= ((int)Math.Pow(2, j));
                    fluentValues[j] = t==1 ? true : false;
                }
                States[i] = new State(fluentValues, action.Count, fluent);
            }
            this.fluent = new string[fluent.Count];
            this.action = new string[action.Count];
            for (int i = 0; i < action.Count; i++) this.action[i] = action[i];
            for (int i = 0; i < fluent.Count; i++) this.fluent[i] = fluent[i];
        }

        // nastepnie przetwarzanie zdan roznych typow w calu wygenerowania odpowiedniego grafu

        public void SetNoninertial(List<Noninertial> noninertial)
        {
            foreach(Noninertial s in noninertial)
                this.noninertial[s.fluent] = true;
            return;
        }

        public void SetAlways(List<Always> always)
        {
            foreach(State s in States)
            {
                foreach(Always a in always)
                {
                    if(!s.SatisfiesCondition(a.condition))
                        s.forbidden = true;
                }
            }
            var forbiddens = States.Where(s => s.forbidden).ToList();
            return;
        }

        public void SetPossibleEffects(List<Causes> causes, List<Releases> releases, List<Impossible> impossibles)
        {
            this.releases = releases;
            this.causes = causes;
            foreach(State s in States)
            {
                if (!s.forbidden)
                    for(int j=0;j<action.Length; j++)
                    {
                        var res = Res(s, j, causes, releases);
                        s.possibleEffects[j] = res.Item1;
                        s.costs[j] = res.Item2;
                    }
            }
            foreach(var impossible in impossibles)
            {
                var states = States.Where(s => s.SatisfiesCondition(impossible.condition));
                foreach(var state in states)
                {
                    state.possibleEffects[impossible.action].Clear();
                }
            }
            return;
        }

        public void SetAbnormalEffects()
        {
            List<State> result = new List<State>();
            foreach (State s in States)
            {
                if (!s.forbidden)
                    for (int j = 0; j < action.Length; j++)
                    {
                        result.Clear();
                        foreach(State res in s.possibleEffects[j])
                        {
                            if (!s.typicalEffects[j].Contains(res)) result.Add(res);
                        }
                        //result.AddRange(s.possibleEffects[i, j]);
                        //foreach(State state in result)
                        //{
                        //    if (s.typicalEffects[i, j].Contains(state)) result.Remove(state);
                        //}
                        s.abnormalEffects[j].AddRange(result);
                    }
            }
            return;
        }

        public void SetInitialStates(List<Initially> initially, List<After> after, List<ObservableAfter> observableAfter)
        {
            List<State> result = new List<State>();

            //initially
            if (initially.Count != 0)
            {
                foreach (State s in States)
                {
                    initial.Add(s);
                    foreach (Initially statement in initially)
                    {
                        if (!s.SatisfiesCondition(statement.condition)) initial.Remove(s);
                    }
                }

            }
            else initial.AddRange(States);

            //after
            if (after.Count != 0)
            {
                foreach (State s in initial)
                {
                    result.Add(s);
                    bool end = false;
                    foreach (After statement in after)
                    {
                        if (end) break;
                        Stack<(State state, int i)> DFS = new Stack<(State state, int i)>();
                        (State state, int index) current;
                        DFS.Push((s, 0));
                        while (DFS.Count != 0)
                        {
                            current = DFS.Pop();
                            if (current.index == statement.activity.Count)
                            {
                                if (!current.state.SatisfiesCondition(statement.effect))
                                {
                                    result.Remove(s);
                                    end = true;
                                    break;
                                }
                            }
                            else foreach (State res in current.state.possibleEffects[statement.activity[current.index]])
                            {
                                DFS.Push((res, current.index + 1));
                            }
                        }

                    }
                }
                initial.Clear();
                initial.AddRange(result);
                result.Clear();
            }

            //observable
            if(observableAfter.Count != 0)
            {
                foreach (State s in initial)
                {
                    result.Add(s);
                    bool end = false;
                    foreach (ObservableAfter statement in observableAfter)
                    {
                        if (end) break;
                        Stack<(State state, int i)> DFS = new Stack<(State state, int i)>();
                        (State state, int index) current;
                        DFS.Push((s, 0));
                        while (DFS.Count != 0)
                        {
                            current = DFS.Pop();
                            if (current.index == statement.activity.Count)
                            {
                                if (current.state.SatisfiesCondition(statement.effect))
                                {
                                    end = true;
                                    break;
                                }
                            }
                            else foreach (State res in current.state.possibleEffects[statement.activity[current.index]])
                            {
                                DFS.Push((res, current.index + 1));
                            }
                        }
                        if (!end) result.Remove(s);
                    }
                }
                initial.Clear();
                initial.AddRange(result);
            }


            return;
        }

        // tutaj mozna wrzucic metody, ktore przetwarzaja kwerendy, przykladowo:
        /*
        public bool IsAlwaysExecutable(Query_ExecutableAlways query)
        {
            costam costam;
        }

        public bool IsEverAccessible(Query_AccessibleEver query)
        {
            // wydaje mi sie, ze tutaj trzeba wyjsc ze stanow poczatkowych
            // przejsc do kolejnych stanow za pomoca state.CertainEffect[agent, akcja]
            // i costam sprawdzic za pomoca state.SatisfiesCondtion(stan koncowy), ale pewien nie jestem
            costam costam;
        }
        */

        public bool AlwaysAfter2(Query_NecessaryAfter query)
        {
            var initialStates = new List<State>();
            if (query.InitialCondition.EmptyRoot)
            {
                initialStates = initial;
            }
            else
            {
                initialStates = States.Where(s => s.SatisfiesCondition(query.InitialCondition) && !s.forbidden).ToList();
            }

            var results = new List<bool>();
            foreach(var initialState in initialStates)
            {
                // bierzemy po kolei mo¿liwe stany pocz¹tkowe i symulujemy na nich wykonanie programu
                var currentStates = new List<State>() { initialState };
                var nextStates = new List<State>();
                foreach (var action in query.program)
                {
                    foreach (var state in currentStates)
                    {
                        if (state.forbidden) return false;
                        List<State> possibleNextStates = state.possibleEffects[action];
                        if (possibleNextStates.Count == 0) return false;
                        nextStates.AddRange(possibleNextStates);
                    }

                    currentStates = nextStates;
                    nextStates = new List<State>();
                }
                // sprawdzamy czy otrzymane stany po przejœciu programu zawieraj¹ tylko stany spe³niaj¹ce warunek koñcowy
                results.Add(currentStates.Count > 0 && currentStates.All(s => s.SatisfiesCondition(query.FinalCondition)));
            }
            // gdyby results okaza³o siê puste (np. initialStates by³oby puste) to jaka jest odpowiedŸ kwerendy?
            // chyba true bo "dla ka¿dego modelu..." a zbiór modeli by³by pusty
            return results.All(r => r == true);
        }

        public bool PossiblyAfter2(Query_PossiblyAfter query)
        {
            var initialStates = new List<State>();
            if (query.InitialCondition.EmptyRoot)
            {
                initialStates = initial;
            }
            else
            {
                initialStates = States.Where(s => s.SatisfiesCondition(query.InitialCondition) && !s.forbidden).ToList();
            }

            var results = new List<bool>();
            foreach (var initialState in initialStates)
            {
                // bierzemy po kolei mo¿liwe stany pocz¹tkowe i symulujemy na nich wykonanie programu
                var currentStates = new List<State>() { initialState };
                var nextStates = new List<State>();
                foreach (var action in query.program)
                {
                    foreach (var state in currentStates)
                    {
                        if (state.forbidden) continue;
                        List<State> possibleNextStates = state.possibleEffects[action];
                        if (possibleNextStates.Count == 0) continue;
                        nextStates.AddRange(possibleNextStates);
                    }

                    currentStates = nextStates;
                    nextStates = new List<State>();
                }
                // sprawdzamy czy otrzymane stany po przejœciu programu zawieraj¹ przynajmniej 1 stan spe³niaj¹cy warunek koñcowy
                results.Add(currentStates.Any(s => s.SatisfiesCondition(query.FinalCondition)));
            }
            // gdyby results okaza³o siê puste (np. initialStates by³oby puste) to jaka jest odpowiedŸ kwerendy?
            // chyba true bo "dla ka¿dego modelu..." a zbiór modeli by³by pusty
            return results.All(r => r == true);
        }

        public bool AlwaysAfter(Query_NecessaryAfter query)
        {
            var currentStates = new List<State>();
            if (query.InitialCondition.EmptyRoot)
            {
                currentStates = initial;
            }
            else
            {
                currentStates = States.Where(s => s.SatisfiesCondition(query.InitialCondition) && !s.forbidden).ToList();
            }
            var nextStates = new List<State>();

            foreach(var action in query.program)
            {
                foreach (var state in currentStates)
                {
                    if (state.forbidden) return false;
                    List<State> possibleNextStates = state.possibleEffects[action];
                    if (possibleNextStates.Count == 0) return false;
                    nextStates.AddRange(possibleNextStates);
                }

                currentStates = nextStates;
                nextStates = new List<State>();
            }

            return currentStates.All(s => s.SatisfiesCondition(query.FinalCondition));
        }

        public bool PossiblyAfter(Query_PossiblyAfter query)
        {
            var currentStates = new List<State>();
            if (query.InitialCondition.EmptyRoot)
            {
                currentStates = initial;
            }
            else
            {
                currentStates = States.Where(s => s.SatisfiesCondition(query.InitialCondition) && !s.forbidden).ToList();
            }
            var nextStates = new List<State>();

            foreach (var action in query.program)
            {
                foreach (var state in currentStates)
                {
                    if (state.forbidden) continue;
                    List<State> possibleNextStates = state.possibleEffects[action];
                    if (possibleNextStates.Count == 0) continue;
                    nextStates.AddRange(possibleNextStates);
                }

                currentStates = nextStates;
                nextStates = new List<State>();
            }

            return currentStates.Any(s => s.SatisfiesCondition(query.FinalCondition));
        }

        public bool IsAlwaysExecutable(Query_ExecutableAlways query)
        {
            // zaczynamy od tych co spelniaja Pi
            var initialStates = new List<State>();
            if (query.InitialCondition.EmptyRoot)
            {
                initialStates = initial;
            }
            else
            {
                initialStates = States.Where(s => s.SatisfiesCondition(query.InitialCondition) && !s.forbidden).ToList();
            }

            List<(State state, int cost)> currentStates = initialStates.Select(x => (x, 0)).ToList();
            List<(State state, int cost)> nextStates = new List<(State state, int cost)>();

            for (int step = 0; step < query.program.Count; step++)
            {
                foreach (var tuple in currentStates)
                {
                    var currentState = tuple.state;
                    var cost = tuple.cost;
                    if (currentState.forbidden) return false;

                    List<State> possibleNextStates = currentState.possibleEffects[query.program[step]];
                    List<int> possibleNextCosts = currentState.costs[query.program[step]];

                    if (possibleNextStates is null ||
                        possibleNextCosts.Count == 0 ||
                        possibleNextStates.All(s => s.forbidden) ||
                        possibleNextCosts.Any(x => x + cost > query.Cost))
                        return false;

                    nextStates.AddRange(possibleNextStates.Where(s => !s.forbidden)
                        .Select((x, index) => (x, possibleNextCosts[index] + cost)));
                }

                currentStates = nextStates;
                nextStates = new List<(State state, int cost)>();
            }

            foreach (var tuple in currentStates)
            {
                if (tuple.state.forbidden) return false;
            }

            return true;
        }

        public bool IsEverExecutable(Query_ExecutableEver query)
        {
            var initialStates = new List<State>();
            if (query.InitialCondition.EmptyRoot)
            {
                initialStates = initial;
            }
            else
            {
                initialStates = States.Where(s => s.SatisfiesCondition(query.InitialCondition) && !s.forbidden).ToList();
            }

            if (initialStates.Count == 0) return true;

            List<(State state, int cost)> currentStates = initialStates.Select(x => (x, 0)).ToList();
            List<(State state, int cost)> nextStates = new List<(State state, int cost)>();

            for (int step = 0; step < query.program.Count; step++)
            {
                if (currentStates.Count == 0) return false;

                foreach (var tuple in currentStates)
                {
                    var state = tuple.state;
                    var cost = tuple.cost;
                    if (state.forbidden) continue;

                    List<State> possibleNextStates = state.possibleEffects[query.program[step]];
                    List<int> possibleNextCosts = state.costs[query.program[step]];

                    if (possibleNextStates is null || possibleNextStates.Count == 0 || possibleNextCosts.Any(x => x + cost > query.Cost)) continue;

                    nextStates.AddRange(possibleNextStates.Select((x, index) => (x, possibleNextCosts[index] + cost)));
                }

                currentStates = nextStates;
                nextStates = new List<(State state, int cost)>();
            }

            if (currentStates.Count > 0) return true;
            else return false;
        }

        public bool IsAlwaysAccessible(Query_AccessibleAlways query)
        {
            var initialStates = new List<State>();
            if (query.initialCondition.EmptyRoot)
            {
                initialStates = initial.Where(s => !s.forbidden).ToList();
            }
            else
            {
                initialStates = States.Where(s => s.SatisfiesCondition(query.initialCondition) && !s.forbidden).ToList();
            }

            var endStates = new List<State>();
            if (query.endCondition.EmptyRoot)
            {
                endStates = States.ToList();
            }
            else
            {
                endStates = States.Where(s => s.SatisfiesCondition(query.endCondition) && !s.forbidden).ToList();
            }

            return initialStates.All(s => DFS(s, endStates, query.cost, false));
        }

        public bool IsTypicallyAccessible(Query_AccessibleTypically query)
        {
            // niepotrzebne
            return true;
        }

        public bool IsEverAccessible(Query_AccessibleEver query)
        {
            var initialStates = new List<State>();
            if (query.initialCondition.EmptyRoot)
            {
                initialStates = initial;
            }
            else
            {
                initialStates = States.Where(s => s.SatisfiesCondition(query.initialCondition)).ToList();
            }

            var endStates = new List<State>();
            if (query.endCondition.EmptyRoot)
            {
                endStates = States.ToList();
            }
            else
            {
                endStates = States.Where(s => s.SatisfiesCondition(query.endCondition)).ToList();
            }

            return initialStates.All(s => DFS(s, endStates, query.cost, true));
        }

        public bool DFS(State currentState, List<State> endStates, int maxCost, bool possibly)
        {
            // Create an array to track visited states
            bool[] visited = new bool[States.Length];

            return DFSHelper(currentState, endStates, maxCost, visited, possibly);
        }

        public bool DFSHelper(State currentState, List<State> endStates, int maxCost, bool[] visited, bool possibly)
        {
            // Check if the current state is one of the end states
            if (endStates.Contains(currentState, new StateEqualityComparer()))
            {
                // Check if the cost is within the given maximum
                return true;
            }

            var currentStateIndex = States.ToList().IndexOf(currentState);

            // Mark the current state as visited
            visited[currentStateIndex] = true;

            // Iterate over the possible effects
            for (int i = 0; i < currentState.possibleEffects.Length; i++)
            {
                var nextStates = currentState.possibleEffects[i];
                var costs = currentState.costs[i];
                var canLeadToEndState = new bool[nextStates.Count];
                for(int j = 0; j < nextStates.Count; j++)
                {
                    var nextState = nextStates[j];
                    var nextStateIndex = States.ToList().IndexOf(nextState);
                    var cost = costs[j];
                    if (!visited[nextStateIndex] && !nextState.forbidden && cost <= maxCost)
                    {
                        // Recursively call DFSHelper on the next state
                        bool canReachEndState = DFSHelper(nextState, endStates, maxCost - cost, visited, possibly);

                        // If any of the end states can be reached from the next state within the remaining cost, return true
                        if (canReachEndState)
                        {
                            canLeadToEndState[j] = true;
                        }
                    }
                }
                // w wersji possibly musi istnieæ wybór mappingu, który zaprowadzi do stanu koñcowego
                if(possibly && canLeadToEndState.Any(canLead => canLead == true)) 
                {
                    return true;
                }
                // w wersji necessary ka¿dy wybór mappingu musi prowadziæ do stanu koncowego
                if (!possibly && canLeadToEndState.Length > 0 && canLeadToEndState.All(canLead => canLead == true))
                {
                    return true;
                }
            }
            // czy ta linijka moze doprowadzic do nieskonczonych pêtli?
            visited[currentStateIndex] = false;
            return false;
        }

        // dalej jest magia, ktora dzieje przy generowaniu grafu

        private List<(State, int)> Res0(State s, int action, List<Causes> causes)
        {
            int Res0_Condition(State cond_state, int cond_action, State res, List<Causes> cond_causes)
            {
                foreach (Causes c in cond_causes)
                {
                    if ((c.action == cond_action &&
                        cond_state.SatisfiesCondition(c.condition)) &&
                        !res.SatisfiesCondition(c.effect))
                    {
                        return -1;
                    }
                }
                var costSum = cond_causes.Where(c => c.action == cond_action && cond_state.SatisfiesCondition(c.condition) && res.SatisfiesCondition(c.effect)).Select(c => c.Cost).Sum();
                return costSum;
            }

            var result = new List<(State, int)>();
            //bool[] contains = new bool[state.Length];
            for (int i = 0; i < States.Length; i++)
            {
                var res0Cond = Res0_Condition(s, action, States[i], causes);
                if (!States[i].forbidden && res0Cond != -1)
                {
                    //if (!contains[i])
                    {
                        result.Add((States[i], res0Cond));
                        //contains[i] = true;
                    }
                }
            }
            return result;
        }

        private (bool[], int) New(State s, int action, State res, List<Releases> releases)
        {
            bool[] result = new bool[fluent.Length];
            var cost = 0;
            for(int i=0;i<fluent.Length;i++)
            {
                if (!noninertial[i] && s.fluents[i] != res.fluents[i]) // warunek 1 dla inercjalnych
                    result[i] = true;
                //else // warunek 2 z releases
                //{
                    foreach(Releases statement in releases)
                    {
                        if(statement.action == action &&
                            s.SatisfiesCondition(statement.condition) &&
                            statement.fluent == i)//!res.SatisfiesCondition(statement.effect))
                        {
                            result[i] = true;
                            cost += statement.Cost;
                        }
                    }
                //}
            }
            return (result, cost);
        }

        private bool Subset(bool[] A, bool[] B)
        {
            bool identical = true;
            for(int i=0;i<A.Length;i++)
                if(A[i]!=B[i]) identical = false;
            if (identical) return false;

            for(int i=0;i<A.Length;i++)
            {
                if (A[i] && !B[i]) return false;
            }
            return true;
        }

        private (List<State>, List<int>) Res(State s, int action, List<Causes> causes, List<Releases> releases)
        {
            var res0Tuple = Res0(s, action, causes);
            List<State> res0 = res0Tuple.Select(r => r.Item1).ToList();
            List<int> res0Costs = res0Tuple.Select(r => r.Item2).ToList();
            List<bool[]> newSet = new List<bool[]>();
            for(int i = 0; i < res0.Count; i++)
            {
                var res = res0[i];
                var newRes = New(s, action, res, releases);
                newSet.Add(newRes.Item1);
                res0Costs[i] += newRes.Item2;
            }

            bool[] isMinimal = new bool[newSet.Count];
            for(int i=0;i<newSet.Count;i++) isMinimal[i] = true;

            for(int i=0;i<newSet.Count;i++)
            {
                for(int j=0;j<newSet.Count;j++)
                {
                    if(!isMinimal[i] || !isMinimal[j] || i == j) continue; // tutaj jeszcze raz zerknac
                    if(Subset(newSet[i], newSet[j])) isMinimal[j] = false;
                }
            }

            List<State> resultStates = new List<State>();
            List<int> resultCosts = new List<int>();
            for(int i=0;i<res0.Count;i++)
            {
                if (isMinimal[i])
                {
                    resultStates.Add(res0[i]);
                    resultCosts.Add(res0Costs[i]);
                }
            }
            return (resultStates, resultCosts);
        }

        // Resy dla typowych

        private List<State> TypicalRes0(State s, int action, List<Causes> causes, List<TypicallyCauses> typically)
        {
            bool Res0_Condition(State s0, int action0, State res, List<Causes> causes0, List<TypicallyCauses> typically0)
            {
                List<CausesOrTypicallyCauses> statements = new List<CausesOrTypicallyCauses>();
                statements.AddRange(causes0);
                statements.AddRange(typically0);
                foreach (CausesOrTypicallyCauses c in statements)
                {
                    if ((c.action == action0 &&
                        s0.SatisfiesCondition(c.condition)) &&
                        !res.SatisfiesCondition(c.effect))
                    {
                        return false;
                    }
                }
                return true;
            }

            List<State> result = new List<State>();
            bool[] contains = new bool[States.Length];
            for (int i = 0; i < States.Length; i++)
            {
                if (!States[i].forbidden && Res0_Condition(s, action, States[i], causes, typically))
                {
                    if (!contains[i])
                    {
                        result.Add(States[i]);
                        contains[i] = true;
                    }
                }
            }
            return result;
        }

        private bool[] TypicalNew(State s, int action, State res, List<Releases> releases, List<TypicallyReleases> typically)
        {
            List<ReleasesOrTypicallyReleases> statements = new List<ReleasesOrTypicallyReleases>();
            statements.AddRange(releases);
            statements.AddRange(typically);

            bool[] result = new bool[fluent.Length];
            for (int i = 0; i < fluent.Length; i++)
            {
                if (!noninertial[i] && s.fluents[i] != res.fluents[i]) // warunek 1 dla inercjalnych
                    result[i] = true;
                else // warunek 2 z releases
                {
                    foreach(ReleasesOrTypicallyReleases statement in statements)
                    {
                        if (statement.action == action &&
                            s.SatisfiesCondition(statement.condition) &&
                            statement.fluent == i)//!res.SatisfiesCondition(statement.effect))
                        {
                            result[i] = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private List<State> TypicalRes(State s, int action, List<Causes> causes, List<TypicallyCauses> typicallyCauses, List<Releases> releases, List<TypicallyReleases> typicallyReleases)
        {
            List<State> res0 = TypicalRes0(s, action, causes, typicallyCauses);
            List<bool[]> newSet = new List<bool[]>();
            foreach (State res in res0)
            {
                newSet.Add(TypicalNew(s, action, res, releases, typicallyReleases));
            }

            bool[] isMinimal = new bool[newSet.Count];
            for (int i = 0; i < newSet.Count; i++) isMinimal[i] = true;

            for (int i = 0; i < newSet.Count; i++)
            {
                for (int j = 0; j < newSet.Count; j++)
                {
                    if (!isMinimal[i] || !isMinimal[j] || i == j) continue; // tutaj jeszcze raz zerknac
                    if (Subset(newSet[i], newSet[j])) isMinimal[j] = false;
                }
            }

            List<State> result = new List<State>();
            for (int i = 0; i < res0.Count; i++)
            {
                if (isMinimal[i]) result.Add(res0[i]);
            }
            return result;
        }
    }
}
