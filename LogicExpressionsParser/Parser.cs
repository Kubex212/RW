using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicExpressionsParser
{
    public class LogicExpressionsParserException : SystemException
    {
        public string msg;

        public LogicExpressionsParserException()
        {
            msg = "LogicExpressionsParserException";
        }

        public LogicExpressionsParserException(string m)
        {
            msg = m;
        }
    }
    public class ParserException : LogicExpressionsParserException
    {
        public ParserException(string m) : base(m)
        {
            msg = "ParserException: " + msg;
        }

        public ParserException()
        {
            msg = "ParserException";
        }
    }
    public static class Parser
    {
        static Terminal StringToTerminal(string s)
        {
            switch (s)
            {
                case "||":
                    return new Terminal(0);
                case "OR":
                    return new Terminal(0);
                case "or":
                    return new Terminal(0);
                case "&&":
                    return new Terminal(1);
                case "AND":
                    return new Terminal(1);
                case "and":
                    return new Terminal(1);
                case "->":
                    return new Terminal(2);
                case "<-":
                    return new Terminal(3);
                case "<->":
                    return new Terminal(4);
                case "~":
                    return new Terminal(5);
                case "NOT":
                    return new Terminal(5);
                case "not":
                    return new Terminal(5);
                case "(":
                    return new Terminal(6);
                case ")":
                    return new Terminal(7);
                default:
                    return new FluentTerminal(s);
            }
        }
        static Terminal[] WrittenWordsToTErminals(string[] words)
        {
            Terminal[] terms = new Terminal[words.Length];

            for (int i = 0; i < terms.Length; i++)
            {
                terms[i] = StringToTerminal(words[i]);
            }

            return terms;
        }

        static public LogicalExpressionRoot Parse(string formula)
        {
            string toSplitFormula = SplitingAutomat.Split(formula);
            string[] splittedToArrFormula = toSplitFormula.Split(' ');
            Terminal[] formulaInTerminals = WrittenWordsToTErminals(splittedToArrFormula);
            SpecialTerminal[] formulaInONP = GramatykaLL1.TranslateWord(formulaInTerminals);
            Stack<SpecialTerminal> stackFormulaONP = new Stack<SpecialTerminal>(formulaInONP);

            return new LogicalExpressionRoot(stackFormulaONP);
        }
    }
}
