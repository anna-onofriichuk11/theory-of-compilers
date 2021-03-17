using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntactic
{
    /*
        Class to store the results of a syntactic analysis
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
