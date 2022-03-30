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

        List<Token> listToken;
        List<Token> tokenList;
        Lexer lexer;
        string file;

        public Interpreter(string file)
        {
            this.file = file;
            lexer = new Lexer(file);
            tokenList = lexer.Lex();
        }

        public void Run()
        {
            Parser parser = new Parser(tokenList);
            parser.Parse();
        }

    }
}