using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicExpressionsParser
{
    class Production
    {
        ISymbol[] left;
        ISymbol[] right;

        public ISymbol[] Left { get { return left; } }
        public ISymbol[] Right { get { return right; } }

        public Production(ISymbol[] l, ISymbol[] r)
        {
            left = l;
            right = r;
        }
    }

    public class GrammarException:LogicExpressionsParserException
    {
        public GrammarException(string m):base(m)
        {
            msg = "GrammarException: " + msg;
        }

        public GrammarException()
        {
            msg = "GrammarException";
        }
    }

    static class GramatykaLL1
    {
        static Terminal[] T = new Terminal[10]
            {
                        new Terminal(0),/* OR */
                        new Terminal(1),/* AND */
                        new Terminal(2),/* => */
                        new Terminal(3),/* <= */
                        new Terminal(4),/* <=> */
                        new Terminal(5),/* NOT */
                        new Terminal(6),/* ( */
                        new Terminal(7),/* ) */
                        new Terminal(8),/* VAR */
                        new Terminal(9) /* -| */
            };
        static SpecialTerminal[] S = new SpecialTerminal[9]
            {
                        new SpecialTerminal(0),/* OR */
                        new SpecialTerminal(1),/* AND */
                        new SpecialTerminal(2),/* => */
                        new SpecialTerminal(3),/* <= */
                        new SpecialTerminal(4),/* <=> */
                        new SpecialTerminal(5),/* NOT */
                        new SpecialTerminal(6),/* ( */
                        new SpecialTerminal(7),/* ) */
                        new SpecialTerminal(8) /* VAR */
            };
        static NonTerminal[] V = new NonTerminal[11]
            {
                        new NonTerminal(0),/* E */
                        new NonTerminal(1),/* E1 */
                        new NonTerminal(2),/* E2 */
                        new NonTerminal(3),/* E3 */
                        new NonTerminal(4),/* E4 */
                        new NonTerminal(5),/* E5 */
                        new NonTerminal(6),/* Ev */
                        new NonTerminal(7),/* Ea */
                        new NonTerminal(8),/* E= */
                        new NonTerminal(9),/* E- */
                        new NonTerminal(10)/* E() */
            };


        static Production[] P = new Production[18]
            {
                        new Production(new ISymbol[1]{ V[0]}, new ISymbol[2] { V[5],V[6] }),           /* E -> E5 Ev */
                        new Production(new ISymbol[1]{ V[6]}, new ISymbol[4] { T[0],V[5],S[0],V[6] }), /* Ev -> v E5 {v} Ev */
                        new Production(new ISymbol[1]{ V[6]}, new ISymbol[0] { }),                     /* E -> eps */
                        new Production(new ISymbol[1]{ V[5]}, new ISymbol[2] { V[4],V[7] }),           /* E5 -> E4 Ea */
                        new Production(new ISymbol[1]{ V[7]}, new ISymbol[4] { T[1],V[4],S[1],V[7]}),  /* Ea -> a E4 {a} Ea */
                        new Production(new ISymbol[1]{ V[7]}, new ISymbol[0] { }),                     /* Ea -> eps */
                        new Production(new ISymbol[1]{ V[4]}, new ISymbol[2] { V[3],V[8]}),            /* E4 -> E3 E= */
                        new Production(new ISymbol[1]{ V[8]}, new ISymbol[4] { T[2],V[3],S[2],V[8]}),  /* E= -> => E3 {=>} E= */
                        new Production(new ISymbol[1]{ V[8]}, new ISymbol[4] { T[3],V[3],S[3],V[8]}),  /* E= -> <= E3 {<=} E= */
                        new Production(new ISymbol[1]{ V[8]}, new ISymbol[4] { T[4],V[3],S[4],V[8]}),  /* E= -> <=> E3 {<=>} E= */
                        new Production(new ISymbol[1]{ V[8]}, new ISymbol[0] { }),                     /* E= -> eps */
                        new Production(new ISymbol[1]{ V[3]}, new ISymbol[1] { V[9]}),                 /* E3 -> E- */
                        new Production(new ISymbol[1]{ V[9]}, new ISymbol[3] { T[5],V[9],S[5]}),       /* E- -> - E- {-} */
                        new Production(new ISymbol[1]{ V[9]}, new ISymbol[1] { V[2]}),                 /* E- -> E2 */
                        new Production(new ISymbol[1]{ V[2]}, new ISymbol[1] { V[10]}),                /* E2 -> E() */
                        new Production(new ISymbol[1]{ V[10]}, new ISymbol[3] { T[6],V[0],T[7]}),      /* E() -> ( E ) */
                        new Production(new ISymbol[1]{ V[10]}, new ISymbol[1] { V[1]}),                /* E() -> E1 */
                        new Production(new ISymbol[1]{ V[1]}, new ISymbol[2] { T[8],S[8]})             /* E1 -> var {var} */
            };
        static Terminal[][] SELECT = new Terminal[18][]
            {
                        new Terminal[3] { T[5],T[8],T[6] },         /* E -> E5 Ev */
                        new Terminal[1] { T[0] },                   /* Ev -> v E5 {v} Ev */
                        new Terminal[2] { T[7], T[9] },             /* E -> eps */
                        new Terminal[3] { T[5],T[8],T[6] },         /* E5 -> E4 Ea */
                        new Terminal[1] { T[1] },                   /* Ea -> a E4 {a} Ea */
                        new Terminal[3] { T[7], T[0] ,T[9] },       /* Ea -> eps */
                        new Terminal[3] { T[5],T[8],T[6] },         /* E4 -> E3 E= */
                        new Terminal[1] { T[2] },                   /* E= -> => E3 {=>} E= */
                        new Terminal[1] { T[3] },                   /* E= -> <= E3 {<=} E= */
                        new Terminal[1] { T[4] },                   /* E= -> <=> E3 {<=>} E= */
                        new Terminal[4] { T[7], T[0], T[1] ,T[9] }, /* E= -> eps */
                        new Terminal[3] { T[5],T[8],T[6] },         /* E3 -> E- */
                        new Terminal[1] { T[5] },                   /* E- -> - E- {-} */
                        new Terminal[2] { T[6], T[8] },             /* E- -> E2 */
                        new Terminal[2] { T[6], T[8] },             /* E2 -> E() */
                        new Terminal[1] { T[6] },                   /* E() -> ( E ) */
                        new Terminal[1] { T[8] },                   /* E() -> E1 */
                        new Terminal[1] { T[8] }                    /* E1 -> var {var} */
            };

        static private bool InSelect(Terminal t, Terminal[] arroft)
        {
            for (int i = 0; i < arroft.Length; i++)
            {
                if (arroft[i].Id == t.Id)
                    return true;
            }

            return false;
        }

        static private int SelectProduction(NonTerminal nt, Terminal t)
        {
            for (int i = 0; i < P.Length; i++)
            {
                if (P[i].Left[0].Id == nt.Id && InSelect(t, SELECT[i]))
                    return i;
            }

            throw new GrammarException("Cannot choose the right production by selection.");
        }

        static public SpecialTerminal[] TranslateWord(Terminal[] terminalword)
        { 
            Stack<ISymbol> Stos = new Stack<ISymbol>();
            Stos.Push(V[0]);

            Stack<SpecialTerminal> translated = new Stack<SpecialTerminal>();

            int i = 0;
            while (Stos.Count > 0 /*&& i <= terminalword.Length*/)
            {
                ISymbol s = Stos.Pop();

                switch (s.Type)
                {
                    case 'V':
                        {
                            if (i == terminalword.Length)
                            {
                                Production prod = P[SelectProduction((NonTerminal)s, T[9])];

                                ISymbol[] right = prod.Right;

                                if (prod.Left[0].Id == V[1].Id)
                                {
                                    Stos.Push(new FluentSpecialTerminal(((IFluentSymbol)terminalword[i]).FluentName));
                                    Stos.Push(new FluentTerminal(((IFluentSymbol)terminalword[i]).FluentName));
                                }
                                else
                                {
                                    for (int j = right.Length - 1; j >= 0; j--)
                                    {
                                        Stos.Push(right[j]);
                                    }
                                }
                            }
                            else
                            {
                                Production prod = P[SelectProduction((NonTerminal)s, terminalword[i])];

                                ISymbol[] right = prod.Right;

                                if (prod.Left[0].Id == V[1].Id)
                                {
                                    Stos.Push(new FluentSpecialTerminal(((IFluentSymbol)terminalword[i]).FluentName));
                                    Stos.Push(new FluentTerminal(((IFluentSymbol)terminalword[i]).FluentName));
                                }
                                else
                                {
                                    for (int j = right.Length - 1; j >= 0; j--)
                                    {
                                        Stos.Push(right[j]);
                                    }
                                }
                            }
                        }
                        break;
                    case 'T':
                        {
                            i++;
                        }
                        break;
                    case 'S':
                        {
                            translated.Push((SpecialTerminal)s);
                        }
                        break;
                }
            }

            if(i<terminalword.Length)
            {
                throw new GrammarException("Not the whole word was translated.");
            }

            return translated.Reverse().ToArray();
        }
    }
}
