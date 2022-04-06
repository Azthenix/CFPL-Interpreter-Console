using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace CFPL_Interpreter_Console
{
    class Interpreter
    {
        #region DFA
		int[,] DeclareReworkDFA = new int[15,18]{
//                var    numid  charid boolid ass    num    char   bool   lpar   rpar   check  andor  not    muldiv addsub comma  AS     TYPE
	/* 0 */       {1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1},   // 0
	/* 1 */       {-1,   2,     2,     2,     -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1},   // 1
	/* 2 */       {-1,   -1,    -1,    -1,    3,     -1,    14,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    1,     12,    -1},   // 2
	/* 3 */       {-1,   4,     11,    4,     -1,    7,     14,    10,    5,     -1,    -1,    -1,    5,     -1,    6,     -1,    -1,    -1},   // 3
	/* 4 */       {-1,   -1,    -1,    -1,    3,     -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    6,     6,     1,     12,    -1},   // 4
	/* 5 */       {-1,   7,     -1,    10,    -1,    7,     -1,    10,    5,     -1,    -1,    -1,    5,     -1,    6,     -1,    -1,    -1},   // 5
	/* 6 */       {-1,   7,     -1,    -1,    -1,    7,     -1,    -1,    6,     -1,    -1,    -1,    -1,    -1,    6,     -1,    -1,    -1},   // 6
	/* 7 */       {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    7,     8,     -1,    -1,    6,     6,     1,     12,    -1},   // 7
	/* 8 */       {-1,   9,     -1,    -1,    -1,    9,     -1,    -1,    8,     -1,    -1,    -1,    -1,    -1,    8,     -1,    -1,    -1},   // 8
	/* 9 */       {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    9,     -1,    5,     -1,    8,     8,     1,     12,    -1},   // 9
	/* 10 */      {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    10,    -1,    5,     -1,    -1,    -1,    1,     12,    -1},   // 10
	/* 11 */      {-1,   -1,    -1,    -1,    2,     -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    12,    -1},   // 11
	/* 12 */      {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    13},   // 12
	/* 13 */      {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1},   // 13
	/* 14 */      {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    1,     12,    -1}    // 14
		};

		int[,] OutputReworkDFA = new int[12,17]{
//                out    colon  numid  charid boolid string num    char   bool   lpar   rpar   check  andor  not    muldiv addsub amp
	/* 0 */       {1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1},   // 0
	/* 1 */       {-1,   2,     -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1},   // 1
	/* 2 */       {-1,   -1,    6,     11,    10,    3,     6,     11,    10,    4,     -1,    -1,    -1,    9,     -1,    5,     -1},   // 2
	/* 3 */       {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    2},    // 3
	/* 4 */       {-1,   -1,    6,     11,    10,    3,     6,     11,    10,    4,     -1,    -1,    -1,    9,     -1,    5,     -1},   // 4
	/* 5 */       {-1,   -1,    6,     -1,    -1,    -1,    6,     -1,    -1,    5,     -1,    -1,    -1,    -1,    -1,    -1,    -1},   // 5
	/* 6 */       {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    6,     7,     -1,    -1,    5,     5,     2},    // 6
	/* 7 */       {-1,   -1,    8,     -1,    -1,    -1,    8,     -1,    -1,    7,     -1,    -1,    -1,    -1,    -1,    7,     -1},   // 7
	/* 8 */       {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    8,     -1,    4,     -1,    7,     7,     2},    // 8
	/* 9 */       {-1,   -1,    -1,    -1,    10,    -1,    -1,    -1,    10,    9,     -1,    -1,    -1,    9,     -1,    -1,    -1},   // 9
	/* 10 */      {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    10,    -1,    4,     -1,    -1,    -1,    2},    // 10
	/* 11 */      {-1,   -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    2},    // 11
		};



        int[,] checkNumAssDFA = new int[6, 8]{
            {1, -1, -1, -1, -1, -1, -1, -1},
            {-1, 2, -1, -1, -1, 4, -1, -1},
            {3, -1, 5, 4, -1, -1, -1, 4},
            {-1, 2, -1, -1, -1, 4, 4, 4},
            {5, -1, 5, 4, -1, -1, -1, 4},
            {-1, -1, -1, -1, 5, -1, 4, 4},
        };

        int[,] checkCharAssDFA = new int[6, 3]{
            {1, -1, -1},
            {-1, 2, -1},
            {3, -1, 5},
            {-1, 4, -1},
            {3, -1, 5},
            {-1, -1, -1}
        };

        int[,] checkBoolAssDFA = new int[9, 10]{
            {1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, 2, -1, -1, -1, -1, -1, -1, -1, -1},
            {3, -1, 5, 7, 4, -1, 4, -1, -1, -1},
            {-1, 2, -1, -1, -1, -1, -1, -1, -1, -1},
            {5, -1, 5, 7, 4, -1, 4, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, 6, -1, 8},
            {7, -1, 7, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, 7, -1, -1, 4, -1},
            {5, -1, 5, -1, -1, -1, -1, -1, -1, -1}
        };

        int[,] inputDFA = new int[4, 4]{
            {1, -1, -1, -1},
            {-1, 2, -1, -1},
            {-1, -1, 3, -1},
            {-1, -1, -1, 2},
        };

        int[,] structureDFA = new int[8, 9]{
            {1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, 2, -1, -1, -1, -1, -1, -1, -1},
            {-1, 2, -1, 3, 5, -1, -1, -1, 2},
            {-1, -1, 3, -1, -1, 4, 2, 2, -1},
            {-1, 4, -1, 5, -1, -1, -1, -1, -1},
            {-1, -1, 5, -1, -1, -1, 7, 6, -1},
            {-1, 6, -1, 3, -1, -1, -1, -1, -1},
            {-1, 7, -1, 3, 5, -1, -1, -1, 7},
        };
        #endregion

        enum bType
        {
            INT,
            FLOAT,
            CHAR,
            BOOL
        };

        List<Token> tokenList;
        string[] lines;
        Dictionary<string, bType> variables;
        Dictionary<string, int> intVars;
        Dictionary<string, char> charVars;
        Dictionary<string, bool> boolVars;
        Dictionary<string, float> floatVars;

        const string symbols = "()[]*/+-%&><>=,#:\"\'";
        List<string> symbolsArray = new List<string> { "(", ")", "[", "]", "*", "/", "+", "-", "%", "&", ">", "<", "==", ">=", "<=", "=", "," };
        List<string> reserved = new List<string>{"INT", "CHAR", "BOOL", "FLOAT", "AND", "OR", "NOT", "WHILE", "IF", "ELSE", "TRUE", "FALSE",
                                        "VAR", "AS", "START", "STOP", "OUTPUT", "INPUT"};



        public Interpreter(string file)
        {
            variables = new Dictionary<string, bType>();
            intVars = new Dictionary<string, int>();
            charVars = new Dictionary<string, char>();
            boolVars = new Dictionary<string, bool>();
            floatVars = new Dictionary<string, float>();

            tokenList = new List<Token>();

            lines = File.ReadAllText(file)
            .Replace("\"TRUE\"", "TRUE")
            .Replace("\"FALSE\"", "FALSE")
            .Replace("[\"]", "$DQUOTE$")
            .Replace("[&]", "$AMP$")
            .Replace("\r", "")
            .Split('\n');



            for (int x = 0; x < lines.Length; x++)
            {
                string[] dqsplit = lines[x].Split('\"');

                for (int y = 1; y < dqsplit.Length; y += 2)
                {
                    dqsplit[y] = dqsplit[y].Replace("[[]", "$LBRACKET$").Replace("[]]", "$RBRACKET$").Replace("[#]", "$SHARP$");
                }

                lines[x] = String.Join('\"', dqsplit);
            }
        }

        public void Run()
        {
            Lex();
            Interpret();
        }

        void Lex()
        {
            int ctr = 1;
            foreach (string line in lines)
            {
                string ln = line.Trim();

                if (ln.Length == 0 || ln[0] == '*')
                {
                    ctr++;
                    continue;
                }

                StringBuilder lit = new StringBuilder();

                for (int x = 0; x < ln.Length; x++)
                {
                    if (Char.IsLetterOrDigit(ln[x]) || ln[x] == '_' || ln[x] == '.' || ln[x] == '$')
                    {
                        lit.Append(ln[x]);
                    }
                    else
                    {
                        if (symbols.Contains(ln[x]))
                        {
                            if (lit.Length > 0)
                                addToken(lit.ToString(), ctr);

                            switch (ln[x])
                            {
                                case '(':
                                    tokenList.Add(new Token(Lexeme.LPAR, null, ctr));
                                    break;
                                case ')':
                                    tokenList.Add(new Token(Lexeme.RPAR, null, ctr));
                                    break;
                                case '*':
                                    if (x < ln.Length && ln[x + 1] == '=')
                                    {
                                        tokenList.Add(new Token(Lexeme.UAST, null, ctr));
                                        x++;
                                    }
                                    else
                                    {
                                        tokenList.Add(new Token(Lexeme.AST, null, ctr));
                                    }
                                    break;
                                case '/':
                                    if (x < ln.Length && ln[x + 1] == '=')
                                    {
                                        tokenList.Add(new Token(Lexeme.UFSLASH, null, ctr));
                                        x++;
                                    }
                                    else
                                    {
                                        tokenList.Add(new Token(Lexeme.FSLASH, null, ctr));
                                    }
                                    break;
                                case '+':
                                    if (x < ln.Length && ln[x + 1] == '=')
                                    {
                                        tokenList.Add(new Token(Lexeme.UPLUS, null, ctr));
                                        x++;
                                    }
                                    else
                                    {
                                        tokenList.Add(new Token(Lexeme.PLUS, null, ctr));
                                    }
                                    break;
                                case '-':
                                    if (x < ln.Length && ln[x + 1] == '=')
                                    {
                                        tokenList.Add(new Token(Lexeme.UMINUS, null, ctr));
                                        x++;
                                    }
                                    else
                                    {
                                        tokenList.Add(new Token(Lexeme.MINUS, null, ctr));
                                    }
                                    break;
                                case '%':
                                    if (x < ln.Length && ln[x + 1] == '=')
                                    {
                                        tokenList.Add(new Token(Lexeme.UPERCENT, null, ctr));
                                        x++;
                                    }
                                    else
                                    {
                                        tokenList.Add(new Token(Lexeme.PERCENT, null, ctr));
                                    }
                                    break;
                                case '&':
                                    tokenList.Add(new Token(Lexeme.AMP, null, ctr));
                                    break;
                                case '>':
                                    if (x < ln.Length && ln[x + 1] == '=')
                                    {
                                        tokenList.Add(new Token(Lexeme.GEQUAL, null, ctr));
                                        x++;
                                    }
                                    else
                                    {
                                        tokenList.Add(new Token(Lexeme.GREATER, null, ctr));
                                    }
                                    break;
                                case '<':
                                    if (x < ln.Length && ln[x + 1] == '=')
                                    {
                                        tokenList.Add(new Token(Lexeme.LEQUAL, null, ctr));
                                        x++;
                                    }
                                    else if (x < ln.Length && ln[x + 1] == '>')
                                    {
                                        tokenList.Add(new Token(Lexeme.NEQUAL, null, ctr));
                                        x++;
                                    }
                                    else
                                    {
                                        tokenList.Add(new Token(Lexeme.LESSER, null, ctr));
                                    }
                                    break;
                                case '=':
                                    if (x < ln.Length && ln[x + 1] == '=')
                                    {
                                        tokenList.Add(new Token(Lexeme.EQUAL, null, ctr));
                                        x++;
                                    }
                                    else
                                    {
                                        tokenList.Add(new Token(Lexeme.ASSIGN, null, ctr));
                                    }
                                    break;
                                case ',':
                                    tokenList.Add(new Token(Lexeme.COMMA, null, ctr));
                                    break;
                                case ':':
                                    tokenList.Add(new Token(Lexeme.COLON, null, ctr));
                                    break;
                                case '\"':
                                    lit.Clear();
                                    try
                                    {
                                        while (ln[++x] != '\"')
                                        {
                                            if (ln[x] == '[' || ln[x] == ']')
                                            {
                                                throw new ErrorException($"Illegal escape code '{ln[x]}' on line {ctr}.");
                                            }

                                            lit.Append(ln[x]);
                                        }
                                        tokenList.Add(new Token(Lexeme.STRING, lit.ToString(), ctr));
                                        lit.Clear();
                                    }
                                    catch (IndexOutOfRangeException)
                                    {
                                        throw new ErrorException($"Missing '\"' on line {ctr}.");
                                    }
                                    lit.Clear();
                                    break;
                                case '\'':
                                    lit.Clear();
                                    try
                                    {
                                        if (ln[x + 2] == '\'')
                                        {
                                            if (ln[x + 1] == '[' || ln[x + 1] == ']' || ln[x + 1] == '\"')
                                            {
                                                throw new ErrorException($"Illegal character '{ln[x + 1]}' on line {ln[x + 1]}.");
                                            }
                                            lit.Append(ln[x + 1]);
                                            tokenList.Add(new Token(Lexeme.CHARACTER, ln[x + 1] == '#' ? "\n" : lit.ToString(), ctr));

                                            x += 2;
                                        }
                                        else if (ln[x + 4] == '\'')
                                        {
                                            string sub = ln.Substring(x + 1, 3);
                                            if (sub == "[[]" || sub == "[]]" || sub == "[\"]" || sub == "[#]")
                                            {
                                                tokenList.Add(new Token(Lexeme.CHARACTER, sub, ctr));
                                            }

                                            x += 4;
                                        }
                                        lit.Clear();
                                    }
                                    catch (IndexOutOfRangeException)
                                    {
                                        throw new ErrorException($"Missing ''' on line {ln[x]}.");
                                    }
                                    break;
                                default:
                                    throw new ErrorException($"Unknown character '{ln[x]}' on line x.");
                            }

                            lit.Clear();
                        }
                        else if (ln[x] == ' ')
                        {
                            if (lit.Length > 0)
                            {
                                addToken(lit.ToString(), ctr);
                                lit.Clear();
                            }
                        }
                        else
                        {
                            throw new ErrorException($"Unknown character '{ln[x]}' on line {ctr}.");
                        }
                    }
                }

                if (lit.Length > 0)
                    addToken(lit.ToString(), ctr);

                tokenList.Add(new Token(Lexeme.NEWLINE, null, ctr));
                ctr += 1;
            }
        }

        void addToken(string literal, int ctr)
        {
            if (reserved.Contains(literal))
            {
                if (literal == "TRUE" || literal == "FALSE")
                {
                    tokenList.Add(new Token(Lexeme.BOOLEAN, literal, ctr));
                }
                else
                {
                    tokenList.Add(new Token(Enum.Parse<Lexeme>(literal), null, ctr));
                }
                return;
            }
            if (Char.IsDigit(literal[0]))
            {
                if (!literal.All(x => Char.IsDigit(x) || x == '.'))
                {
                    throw new ErrorException($"Illegal Identifier '{literal}' on line {ctr}.");
                }

                tokenList.Add(new Token(Lexeme.NUMBER, literal, ctr));
                return;
            }
            if (Char.IsLetter(literal[0]) || literal[0] == '_' || literal[0] == '$')
            {
                if (!literal.All(x => Char.IsLetterOrDigit(x) || x == '_' || literal[0] == '$'))
                {
                    throw new ErrorException($"Illegal Identifier '{literal}' on line {ctr}.");
                }

                tokenList.Add(new Token(Lexeme.IDENTIFIER, literal, ctr));
                return;
            }

            throw new ErrorException($"Illegal Identifier '{literal}' on line {ctr}.");
        }

        void Interpret()
        {
            List<Token> tks = new List<Token>(tokenList);

            for (int x = 0; x < tks.Count; x++)
            {
                switch (tks[x].lex)
                {
                    case Lexeme.VAR:
                        checkDeclareRework(x);
                        DeclareRework(x, ref x);
                        break;
                    case Lexeme.START:
                        if (tokenList[x + 1].lex != Lexeme.NEWLINE)
                        {
                            throw new ErrorException($"Illegal '{tks[x + 1].lex}' on line {tks[x + 1].line}.");
                        }
                        x++;
                        executeBody(x, ref x, false);
                        break;
                    case Lexeme.NEWLINE:
                        break;
                    default:
                        throw new ErrorException($"Illegal '{tks[x].lex}' on line {tks[x].line}.");
                }
            }
        }

        void executeBody(int index, ref int y, bool skip)
        {
            int x = index;

            int skipTo;

            for (; x < tokenList.Count; x++)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.IDENTIFIER:
                        if (tokenList[x + 1].lex != Lexeme.ASSIGN && (tokenList[x + 1].lex < Lexeme.UAST && tokenList[x + 1].lex > Lexeme.UPERCENT))
                        {
                            throw new ErrorException($"Illegal '{tokenList[x].literal}' on line {tokenList[x].line}.");
                        }

                        switch (variables[tokenList[x].literal])
                        {
                            case bType.INT:
                            case bType.FLOAT:
                                skipTo = checkAssignToNum(x);
                                if (skip)
                                {
                                    x = skipTo;
                                }
                                else
                                {
                                    assignToNum(x, ref x);
                                }
                                break;
                            case bType.CHAR:
                                skipTo = checkAssignToChar(x);
                                if (skip)
                                {
                                    x = skipTo;
                                }
                                else
                                {
                                    assignToChar(x, ref x);
                                }
                                break;
                            case bType.BOOL:
                                skipTo = checkAssignToBool(x);
                                if (skip)
                                {
                                    x = skipTo;
                                }
                                else
                                {
                                    assignToBool(x, ref x);
                                }
                                break;
                        }
                        break;
                    case Lexeme.WHILE:
                        checkStructure(x);
                        int loopIn = x;
                        bool loop = evaluateBool(x + 1, ref x);

                        while (tokenList[++x].lex == Lexeme.NEWLINE) ;

                        if (tokenList[x].lex == Lexeme.START)
                        {
                            x++;
                            if (tokenList[x].lex != Lexeme.NEWLINE)
                            {
                                throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                            }
                            executeBody(x, ref x, skip || !loop);
                        }
                        else
                        {
                            throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                        }
                        if (!(skip || !loop))
                        {
                            x = loopIn - 1;
                        }
                        break;
                    case Lexeme.IF:
                        checkStructure(x);
                        bool run = evaluateBool(x + 1, ref x);

                        while (tokenList[++x].lex == Lexeme.NEWLINE) ;

                        if (tokenList[x].lex == Lexeme.START)
                        {
                            x++;
                            if (tokenList[x].lex != Lexeme.NEWLINE)
                            {
                                throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                            }
                            executeBody(x, ref x, skip || !run);
                        }
                        else
                        {
                            throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                        }

                        while (tokenList[++x].lex == Lexeme.NEWLINE) ;

                        if (tokenList[x].lex == Lexeme.ELSE)
                        {
                            while (tokenList[++x].lex == Lexeme.NEWLINE) ;

                            if (tokenList[x].lex == Lexeme.START)
                            {
                                x++;
                                if (tokenList[x].lex != Lexeme.NEWLINE)
                                {
                                    throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                                }
                                executeBody(x, ref x, skip || run);
                            }
                            else
                            {
                                throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                            }
                        }
                        x--;
                        break;
                    case Lexeme.STOP:
                        y = x + 1;
                        return;
                    case Lexeme.OUTPUT:
                        skipTo = checkOutputRework(x);
                        if (skip)
                        {
                            x = skipTo;
                        }
                        else
                        {
                            OutputRework(x, ref x);
                        }
                        break;
                    case Lexeme.INPUT:
                        checkInput(x);
                        Input(x, ref x);
                        break;
                    case Lexeme.NEWLINE:
                        break;
                    default:
                        throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                }
            }

            y = x + 1;
        }

        void DeclareRework(int index, ref int y)
        {
            int x = index;
            while (tokenList[x + 1].lex != Lexeme.NEWLINE)
            {
                x++;
            }

            bType t;

            switch (tokenList[x].lex)
            {
                case Lexeme.INT:
                    t = bType.INT;
                    break;
                case Lexeme.FLOAT:
                    t = bType.FLOAT;
                    break;
                case Lexeme.BOOL:
                    t = bType.BOOL;
                    break;
                default:
                    t = bType.CHAR;
                    break;
            }

            x = index;

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                if (tokenList[x].lex == Lexeme.IDENTIFIER)
                {
                    DeclareAssign(x, ref x, t);
                }

                x++;
            }

            y = x;
        }

        void checkDeclareRework(int index)
        {
            int state = 0;
            int x = index;
            int pars = 0;

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.VAR:
                        state = DeclareReworkDFA[state, 0];
                        break;
                    case Lexeme.IDENTIFIER:
                        if (state > 4)
                        {
                            if (!variables.ContainsKey(tokenList[x].literal))
                            {
                                throw new ErrorException($"Use of unassigned variable '{tokenList[x].literal}' on line {tokenList[x].line}.");
                            }
                            switch (variables[tokenList[x].literal])
                            {
                                case bType.INT:
                                case bType.FLOAT:
                                    state = DeclareReworkDFA[state, 1];
                                    break;
                                case bType.CHAR:
                                    state = DeclareReworkDFA[state, 2];
                                    break;
                                case bType.BOOL:
                                    state = DeclareReworkDFA[state, 3];
                                    break;
                            }
                        }
                        else
                        {
                            state = DeclareReworkDFA[state, 1];
                        }
                        break;
                    case Lexeme.ASSIGN:
                        state = DeclareReworkDFA[state, 4];
                        break;
                    case Lexeme.NUMBER:
                        state = DeclareReworkDFA[state, 5];
                        break;
                    case Lexeme.CHARACTER:
                        state = DeclareReworkDFA[state, 6];
                        break;
                    case Lexeme.BOOLEAN:
                        state = DeclareReworkDFA[state, 7];
                        break;
                    case Lexeme.LPAR:
                        pars++;
                        state = DeclareReworkDFA[state, 8];
                        break;
                    case Lexeme.RPAR:
                        if (pars == 0)
                        {
                            throw new ErrorException($"Illegal closing ')' on line {tokenList[x].line}.");
                        }
                        pars--;
                        state = DeclareReworkDFA[state, 9];
                        break;
                    case Lexeme.GREATER:
                    case Lexeme.LESSER:
                    case Lexeme.EQUAL:
                    case Lexeme.GEQUAL:
                    case Lexeme.LEQUAL:
                    case Lexeme.NEQUAL:
                        state = DeclareReworkDFA[state, 10];
                        break;
                    case Lexeme.AND:
                    case Lexeme.OR:
                        state = DeclareReworkDFA[state, 11];
                        break;
                    case Lexeme.NOT:
                        state = DeclareReworkDFA[state, 12];
                        break;
                    case Lexeme.AST:
                    case Lexeme.FSLASH:
                        state = DeclareReworkDFA[state, 13];
                        break;
                    case Lexeme.PLUS:
                    case Lexeme.MINUS:
                        state = DeclareReworkDFA[state, 14];
                        break;
                    case Lexeme.COMMA:
                        if (pars != 0)
                        {
                            throw new ErrorException($"Unclosed '(' on line {tokenList[x].line}.");
                        }
                        state = DeclareReworkDFA[state, 15];
                        break;
                    case Lexeme.AS:
                        state = DeclareReworkDFA[state, 16];
                        break;
                    case Lexeme.INT:
                    case Lexeme.FLOAT:
                    case Lexeme.CHAR:
                    case Lexeme.BOOL:
                        state = DeclareReworkDFA[state, 17];
                        break;
                    default:
                        state = -1;
                        break;
                }

                if (state == -1)
                {
                    throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");
                }

                x++;
            }


            if (state != 13)
                throw new ErrorException($"Invalid assignment on line {tokenList[x].line}.");
        }

        void DeclareAssign(int index, ref int y, bType t)
        {
            int x = index;
            if (variables.ContainsKey(tokenList[x].literal))
            {
                throw new ErrorException($"Variable '{tokenList[x].literal}' has already been declared previously.");
            }

            variables[tokenList[x].literal] = t;

            if (tokenList[x + 1].lex == Lexeme.AS || tokenList[x + 1].lex == Lexeme.COMMA)
            {
                y = x;
                return;
            }

            x += 2;
            switch (tokenList[x + 1].lex)
            {
                case Lexeme.ASSIGN:
                    DeclareAssign(x, ref x, t);
                    switch (t)
                    {
                        case bType.INT:
                            intVars[tokenList[index].literal] = intVars[tokenList[index + 2].literal];
                            break;
                        case bType.FLOAT:
                            floatVars[tokenList[index].literal] = floatVars[tokenList[index + 2].literal];
                            break;
                        case bType.BOOL:
                            boolVars[tokenList[index].literal] = boolVars[tokenList[index + 2].literal];
                            break;
                        case bType.CHAR:
                            charVars[tokenList[index].literal] = charVars[tokenList[index + 2].literal];
                            break;
                    }
                    break;
                default:
                    Object res = Evaluate(x, ref x);
                    switch (t)
                    {
                        case bType.INT:
                            if (res is not Int32)
                            {
                                if (res is Single || res is Decimal || res is Double)
                                {
                                    float r = Convert.ToSingle(res);
                                    if (r % 1 != 0)
                                        throw new ErrorException($"Cannot assign FLOAT to type INT on line {tokenList[x].line}.");

                                    intVars[tokenList[index].literal] = Convert.ToInt32(r);
                                }
                                else
                                {
                                    throw new ErrorException($"Cannot assign " + (res is bool ? "BOOL" : "CHAR") + $" to type INT on line {tokenList[x].line}.");
                                }
                            }

                            intVars[tokenList[index].literal] = Convert.ToInt32(res);

                            break;
                        case bType.FLOAT:
                            if (res is not Int32 && res is not Single && res is not Double && res is not Decimal)
                                throw new ErrorException($"Cannot assign " + (res is bool ? "BOOL" : "CHAR") + $" to type FLOAT on line {tokenList[x].line}.");

                            floatVars[tokenList[index].literal] = Convert.ToSingle(res);

                            break;
                        case bType.BOOL:
                            if (res is not Boolean)
                                throw new ErrorException($"Cannot assign " + (res is Int32 ? "INT" : res is Single ? "FLOAT" : "CHAR") + $" to type BOOL on line {tokenList[x].line}.");

                            boolVars[tokenList[index].literal] = Convert.ToBoolean(res);

                            break;
                        case bType.CHAR:
                            if (res is not String)
                                throw new ErrorException($"Cannot assign " + (res is Int32 ? "INT" : res is Single ? "FLOAT" : "BOOL") + $" to type CHAR on line {tokenList[x].line}.");

                            charVars[tokenList[index].literal] = res.ToString()[0];

                            break;
                    }
                    break;
            }

            y = x;
        }

        Object Evaluate(int index, ref int y)
        {
            int x = index;
            DataTable dt = new DataTable();

            string value;
            string pad = "";

            if (tokenList[x - 1].lex >= Lexeme.UAST && tokenList[x - 1].lex <= Lexeme.UPERCENT)
            {
                switch (variables[tokenList[index].literal])
                {
                    case bType.INT:
                        value = intVars[tokenList[index].literal].ToString();
                        break;
                    case bType.FLOAT:
                        value = floatVars[tokenList[index].literal].ToString();
                        break;
                    default:
                        throw new ErrorException($"Illegal IDENTIFIER '{tokenList[x].literal}' on line {tokenList[x].line}.");
                }

                switch (tokenList[index + 1].lex)
                {
                    case Lexeme.UAST:
                        pad = $" {value} * ";
                        break;
                    case Lexeme.UFSLASH:
                        pad = $" {value} / ";
                        break;
                    case Lexeme.UPLUS:
                        pad = $" {value} + ";
                        break;
                    case Lexeme.UMINUS:
                        pad = $" {value} - ";
                        break;
                    case Lexeme.UPERCENT:
                        pad = $" {value} % ";
                        break;
                }
                x += 2;
            }

            Object res = dt.Compute(pad + GetEquation(x, ref x), "");

            y = x;

            return res;
        }

        string GetEquation(int index, ref int y)
        {
            int x = index;

            StringBuilder sb = new StringBuilder();

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.IDENTIFIER:
                        try
                        {
                            switch (variables[tokenList[x].literal])
                            {
                                case bType.INT:
                                    sb.Append(" " + intVars[tokenList[x].literal].ToString());
                                    break;
                                case bType.FLOAT:
                                    sb.Append(" " + floatVars[tokenList[x].literal].ToString());
                                    break;
                                case bType.CHAR:
                                    sb.Append(" '" + charVars[tokenList[x].literal].ToString() + "'");
                                    break;
                                case bType.BOOL:
                                    sb.Append(" " + boolVars[tokenList[x].literal].ToString());
                                    break;
                                default:
                                    throw new ErrorException($"Illegal IDENTIFIER '{tokenList[x].literal}' on line {tokenList[x].line}.");
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ErrorException($"Use of undeclared or unassigned variable '{tokenList[x].literal}' on line {tokenList[x].line}.");
                        }
                        break;
                    case Lexeme.NUMBER:
                        sb.Append(" " + tokenList[x].literal);
                        break;
                    case Lexeme.CHARACTER:
                        sb.Append(" '" + tokenList[x].literal + "'");
                        break;
                    case Lexeme.BOOLEAN:
                        sb.Append(" " + tokenList[x].literal);
                        break;
                    case Lexeme.LPAR:
                        sb.Append(" (");
                        break;
                    case Lexeme.RPAR:
                        sb.Append(" )");
                        break;
                    case Lexeme.AST:
                        sb.Append(" *");
                        break;
                    case Lexeme.FSLASH:
                        sb.Append(" /");
                        break;
                    case Lexeme.PLUS:
                        sb.Append(" +");
                        break;
                    case Lexeme.MINUS:
                        sb.Append(" -");
                        break;
                    case Lexeme.PERCENT:
                        sb.Append(" %");
                        break;
                    case Lexeme.GREATER:
                        sb.Append(" >");
                        break;
                    case Lexeme.LESSER:
                        sb.Append(" <");
                        break;
                    case Lexeme.EQUAL:
                        sb.Append(" =");
                        break;
                    case Lexeme.GEQUAL:
                        sb.Append(" >=");
                        break;
                    case Lexeme.LEQUAL:
                        sb.Append(" <=");
                        break;
                    case Lexeme.NEQUAL:
                        sb.Append(" <>");
                        break;
                    case Lexeme.NOT:
                        sb.Append(" NOT");
                        break;
                    case Lexeme.AND:
                        sb.Append(" AND");
                        break;
                    case Lexeme.OR:
                        sb.Append(" OR");
                        break;
                    default:
                        y = x;

                        return sb.ToString();
                }
                x++;
            }

            y = x - 1;

            return sb.ToString();
        }

        float assignToNum(int index, ref int y)
        {
            int x = index;
            float res;

            x += 2;

            if (tokenList[x + 1].lex == Lexeme.ASSIGN || (tokenList[x + 1].lex >= Lexeme.UAST && tokenList[x + 1].lex <= Lexeme.UPERCENT))
            {
                if (tokenList[x].lex == Lexeme.IDENTIFIER)
                {
                    res = assignToNum(x, ref x);

                    switch (variables[tokenList[index].literal])
                    {
                        case bType.INT:
                            intVars[tokenList[index].literal] = Convert.ToInt32(res);
                            break;
                        case bType.FLOAT:
                            floatVars[tokenList[index].literal] = res;
                            break;
                        default:
                            throw new ErrorException($"Illegal IDENTIFIER '{tokenList[x].literal}' on line {tokenList[x].line}.");
                    }

                    y = x;

                    return res;
                }
            }

            StringBuilder sb = new StringBuilder();

            string value;

            if (tokenList[index + 1].lex != Lexeme.ASSIGN)
            {
                switch (variables[tokenList[index].literal])
                {
                    case bType.INT:
                        value = intVars[tokenList[index].literal].ToString();
                        break;
                    case bType.FLOAT:
                        value = floatVars[tokenList[index].literal].ToString();
                        break;
                    default:
                        throw new ErrorException($"Illegal IDENTIFIER '{tokenList[x].literal}' on line {tokenList[x].line}.");
                }

                switch (tokenList[index + 1].lex)
                {
                    case Lexeme.UAST:
                        sb.Append($"{value} *");
                        break;
                    case Lexeme.UFSLASH:
                        sb.Append($"{value} /");
                        break;
                    case Lexeme.UPLUS:
                        sb.Append($"{value} +");
                        break;
                    case Lexeme.UMINUS:
                        sb.Append($"{value} -");
                        break;
                    case Lexeme.UPERCENT:
                        sb.Append($"{value} %");
                        break;
                }
            }

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.IDENTIFIER:
                        try
                        {
                            switch (variables[tokenList[x].literal])
                            {
                                case bType.INT:
                                    sb.Append(intVars[tokenList[x].literal]);
                                    break;
                                case bType.FLOAT:
                                    sb.Append(floatVars[tokenList[x].literal]);
                                    break;
                                default:
                                    throw new ErrorException($"Illegal IDENTIFIER '{tokenList[x].literal}' on line {tokenList[x].line}.");
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ErrorException($"Use of unassigned variable '{tokenList[x].literal}' on line {tokenList[x].line}.");
                        }
                        break;
                    case Lexeme.NUMBER:
                        sb.Append(tokenList[x].literal);
                        break;
                    case Lexeme.LPAR:
                        sb.Append('(');
                        break;
                    case Lexeme.RPAR:
                        sb.Append(')');
                        break;
                    case Lexeme.AST:
                        sb.Append('*');
                        break;
                    case Lexeme.FSLASH:
                        sb.Append('/');
                        break;
                    case Lexeme.PLUS:
                        sb.Append('+');
                        break;
                    case Lexeme.MINUS:
                        sb.Append('-');
                        break;
                    case Lexeme.PERCENT:
                        sb.Append('%');
                        break;
                    default:
                        throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                }
                x++;
            }

            y = x;

            DataTable dt = new DataTable();

            string equation = sb.ToString();

            res = Convert.ToSingle(dt.Compute(sb.ToString(), ""));

            switch (variables[tokenList[index].literal])
            {
                case bType.INT:
                    intVars[tokenList[index].literal] = Convert.ToInt32(dt.Compute(sb.ToString(), ""));
                    break;
                case bType.FLOAT:
                    floatVars[tokenList[index].literal] = Convert.ToSingle(dt.Compute(sb.ToString(), ""));
                    break;
            }

            return res;
        }

        int checkAssignToNum(int index)
        {
            int state = 0;
            int x = index;
            int pars = 0;

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.IDENTIFIER:
                        state = checkNumAssDFA[state, 0];
                        break;
                    case Lexeme.ASSIGN:
                        state = checkNumAssDFA[state, 1];
                        break;
                    case Lexeme.NUMBER:
                        state = checkNumAssDFA[state, 2];
                        break;
                    case Lexeme.LPAR:
                        pars++;
                        state = checkNumAssDFA[state, 3];
                        break;
                    case Lexeme.RPAR:
                        if (--pars < 0)
                        {
                            throw new ErrorException($"Illegal ')' on line {tokenList[x].line}.");
                        }
                        state = checkNumAssDFA[state, 4];
                        break;
                    case Lexeme.UAST:
                    case Lexeme.UFSLASH:
                    case Lexeme.UPLUS:
                    case Lexeme.UMINUS:
                    case Lexeme.UPERCENT:
                        state = checkNumAssDFA[state, 5];
                        break;
                    case Lexeme.AST:
                    case Lexeme.FSLASH:
                    case Lexeme.PERCENT:
                        state = checkNumAssDFA[state, 6];
                        break;
                    case Lexeme.PLUS:
                    case Lexeme.MINUS:
                        state = checkNumAssDFA[state, 7];
                        break;
                    default:
                        state = -1;
                        break;
                }

                if (state == -1)
                {
                    throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");
                }

                x++;
            }

            if (state != 3 && state != 5)
                throw new ErrorException($"Invalid assignment on line {tokenList[x].line}.");

            return x;
        }

        char assignToChar(int index, ref int y)
        {
            int x = index;
            char res;

            x += 2;

            if (tokenList[x + 1].lex == Lexeme.ASSIGN)
            {
                if (tokenList[x].lex == Lexeme.IDENTIFIER)
                {
                    res = assignToChar(x, ref x);
                    charVars[tokenList[index].literal] = res;

                    y = x;

                    return res;
                }
            }

            if (tokenList[x].lex == Lexeme.IDENTIFIER)
            {
                res = charVars[tokenList[x].literal];
            }
            else
            {
                res = tokenList[x].literal[0];
            }

            y = x;

            charVars[tokenList[index].literal] = res;

            return res;
        }

        int checkAssignToChar(int index)
        {
            int state = 0;
            int x = index;

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.IDENTIFIER:
                        state = checkCharAssDFA[state, 0];
                        break;
                    case Lexeme.ASSIGN:
                        state = checkCharAssDFA[state, 1];
                        break;
                    case Lexeme.CHARACTER:
                        state = checkCharAssDFA[state, 2];
                        break;
                    default:
                        state = -1;
                        break;
                }

                if (state == -1)
                {
                    throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");
                }

                x++;
            }

            if (state != 3 && state != 5)
                throw new ErrorException($"Invalid assignment on line {tokenList[x].line}.");

            return x;
        }

        bool assignToBool(int index, ref int y)
        {
            int x = index;
            bool res;

            x += 2;

            if (tokenList[x + 1].lex == Lexeme.ASSIGN)
            {
                if (tokenList[x].lex == Lexeme.IDENTIFIER)
                {
                    res = assignToBool(x, ref x);

                    boolVars[tokenList[index].literal] = res;

                    y = x;

                    return res;
                }
            }

            boolVars[tokenList[index].literal] = evaluateBool(x, ref x);

            y = x;

            return true;
        }

        bool evaluateBool(int index, ref int y)
        {
            bool res;
            int x = index;

            List<string> eval = new List<string>();

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.IDENTIFIER:
                        try
                        {
                            switch (variables[tokenList[x].literal])
                            {
                                case bType.INT:
                                    eval.Add(intVars[tokenList[x].literal].ToString());
                                    break;
                                case bType.FLOAT:
                                    eval.Add(floatVars[tokenList[x].literal].ToString());
                                    break;
                                case bType.CHAR:
                                    eval.Add(charVars[tokenList[x].literal].ToString());
                                    break;
                                case bType.BOOL:
                                    eval.Add(boolVars[tokenList[x].literal].ToString());
                                    break;
                                default:
                                    throw new ErrorException($"Illegal IDENTIFIER '{tokenList[x].literal}' on line {tokenList[x].line}.");
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ErrorException($"Use of unassigned variable '{tokenList[x].literal}' on line {tokenList[x].line}.");
                        }
                        break;
                    case Lexeme.NUMBER:
                        eval.Add(tokenList[x].literal);
                        break;
                    case Lexeme.CHARACTER:
                        eval.Add($"'{tokenList[x].literal}'");
                        break;
                    case Lexeme.BOOLEAN:
                        eval.Add($"{tokenList[x].literal}");
                        break;
                    case Lexeme.LPAR:
                        eval.Add("(");
                        break;
                    case Lexeme.RPAR:
                        eval.Add(")");
                        break;
                    case Lexeme.AST:
                        eval.Add("*");
                        break;
                    case Lexeme.FSLASH:
                        eval.Add("/");
                        break;
                    case Lexeme.PLUS:
                        eval.Add("+");
                        break;
                    case Lexeme.MINUS:
                        eval.Add("-");
                        break;
                    case Lexeme.PERCENT:
                        eval.Add("%");
                        break;
                    case Lexeme.GREATER:
                        eval.Add(">");
                        break;
                    case Lexeme.LESSER:
                        eval.Add("<");
                        break;
                    case Lexeme.EQUAL:
                        eval.Add("=");
                        break;
                    case Lexeme.GEQUAL:
                        eval.Add(">=");
                        break;
                    case Lexeme.LEQUAL:
                        eval.Add("<=");
                        break;
                    case Lexeme.NEQUAL:
                        eval.Add("<>");
                        break;
                    case Lexeme.NOT:
                        eval.Add("NOT");
                        break;
                    case Lexeme.AND:
                        eval.Add("AND");
                        break;
                    case Lexeme.OR:
                        eval.Add("OR");
                        break;
                    default:
                        throw new ErrorException($"Illegal '{tokenList[x].lex}' on line {tokenList[x].line}.");
                }
                x++;
            }

            y = x;

            DataTable dt = new DataTable();

            res = (bool)dt.Compute(String.Join(' ', eval), "");

            return res;
        }

        int checkAssignToBool(int index)
        {
            int state = 0;
            int x = index;
            int pars = 0;

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.IDENTIFIER:
                        state = checkBoolAssDFA[state, 0];
                        break;
                    case Lexeme.ASSIGN:
                        state = checkBoolAssDFA[state, 1];
                        break;
                    case Lexeme.STRING:
                    case Lexeme.NUMBER:
                    case Lexeme.CHARACTER:
                        state = checkBoolAssDFA[state, 2];
                        break;
                    case Lexeme.BOOLEAN:
                        state = checkBoolAssDFA[state, 3];
                        break;
                    case Lexeme.LPAR:
                        pars++;
                        state = checkBoolAssDFA[state, 4];
                        break;
                    case Lexeme.RPAR:
                        if (--pars < 0)
                        {
                            throw new ErrorException($"Illegal ')' on line {tokenList[x].line}.");
                        }
                        state = checkBoolAssDFA[state, 5];
                        break;
                    case Lexeme.NOT:
                        state = checkBoolAssDFA[state, 6];
                        break;
                    case Lexeme.GREATER:
                    case Lexeme.LESSER:
                    case Lexeme.EQUAL:
                    case Lexeme.GEQUAL:
                    case Lexeme.LEQUAL:
                    case Lexeme.NEQUAL:
                        state = checkBoolAssDFA[state, 7];
                        break;
                    case Lexeme.AND:
                    case Lexeme.OR:
                        state = checkBoolAssDFA[state, 8];
                        break;
                    case Lexeme.AST:
                    case Lexeme.FSLASH:
                    case Lexeme.PLUS:
                    case Lexeme.MINUS:
                    case Lexeme.PERCENT:
                        state = checkBoolAssDFA[state, 9];
                        break;
                    default:
                        state = -1;
                        break;
                }

                if (state == -1)
                {
                    throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");
                }

                x++;
            }

            if (pars != 0)
                throw new ErrorException($"Unclosed '(' on line {tokenList[x].line}.");

            if (state != 3 && state != 7)
                throw new ErrorException($"Invalid assignment on line {tokenList[x].line}.");

            return x;
        }

        int checkStructure(int index)
        {
            int state = 0;
            int x = index;
            int pars = 0;

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.WHILE:
                    case Lexeme.IF:
                        state = structureDFA[state, 0];
                        break;
                    case Lexeme.LPAR:
                        pars++;
                        state = structureDFA[state, 1];
                        break;
                    case Lexeme.RPAR:
                        if (--pars < 0)
                        {
                            throw new ErrorException($"Illegal ')' on line {tokenList[x].line}.");
                        }
                        state = structureDFA[state, 2];
                        break;
                    case Lexeme.IDENTIFIER:
                    case Lexeme.NUMBER:
                    case Lexeme.CHARACTER:
                    case Lexeme.STRING:
                        state = structureDFA[state, 3];
                        break;
                    case Lexeme.BOOLEAN:
                        state = structureDFA[state, 4];
                        break;
                    case Lexeme.AST:
                    case Lexeme.FSLASH:
                    case Lexeme.PLUS:
                    case Lexeme.MINUS:
                    case Lexeme.PERCENT:
                        state = structureDFA[state, 5];
                        break;
                    case Lexeme.AND:
                    case Lexeme.OR:
                        state = structureDFA[state, 6];
                        break;
                    case Lexeme.GREATER:
                    case Lexeme.LESSER:
                    case Lexeme.EQUAL:
                    case Lexeme.GEQUAL:
                    case Lexeme.LEQUAL:
                    case Lexeme.NEQUAL:
                        state = structureDFA[state, 7];
                        break;
                    case Lexeme.NOT:
                        state = structureDFA[state, 8];
                        break;
                    default:
                        state = -1;
                        break;
                }

                if (state == -1)
                {
                    throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");
                }

                x++;
            }

            if (pars != 0)
                throw new ErrorException($"Unclosed '(' on line {tokenList[x].line}.");

            if (state != 3 && state != 5)
                throw new ErrorException($"Invalid assignment on line {tokenList[x].line}.");

            return x;
        }

        void OutputRework(int index, ref int y)
        {
            int x = index + 2;

            StringBuilder sb = new StringBuilder();

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.STRING:
                        sb.Append(tokenList[x].literal.Replace("$DQUOTE$", "\"").Replace("$LBRACKET$", "[").Replace("$RBRACKET$", "]").Replace("$AMP$", "&").Replace("#", "\n").Replace("$SHARP$", "#"));
                        break;
                    case Lexeme.AMP:
                        break;
                    default:
                        Object res = Evaluate(x, ref x);
                        sb.Append(res is not bool ? res : Convert.ToBoolean(res) ? "\"TRUE\"" : "\"FALSE\"");
                        break;
                }

                x++;
            }

            y = x;

            Console.Write(sb.ToString());
        }

        int checkOutputRework(int index)
        {
            int state = 0;
            int x = index;

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.OUTPUT:
                        state = OutputReworkDFA[state, 0];
                        break;
                    case Lexeme.COLON:
                        state = OutputReworkDFA[state, 1];
                        break;
                    case Lexeme.IDENTIFIER:
                        try
                        {
                            switch (variables[tokenList[x].literal])
                            {
                                case bType.INT:
                                case bType.FLOAT:
                                    state = OutputReworkDFA[state, 2];
                                    break;
                                case bType.CHAR:
                                    state = OutputReworkDFA[state, 3];
                                    break;
                                case bType.BOOL:
                                    state = OutputReworkDFA[state, 4];
                                    break;
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ErrorException($"Use of undeclared variable '{tokenList[x].literal}' on line {tokenList[x].line}.");
                        }
                        break;
                    case Lexeme.STRING:
                        state = OutputReworkDFA[state, 5];
                        break;
                    case Lexeme.NUMBER:
                        state = OutputReworkDFA[state, 6];
                        break;
                    case Lexeme.CHARACTER:
                        state = OutputReworkDFA[state, 7];
                        break;
                    case Lexeme.BOOLEAN:
                        state = OutputReworkDFA[state, 8];
                        break;
                    case Lexeme.LPAR:
                        state = OutputReworkDFA[state, 9];
                        break;
                    case Lexeme.RPAR:
                        state = OutputReworkDFA[state, 10];
                        break;
                    case Lexeme.GREATER:
                    case Lexeme.LESSER:
                    case Lexeme.EQUAL:
                    case Lexeme.GEQUAL:
                    case Lexeme.LEQUAL:
                    case Lexeme.NEQUAL:
                        state = OutputReworkDFA[state, 11];
                        break;
                    case Lexeme.AND:
                    case Lexeme.OR:
                        state = OutputReworkDFA[state, 12];
                        break;
                    case Lexeme.NOT:
                        state = OutputReworkDFA[state, 13];
                        break;
                    case Lexeme.AST:
                    case Lexeme.FSLASH:
                        state = OutputReworkDFA[state, 14];
                        break;
                    case Lexeme.PLUS:
                    case Lexeme.MINUS:
                        state = OutputReworkDFA[state, 15];
                        break;
                    case Lexeme.AMP:
                        state = OutputReworkDFA[state, 16];
                        break;
                    default:
                        state = -1;
                        break;
                }

                if (state == -1)
                {
                    throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");
                }

                x++;
            }

            if (state != 3 && state != 6 && state != 8 && state != 10 && state != 11)
            {
                throw new ErrorException($"Invalid output line on line {tokenList[x].line}.");
            }

            return x;
        }

        void checkInput(int index)
        {
            int state = 0;
            int x = index;

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.INPUT:
                        state = inputDFA[state, 0];
                        break;
                    case Lexeme.COLON:
                        state = inputDFA[state, 1];
                        break;
                    case Lexeme.IDENTIFIER:
                        state = inputDFA[state, 2];
                        break;
                    case Lexeme.COMMA:
                        state = inputDFA[state, 3];
                        break;
                    default:
                        state = -1;
                        break;
                }

                if (state == -1)
                {
                    throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");
                }
                x++;
            }
        }

        void Input(int index, ref int y)
        {
            int x = index + 2;
            List<string> inputList;
            List<string> identifiers = new List<string>();

            string input = Console.ReadLine();
            inputList = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

            while (tokenList[x].lex != Lexeme.NEWLINE)
            {
                switch (tokenList[x].lex)
                {
                    case Lexeme.IDENTIFIER:
                        identifiers.Add(tokenList[x].literal);
                        break;
                    case Lexeme.COMMA:
                        break;

                    default:
                        throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");

                }
                x++;
            }

            if (identifiers.Count != inputList.Count)
                throw new ErrorException($"Number of inputs({inputList.Count}) does not match up with number of variables({identifiers.Count}).");

            var idenInp = identifiers.Zip(inputList, (iden, inp) => new { Iden = iden, Inp = inp });

            foreach (var match in idenInp)
            {
                bType t = variables[match.Iden];
                try
                {
                    switch (t)
                    {
                        case bType.INT:
                            if (match.Inp.Contains('.'))
                                throw new ErrorException($"Cannot implicitly cast FLOAT to INT");
                            else
                                intVars[match.Iden] = Convert.ToInt32(Convert.ToSingle(match.Inp));
                            break;
                        case bType.FLOAT:
                            floatVars[match.Iden] = Convert.ToSingle(match.Inp);
                            break;
                        case bType.CHAR:
                            charVars[match.Iden] = Convert.ToChar(match.Inp);
                            break;
                        case bType.BOOL:
                            boolVars[match.Iden] = match.Inp == "\"TRUE\"" ? true : match.Inp == "\"FALSE\"" ? false : throw new FormatException();
                            break;
                    }
                }
                catch (FormatException)
                {
                    throw new ErrorException($"Cannot assign '{match.Inp}' to type {t}.");
                }
            }

            y = x;
        }


        #region Commented
        // int[,] DeclareDFA = new int[7, 7]{
        //     {1, -1, -1, -1, -1, -1, -1},
        //     {-1, 2, -1, -1, -1, -1, -1},
        //     {-1, -1, 3, 1, -1, 5, -1},
        //     {-1, 4, -1, -1, 4, -1, -1},
        //     {-1, -1, -1, 1, -1, 5, -1},
        //     {-1, -1, -1, -1, -1, -1, 6},
        //     {-1, -1, -1, -1, -1, -1, -1}
        // };

        // int[,] numberDeclare = new int[9, 10]{
        // // VAR iden = u/lpar const op AS TYPE , rpar
        //     {1, -1, -1, -1,  -1,  -1, -1, -1, -1, -1}, //0 
        //     {-1, 2, -1, -1,  -1,  -1, -1, -1, -1, -1}, //1
        //     {-1,-1,  3, -1,  -1,  -1, -1, -1,  1, -1}, //2
        //     {-1, 4, -1,  5,  -1,  -1, -1, -1, -1, -1}, //3
        //     {-1,-1, -1, -1,  -1,   5, -1, -1, -1, -1}, //4
        //     {-1, 6, -1,  5,   6,  -1, -1, -1, -1, -1}, //5
        //     {-1,-1, -1, -1,  -1,  -1, -1, -1,  1,  6}, //6
        //     {-1,-1, -1, -1,  -1,  -1,  8, -1, -1, -1}, //7 
        //     {-1,-1, -1, -1,  -1,  -1, -1, -1, -1, -1}  //8
        // };

        // int[,] boolDeclare = new int[7, 7]
        // { // VAR  iden =   bool  ,  AS BOOL
        //      {1,  -1, -1,  -1,  -1, -1, -1}, //0
        //      {-1,  2, -1,  -1,   1, -1, -1}, //1
        //      {-1, -1,  3,  -1,  -1, -1, -1}, //2
        //      {-1,  2, -1,   4,  -1, -1, -1}, //3
        //      {-1, -1, -1,  -1,   1,  5, -1}, //4
        //      {-1, -1, -1,  -1,  -1, -1,  6}, //5
        //      {-1, -1, -1,  -1,   1,  7, -1}, //6
        // };

        // int[,] charDeclare = new int[7, 7]
        // { // VAR  iden =   char  ,  AS BOOL
        //      {1,  -1, -1,  -1,  -1, -1, -1}, //0
        //      {-1,  2, -1,  -1,   1, -1, -1}, //1
        //      {-1, -1,  3,  -1,  -1, -1, -1}, //2
        //      {-1,  2, -1,   4,  -1, -1, -1}, //3
        //      {-1, -1, -1,  -1,   1,  5, -1}, //4
        //      {-1, -1, -1,  -1,  -1, -1,  6}, //5
        //      {-1, -1, -1,  -1,   1,  7, -1}, //6
        // };

        //  void checkCharDeclare(int index)
        // {
        //     int state = 0;
        //     int x = index;

        //     while (tokenList[x].lex != Lexeme.NEWLINE)
        //     {
        //         switch (tokenList[x].lex)
        //         {
        //             case Lexeme.VAR:
        //                 state = DeclareDFA[state, 0];
        //                 break;
        //             case Lexeme.IDENTIFIER:
        //                 state = DeclareDFA[state, 1];
        //                 break;
        //             case Lexeme.ASSIGN:
        //                 state = DeclareDFA[state, 2];
        //                 break;
        //             case Lexeme.BOOLEAN:
        //                 state = charDeclare[state, 4];
        //                 break;
        //             case Lexeme.COMMA:
        //                 state = DeclareDFA[state, 6];
        //                 break;
        //             case Lexeme.STRING:
        //             case Lexeme.NUMBER:
        //             case Lexeme.CHARACTER:
        //             case Lexeme.BOOLEAN:
        //                 state = DeclareDFA[state, 4];
        //                 break;
        //             case Lexeme.AS:
        //                 state = DeclareDFA[state, 5];
        //                 break;
        //             case Lexeme.INT:
        //             case Lexeme.FLOAT:
        //             case Lexeme.CHAR:
        //             case Lexeme.BOOL:
        //                 state = DeclareDFA[state, 6];
        //                 break;
        //             default:
        //                 state = -1;
        //                 break;
        //         }

        //         if (state == -1)
        //         {
        //             throw new ErrorException($"Illegal {tokenList[x].lex} on line {tokenList[x].line}.");
        //         }

        //         x++;
        //     }
        // }

        // int[,] outputDFA = new int[4, 4]{
        //     {1, -1, -1, -1},
        //     {-1, 2, -1, -1},
        //     {-1, -1, 3, -1},
        //     {-1, -1, -1, 2}
        // };
        #endregion

    }
}