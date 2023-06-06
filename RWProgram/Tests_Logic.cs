using System.Collections.Generic;
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

        public static Logic Test6
        {
            get
            {
                var pass = new Fluent { Name = "pass", Index = 0 };
                var notPass = new NegatedFluent { Name = "pass", Original = pass };
                var bookA = new Fluent { Name = "bookA", Index = 1 };
                var notBookA = new NegatedFluent { Name = "bookA", Original = bookA };
                var bookB = new Fluent { Name = "bookB", Index = 2 };
                var notBookB = new NegatedFluent { Name = "bookB", Original = bookB };
                var bookC = new Fluent { Name = "bookC", Index = 3 };
                var notBookC = new NegatedFluent { Name = "bookC", Original = bookC };
                var readA = new Fluent { Name = "readA", Index = 4 };
                var notReadA = new NegatedFluent { Name = "readA", Original = readA };
                var readB = new Fluent { Name = "readB", Index = 5 };
                var notReadB = new NegatedFluent { Name = "readB", Original = readB };
                var readC = new Fluent { Name = "readC", Index = 6 };
                var notReadC = new NegatedFluent { Name = "readC", Original = readC };

                var buyA = new Action { Name = "BUY_A", Index = 0 };
                var buyB = new Action { Name = "BUY_B", Index = 1 };
                var buyC = new Action { Name = "BUY_C", Index = 2 };
                var readBooks = new Action { Name = "READ", Index = 3 };

                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent> 
                { 
                    pass, notPass, bookA, notBookA, bookB, notBookB, bookC, notBookC,
                    readA, notReadA, readB, notReadB, readC, notBookC
                };

                return new Logic
                {
                    Fluents = new List<Fluent> 
                    {
                        pass, notPass, bookA, notBookA, bookB, notBookB, bookC, notBookC,
                        readA, notReadA, readB, notReadB, readC, notBookC
                    },
                    Actions = new List<Action> { buyA, buyB, buyC, readBooks },
                    Statements = new List<Statement> {
                        new NoninertialFluent(pass),
                        new InitiallyFluent(new State("bookA", fluents)),
                        new InitiallyFluent(new State("not bookB", fluents)),
                        new InitiallyFluent(new State("not bookC", fluents)),
                        new InitiallyFluent(new State("not readA", fluents)),
                        new InitiallyFluent(new State("not readC", fluents)),
                        new InitiallyFluent(new State("readC", fluents)),
                        new ActionCausesAlphaIfFluents(new State("bookA", fluents), buyA, new State(), 150),
                        new ActionCausesAlphaIfFluents(new State("bookB", fluents), buyB, new State(), 120),
                        new ActionCausesAlphaIfFluents(new State("bookC", fluents), buyC, new State(), 80),
                        new ActionCausesAlphaIfFluents(new State("readA", fluents), readBooks, new State("bookA", fluents), 100),
                        new ActionCausesAlphaIfFluents(new State("readB", fluents), readBooks, new State("bookB", fluents), 150),
                        new ActionCausesAlphaIfFluents(new State("readC", fluents), readBooks, new State("bookC", fluents), 180),
                        new AlwaysPi(new State("((readA && readB) || (readA && readC) || (readB && readC)) -> pass && pass -> ((readA && readB) || (readA && readC) || (readB && readC))", fluents)),

                    }
                };
            }
        }

        public static Logic Test7
        {
            get
            {
                var fuel = new Fluent { Name = "fuel", Index = 0 };
                var notFuel = new NegatedFluent { Name = "fuel", Original = fuel };
                var oil = new Fluent { Name = "oil", Index = 1 };
                var notOil = new NegatedFluent { Name = "oil", Original = oil };
                var wood = new Fluent { Name = "wood", Index = 2 };
                var notWood = new NegatedFluent { Name = "wood", Original = wood };
                var canUseSaw = new Fluent { Name = "canUseSaw", Index = 3 };
                var notSaw = new NegatedFluent { Name = "canUseSaw", Original = canUseSaw };
                var canUseChainsaw = new Fluent { Name = "canUseChainsaw", Index = 4 };
                var notChainasaw = new NegatedFluent { Name = "canUseChainsaw", Original = canUseChainsaw };

                var getFuel = new Action { Name = "GET_FUEL", Index = 0 };
                var getOil = new Action { Name = "GET_OIL", Index = 1 };
                var cutTree = new Action { Name = "CUT_TREE", Index = 2 };

                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent>
                {
                    fuel, notFuel, oil, notOil, wood, notWood, canUseSaw, notSaw,
                    canUseChainsaw, notChainasaw, 
                };

                return new Logic
                {
                    Fluents = new List<Fluent>
                    {
                        fuel, notFuel, oil, notOil, wood, notWood, canUseSaw, notSaw,
                        canUseChainsaw, notChainasaw, 
                    },
                    Actions = new List<Action> { getFuel, getFuel, cutTree },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("not fuel", fluents)),
                        new InitiallyFluent(new State("not oil", fluents)),
                        new InitiallyFluent(new State("not wood", fluents)),
                        new InitiallyFluent(new State("canUseSaw", fluents)),
                        new AlwaysPi(new State("canUseChainsaw -> (fuel && oil) && (fuel && oil) -> canUseChainsaw", fluents)),
                        new ActionCausesAlphaIfFluents(new State("fuel", fluents), getFuel, new State("", fluents), 3),
                        new ActionCausesAlphaIfFluents(new State("oil", fluents), getOil, new State("", fluents), 3),
                        new ActionCausesAlphaIfFluents(new State("wood", fluents), cutTree, new State("canUseSaw", fluents), 10),
                        new ActionCausesAlphaIfFluents(new State("wood", fluents), cutTree, new State("canUseChainsaw", fluents), 3),

                    }
                };
            }
        }

        public static Logic Test8
        {
            get
            {
                var fuel = new Fluent { Name = "fuel", Index = 0 };
                var notFuel = new NegatedFluent { Name = "fuel", Original = fuel };
                var inWarsaw = new Fluent { Name = "inWarsaw", Index = 1 };
                var notInWarsaw = new NegatedFluent { Name = "inWarsaw", Original = inWarsaw };
                var inCracow = new Fluent { Name = "inCracow", Index = 2 };
                var notInCracow = new NegatedFluent { Name = "inCracow", Original = inCracow };
               

                var getFuel = new Action { Name = "REFUEL", Index = 0 };
                var drive = new Action { Name = "DRIVE", Index = 1 };

                var anything = new Action { Name = "Anything", Index = 1000 };

                var fluents = new List<Fluent>
                {
                    fuel, notFuel, inWarsaw, notInWarsaw, inCracow, notInCracow,
                };

                return new Logic
                {
                    Fluents = new List<Fluent>
                    {
                        fuel, notFuel, inWarsaw, notInWarsaw, inCracow, notInCracow,
                    },
                    Actions = new List<Action> { getFuel, drive },
                    Statements = new List<Statement> {
                        new InitiallyFluent(new State("inWarsaw", fluents)),
                        new AlwaysPi(new State("inWarsaw -> not inCracow", fluents)),
                        new AlwaysPi(new State("inCracow -> not inWarsaw", fluents)),
                        new ActionCausesAlphaIfFluents(new State("fuel", fluents), getFuel, new State("not fuel", fluents), 3),
                        new ActionCausesAlphaIfFluents(new State("not fuel", fluents), drive, new State("fuel", fluents), 3),
                        new ActionCausesAlphaIfFluents(new State("inWarsaw", fluents), drive, new State("fuel && inCracow", fluents), 10),
                        new ActionCausesAlphaIfFluents(new State("inCracow", fluents), drive, new State("fuel && inWarsaw", fluents), 10),

                    }
                };
            }
        }
    }
}
