using COMP442_Assignment2.Lexical;
using COMP442_Assignment2.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP442_Assignment2.Syntactic
{
    /*
        This syntactic analyzer is a top-down table
        driven predictive parser. Non-terminal symbols (the beginning of all productions)
        are held in Production classes and their first and follower sets are defined below.
        All productions are defined in Rule classes. The table is also
        automatically generated in this class.

        For COMP 442 Assignment 2 by Michael Bilinsky 26992358
    */
    public class SyntacticAnalyzer
    {
        // The table for the table driver parser, a symbol and terminal token map to a rule
        Dictionary<IProduceable, Dictionary<Token, Rule>> table = new Dictionary<IProduceable, Dictionary<Token, Rule>>();

        // List of rules
        List<Rule> rules = new List<Rule>();

        // List of non-terminal symbols
        List<Production> productions = new List<Production>();

        // The start symbol of the grammar
        IProduceable startProduct;

        public SyntacticAnalyzer()
        {
            // The list of non-terminal symbols, their first and follow sets are passed to their constructor.
            Production prog = new Production("prog", new List<Token> { TokenList.Class, TokenList.Epsilon, TokenList.Program}, new List<Token> { TokenList.EndOfProgram});
            Production classDecl = new Production("classDecl", new List<Token> { TokenList.Class, TokenList.Epsilon}, new List<Token> { TokenList.Program});
            Production varFuncList = new Production("varFuncList", new List<Token> { TokenList.Epsilon, TokenList.IntRes, TokenList.FloatRes, TokenList.Identifier}, new List<Token> { TokenList.CloseCurlyBracket});
            Production varFunc = new Production("varFunc", new List<Token> { TokenList.SemiColon, TokenList.Epsilon, TokenList.OpenSquareBracket, TokenList.OpenParanthesis}, new List<Token> { TokenList.IntRes, TokenList.FloatRes, TokenList.Identifier, TokenList.CloseCurlyBracket});
            Production progBody = new Production("progBody", new List<Token> { TokenList.Program}, new List<Token> { TokenList.EndOfProgram});
            Production funcList = new Production("funcList", new List<Token> { TokenList.Epsilon, TokenList.IntRes, TokenList.FloatRes, TokenList.Identifier}, new List<Token> { TokenList.EndOfProgram});
            Production funcDef = new Production("funcDef", new List<Token> { TokenList.OpenParanthesis}, new List<Token> { TokenList.IntRes, TokenList.FloatRes, TokenList.Identifier, TokenList.EndOfProgram, TokenList.CloseCurlyBracket});
            Production funcBody = new Production("funcBody", new List<Token> { TokenList.OpenCurlyBracket}, new List<Token> { TokenList.SemiColon});
            Production funcBodyList = new Production("funcBodyList", new List<Token> { TokenList.IntRes, TokenList.FloatRes,  TokenList.Identifier, TokenList.Epsilon, TokenList.If, TokenList.For, TokenList.Get, TokenList.Put, TokenList.Return}, new List<Token> { TokenList.CloseCurlyBracket});
            Production idtypeFuncBodyList = new Production("idtypeFuncBodyList", new List<Token> { TokenList.Identifier, TokenList.If, TokenList.For, TokenList.Get, TokenList.Put, TokenList.Return, TokenList.Epsilon, TokenList.OpenSquareBracket, TokenList.Period, TokenList.EqualsToken}, new List<Token> { TokenList.CloseCurlyBracket});
            Production ntypeFuncBodyList = new Production("ntypeFuncBodyList", new List<Token> { TokenList.Identifier}, new List<Token> { TokenList.CloseCurlyBracket});
            Production varDecl = new Production("varDecl", new List<Token> { TokenList.SemiColon, TokenList.Epsilon, TokenList.OpenSquareBracket}, new List<Token> { TokenList.IntRes, TokenList.FloatRes, TokenList.Identifier, TokenList.CloseCurlyBracket, TokenList.If, TokenList.For, TokenList.Get, TokenList.Put, TokenList.Return});
            Production arraySizeList = new Production("arraySizeList", new List<Token> { TokenList.Epsilon, TokenList.OpenSquareBracket}, new List<Token> { TokenList.Comma, TokenList.SemiColon, TokenList.CloseParanthesis});
            Production statement = new Production("statement", new List<Token> { TokenList.If, TokenList.For, TokenList.Get, TokenList.Put,TokenList.Return}, new List<Token> { TokenList.Identifier, TokenList.If,TokenList.For, TokenList.Get, TokenList.Put, TokenList.Return, TokenList.IntRes,TokenList.FloatRes, TokenList.CloseCurlyBracket, TokenList.Else, TokenList.SemiColon});
            Production assignStat = new Production("assignStat", new List<Token> { TokenList.Epsilon, TokenList.OpenSquareBracket, TokenList.Period, TokenList.EqualsToken}, new List<Token> { TokenList.SemiColon, TokenList.CloseParanthesis});
            Production statBlock = new Production("statBlock", new List<Token> { TokenList.OpenCurlyBracket, TokenList.Identifier, TokenList.Epsilon, TokenList.If, TokenList.For, TokenList.Get, TokenList.Put, TokenList.Return}, new List<Token> { TokenList.Else, TokenList.SemiColon});
            Production statementList = new Production("statementList", new List<Token> { TokenList.Identifier, TokenList.Epsilon, TokenList.If, TokenList.For, TokenList.Get, TokenList.Put, TokenList.Return }, new List<Token> { TokenList.CloseCurlyBracket});
            Production expr = new Production("expr", new List<Token> { TokenList.OpenParanthesis, TokenList.Not,TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float}, new List<Token> { TokenList.Comma, TokenList.CloseParanthesis, TokenList.SemiColon });
            Production relOption = new Production("relOption", new List<Token> { TokenList.Epsilon, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual }, new List<Token> { TokenList.Comma, TokenList.CloseParanthesis, TokenList.SemiColon });
            Production relExpr = new Production("relExpr", new List<Token> { TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual }, new List<Token> { TokenList.SemiColon, TokenList.Comma, TokenList.CloseParanthesis});
            Production arithExpr = new Production("arithExpr", new List<Token> { TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float}, new List<Token> { TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma});
            Production arithExprPrime = new Production("arithExprPrime", new List<Token> { TokenList.Epsilon, TokenList.Plus, TokenList.Minus, TokenList.Or}, new List<Token> { TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma });
            Production sign = new Production("sign", new List<Token> { TokenList.Plus, TokenList.Minus}, new List<Token> { TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float});
            Production term = new Production("term", new List<Token> { TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float }, new List<Token> { TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma});
            Production termPrime = new Production("termPrime", new List<Token> { TokenList.Epsilon, TokenList.Asterisk, TokenList.Slash, TokenList.And}, new List<Token> { TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma });
            Production factor = new Production("factor", new List<Token> { TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float }, new List<Token> { TokenList.Asterisk, TokenList.Slash, TokenList.And, TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma});
            Production variable = new Production("variable", new List<Token> { TokenList.Epsilon, TokenList.OpenSquareBracket, TokenList.Period }, new List<Token> { TokenList.EqualsToken, TokenList.CloseParanthesis});
            Production furtherIdNest = new Production("furtherIdNest", new List<Token> { TokenList.Period, TokenList.Epsilon}, new List<Token> { TokenList.EqualsToken, TokenList.CloseParanthesis });
            Production factorVarOrFunc = new Production("factorVarOrFunc", new List<Token> { TokenList.Identifier}, new List<Token> { TokenList.Asterisk, TokenList.Slash, TokenList.And, TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma });
            Production furtherFactor = new Production("furtherFactor", new List<Token> { TokenList.OpenParanthesis, TokenList.Epsilon, TokenList.OpenSquareBracket, TokenList.Period}, new List<Token> { TokenList.Asterisk, TokenList.Slash, TokenList.And, TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma });
            Production furtherIndice = new Production("furtherIndice", new List<Token> { TokenList.Period, TokenList.Epsilon}, new List<Token> { TokenList.Asterisk, TokenList.Slash, TokenList.And, TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma });
            Production indiceList = new Production("indiceList", new List<Token> { TokenList.Epsilon, TokenList.OpenSquareBracket}, new List<Token> { TokenList.Period, TokenList.Asterisk, TokenList.Slash, TokenList.And, TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma, TokenList.EqualsToken });
            Production indice = new Production("indice", new List<Token> { TokenList.OpenSquareBracket}, new List<Token> { TokenList.OpenSquareBracket, TokenList.Period, TokenList.Asterisk, TokenList.Slash, TokenList.And, TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma, TokenList.EqualsToken });
            Production arraySize = new Production("arraySize", new List<Token> { TokenList.OpenSquareBracket}, new List<Token> { TokenList.OpenSquareBracket, TokenList.Comma, TokenList.SemiColon, TokenList.CloseParanthesis});
            Production type = new Production("type", new List<Token> { TokenList.IntRes, TokenList.FloatRes, TokenList.Identifier}, new List<Token> { TokenList.Identifier });
            Production fParams = new Production("fParams", new List<Token> { TokenList.Epsilon, TokenList.IntRes, TokenList.FloatRes, TokenList.Identifier}, new List<Token> { TokenList.CloseParanthesis});
            Production aParams = new Production("aParams", new List<Token> { TokenList.Epsilon, TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float }, new List<Token> { TokenList.CloseParanthesis });
            Production fParamsTail = new Production("fParamsTail", new List<Token> { TokenList.Comma, TokenList.Epsilon}, new List<Token> { TokenList.CloseParanthesis });
            Production aParamsTail = new Production("aParamsTail", new List<Token> { TokenList.Comma, TokenList.Epsilon}, new List<Token> { TokenList.CloseParanthesis });
            Production assignOp = new Production("assignOp", new List<Token> { TokenList.EqualsToken }, new List<Token> { TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float});
            Production relOp = new Production("relOp", new List<Token> { TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual}, new List<Token> { TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float });
            Production addOp = new Production("addOp", new List<Token> { TokenList.Plus, TokenList.Minus, TokenList.Or }, new List<Token> { TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float });
            Production multOp = new Production("multOp", new List<Token> { TokenList.Asterisk, TokenList.Slash, TokenList.And}, new List<Token> { TokenList.OpenParanthesis, TokenList.Not, TokenList.Identifier, TokenList.Plus, TokenList.Minus, TokenList.Integer, TokenList.Float });
            Production num = new Production("num", new List<Token> { TokenList.Integer, TokenList.Float}, new List<Token> { TokenList.Asterisk, TokenList.Slash, TokenList.And, TokenList.Plus, TokenList.Minus, TokenList.Or, TokenList.CloseSquareBracket, TokenList.CloseParanthesis, TokenList.DoubleEquals, TokenList.NotEqual, TokenList.LessThan, TokenList.GreaterThan, TokenList.LessThanOrEqual, TokenList.GreaterThanOrEqual, TokenList.SemiColon, TokenList.Comma });

            // Generate a list of non-terminal symmbols
            productions.AddRange(new List<Production> {
                prog, classDecl, varFuncList, varFunc, progBody, funcList, funcDef, funcBody, funcBodyList,
                idtypeFuncBodyList, ntypeFuncBodyList, varDecl, arraySizeList, statement, assignStat, statBlock, statementList, expr, relOption, relExpr, arithExpr,
                arithExprPrime, sign, term, termPrime, factor, variable, furtherIdNest, factorVarOrFunc, furtherFactor, furtherIndice, indiceList, indice, arraySize,
                type, fParams, aParams, fParamsTail, aParamsTail, assignOp, relOp, addOp, multOp, num
            });

            // The start production of this grammar
            startProduct = prog;

            // All the rules defined in the grammar
            Rule r1 = new Rule(prog, new List<IProduceable> { classDecl, progBody }); // prog -> classDecl progBody
            Rule r2 = new Rule(classDecl, new List<IProduceable> {
                TokenList.Class, TokenList.Identifier, TokenList.OpenCurlyBracket, varFuncList, TokenList.CloseCurlyBracket, TokenList.SemiColon, classDecl
            }); // classDecl -> class id { varFuncList } ; classDecl
            Rule r3 = new Rule(classDecl); // classDecl -> EPSILON
            Rule r4 = new Rule(varFuncList, new List<IProduceable> { type, TokenList.Identifier, varFunc, varFuncList}); // varFuncList->type id varFunc varFuncList
            Rule r5 = new Rule(varFuncList); // varFuncList -> EPSILON
            Rule r6 = new Rule(varFunc, new List<IProduceable> { varDecl }); // varFunc-> varDecl
            Rule r7 = new Rule(varFunc, new List<IProduceable> { funcDef }); // varFunc-> funcDef
            Rule r8 = new Rule(progBody, new List<IProduceable> { TokenList.Program, funcBody, TokenList.SemiColon, funcList }); //progBody -> program funcBody ; funcList
            Rule r9 = new Rule(funcList, new List<IProduceable> { type, TokenList.Identifier, funcDef, funcList}); //funcList -> type id funcDef funcList 
            Rule r10 = new Rule(funcList); // funcList -> EPSILON
            Rule r11 = new Rule(funcDef, new List<IProduceable> { TokenList.OpenParanthesis, fParams, TokenList.CloseParanthesis, funcBody, TokenList.SemiColon}); //funcDef -> ( fParams ) funcBody ;
            Rule r12 = new Rule(funcBody, new List<IProduceable> { TokenList.OpenCurlyBracket, funcBodyList, TokenList.CloseCurlyBracket}); //funcBody -> { funcBodyList }
            Rule r13 = new Rule(funcBodyList, new List<IProduceable> { TokenList.IntRes, ntypeFuncBodyList}); //funcBodyList -> intRes ntypeFuncBodyList
            Rule r14 = new Rule(funcBodyList, new List<IProduceable> { TokenList.FloatRes, ntypeFuncBodyList }); //funcBodyList -> floatRes ntypeFuncBodyList
            Rule r15 = new Rule(funcBodyList, new List<IProduceable> { TokenList.Identifier, idtypeFuncBodyList }); //funcBodyList -> id idtypeFuncBodyList
            Rule r15a = new Rule(funcBodyList, new List<IProduceable> { statement, funcBodyList}); // funcBodyList -> statement funcBodyList
            Rule r16 = new Rule(funcBodyList); // funcBodyList -> EPSILON
            Rule r17 = new Rule(idtypeFuncBodyList, new List<IProduceable> { TokenList.Identifier, varDecl, funcBodyList}); //idtypeFuncBodyList -> id varDecl funcBodyList
            Rule r18 = new Rule(idtypeFuncBodyList, new List<IProduceable> { statement, funcBodyList}); //idtypeFuncBodyList -> statement funcBodyList
            Rule r19 = new Rule(idtypeFuncBodyList, new List<IProduceable> { assignStat, TokenList.SemiColon, funcBodyList}); //idtypeFuncBodyList -> assignStat ; funcBodyList
            Rule r20 = new Rule(ntypeFuncBodyList, new List<IProduceable> { TokenList.Identifier, varDecl, funcBodyList}); // ntypeFuncBodyList -> id varDecl funcBodyList
            Rule r21 = new Rule(varDecl, new List<IProduceable> { arraySizeList, TokenList.SemiColon }); //varDecl -> arraySizeList ;
            Rule r22 = new Rule(arraySizeList, new List<IProduceable> { arraySize, arraySizeList}); //arraySizeList -> arraySize arraySizeList 
            Rule r23 = new Rule(arraySizeList); // arraySizeList -> EPSILON
            Rule r24 = new Rule(statement, new List<IProduceable> {
                TokenList.If, TokenList.OpenParanthesis, expr, TokenList.CloseParanthesis, TokenList.Then, statBlock, TokenList.Else, statBlock, TokenList.SemiColon
            }); // statement ->  if ( expr ) then statBlock else statBlock ;
            Rule r25 = new Rule(statement, new List<IProduceable> {
                TokenList.For, TokenList.OpenParanthesis, type, TokenList.Identifier, assignOp, expr, TokenList.SemiColon, arithExpr, relExpr, TokenList.SemiColon,
                TokenList.Identifier, assignStat, TokenList.CloseParanthesis, statBlock, TokenList.SemiColon
            }); // statement -> for ( type id assignOp expr ; relExpr ; id assignStat ) statBlock ;
            Rule r26 = new Rule(statement, new List<IProduceable> {
                TokenList.Get, TokenList.OpenParanthesis, TokenList.Identifier, variable, TokenList.CloseParanthesis, TokenList.SemiColon
            }); // statement -> get(id variable);
            Rule r27 = new Rule(statement, new List<IProduceable> { TokenList.Put, TokenList.OpenParanthesis, expr, TokenList.CloseParanthesis, TokenList.SemiColon }); // statement -> put ( expr ) ;
            Rule r28 = new Rule(statement, new List<IProduceable> { TokenList.Return, TokenList.OpenParanthesis, expr, TokenList.CloseParanthesis, TokenList.SemiColon }); // statement -> return ( expr ) ;
            Rule r29 = new Rule(assignStat, new List<IProduceable> { variable, assignOp, expr }); //assignStat -> variable assignOp expr
            Rule r30 = new Rule(statBlock, new List<IProduceable> { TokenList.OpenCurlyBracket, statementList, TokenList.CloseCurlyBracket }); // statBlock -> { statementList }
            Rule r31 = new Rule(statBlock, new List<IProduceable> { statement });  // statBlock -> statement
            Rule r32 = new Rule(statBlock, new List<IProduceable> { TokenList.Identifier, assignStat, TokenList.SemiColon }); // statBlock -> id assignStat ;
            Rule r33 = new Rule(statBlock); // statBlock -> EPSILON
            Rule r34 = new Rule(statementList, new List<IProduceable> { statement, statementList}); // statementList -> statement statementList
            Rule r35 = new Rule(statementList, new List<IProduceable> { TokenList.Identifier, assignStat, TokenList.SemiColon, statementList }); // statementList -> id assignStat ; statementList
            Rule r36 = new Rule(statementList); // statementList -> EPSILON
            Rule r37 = new Rule(expr, new List<IProduceable> { arithExpr, relOption }); // expr -> arithExpr relOption
            Rule r38 = new Rule(relOption, new List<IProduceable> { relExpr }); // relOption->relExpr
            Rule r39 = new Rule(relOption); // relOption -> EPSILON
            Rule r40 = new Rule(relExpr, new List<IProduceable> { relOp, arithExpr }); //relExpr -> relOp arithExpr
            Rule r41 = new Rule(arithExpr, new List<IProduceable> { term, arithExprPrime }); // arithExpr -> term arithExprPrime
            Rule r42 = new Rule(arithExprPrime, new List<IProduceable> { addOp, term, arithExprPrime }); //arithExprPrime -> addOp term arithExprPrime
            Rule r43 = new Rule(arithExprPrime); // arithExprPrime -> EPSILON
            Rule r44 = new Rule(sign, new List<IProduceable> { TokenList.Plus }); // sign -> +
            Rule r45 = new Rule(sign, new List<IProduceable> { TokenList.Minus }); // sign -> -
            Rule r46 = new Rule(term, new List<IProduceable> { factor, termPrime}); //term -> factor termPrime
            Rule r47 = new Rule(termPrime, new List<IProduceable> { multOp, factor, termPrime }); // termPrime -> multOp factor termPrime
            Rule r48 = new Rule(termPrime); // termPrime -> EPSILON
            Rule r49 = new Rule(factor, new List<IProduceable> { factorVarOrFunc }); // factor -> factorVarOrFunc
            Rule r50 = new Rule(factor, new List<IProduceable> { num }); // factor -> num 
            Rule r51 = new Rule(factor, new List<IProduceable> { TokenList.OpenParanthesis, arithExpr, TokenList.CloseParanthesis }); // factor -> ( arithExpr )
            Rule r52 = new Rule(factor, new List<IProduceable> { TokenList.Not, factor }); // factor -> not factor
            Rule r53 = new Rule(factor, new List<IProduceable> { sign, factor }); // factor -> sign factor
            Rule r54 = new Rule(variable, new List<IProduceable> { indiceList, furtherIdNest }); // variable -> indiceList furtherIdNest
            Rule r55 = new Rule(furtherIdNest, new List<IProduceable> { TokenList.Period, TokenList.Identifier, indiceList, furtherIdNest}); // furtherIdNest -> . id indiceList furtherIdNest
            Rule r56 = new Rule(furtherIdNest); // furtherIdNest  -> EPSILON
            Rule r57 = new Rule(factorVarOrFunc, new List<IProduceable> { TokenList.Identifier, furtherFactor}); // factorVarOrFunc -> id furtherFactor
            Rule r58 = new Rule(furtherFactor, new List<IProduceable> { indiceList, furtherIndice }); //furtherFactor -> indiceList furtherIndice
            Rule r59 = new Rule(furtherFactor, new List<IProduceable> { TokenList.OpenParanthesis, aParams, TokenList.CloseParanthesis }); //furtherFactor -> ( aParams )
            Rule r60 = new Rule(furtherIndice, new List<IProduceable> { TokenList.Period, factorVarOrFunc}); //furtherIndice -> . factorVarOrFunc 
            Rule r61 = new Rule(furtherIndice); // furtherIndice  -> EPSILON
            Rule r62 = new Rule(indiceList, new List<IProduceable> { indice, indiceList}); // indiceList -> indice indiceList
            Rule r63 = new Rule(indiceList); // indiceList -> EPSILON
            Rule r64 = new Rule(indice, new List<IProduceable> { TokenList.OpenSquareBracket, arithExpr, TokenList.CloseSquareBracket }); // indice -> [ arithExpr ]
            Rule r65 = new Rule(arraySize, new List<IProduceable> { TokenList.OpenSquareBracket, TokenList.Integer, TokenList.CloseSquareBracket}); // arraySize -> [ integer ]
            Rule r66 = new Rule(type, new List<IProduceable> { TokenList.IntRes}); // type -> intRes
            Rule r67 = new Rule(type, new List<IProduceable> { TokenList.FloatRes }); // type -> floatRes
            Rule r68 = new Rule(type, new List<IProduceable> { TokenList.Identifier }); // type -> id
            Rule r69 = new Rule(fParams, new List<IProduceable> { type, TokenList.Identifier, arraySizeList, fParamsTail}); // fParams -> type id arraySizeList fParamsTail
            Rule r70 = new Rule(fParams); // fParams -> EPSILON
            Rule r71 = new Rule(aParams, new List<IProduceable> { expr, aParamsTail}); // aParams -> expr aParamsTail
            Rule r72 = new Rule(aParams); // aParams -> EPSILON
            Rule r73 = new Rule(fParamsTail, new List<IProduceable> { TokenList.Comma, type, TokenList.Identifier, arraySizeList, fParamsTail}); // fParamsTail -> , type id arraySizeList fParamsTail
            Rule r74 = new Rule(fParamsTail); // fParamsTail  -> EPSILON
            Rule r75 = new Rule(aParamsTail, new List<IProduceable> { TokenList.Comma, expr, aParamsTail}); // aParamsTail -> , expr aParamsTail
            Rule r76 = new Rule(aParamsTail); // aParamsTail  -> EPSILON
            Rule r77 = new Rule(assignOp, new List<IProduceable> { TokenList.EqualsToken }); // assignOp -> =

            // relOp -> == | <> | < | > | <= | >=
            Rule r78 = new Rule(relOp, new List<IProduceable> { TokenList.DoubleEquals});
            Rule r79 = new Rule(relOp, new List<IProduceable> { TokenList.NotEqual });
            Rule r80 = new Rule(relOp, new List<IProduceable> { TokenList.LessThan });
            Rule r81 = new Rule(relOp, new List<IProduceable> { TokenList.GreaterThan });
            Rule r82 = new Rule(relOp, new List<IProduceable> { TokenList.LessThanOrEqual });
            Rule r83 = new Rule(relOp, new List<IProduceable> { TokenList.GreaterThanOrEqual });

            // addOp -> + | - | or
            Rule r84 = new Rule(addOp, new List<IProduceable> { TokenList.Plus });
            Rule r85 = new Rule(addOp, new List<IProduceable> { TokenList.Minus });
            Rule r86 = new Rule(addOp, new List<IProduceable> { TokenList.Or });

            // multOp -> * | / | and
            Rule r87 = new Rule(multOp, new List<IProduceable> { TokenList.Asterisk });
            Rule r88 = new Rule(multOp, new List<IProduceable> { TokenList.Slash });
            Rule r89 = new Rule(multOp, new List<IProduceable> { TokenList.And });

            // num -> integer | float
            Rule r90 = new Rule(num, new List<IProduceable> { TokenList.Integer});
            Rule r91 = new Rule(num, new List<IProduceable> { TokenList.Float});

            // I didn't write this out by hand, trust me
            rules.AddRange(new List<Rule> { r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12, r13, r14,
                r15, r15a, r16, r17, r18, r19, r20, r21, r22, r23, r24, r25, r26, r27, r28, r29, r30, r31, r32,
                r33, r34, r35, r36, r37, r38, r39, r40, r41, r42, r43, r44, r45, r46, r47, r48, r49, r50,
                r51, r52, r53, r54, r55, r56, r57, r58, r59, r60, r61, r62, r63, r64, r65, r66, r67, r68,
                r69, r70, r71, r72, r73, r74, r75, r76, r77, r78, r79, r80, r81, r82, r83, r84, r85, r86, r87, r88, r89, r90, r91 });

            // Generate the table for the predictions from the rules
            foreach (Rule rule in rules)
            {
                Production production = rule.getProduction();

                // Add a row in the table if it doesn't exist
                if (!table.ContainsKey(production))
                {
                    table.Add(rule.getProduction(), new Dictionary<Token, Rule>());
                }


                // Add the tokens for the rule to the table
                foreach (Token token in rule.getTableSet())
                {
                    // Ensure that the grammar is a valid LL(1) grammar
                    if (table[production].ContainsKey(token))
                        Console.WriteLine("Illegal Duplicate Key");

                    table[production].Add(token, rule);
                    rule.addPredict(token);
                }
            }

            Console.WriteLine("done!");
        }

        // The analyze method for the syntactic analyzer takes in a list
        // of tokens from the lexical analyzer, derives the grammar and handles errors
        public SyntaxResult analyzeSyntax(List<IToken> lexical)
        {
            SyntaxResult results = new SyntaxResult();

            // Remove all errors and comments from the lexical analyzer
            lexical.RemoveAll(x => x.isError() || x.getToken() == TokenList.BlockComment || x.getToken() == TokenList.LineComment);
            
            // Add an end of program token
            lexical.Add(new SimpleToken(TokenList.EndOfProgram, false));

            // The stack for the table parser
            Stack<IProduceable> parseStack = new Stack<IProduceable>();

            parseStack.Push(TokenList.EndOfProgram);
            parseStack.Push(startProduct);

            var tokenEnumerator = lexical.GetEnumerator();
            // Start enumerating over the list
            tokenEnumerator.MoveNext();

            // The table driven algorithm as seen in class slides
            while(parseStack.Peek() != TokenList.EndOfProgram)
            {
                var top = parseStack.Peek();

                if(top.isTerminal())
                {
                    if (top == tokenEnumerator.Current.getToken())
                    {
                        parseStack.Pop();
                        tokenEnumerator.MoveNext();
                    }
                    else
                    {
                        string resumeMessage = skipErrors(ref tokenEnumerator, parseStack);
                        results.Errors.Add(string.Format("Error Parsing terminal: Expecting {0}, got {1} at token {2}. {3}", top.getProductName(), tokenEnumerator.Current.getToken().getProductName(), tokenEnumerator.Current.getName(), resumeMessage));
                    }
                        
                }
                else
                {
                    Rule rule;
                    Token token = tokenEnumerator.Current.getToken();
                    var row = table[top]; // consider removing

                    if (table[top].TryGetValue(token, out rule))
                    {
                        parseStack.Pop();

                        for (int i = rule.getSymbols().Count - 1; i >= 0; i--)
                        {
                            if(rule.getSymbols()[i] != TokenList.Epsilon)
                                parseStack.Push(rule.getSymbols()[i]);
                        }
                    }
                    else // if no entry exists for a symbol token pair, then it is an error
                    {
                        string resumeMessage = skipErrors(ref tokenEnumerator, parseStack);
                        results.Errors.Add(string.Format("Could not find rule for produce {0} to produce {1} at token {2}. {3}", top.getProductName() ,token.getProductName(), tokenEnumerator.Current.getName(), resumeMessage));
                    }
                        
                }

                // Add the current state of the stack to the derivation list
                results.Derivation.Add(new List<IProduceable>(parseStack));
            }

            return results;
        }

        // Skip errors in the syntax following the algorithm shown in class
        private string skipErrors(ref List<IToken>.Enumerator tokenEnumerator, Stack<IProduceable> parseStack)
        {
            var topFirstSet = parseStack.Peek().getFirstSet();
            var topFollowSet = parseStack.Peek().getFollowSet();

            if (tokenEnumerator.Current.getToken() == TokenList.EndOfProgram || topFollowSet.Contains(tokenEnumerator.Current.getToken()))
                parseStack.Pop();
            else
            {
                while (!(tokenEnumerator.Current.getToken() == TokenList.EndOfProgram) && !topFirstSet.Contains(tokenEnumerator.Current.getToken()) &&
                    (!topFollowSet.Contains(tokenEnumerator.Current.getToken())))
                {
                    tokenEnumerator.MoveNext();
                }
            }

             return "Resuming parsing at: " + tokenEnumerator.Current.getName();
                
        }

        // Print a list of rules
        public void printRules()
        {
            var grouping = rules.GroupBy(x => x.getProduction());

            foreach(var group in grouping)
            {
                Console.WriteLine(string.Format("{0} -> {1}", group.Key.getProductName(), string.Join(" | ", group.Select(x => string.Join(" ", x.getSymbols().Select(y => y.getProductName()))))));
            }
        }

        // Print a list of non-terminal symbols
        public void printProductions()
        {
            foreach(var production in productions)
            {
                Console.WriteLine(string.Join(", ", production._followSet.Select(x => x.getProductName())));
            }
        }

        // Print a list of prediction sets
        public void printPredicts()
        {
            foreach(var rule in rules)
            {
                Console.WriteLine(string.Format("{1}", rule.getProduction().getProductName(), string.Join(", ", rule.getPredicts().Select(x => x.getProductName()))));
            }
        }
    }
}
