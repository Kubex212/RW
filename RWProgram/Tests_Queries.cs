using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWProgram.Classes;
using Action = RWProgram.Classes.Action;

namespace RWProgram
{
    public static class Tests_Queries
    {
        public static AlwaysAccesibleYFromPi Test1a
        {
            get
            {
                return new AlwaysAccesibleYFromPi(new State("not alive", new string[] { "alive" }),new State("alive", new string[] { "loaded", "alive" }), 10);
            }
        }

        public static AlwaysAfter Test1b
        {
            get
            {
                return new AlwaysAfter()
                {
                    Alpha = new State("not alive", new string[] { "loaded", "alive"}),
                    Pi = new State("loaded", new string[] { "loaded", "alive"})
                };
            }
        }

        public static AlwaysExecutable Test1c
        {
            get
            {
                return new AlwaysExecutable( new State("not loaded && alive", new string[] { "loaded", "alive" }), cost: 19);
            }
        }

        public static EverAccesibleYFromPi Test2a
        {
            get
            {
                return new EverAccesibleYFromPi(Gamma: new State("not alive", new string[] { "loaded", "alive" }), Pi: new State("alive", new string[] { "loaded", "alive" }), Cost: 21);
            }
        }

        public static AlwaysExecutable Test2b
        {
            get
            {
                return new AlwaysExecutable(cost: 18, pi: new State("alive", new string[] { "loaded", "alive" }));
            }
        }

        public static EverExecutable Test2c
        {
            get
            {
                return new EverExecutable(cost: 18, pi: new State("alive", new string[] { "loaded", "alive" }));
            }
        }

        public static AlwaysExecutable Test3a
        {
            get
            {
                return new AlwaysExecutable(new State("alive && not loaded", new string[] { "loaded", "alive" }), 35);
            }
        }

        public static AlwaysAccesibleYFromPi Test3b
        {
            get
            {
                return new AlwaysAccesibleYFromPi(new State("loaded", new string[] { "loaded" }), new State("not loaded", new string[] { "loaded" }), 10);

            }
        }

        public static EverExecutable Test3c
        {
            get
            {
                return new EverExecutable(new State("alive && not loaded", new string[] { "loaded", "alive" }), 35);
            }
        }

        public static AlwaysAccesibleYFromPi Test4a
        {
            get
            {
                return new AlwaysAccesibleYFromPi(new State("open", new string[] { "open" }), new State("open || hasCard", new string[] { "open", "hasCard" }), 100);
            }
        }

        public static EverExecutable Test4b
        {
            get
            {
                return new EverExecutable(new State("not open", new string[] { "open" }), 10);
            }
        }

        public static AlwaysExecutable Test4c
        {
            get
            {
                return new AlwaysExecutable(new State("not open", new string[] { "open" }), 10);
            }
        }
        
        public static AlwaysAccesibleYFromPi Test5a
        {
            get
            {
                return new AlwaysAccesibleYFromPi(new State("not vase", new string[] { "vase" }), new State(), 2);
            }
        }

        public static EverAccesibleYFromPi Test5b
        {
            get
            {
                return new EverAccesibleYFromPi(new State("not vase", new string[] { "vase" }), new State(), 2);
            }
        }

        public static AlwaysAfter Test5c
        {
            get
            {
                return new AlwaysAfter()
                {
                    Alpha = new State("not vase", new string[] { "vase", "cat" }),
                    Pi = new State("vase", new string[] { "vase", "cat" })
                };
            }
        }

        public static AlwaysAccesibleYFromPi Test6a
        {
            get
            {
                return new AlwaysAccesibleYFromPi(
                    Gamma: new State("pass", new string[] { "pass", "bookA", "bookB", "bookC", "readA", "readB", "readC" }),
                    Pi: new State("bookA && readC", new string[] { "pass", "bookA", "bookB" ,"bookC" ,"readA", "readB", "readC" }),
                    Cost: 100);
                
            }
        }

        public static EverAccesibleYFromPi Test6b
        {
            get
            {
                return new EverAccesibleYFromPi(
                    Gamma: new State("pass", new string[] { "pass", "bookA", "bookB", "bookC", "readA", "readB", "readC" }),
                    Pi: new State("readA", new string[] { "pass", "bookA", "bookB", "bookC", "readA", "readB", "readC" }),
                    Cost: 350);

            }
        }

        public static AlwaysAfter Test6c
        {
            get
            {
                return new AlwaysAfter()
                {
                    Alpha = new State("pass", new string[] { "pass", "bookA", "bookB", "bookC", "readA", "readB", "readC" }),
                    Pi = new State("", new string[] { "pass", "bookA", "bookB", "bookC", "readA", "readB", "readC" }),
                };

            }
        }

        public static AlwaysExecutable Test7a
        {
            get
            {
                return new AlwaysExecutable(new State("not canUseSaw", new string[] { "fuel", "oil", "wood", "canUseSaw", "canUseChainsaw" }), 10);
            }
        }

        public static AlwaysExecutable Test7b
        {
            get
            {
                return new AlwaysExecutable(new State("not canUseSaw", new string[] { "fuel", "oil", "wood", "canUseSaw", "canUseChainsaw" }), 5);
            }
        }

        public static AlwaysAccesibleYFromPi Test7c
        {
            get
            {
                return new AlwaysAccesibleYFromPi(
                    new State("wood", new string[] { "fuel", "oil", "wood", "canUseSaw", "canUseChainsaw" }),
                    new State("not canUseSaw", new string[] { "fuel", "oil", "wood", "canUseSaw", "canUseChainsaw" }),
                    8);
            }
        }

        public static AlwaysExecutable Test8a
        {
            get
            {
                return new AlwaysExecutable(new State("fuel", new string[] { "fuel", "inWarsaw", "inCracow", }), 10);
            }
        }

        public static AlwaysExecutable Test8b
        {
            get
            {
                return new AlwaysExecutable(new State("fuel && inCracow", new string[] { "fuel", "inWarsaw", "inCracow", }), 20);
            }
        }

        public static AlwaysAfter Test8c
        {
            get
            {
                return new AlwaysAfter()
                {
                    Alpha = new State("inCracow", new string[] { "fuel", "inWarsaw", "inCracow", }),
                    Pi = new State("fuel && inWarsaw", new string[] { "fuel", "inWarsaw", "inCracow", }),
                };

            }
        }
    }
}
