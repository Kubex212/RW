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
                    Alpha = new State("alive", new string[] { "alive" })
                };
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
                return new AlwaysExecutable
                {
                    Cost = 18,
                    Pi = new State("alive", new string[] { "alive" })
                };
            }
        }

        public static EverExecutable Test2c
        {
            get
            {
                return new EverExecutable
                {
                };
            }
        }

        public static AlwaysExecutable Test3a
        {
            get
            {
                return new AlwaysExecutable
                {
                };
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
                return new EverExecutable
                {
                };
            }
        }

        public static EverExecutable Test4b
        {
            get
            {
                return new EverExecutable
                {
                };
            }
        }

        public static AlwaysExecutable Test4c
        {
            get
            {
                return new AlwaysExecutable
                {
                };
            }
        }

        public static AlwaysExecutable Test5b
        {
            get
            {
                return new AlwaysExecutable
                {
                };
            }
        }
    }
}
