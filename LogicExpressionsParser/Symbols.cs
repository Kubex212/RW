using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicExpressionsParser
{
    public interface ISymbol
    {
        int Id { get; }
        char Type { get; }
    }

    public interface ITerminalSymbol : ISymbol { }

    public class Terminal : ITerminalSymbol
    {
        private int id;
        private char type;
        public int Id { get { return id; } }
        public char Type { get { return type; } }
        public Terminal(int i)
        {
            id = i;
            type = 'T';
        }
    }

    public class NonTerminal : ISymbol
    {
        private int id;
        private char type;
        public int Id { get { return id; } }
        public char Type { get { return type; } }
        public NonTerminal(int i)
        {
            id = i;
            type = 'V';
        }
    }
    public class SpecialTerminal : ITerminalSymbol
    {
        private int id;
        private char type;
        public int Id { get { return id; } }
        public char Type { get { return type; } }
        public SpecialTerminal(int i)
        {
            id = i;
            type = 'S';
        }
    }

    public interface IFluentSymbol
    {
        string FluentName { get; }

        void SetFluentName(string n);
    }
    class FluentTerminal : Terminal, IFluentSymbol
    {
        string fluentname;
        public string FluentName { get { return fluentname; } }

        public FluentTerminal(string n) : base(8)
        {
            fluentname = n;
        }
        public void SetFluentName(string n)
        {
            fluentname = n;
        }
    }
    public class FluentSpecialTerminal : SpecialTerminal, IFluentSymbol
    {
        string fluentname;
        public string FluentName { get { return fluentname; } }

        public FluentSpecialTerminal(string n) : base(8)
        {
            fluentname = n;
        }
        public void SetFluentName(string n)
        {
            fluentname = n;
        }
    }
}
