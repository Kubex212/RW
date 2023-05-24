using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicExpressionsParser;
namespace RWProgram.Classes
{
    public  class State
    {
        public LogicalExpressionRoot LogicalExpressionRoot;
        public string LogicalExpressionString;
        public string[] FluentNames;
        public State()
        {
            this.LogicalExpressionRoot = null;
            this.LogicalExpressionString = string.Empty;
        }

        public State(string LogicalExpressionString, List<Fluent> Fluents) : this(LogicalExpressionString, Fluents.Where(f => !f.IsNegation).Select(f => f.Name).ToArray())
        { }

        public State(string LogicalExpressionString, string[] Fluents)
        {
            this.FluentNames = Fluents;
            if (string.IsNullOrEmpty(LogicalExpressionString.Trim()))
            {
                this.LogicalExpressionRoot = null;
                this.LogicalExpressionString = string.Empty;
            }
            else
            {
                this.LogicalExpressionRoot = Parser.Parse(LogicalExpressionString);
                this.LogicalExpressionString = LogicalExpressionString;
            }
        }

        public State(LogicalExpressionRoot LogicalExpressionRoot, string LogicalExpressionString, List<Fluent> Fluents) : this(LogicalExpressionRoot, LogicalExpressionString, Fluents.Where(f => !f.IsNegation).Select(f => f.Name).ToArray())
        { }

        public State(LogicalExpressionRoot LogicalExpressionRoot, string LogicalExpressionString, string[] Fluents)
        {
            this.FluentNames = Fluents;
            this.LogicalExpressionRoot = LogicalExpressionRoot;
            this.LogicalExpressionString = LogicalExpressionString;
        }

        public override string ToString() { return LogicalExpressionString; }

        public Formula ToLogic()
        {
            if (string.IsNullOrEmpty(this.LogicalExpressionString))
                return new Formula();

            return new Formula(LogicalExpressionRoot, FluentNames);
        }

    }
}
