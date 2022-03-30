using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace CFPL_Interpreter_Console
{
    public class Lexer
    {
        private readonly string file;
        string[] lines;
        public Lexer(String file)
        {
            this.file = file;
            lines = File.ReadAllText(file)
            .Replace("\"TRUE\"", "TRUE")
            .Replace("\"FALSE\"", "FALSE")
            .Replace("[\"]", "$DQUOTE$")
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

        List<Token> tokenList = new List<Token>();
        const string symbols = "()[]*/+-%&><>=,#:\"\'";
        List<string> reserved = new List<string>{"INT", "CHAR", "BOOL", "FLOAT", "AND", "OR", "NOT", "WHILE", "IF", "ELSE", "TRUE", "FALSE",
                                        "VAR", "AS", "START", "STOP", "OUTPUT", "INPUT"};

        public List<Token> Lex()
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
                    else if ((ln[x] == '-' || ln[x] == '+') && Char.IsDigit(ln[x + 1]))
                    {
                        lit.Append(ln[x]);
                    }
                    else
                    {
                        if (symbols.Contains(ln[x]))
                        {
                            if (lit.Length > 0)
                                addToken(lit.ToString(), ctr);
                            else if ((ln[x] == '-' || ln[x] == '+') && Char.IsDigit(ln[x + 1]))
                            {
                                lit.Append(ln[x]);
                            }

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
            return tokenList;
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
            if (Char.IsDigit(literal[0]) || literal[0] == '-' || literal[0] == '+')
            {
                if (!literal.All(x => Char.IsDigit(x) || x == '.' || x == '.' || x == '+' || x == '-'))
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


    }
}