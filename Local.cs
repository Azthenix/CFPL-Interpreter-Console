using System;
using System.Collections.Generic;

namespace CFPL_Interpreter_Console
{
	class Local
	{
		internal Dictionary<string, bType> variables;
		internal Dictionary<string, int> intVars;
		internal Dictionary<string, char> charVars;
		internal Dictionary<string, bool> boolVars;
		internal Dictionary<string, float> floatVars;

        public Local()
        {
            variables = new Dictionary<string, bType>();
            intVars = new Dictionary<string, int>();
            floatVars = new Dictionary<string, float>();
            charVars = new Dictionary<string, char>();
            boolVars = new Dictionary<string, bool>();
        }
	}
}
