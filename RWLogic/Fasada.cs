using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicExpressionsParser;

namespace RWLogic
{
    public class Fasada
    {
        private Model model;

        public Fasada(
            List<string> fluents,
            List<string> actions,
            List<Noninertial> noninertial,
            List<Always> always,
            List<Causes> causes,
            List<Releases> releases,
            List<Initially> initially,
            List<After> after,
            List<ObservableAfter> observableAfter)
        {
            model = new Model(fluents, actions);
            model.SetNoninertial(noninertial);
            model.SetAlways(always);
            model.SetPossibleEffects(causes, releases);
            model.SetAbnormalEffects();
            model.SetInitialStates(initially, after, observableAfter);
        }

        public string test()
        {
            string log = "";
            log += "nazwy fluentow:\n(";
            for (int i = 0; i < model.fluent.Length; i++) log += model.fluent[i] + " ";
            log += ")\n";
            log += "stany poczatkowe to:\n";
            for (int i = 0; i < model.initial.Count; i++) log += model.initial[i].Print();
            log += "efekty akcji:\n";
            for(int i=0;i<model.States.Length;i++)
            {
                log += "\nstan " + model.States[i].Print();
                if (model.States[i].forbidden)
                {
                    log += "jest niezgodny z dziedzina\n";
                    continue;
                }
                /*log += "- mozliwe efekty: \n";
                for(int action=0; action < model.action.Length;action++)
                {
                    for(int agent=0; agent<model.agent.Length;agent++)
                    {
                        if (model.state[i].possibleEffects[agent, action].Count != 0)
                        {
                            log += model.action[action] + " by " + model.agent[agent] + ":\n";
                            foreach(Model.State s in model.state[i].possibleEffects[agent, action]) log += s.Print();
                        }

                    }
                }*/
                log += "- typowe efekty: \n";
                for (int action = 0; action < model.action.Length; action++)
                {
                    if (model.States[i].typicalEffects[action].Count != 0)
                    {
                        log += model.action[action] + ":\n";
                        foreach (State s in model.States[i].typicalEffects[action]) log += s.Print();
                    }
                }
                log += "- nietypowe efekty: \n";
                for (int action = 0; action < model.action.Length; action++)
                {
                    if (model.States[i].abnormalEffects[action].Count != 0)
                    {
                        log += model.action[action] + ":\n";
                        foreach (State s in model.States[i].abnormalEffects[action]) log += s.Print();
                    }
                }
            }
            return log;
        }
        // tutaj bedzie obsluga kwerend
        
        public bool Query(Query_NecessaryAfter query)
        {
            return model.AlwaysAfter(query);
        }

        public bool Query(Query_PossiblyAfter query)
        {
            return model.PossiblyAfter(query);
        }

        public bool Query(Query_ExecutableAlways query)
        {
            return model.IsAlwaysExecutable(query);
        }

        public bool Query(Query_ExecutableEver query)
        {
            return model.IsEverExecutable(query);
        }

        public bool Query(Query_AccessibleTypically query)
        {
            return model.IsTypicallyAccessible(query);
        }

        public bool Query(Query_AccessibleAlways query)
        {
            return model.IsAlwaysAccessible(query);
        }

        public bool Query(Query_AccessibleEver query)
        {
            return model.IsEverAccessible(query);
        }
    }
}
