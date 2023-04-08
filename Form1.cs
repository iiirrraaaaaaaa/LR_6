using System;
using System.Text;
using System.Windows.Forms;
using CPL.src.core;

namespace CPL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            Lexic tpl = new Lexic();
            tpl.Analysis(textBox1.Text);

            var result = new StringBuilder();

            foreach(var lex in tpl.Lexemes)
            {
                string lexName = null;

                switch (lex.Id)
                {
                    case 1:
                        lexName = " службовi слова ";
                        break;
                    case 2:
                        lexName = " обмежувачi ";
                        break;
                    case 3:
                        lexName = " числа ";
                        break;
                    case 4:
                        lexName = " iдентифiкатор ";
                        break;
                }

                if (lexName != null)
                {
                    result.Append("id: ");
                    result.Append(lex.Id);
                    result.Append(" lex: ");
                    result.Append(lex.Lexeme);
                    result.Append(" val: ");
                    result.Append(lex.Value);
                    result.Append(" |");
                    result.AppendLine(lexName);
                }
            }

            textBox2.Text = result.ToString();
        }
    }
