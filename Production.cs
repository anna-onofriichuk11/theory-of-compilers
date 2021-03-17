using COMP442_Assignment2.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP442_Assignment2.Syntactic
{
    /*
        A non-terminal symbol in the grammar used by the syntactic analyzer

        For COMP 442 Assignment 2 by Michael Bilinsky 26992358
    */
    class Production : IProduceable
    {
        string _name;
        public List<Token> _firstSet { get; private set; }
        public List<Token> _followSet { get; private set; }

        public Production(string name, List<Token> firstSet, List<Token> followSet)
        {
            _name = name;
            _firstSet = firstSet;
            _followSet = followSet;
        }

        public string getProductName()
        {
            return string.Format("<{0}>", _name);
        }

        public List<Token> getFirstSet()
        {
            return _firstSet;
        }

        public List<Token> getFollowSet()
        {
            return _followSet;
        }

        public bool isTerminal()
        {
            return false;
        }
    }
}
