using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicExpressionsParser
{
    public class LogicalExpressionException : LogicExpressionsParserException
    {
        public LogicalExpressionException(string m) : base(m)
        {
            msg = "LogicalExpressionException: " + msg;
        }

        public LogicalExpressionException()
        {
            msg = "LogicalExpressionException";
        }
    }
    interface ILogicalExpressionOperator
    {
        string OperatorName { get; }
    }
    public abstract class LogicalExpression
    {
        static protected LogicalExpression GetChild(Stack<SpecialTerminal> expression)
        {
            switch(expression.Peek().Id)
            {
                case 0:
                    return new LogicalExpressionOr(expression);
                case 1:
                    return new LogicalExpressionAnd(expression);
                case 2:
                    return new LogicalExpressionLeftToRightImplication(expression);
                case 3:
                    return new LogicalExpressionRightToLeftImplication(expression);
                case 4:
                    return new LogicalExpressionEquality(expression);
                case 5:
                    return new LogicalExpressionNegation(expression);
                case 8:
                    if(((FluentSpecialTerminal)expression.Peek()).FluentName == "FALSE")
                    {
                        return new LogicalExpressionFALSE(expression);
                    }
                    else if (((FluentSpecialTerminal)expression.Peek()).FluentName == "TRUE")
                    {
                        return new LogicalExpressionTRUE(expression);
                    }
                    else
                    {
                        return new LogicalExpressionFluent(expression);
                    }
                default:
                    throw new LogicalExpressionException("Unexpected symbol in expression in the form of ONP.");
            }
        }

        protected LogicalExpression left, right;
        abstract public bool Compute(State s);
        abstract public void SetFluentId(string[] fluentsName);
    }
    public class LogicalExpressionRoot : LogicalExpression
    {
        public static bool[] addOne(bool[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i])
                {
                    tab[i] = false;
                }
                else
                {
                    tab[i] = true;
                    break;
                }
            }

            return tab;
        }

        public override string ToString()
        {
            bool[] tab = new bool[FluentsName.Length];

            StringBuilder sb = new StringBuilder();

            for(int i=0;i<FluentsName.Length;i++)
            {
                sb.Append(FluentsName[i]);
                sb.Append(" ");
            }
            sb.Append("Expression");
            sb.Append('\n');


            for (int i = 0; i < Math.Pow(2, FluentsName.Length); i++)
            {
                State s = new State(tab, 1, FluentsName.ToList());

                for(int j=0;j<FluentsName.Length;j++)
                {
                    string liczba = tab[j] ? "1" : "0";
                    sb.Append(liczba);
                    for(int l=0;l<FluentsName[j].Length;l++)
                    {
                        sb.Append(' ');
                    }
                }

                string liczba2 = this.Compute(s) ? "1" : "0";
                sb.Append(liczba2);
                sb.Append('\n');

                tab = addOne(tab);
            }

            return sb.ToString();
        }

        Stack<SpecialTerminal> ExpInONP;
        string[] FluentsName;
        bool fluentsIdSetted;

        public LogicalExpressionRoot(Stack<SpecialTerminal> expression)
        {
            ExpInONP = expression;
            fluentsIdSetted = false;

            right = LogicalExpression.GetChild(ExpInONP);
        }

        public LogicalExpressionRoot(Stack<SpecialTerminal> expression, string[] fn)
        {
            ExpInONP = expression;
            fluentsIdSetted = false;
            FluentsName = fn;
        }

        public override bool Compute(State s)
        {
            if (fluentsIdSetted)
            {
                return right.Compute(s);
            }
            else
            {
                return true;
                //throw new LogicalExpressionException("Fluent Id not setted.");
            }
        }

        public void SetFluentsName(string[] fn)
        {
            FluentsName = fn;
        }

        public override void SetFluentId(string[] fluentsName)
        {
            right.SetFluentId(fluentsName);
            fluentsIdSetted = true;
        }

        public void SetFluentId()
        {
            right.SetFluentId(FluentsName);
            fluentsIdSetted = true;
        }
    }
    class LogicalExpressionFluent : LogicalExpression, ILogicalExpressionOperator
    {
        string fluentName;
        int fluentId;

        public string OperatorName => "Fluent";

        public LogicalExpressionFluent(Stack<SpecialTerminal> expression)
        {
            SpecialTerminal term =  expression.Pop();

            fluentName = ((FluentSpecialTerminal)term).FluentName;
        }

        public override void SetFluentId(string[] fluentsName)
        {
            for(int i=0;i<fluentsName.Length;i++)
            {
                if(string.Equals(fluentsName[i],fluentName, StringComparison.OrdinalIgnoreCase))
                {
                    fluentId = i;
                    return;
                }
            }

            throw new LogicalExpressionException("Fluent name unknown.");
        }

        public override bool Compute(State s)
        {
            return s.fluents[fluentId];
        }
    }
    class LogicalExpressionNegation : LogicalExpression, ILogicalExpressionOperator
    {
        public LogicalExpressionNegation(Stack<SpecialTerminal> expression)
        {
            expression.Pop();

            right = LogicalExpression.GetChild(expression);
        }

        public string OperatorName => "Negation";

        public override bool Compute(State s)
        {
            return !right.Compute(s);
        }

        public override void SetFluentId(string[] fluentsName)
        {
            right.SetFluentId(fluentsName);
        }
    }
    class LogicalExpressionLeftToRightImplication : LogicalExpression,ILogicalExpressionOperator
    {
        public LogicalExpressionLeftToRightImplication(Stack<SpecialTerminal> expression)
        {
            expression.Pop();

            right = LogicalExpression.GetChild(expression);
            left = LogicalExpression.GetChild(expression);
        }

        public string OperatorName => "Left to right implication";

        public override bool Compute(State s)
        {
            return !left.Compute(s) || right.Compute(s);
        }

        public override void SetFluentId(string[] fluentsName)
        {
            right.SetFluentId(fluentsName);
            left.SetFluentId(fluentsName);
        }
    }
    class LogicalExpressionRightToLeftImplication : LogicalExpression,ILogicalExpressionOperator
    {
        public LogicalExpressionRightToLeftImplication(Stack<SpecialTerminal> expression)
        {
            expression.Pop();

            right = LogicalExpression.GetChild(expression);
            left = LogicalExpression.GetChild(expression);
        }

        public string OperatorName => "Right to left implication";

        public override bool Compute(State s)
        {
            return left.Compute(s) || !right.Compute(s);
        }

        public override void SetFluentId(string[] fluentsName)
        {
            right.SetFluentId(fluentsName);
            left.SetFluentId(fluentsName);
        }
    }
    class LogicalExpressionEquality : LogicalExpression,ILogicalExpressionOperator
    {
        public LogicalExpressionEquality(Stack<SpecialTerminal> expression)
        {
            expression.Pop();

            right = LogicalExpression.GetChild(expression);
            left = LogicalExpression.GetChild(expression);
        }

        public string OperatorName => "Equality";

        public override bool Compute(State s)
        {
            return left.Compute(s) == right.Compute(s);
        }

        public override void SetFluentId(string[] fluentsName)
        {
            right.SetFluentId(fluentsName);
            left.SetFluentId(fluentsName);
        }
    }
    class LogicalExpressionAnd : LogicalExpression, ILogicalExpressionOperator
    {
        public LogicalExpressionAnd(Stack<SpecialTerminal> expression)
        {
            expression.Pop();

            right = LogicalExpression.GetChild(expression);
            left = LogicalExpression.GetChild(expression);
        }

        public string OperatorName => "And";

        public override bool Compute(State s)
        {
            return left.Compute(s) && right.Compute(s);
        }

        public override void SetFluentId(string[] fluentsName)
        {
            right.SetFluentId(fluentsName);
            left.SetFluentId(fluentsName);
        }
    }
    class LogicalExpressionOr : LogicalExpression, ILogicalExpressionOperator
    {
        public LogicalExpressionOr(Stack<SpecialTerminal> expression)
        {
            expression.Pop();

            right = LogicalExpression.GetChild(expression);
            left = LogicalExpression.GetChild(expression);
        }

        public string OperatorName => "Or";

        public override bool Compute(State s)
        {
            return left.Compute(s) || right.Compute(s);
        }

        public override void SetFluentId(string[] fluentsName)
        {
            right.SetFluentId(fluentsName);
            left.SetFluentId(fluentsName);
        }
    }
    class LogicalExpressionFALSE : LogicalExpression, ILogicalExpressionOperator
    {
        public LogicalExpressionFALSE(Stack<SpecialTerminal> expression) { expression.Pop(); }

        public string OperatorName => "FALSE";

        public override bool Compute(State s)
        {
            return false;
        }
        public override void SetFluentId(string[] fluentsName){}
    }
    class LogicalExpressionTRUE : LogicalExpression, ILogicalExpressionOperator
    {
        public LogicalExpressionTRUE(Stack<SpecialTerminal> expression) { expression.Pop(); }

        public string OperatorName => "TRUE";

        public override bool Compute(State s)
        {
            return true;
        }

        public override void SetFluentId(string[] fluentsName){}
    }
}
