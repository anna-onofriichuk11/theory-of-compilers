using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP442_Assignment2.Syntactic
{
    /*
        Class to store the results of a syntactic analysis

        For COMP 442 Assignment 2 by Michael Bilinsky 26992358
    */
    public class SyntaxResult
    {
        public List<List<IProduceable>> Derivation;
        public List<string> Errors;

        public SyntaxResult()
        {
            Derivation = new List<List<IProduceable>>(); // The states of the parse stack during the parse
            Errors = new List<string>();
        }
    }
}
