using COMP442_Assignment2.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP442_Assignment2.Syntactic
{
    /*
        A single rule for the grammar used by the syntactic analyzer
        where a non-terminal symbol can produce a single set of
        symbols

        For COMP 442 Assignment 2 by Michael Bilinsky 26992358
    */
    class Rule
    {
        Production _production;
        List<IProduceable> _symbols;
        List<Token> _predicts = new List<Token>();

        // If the rule does not produce any symbols, add the epsilon token
        public Rule(Production production)
        {
            _production = production;
            _symbols = new List<IProduceable> { Tokens.TokenList.Epsilon};
        }

        // Create a rule with a list of produced symbols
        public Rule(Production production, List<IProduceable> symbols)
        {
            _production = production;
            _symbols = symbols;
        }

        // Create a string for this rule
        public string printProduction()
        {
            return string.Format("{0} -> {1}", _production.getProductName(), string.Join(" ", _symbols.Select(x => x.getProductName())));
        }

        // add a token to the predict set for this rule
        public void addPredict(Token product)
        {
            if (_predicts.Contains(product))
                Console.WriteLine("LLC Rule Violation");

            _predicts.Add(product);
        }

        public Production getProduction()
        {
            return _production;
        }

        public List<IProduceable> getSymbols()
        {
            return _symbols;
        }

        public List<Token> getPredicts()
        {
            return _predicts;
        }

        // Get all the tokens were this rule should be placed
        // in the prediction table
        public List<Token> getTableSet()
        {
            List<Token> firstSets = new List<Token>();

            bool epsilonFound = false;

            // Go through each symbol in this production
            foreach(IProduceable product in _symbols)
            {
                List<Token> productFirstSets = product.getFirstSet();

                epsilonFound = false;

                foreach(Token token in productFirstSets)
                {
                    if (token != TokenList.Epsilon)
                        firstSets.Add(token);
                    else
                        epsilonFound = true;
                }

                // If the symbol's first set does not contain epsilon, 
                // no need to continue reading symbols
                if (!epsilonFound)
                    break;
            }

            // If all the symbols contained epsilons, add the
            // non-terminal's follow set
            if (epsilonFound)
                firstSets.AddRange(_production.getFollowSet());

            return firstSets;
        }
    }
}
