using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicExpressionsParser
{
    public class SplitingAutomatException : LogicExpressionsParserException
    {
        public SplitingAutomatException(string m) : base(m)
        {
            msg = "SplitingAutomatException: " + msg;
        }

        public SplitingAutomatException()
        {
            msg = "SplitingAutomatException";
        }
    }
    static class SplitingAutomat
    {
        static (string outValue, int newAutomatState) AutomatState0(char s, char next)
        {
            switch(s)
            {
                case ' ':
                    return ("", 0);
                case '(':
                    return ("( ", 0);
                case ')':
                    return (") ", 0);
                case '~':
                    return ("~ ", 0);
                case '&':
                    return ("&", 2);
                case '-':
                    return ("-", 3);
                case '|':
                    return ("|", 4);
                case '<':
                    return ("<", 5);
                default:
                    if((s>='a' && s<='z') || (s >= 'A' && s <= 'Z') || (s >= '0' && s <= '9'))
                    {
                        if ((next >= 'a' && next <= 'z') || (next >= 'A' && next <= 'Z') || (next >= '0' && next <= '9'))
                            return ("" + s, 1);
                        else
                            return (s + " ", 0);
                    }
                    else
                    {
                        throw new SplitingAutomatException("Incorrect symbol read in state 0");
                    }
            }
        }

        static (string outValue, int newAutomatState) AutomatState1(char s, char next)
        {

            if ((s >= 'a' && s <= 'z') || (s >= 'A' && s <= 'Z') || (s >= '0' && s <= '9'))
            {
                if ((next >= 'a' && next <= 'z') || (next >= 'A' && next <= 'Z') || (next >= '0' && next <= '9'))
                    return ("" + s, 1);
                else
                    return (s + " ", 0);
            }
            else
            {
                throw new SplitingAutomatException("Incorrect symbol read in state 1");
            }
        }

        static (string outValue, int newAutomatState) AutomatState2(char s)
        {
            if(s=='&')
            {
                return ("& ", 0);
            }
            else
            {
                throw new SplitingAutomatException("Incorrect symbol read in state 2");
            }
        }

        static (string outValue, int newAutomatState) AutomatState3(char s)
        {
            if (s == '>')
            {
                return ("> ", 0);
            }
            else
            {
                throw new SplitingAutomatException("Incorrect symbol read in state 3");
            }
        }

        static (string outValue, int newAutomatState) AutomatState4(char s)
        {
            if (s == '|')
            {
                return ("| ", 0);
            }
            else
            {
                throw new SplitingAutomatException("Incorrect symbol read in state 4");
            }
        }

        static (string outValue, int newAutomatState) AutomatState5(char s, char next)
        {
            if(s=='-')
            {
                if(next == '>')
                {
                    return ("-", 6);
                }
                else
                {
                    return ("- ", 0);
                }
            }
            else
            {
                throw new SplitingAutomatException("Incorrect symbol read in state 5");
            }
        }

        static (string outValue, int newAutomatState) AutomatState6(char s)
        {
            if (s == '>')
            {
                return ("> ", 0);
            }
            else
            {
                throw new SplitingAutomatException("Incorrect symbol read in state 6");
            }
        }

        static public string Split(string word)
        {
            StringBuilder splitedWordStringBuilder = new StringBuilder();

            int automatState = 0;

            string automatStateOut;

            for(int i=0;i<word.Length-1;i++)
            {
                switch(automatState)
                {
                    case 0:
                        (automatStateOut, automatState) = AutomatState0(word[i], word[i + 1]);
                        splitedWordStringBuilder.Append(automatStateOut);
                        break;
                    case 1:
                        (automatStateOut, automatState) = AutomatState1(word[i], word[i + 1]);
                        splitedWordStringBuilder.Append(automatStateOut);
                        break;
                    case 2:
                        (automatStateOut, automatState) = AutomatState2(word[i]);
                        splitedWordStringBuilder.Append(automatStateOut);
                        break;
                    case 3:
                        (automatStateOut, automatState) = AutomatState3(word[i]);
                        splitedWordStringBuilder.Append(automatStateOut);
                        break;
                    case 4:
                        (automatStateOut, automatState) = AutomatState4(word[i]);
                        splitedWordStringBuilder.Append(automatStateOut);
                        break;
                    case 5:
                        (automatStateOut, automatState) = AutomatState5(word[i], word[i + 1]);
                        splitedWordStringBuilder.Append(automatStateOut);
                        break;
                    case 6:
                        (automatStateOut, automatState) = AutomatState6(word[i]);
                        splitedWordStringBuilder.Append(automatStateOut);
                        break;
                    default:
                        throw new SplitingAutomatException($"Unknown state number: {automatState}");
                }
            }

            splitedWordStringBuilder.Append(word[word.Length - 1]);

            return splitedWordStringBuilder.ToString();
        }
    }
}
