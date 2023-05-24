using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWProgram.Classes;
using Action = RWProgram.Classes.Action;

namespace RWProgram
{
    public static class Tests_Programs
    {
        public static List<(Action action, Actor actor)> Test1a
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "LOAD", Index = 0 }, new Actor { Name = "Bill", Index = 0 }),
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test1b
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "LOAD", Index = 0 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test1c
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "LOAD", Index = 0 }, new Actor { Name = "Bill", Index = 0 }),
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test2a
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test2b
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "Jim", Index = 1 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test2c
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "ɛ", Index = 2 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test3a
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "LOAD", Index = 0 }, new Actor { Name = "Bill", Index = 0 }),
                    (new Action { Name = "SPIN", Index = 2 }, new Actor { Name = "Bill", Index = 0 }),
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test3b
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "LOAD", Index = 0 }, new Actor { Name = "Bill", Index = 0 }),
                    (new Action { Name = "SPIN", Index = 2 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test3c
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "LOAD", Index = 0 }, new Actor { Name = "Bill", Index = 0 }),
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test4a
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "INSERT_CARD", Index = 0 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test4b
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "INSERT_CARD", Index = 0 }, new Actor { Name = "ɛ", Index = 1 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test4c
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "INSERT_CARD", Index = 0 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test5a
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "LOAD", Index = 0 }, new Actor { Name = "Bill", Index = 0 }),
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test5b
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "SHOOT", Index = 1 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }

        public static List<(Action action, Actor actor)> Test5c
        {
            get
            {
                return new List<(Action action, Actor actor)>
                {
                    (new Action { Name = "LOAD", Index = 0 }, new Actor { Name = "Bill", Index = 0 })
                };
            }
        }
    }
}
