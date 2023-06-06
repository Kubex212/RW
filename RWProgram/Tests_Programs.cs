using System.Collections.Generic;
using Action = RWProgram.Classes.Action;

namespace RWProgram
{
    public static class Tests_Programs
    {
        public static List<Action> Test1a
        {
            get
            {
                return new List<Action>
                {
                   new Action { Name = "LOAD", Index = 0 },
                   new Action { Name = "SHOOT", Index = 1 },
                };
            }
        }

        public static List<Action> Test1b
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "SHOOT", Index = 1 },
                };
            }
        }

        public static List<Action> Test1c
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "LOAD", Index = 0 },
                    new Action { Name = "SHOOT", Index = 1 },
                };
            }
        }

        public static List<Action> Test2a
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "SHOOT", Index = 1 },
                };
            }
        }

        public static List<Action> Test2b
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "SHOOT", Index = 1 },
                };
            }
        }

        public static List<Action> Test2c
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "SHOOT", Index = 1 },
                };
            }
        }

        public static List<Action> Test3a
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "LOAD", Index = 0 },
                    new Action { Name = "SPIN", Index = 2 },
                    new Action { Name = "SHOOT", Index = 1 },
                };
            }
        }

        public static List<Action> Test3b
        {
            get
            {
                return new List<Action>
                {   
                    new Action { Name = "LOAD", Index = 0 },
                    new Action { Name = "SPIN", Index = 2 },
                };
            }
        }

        public static List<Action> Test3c
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "LOAD", Index = 0 },
                    new Action { Name = "SHOOT", Index = 1 },
                };
            }
        }

        public static List<Action> Test4a
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "INSERT_CARD", Index = 0 },
                };
            }
        }

        public static List<Action> Test4b
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "INSERT_CARD", Index = 0 },
                };
            }
        }

        public static List<Action> Test4c
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "INSERT_CARD", Index = 0 },
                };
            }
        }

        public static List<Action> Test5a
        {
            get
            {
                return new List<Action>
                {

                };
            }
        }

        public static List<Action> Test5b
        {
            get
            {
                return new List<Action>
                {

                };
            }
        }

        public static List<Action> Test5c
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "OPENDOOR", Index = 0 },
                    new Action { Name = "JUMP", Index = 1 },
                };
            }
        }

        public static List<Action> Test6c
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "BUY_A", Index = 0 },
                    new Action { Name = "BUY_B", Index = 1 },
                    new Action { Name = "READ", Index = 3 },
                };
            }
        }

        public static List<Action> Test7
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "GET_FUEL", Index = 0 },
                    new Action { Name = "GET_OIL", Index = 1 },
                    new Action { Name = "CUT_TREE", Index = 2 },
                };
            }
        }

        public static List<Action> Test8a
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "REFUEL", Index = 0 },
                    new Action { Name = "DRIVE", Index = 1 },
                };
            }
        }

        public static List<Action> Test8b
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "DRIVE", Index = 1 },
                    new Action { Name = "DRIVE", Index = 1 },
                };
            }
        }

        public static List<Action> Test8c
        {
            get
            {
                return new List<Action>
                {
                    new Action { Name = "DRIVE", Index = 1 },
                    new Action { Name = "DRIVE", Index = 1 },
                    new Action { Name = "DRIVE", Index = 1 },
                };
            }
        }
    }
}
