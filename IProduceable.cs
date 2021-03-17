using Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntactic
{
    /*
        A symbol in the grammar used by the syntactic anaylzer
    */
    public interface IProduceable
    {
        string getProductName();
        List<Token> getFirstSet();
        List<Token> getFollowSet();
        bool isTerminal();
    }
}
