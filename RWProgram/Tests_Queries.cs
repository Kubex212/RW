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
                return new AlwaysAccesibleYFromPi(new State("alive", new string[] { "alive" }),new State("alive && not loaded", new string[] { "alive", "loaded" }), 10);
            }
        }

        public static AlwaysAfter Test1b
        {
            get
            {
                return new AlwaysAfter()
                {
                    Alpha = new State("alive", new string[] { "loaded", "alive"}),
                    Pi = new State("loaded", new string[] { "loaded", "alive"})
                };
            }
        }

        public static AlwaysExecutable Test1c
        {
            get
            {
                return new AlwaysExecutable( new State("alive && not loaded", new string[] { "loaded", "alive" }), cost: 19);
            }
        }

        public static EverAccesibleYFromPi Test2a
        {
            get
            {
                return new EverAccesibleYFromPi(new State("alive", new string[] { "alive" }), new State("alive", new string[] { "alive" }), 10);
            }
        }

        public static AlwaysExecutable Test2b
        {
            get
            {
                return new AlwaysExecutable(cost: 18, pi: new State("alive", new string[] { "alive" }));
            }
        }

        public static EverExecutable Test2c
        {
            get
            {
                return new EverExecutable(cost: 18, pi: new State("alive", new string[] { "alive" }));
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

        public static AlwaysExecutable Test5b
        {
            get
            {
                return new AlwaysExecutable(new State("alive && not loaded", new string[] { "loaded", "alive" }), cost: 19);
            }
        }
    }
}
