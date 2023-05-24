using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWProgram.Classes;
using Action = RWProgram.Classes.Action;

namespace RWProgram
{
    public static class Tests_Logic
    {
        public static Logic Test1
        {
            get
            {
                var loaded = new Fluent { Name = "loaded", Index = 0 };
                var notLoaded = new NegatedFluent { Name = "loaded", Original = loaded };
                var alive = new Fluent { Name = "alive", Index = 1 };
                var notAlive = new NegatedFluent { Name = "alive", Original = alive };
                var bill = new Actor { Name = "Bill", Index = 0 };
                var jim = new Actor { Name = "Jim", Index = 1 };
                var empty = new Actor { Name = "ɛ", Index = 2 };
                var load = new Action { Name = "LOAD", Index = 0 };
                var shoot = new Action { Name = "SHOOT", Index = 1 };

                var anyone = new Actor { Name = "Anyone", Index = 1000 };
                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive };
                return new Logic
                {
                    Fluents = fluents,
                    Actors = new List<Actor> { bill, jim, anyone, empty },
                    Actions = new List<Action> { load, shoot, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("not loaded", fluents)),
                        new InitiallyFluent(new State("alive", fluents)),
                        new ActionByActorCausesAlphaIfFluents(new State("loaded", fluents), load, bill, new State()),
                        new ActionByActorCausesAlphaIfFluents(new State("not loaded", fluents), shoot, bill, new State()),
                        new ActionByActorCausesAlphaIfFluents(new State("not alive", fluents), shoot, bill, new State("loaded", fluents)),
                    }
                };
            }
        }

        public static Logic Test2
        {
            get
            {
                var loaded = new Fluent { Name = "loaded", Index = 0 };
                var notLoaded = new NegatedFluent { Name = "loaded", Original = loaded };
                var alive = new Fluent { Name = "alive", Index = 1 };
                var notAlive = new NegatedFluent { Name = "alive", Original = alive };
                var bill = new Actor { Name = "Bill", Index = 0 };
                var jim = new Actor { Name = "Jim", Index = 1 };
                var empty = new Actor { Name = "ɛ", Index = 2 };
                var load = new Action { Name = "LOAD", Index = 0 };
                var shoot = new Action { Name = "SHOOT", Index = 1 };

                var anyone = new Actor { Name = "Anyone", Index = 1000 };
                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive };

                return new Logic
                {
                    Fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive },
                    Actors = new List<Actor> { bill, jim, anyone, empty },
                    Actions = new List<Action> { load, shoot, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("alive", fluents)),
                        new ActionByActorCausesAlphaIfFluents(new State("loaded", fluents), load, bill, new State()),
                        new ActionByActorCausesAlphaIfFluents(new State("not loaded", fluents), shoot, bill, new State()),
                        new ActionByActorCausesAlphaIfFluents(new State("not alive", fluents), shoot, bill, new State("loaded", fluents)),
                        new FluentAfterActionbyActor(new State("not alive", fluents), new List<(Classes.Action action, Actor actor)> { (shoot, bill) })
                    }
                };
            }
        }

        public static Logic Test3
        {
            get
            {
                var loaded = new Fluent { Name = "loaded", Index = 0 };
                var notLoaded = new NegatedFluent { Name = "loaded", Original = loaded };
                var alive = new Fluent { Name = "alive", Index = 1 };
                var notAlive = new NegatedFluent { Name = "alive", Original = alive };
                var bill = new Actor { Name = "Bill", Index = 0 };
                var empty = new Actor { Name = "ɛ", Index = 1 };
                var load = new Action { Name = "LOAD", Index = 0 };
                var shoot = new Action { Name = "SHOOT", Index = 1 };
                var spin = new Action { Name = "SPIN", Index = 2 };

                var anyone = new Actor { Name = "Anyone", Index = 1000 };
                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive };

                return new Logic
                {
                    Fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive },
                    Actors = new List<Actor> { bill, anyone, empty },
                    Actions = new List<Action> { load, shoot, spin, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("not loaded && alive", fluents)),
                        new ActionByActorCausesAlphaIfFluents(new State("loaded", fluents), load, bill, new State()),
                        new ActionByActorCausesAlphaIfFluents(new State("not loaded", fluents), shoot, bill, new State()),
                        new ActionByActorCausesAlphaIfFluents(new State("not alive", fluents), shoot, bill, new State("loaded", fluents)),
                        new ActionByActorReleasesFluent1IfFluents(loaded, spin, bill,  new State("loaded", fluents)),
                    }
                };
            }
        }

        public static Logic Test4
        {
            get
            {
                var open = new Fluent { Name = "open", Index = 0 };
                var notOpen = new NegatedFluent { Name = "open", Original = open };
                var hasCard = new Fluent { Name = "hasCard", Index = 1 };
                var notHasCard = new NegatedFluent { Name = "hasCard", Original = hasCard };
                var bill = new Actor { Name = "Bill", Index = 0 };
                var empty = new Actor { Name = "ɛ", Index = 1 };
                var insertCard = new Action { Name = "INSERT_CARD", Index = 0 };

                var anyone = new Actor { Name = "Anyone", Index = 1000 };
                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { open, notOpen, hasCard, notHasCard };

                return new Logic
                {
                    Fluents = new List<Fluent> { open, notOpen, hasCard, notHasCard },
                    Actors = new List<Actor> { bill, anyone, empty },
                    Actions = new List<Action> { insertCard, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("not open", fluents)),
                        new ActionByActorCausesAlphaIfFluents(new State("open", fluents), insertCard, bill, new State()),
                        new ImpossibleActionByActorIfFluents(insertCard, bill, new State("hasCard", fluents))
                    }
                };
            }
        }

        public static Logic Test5
        {
            get
            {
                var loaded = new Fluent { Name = "loaded", Index = 0 };
                var notLoaded = new NegatedFluent { Name = "loaded", Original = loaded };
                var alive = new Fluent { Name = "alive", Index = 1 };
                var notAlive = new NegatedFluent { Name = "alive", Original = alive };
                var bill = new Actor { Name = "Bill", Index = 0 };
                var jim = new Actor { Name = "Jim", Index = 1 };
                var empty = new Actor { Name = "ɛ", Index = 2 };
                var load = new Action { Name = "LOAD", Index = 0 };
                var shoot = new Action { Name = "SHOOT", Index = 1 };

                var anyone = new Actor { Name = "Anyone", Index = 1000 };
                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive };

                return new Logic
                {
                    Fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive },
                    Actors = new List<Actor> { bill, jim, anyone, empty },
                    Actions = new List<Action> { load, shoot, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("not loaded", fluents)),
                        new InitiallyFluent(new State("alive", fluents)),
                        new ActionByActorCausesAlphaIfFluents(new State("loaded", fluents), load, bill, new State()),
                        new ActionByActorCausesAlphaIfFluents(new State("not loaded", fluents), shoot, bill, new State()),
                        new ActionByActorCausesAlphaIfFluents(new State("not alive", fluents), shoot, bill, new State("loaded", fluents)),
                    }
                };
            }
        }
    }
}
