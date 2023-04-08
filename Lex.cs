using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.src.core
{
    public class Lex
    {
        public readonly int Id;
        public readonly int Lexeme;
        public readonly string Value;

        public Lex(int id, int lexeme, string value)
        {
            this.Id = id;
            this.Lexeme = lexeme;
            this.Value = value;
        }
    }
}
