using COMP442_Assignment2.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP442_Assignment2.Syntactic
{
    /*
        A symbol in the grammar used by the syntactic anaylzer

        For COMP 442 Assignment 2 by Michael Bilinsky 26992358
    */
    public interface IProduceable
    {
        string getProductName();
        List<Token> getFirstSet();
        List<Token> getFollowSet();
        bool isTerminal();
    }
}
