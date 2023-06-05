﻿using System.Collections.Generic;
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
                var load = new Action { Name = "LOAD", Index = 0 };
                var shoot = new Action { Name = "SHOOT", Index = 1 };

                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive };
                return new Logic
                {
                    Fluents = fluents,
                    Actions = new List<Action> { load, shoot, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("not loaded", fluents)),
                        new InitiallyFluent(new State("alive", fluents)),
                        new ActionCausesAlphaIfFluents(new State("loaded", fluents), load, new State(), 10),
                        new ActionCausesAlphaIfFluents(new State("not loaded", fluents), shoot, new State(), 10),
                        new ActionCausesAlphaIfFluents (new State("not alive", fluents), shoot, new State("loaded", fluents), 10),
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
                var load = new Action { Name = "LOAD", Index = 0 };
                var shoot = new Action { Name = "SHOOT", Index = 1 };

                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive };

                return new Logic
                {
                    Fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive },
                    Actions = new List<Action> { load, shoot, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("alive", fluents)),
                        new ActionCausesAlphaIfFluents(new State("loaded", fluents), load, new State(), 10),
                        new ActionCausesAlphaIfFluents(new State("not loaded", fluents), shoot, new State(), 10),
                        new ActionCausesAlphaIfFluents(new State("not alive", fluents), shoot, new State("loaded", fluents), 10),
                        new FluentAfterActionbyActor(new State("not alive", fluents), new List<Classes.Action> { shoot })
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
                var load = new Action { Name = "LOAD", Index = 0 };
                var shoot = new Action { Name = "SHOOT", Index = 1 };
                var spin = new Action { Name = "SPIN", Index = 2 };

                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive };

                return new Logic
                {
                    Fluents = new List<Fluent> { loaded, notLoaded, alive, notAlive },
                    Actions = new List<Action> { load, shoot, spin, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("not loaded && alive", fluents)),
                        new ActionCausesAlphaIfFluents(new State("loaded", fluents), load, new State(), 10),
                        new ActionCausesAlphaIfFluents(new State("not loaded", fluents), shoot, new State(), 10),
                        new ActionCausesAlphaIfFluents(new State("not alive", fluents), shoot, new State("loaded", fluents), 10),
                        new ActionReleasesFluent1IfFluents(loaded, spin,  new State("loaded", fluents), 10),
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
                var insertCard = new Action { Name = "INSERT_CARD", Index = 0 };

                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { open, notOpen, hasCard, notHasCard };

                return new Logic
                {
                    Fluents = new List<Fluent> { open, notOpen, hasCard, notHasCard },
                    Actions = new List<Action> { insertCard, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("not open", fluents)),
                        new ActionCausesAlphaIfFluents(new State("open", fluents), insertCard, new State(), 10),
                        new ImpossibleActionIfFluents(insertCard, new State("hasCard", fluents), 10)
                    }
                };
            }
        }

        public static Logic Test5
        {
            get
            {
                var vase = new Fluent { Name = "vase", Index = 0 };
                var notVase = new NegatedFluent { Name = "vase", Original = vase };
                var cat = new Fluent { Name = "cat", Index = 1 };
                var notCat = new NegatedFluent { Name = "cat", Original = cat };
                var openDoor = new Action { Name = "OPENDOOR", Index = 0 };
                var jump = new Action { Name = "JUMP", Index = 1 };

                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> { vase, notVase, cat, notCat };

                return new Logic
                {
                    Fluents = new List<Fluent> { vase, notVase, cat, notCat },
                    Actions = new List<Action> { openDoor, jump, anything },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("vase", fluents)),
                        new ActionCausesAlphaIfFluents(new State("cat", fluents), openDoor, new State(), 2),
                        new ActionReleasesFluent1IfFluents(vase, jump, new State("cat", fluents), 1),
                        new ActionCausesAlphaIfFluents(new State("not cat", fluents), jump, new State("cat", fluents), 1),
                    }
                };
            }
        }
    }
}
