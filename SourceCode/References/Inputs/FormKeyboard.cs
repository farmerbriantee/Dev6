using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormKeyboard : Form
    {
        bool ShiftPressed, OldUpperString, CapsPressed, OldCapsPressed, Shiftforce;
        readonly List<Button> ButtonList = new List<Button>();
        readonly List<string> CharList;

        public string ReturnString { get; set; }
        public FormKeyboard(string currentString)
        {
            KeyPreview = true;
            InitializeComponent();

            this.Text = "Enter a Value";

            keyboardString.Text = currentString.ToString();

            string currentculture = Properties.Settings.Default.setF_culture;
            if (currentculture == "de")
            {
                CharList = new List<string>
                {
                    "1!1!", "2@2@", "3#3#", "4$4$", "5%5%", "6^6^", "7&7&", "8*8*", "9(9(", "0)0)", "ẞẞẞẞ", "\b",
                    "/?/?", "qQQq", "wWWw", "eEEe", "rRRr", "tTTt", "zZZz", "uUUu", "iIIi", "oOOo", "pPPp", "üÜÜü",
                    "_-_-", "aAAa", "sSSs", "dDDd", "fFFf", "gGGg", "hHHh", "jJJj", "kKKk", "lLLl", "öÖÖö", "äÄÄä",
                    "|~|~", "=+=+", "yYYy", "xXXx", "cCCc", "vVVv", "bBBb", "nNNn", "mMMm", ",<,<", ".>.>", ";:;:",
                    "\u001b", "\u0018", "`'`'", "\u0000", " " , "\r"
                };
            }
            else if (currentculture == "fr")
            {
                CharList = new List<string>
                {
                    "1!1!", "2@2@", "3#3#", "4$4$", "5%5%", "6^6^", "7&7&", "8*8*", "9(9(", "0)0)", "=+=+", "\b",
                    "àÀÀà", "âÂÂâ", "æÆÆæ", "çÇÇç", "éÉÉé", "èÈÈè", "êÊÊê", "ëËËë", "ïÏÏï", "îÎÎî", "ôÔÔô", "œŒŒœ",
                    "aAAa", "zZZz", "eEEe", "rRRr", "tTTt", "zZZz", "uUUu", "iIIi", "oOOo", "pPPp", "üÜÜü", "ûÛÛû",
                    "qQQq", "sSSs", "dDDd", "fFFf", "gGGg", "hHHh", "jJJj", "kKKk", "lLLl", "mMMm", "ùÙÙù", "_-_-",
                    "<><>", "wWWw", "xXXx", "cCCc", "vVVv", "bBBb", "nNNn", ",?,?", ";.;.", ":/:/", ".§.§", "|€|€",
                    "\u001b", "\u0018", "''''", "\u0000", " " , "\r"
                };
            }
            else
            {
                CharList = new List<string>
                {
                    "1!1!", "2@2@", "3#3#", "4$4$", "5%5%", "6^6^", "7&7&", "8*8*", "9(9(", "0)0)", "=+=+", "\b",
                    "/?/?", "qQQq", "wWWw", "eEEe", "rRRr", "tTTt", "yYYy", "uUUu", "iIIi", "oOOo", "pPPp",
                    "~|~|", "aAAa", "sSSs", "dDDd", "fFFf", "gGGg", "hHHh", "jJJj", "kKKk", "lLLl", "_-_-",
                    "[{[{", "]}]}", "zZZz", "xXXx", "cCCc", "vVVv", "bBBb", "nNNn", "mMMm", ",<,<", ".>.>", ";:;:",
                    "\u001b", "\u0018", "`'`'", "\u0000", " " , "\r"
                };
            }

            keyboardString.SelectionStart = keyboardString.Text.Length;
            keyboardString.SelectionLength = 0;

            int left = 0;
            int top = 9;

            Font usefont = new Font("Microsoft Sans Serif", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);

            int row = -1;
            int rowlength = 0;
            for (int i = 0; i < CharList.Count; i++)
            {
                if (i == rowlength)
                {
                    top += 73;

                    if (++row == 0)
                    {
                        rowlength += 12;
                        left = 9;
                        if (currentculture != "fr") row++;
                    }
                    else if (row == 1)
                    {
                        left = 20;
                        rowlength += 12;
                    }
                    else if (row == 2)
                    {
                        left = 31;
                        if (currentculture == "fr" || currentculture == "de")
                            rowlength += 12;
                        else
                            rowlength += 11;
                    }
                    else if (row == 3)
                    {
                        left = 44;
                        if (currentculture == "fr" || currentculture == "de")
                            rowlength += 12;
                        else
                            rowlength += 11;
                    }
                    else if (row == 4)
                    {
                        left = 9;
                        rowlength += 12;
                    }
                    else
                    {
                        left = 9;
                        rowlength += 6;
                    }
                }
                var a = new Button
                {
                    Enabled = true,
                    Margin = new Padding(0),
                    Font = usefont,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = SystemColors.Control,
                    Size = new Size(64, 64),
                    Name = string.Format("{0}", CharList[i][0]),
                    TextAlign = ContentAlignment.MiddleCenter,
                };

                a.Click += B_Click;
                Controls.Add(a);
                a.BringToFront();
                a.Top = top;
                a.Left = left;

                left += 73;

                if (CharList[i][0] == '0') usefont = new Font("Microsoft Sans Serif", 26F, FontStyle.Bold, GraphicsUnit.Point, 0);
                else if (CharList[i][0] == '\b')
                {
                    a.Size = new Size(101, 64);
                    a.Text = "⌫";
                }
                else if (CharList[i][0] == '\u001b')
                {
                    a.Size = new Size(101, 64);
                    a.Image = Properties.Resources.Cancel64;
                    left += 37;
                }
                else if (CharList[i][0] == '\u0018')
                {
                    a.Size = new Size(100, 64);
                    a.BackColor = Color.LightSalmon;
                    left += 36;
                    a.Text = "Clr";
                }
                else if (CharList[i][0] == '\u0000')
                {
                    a.Size = new Size(246, 64);
                    left += 182;
                    a.Image = Properties.Resources.UpArrow64;
                }
                else if (CharList[i][0] == ' ')
                {
                    a.Size = new Size(247, 64);
                    left += 183;
                    a.Text = "___";
                }
                else if (CharList[i][0] == '\r')
                {
                    a.Size = new Size(101, 64);
                    a.Image = Properties.Resources.OK64;
                    Width = left + 53;
                }

                ButtonList.Add(a);
                if (CharList[i].Length > 3)
                {
                    if (CharList[i] == "7&7&")
                    {
                        a.UseMnemonic = false;
                    }
                    a.Text = CharList[i].ToCharArray()[0].ToString();
                }
            }
            Height = top += 113;
        }

        void UpdateChars()
        {
            for (int i = 0; i < ButtonList.Count; i++)
            {
                if (CharList[i].Length < 4) continue;
                byte idx = 0;
                if (ShiftPressed) idx += 1;
                if (CapsPressed) idx += 2;
                char tt = CharList[i].ToCharArray()[idx];
                ButtonList[i].Text = tt.ToString();
            }
        }

        void B_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                int index = ButtonList.IndexOf(b);
                if (index > -1)
                {
                    byte idx = 0;
                    if (ShiftPressed || Shiftforce) idx += 1;
                    if (CapsPressed) idx += 2;

                    if (CharList[index].Length < 4)
                    {
                        char character = b.Name.ToCharArray()[0];
                        FormNumeric_KeyPress(null, new KeyPressEventArgs(character));
                    }
                    else
                    {
                        char character = CharList[index].ToCharArray()[idx];
                        FormNumeric_KeyPress(null, new KeyPressEventArgs(character));
                    }
                }
            }
        }

        private void FormNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            //clear the error as user entered new values
            if (keyboardString.Text == gStr.gsError)
            {
                keyboardString.Text = "";
            }

            //Backspace key, remove 1 char
            if (e.KeyChar == '\b')
            {
                int start = keyboardString.SelectionStart;
                if (keyboardString.SelectionLength > 0)
                {
                    keyboardString.Text = keyboardString.Text.Remove(keyboardString.SelectionStart, keyboardString.SelectionLength);
                }
                else if (keyboardString.SelectionStart > 0)
                {
                    keyboardString.Text = keyboardString.Text.Remove(keyboardString.SelectionStart - 1, 1);
                    start--;
                }
                keyboardString.SelectionLength = 0;
                keyboardString.SelectionStart = start;
            }
            else if (e.KeyChar == '\0')
            {
                Shiftforce = !Shiftforce;
                FormKeyboard_MouseEnter(null, null);
            }
            //Exit or cancel
            else if (e.KeyChar == '\u001b')
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }

            //clear whole display
            else if (e.KeyChar == '\u0018')
            {
                keyboardString.Text = "";
            }
            else if (e.KeyChar == '\u0016')
            {
            }
            //ok button
            else if (e.KeyChar == '\r')
            {
                //all good, return the value
                ReturnString = keyboardString.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            else if (!char.IsControl(e.KeyChar))
            {
                int start = keyboardString.SelectionStart;
                if (keyboardString.SelectionLength > 0)
                {
                    keyboardString.Text = keyboardString.Text.Remove(keyboardString.SelectionStart, keyboardString.SelectionLength);
                    keyboardString.Text = keyboardString.Text.Insert(start, e.KeyChar.ToString());
                }
                else
                {
                    keyboardString.Text = keyboardString.Text.Insert(start, e.KeyChar.ToString());
                }
                keyboardString.SelectionLength = 0;
                keyboardString.SelectionStart = ++start;
            }
            else
            {
            }
            e.Handled = true;
            keyboardString.Focus();
        }

        private void FormKeyboard_MouseEnter(object sender, EventArgs e)
        {
            ShiftPressed = (ModifierKeys == Keys.Shift) || Shiftforce;
            CapsPressed = IsKeyLocked(Keys.CapsLock);

            if (OldUpperString != ShiftPressed || CapsPressed != OldCapsPressed)
            {
                UpdateChars();
                OldUpperString = ShiftPressed;
                OldCapsPressed = CapsPressed;
            }
        }
        private void FormKeyboard_KeyUpDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Capital)
            {
                ShiftPressed = (ModifierKeys == Keys.Shift) || Shiftforce;
                CapsPressed = IsKeyLocked(Keys.CapsLock);

                if (OldUpperString != ShiftPressed || CapsPressed != OldCapsPressed)
                {
                    UpdateChars();
                    OldUpperString = ShiftPressed;
                    OldCapsPressed = CapsPressed;
                }
            }
        }

        private void FormKeyboard_Activated(object sender, EventArgs e)
        {
            Left = Owner.Left + Owner.Width / 2 - Width / 2;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;
        }
    }
}