using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPL.src.core
{
    class Lexic 
    {
        private string buf = ""; 
        private char[] sm = new char[1];
        private string[] Words = { "program", "var", "integer", "real", "bool", "begin", "end", "if", "then", "else", "while", "do", "read", "write", "true", "false" };
        private string[] Delimiter = { ".", ";", ",", "(", ")", "+", "-", "*", "/", "=", ">", "<" };
        private int dt = 0;
        private enum States { S, NUM, DLM, FIN, ID, ER, ASGN, COM } 
        private States _state; 
        
        public readonly List<Lex> Lexemes = new List<Lex>();
        private string[] TID = { "" };
        private string[] TNUM = { "" };
        private StringReader sr; 

        private void GetNext()
        {
            sr.Read(sm, 0, 1);
        }

        private void ClearBuf()
        {
            buf = "";
        }

        private void AddBuf(char symbol)
        {
            buf += symbol;
        }

        private (int, string) SearchLex(string[] lexemes)
        {
            var srh = Array.FindIndex(lexemes, s => s.Equals(buf));
            if (srh != -1)
                return (srh, buf);
            return (-1, "");
        }

        private (int, string) PushLex(string[] lexemes, string buf)
        {
            var srh = Array.FindIndex(lexemes, s => s.Equals(buf));
            if (srh != -1)
                return (-1, "");
            Array.Resize(ref lexemes, lexemes.Length + 1);
            lexemes[lexemes.Length - 1] = buf;
            return (lexemes.Length - 1, buf);
        }

        private void AddLex(List<Lex> lexes, int key, int val, string lex)
        {
            lexes.Add(new Lex(key, val, lex));
        }

        public void Analysis(string text)
        {
            sr = new StringReader(text);
            while (_state != States.FIN)
            {
                switch (_state)
                {

                    case States.S:
                        if (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r')
                            GetNext();
                        else if (Char.IsLetter(sm[0]))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            _state = States.ID;
                            GetNext();
                        }
                        else if (char.IsDigit(sm[0]))
                        {
                            dt = (int)(sm[0] - '0');
                            GetNext();
                            _state = States.NUM;

                        }
                        else if (sm[0] == '{')
                        {
                            _state = States.COM;
                            GetNext();
                        }
                        else if (sm[0] == ':')
                        {
                            _state = States.ASGN;
                            ClearBuf();
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else if (sm[0] == '.')
                        {
                            AddLex(Lexemes, 2, 0, sm[0].ToString());
                            _state = States.FIN;
                        }
                        else
                        {
                            _state = States.DLM;

                        }

                        break;
                    case States.ID:
                        if (Char.IsLetterOrDigit(sm[0]))
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else
                        {
                            var srch = SearchLex(Words);
                            if (srch.Item1 != -1)
                                AddLex(Lexemes, 1, srch.Item1, srch.Item2);
                            else
                            {
                                var j = PushLex(TID, buf);
                                AddLex(Lexemes, 4, j.Item1, j.Item2);
                            }
                            _state = States.S;
                        }
                        break;

                    case States.NUM:
                        if (Char.IsDigit(sm[0]))
                        {
                            dt = dt * 10 + (int)(sm[0] - '0');
                            GetNext();
                        }
                        else
                        {

                            var j = PushLex(TNUM, dt.ToString());
                            AddLex(Lexemes, 3, j.Item1, j.Item2);
                            _state = States.S;
                        }
                        break;
                    case States.DLM:
                        ClearBuf();
                        AddBuf(sm[0]);

                        var r = SearchLex(Delimiter);
                        if (r.Item1 != -1)
                        {
                            AddLex(Lexemes, 2, r.Item1, r.Item2);
                            _state = States.S;
                            GetNext();
                        }
                        else
                            _state = States.ER;
                        break;
                    case States.ASGN:
                        if (sm[0] == '=')
                        {
                            AddBuf(sm[0]);
                            AddLex(Lexemes, 2, 4, buf);
                            ClearBuf();
                            GetNext();
                        }
                        else
                            AddLex(Lexemes, 2, 3, buf);
                        _state = States.S;

                        break;
                    case States.ER:
                        MessageBox.Show("Помилка в програмi");
                        _state = States.FIN;
                        break;
                    case States.FIN:
                        MessageBox.Show("Лексичний аналiз закiнчений");
                        break;
                }

            }
        }
    }
}
