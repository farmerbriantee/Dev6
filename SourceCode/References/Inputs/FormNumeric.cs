using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormNumeric : Form
    {
        private readonly double MaxValue, MinValue, Unit2Mtr;
        private bool isFirstKey;
        private readonly string guifix = "";
        private readonly bool WholeNumbers, ChangeToCulture;
        private readonly int Decimals = 0;
        private readonly decimal Divisible = 1;
        private readonly System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();
        private bool UPorDOWN = true;

        public double ReturnValue { get; set; }

        public FormNumeric(double minvalue, double maxvalue, double currentValue, int decimals, bool changetoculture = false, double unit2mtr = 1, double Mtr2Unit = 1, decimal divisible = -1)
        {
            Unit2Mtr = unit2mtr;
            Decimals = decimals;
            ChangeToCulture = changetoculture;

            if (ChangeToCulture)
            {
                currentValue = Math.Round(currentValue * Mtr2Unit, decimals, MidpointRounding.AwayFromZero);

                MaxValue = Math.Round(maxvalue * Mtr2Unit, Decimals, MidpointRounding.AwayFromZero);
                if (MaxValue * Unit2Mtr > maxvalue)
                {
                    MaxValue = Math.Round(MaxValue - Math.Pow(0.1, Decimals), decimals, MidpointRounding.AwayFromZero);
                }
                MinValue = Math.Round(minvalue * Mtr2Unit, Decimals, MidpointRounding.AwayFromZero);
                if (MinValue * Unit2Mtr < minvalue)
                {
                    MinValue += Math.Pow(0.1, Decimals);
                }
            }
            else
            {
                MinValue = minvalue;
                MaxValue = maxvalue;
            }

            InitializeComponent();
            Timer.Tick += new EventHandler(this.TimerRepeat_Tick);

            KeyPreview = true;

            Divisible = divisible;

            Text = "Enter Value";
            //fill in the display


            BtnSeparator.Enabled = !(WholeNumbers = decimals == 0);
            BtnPlus.Enabled = minvalue < 0;
            BtnSeparator.Text = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            isFirstKey = true;

            lblMax.Text = MaxValue.ToString();
            lblMin.Text = MinValue.ToString();

            tboxNumber.SelectionStart = tboxNumber.Text.Length;
            tboxNumber.SelectionLength = 0;

            BtnOk.Focus();
            if (decimals > 0)
            {
                guifix = "0.0";
                while (--decimals > 0)
                    guifix += "#";
            }
            tboxNumber.Text = currentValue.ToString(guifix);
        }

        private void BtnDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            UPorDOWN = true;
            Timer.Enabled = false;
            TimerRepeat_Tick(null, EventArgs.Empty);
        }

        private void BtnDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            UPorDOWN = false;
            Timer.Enabled = false;
            TimerRepeat_Tick(null, EventArgs.Empty);
        }

        private void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            Timer.Enabled = false;
        }

        private void TimerRepeat_Tick(object sender, EventArgs e)
        {
            if (Timer.Enabled)
            {
                if (Timer.Interval > 50) Timer.Interval -= 50;
            }
            else
                Timer.Interval = 500;

            Timer.Enabled = true;


            if (tboxNumber.Text == "" || tboxNumber.Text == "-" || tboxNumber.Text == "Error")
            {
                tboxNumber.Text = "0";
            }

            double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);

            if (UPorDOWN)
            {
                tryNumber++;
            }
            else
            {
                tryNumber--;
            }

            if (tryNumber < MinValue)
            {
                tryNumber = MinValue;
            }
            else if (tryNumber > MaxValue)
            {
                tryNumber = MaxValue;
            }

            tboxNumber.Text = tryNumber.ToString();

            isFirstKey = false;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('\r'));
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('\u001b'));
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('1'));
        }
        private void Btn2_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('2'));
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('3'));
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('4'));
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('5'));
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('6'));
        }

        private void Btn7_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('7'));
        }

        private void Btn8_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('8'));
        }

        private void Btn9_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('9'));
        }

        private void Btn0_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('0'));
        }

        private void BtnSeparator_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('.'));
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('-'));
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('\b'));
        }

        private void BtnClear2_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('C'));
        }

        private void FormNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isFirstKey && char.IsNumber(e.KeyChar))
            {
                tboxNumber.Text = "";
                isFirstKey = false;
            }
            isFirstKey = false;

            //clear the error as user entered new values
            if (tboxNumber.Text == gStr.gsError)
            {
                tboxNumber.Text = "";
                lblMin.ForeColor = SystemColors.ControlText;
                lblMax.ForeColor = SystemColors.ControlText;
            }

            int cursorPosition = tboxNumber.SelectionStart;

            if (e.KeyChar == '\b')
            {
                if (tboxNumber.Text.Length > 0 && cursorPosition > 0)
                {
                    tboxNumber.Text = tboxNumber.Text.Remove(--cursorPosition, 1);
                    tboxNumber.SelectionStart = cursorPosition;
                }
            }
            else if (e.KeyChar == 'C')
            {
                tboxNumber.Text = "";
            }

            int decSeparator = tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            //if its a number just add it
            if (char.IsNumber(e.KeyChar))
            {
                if (decSeparator < 0 || tboxNumber.Text.Length - decSeparator <= Decimals)
                {
                    string tt = e.KeyChar.ToString();
                    tboxNumber.Text = tboxNumber.Text.Insert(cursorPosition, tt);
                    tboxNumber.SelectionStart = ++cursorPosition;
                }

                if (double.TryParse(tboxNumber.Text, out double value))
                {
                    if (value > MaxValue)
                    {
                        tboxNumber.Text = MaxValue.ToString();
                        tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    }
                    else if (MinValue <= 0 && value < MinValue)
                    {
                        tboxNumber.Text = MinValue.ToString();
                        tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    }
                }
            }
            else if (e.KeyChar == '.')
            {
                if (!WholeNumbers && decSeparator == -1)
                {
                    tboxNumber.Text = tboxNumber.Text.Insert(cursorPosition, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    tboxNumber.SelectionStart = ++cursorPosition;

                    //if decimal is first char, prefix with a zero
                    if (tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) == 0)
                    {
                        tboxNumber.Text = "0" + tboxNumber.Text;
                        tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    }

                    //neg sign then added a decimal, insert a 0 
                    if (tboxNumber.Text.IndexOf("-") == 0 && tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) == 1)
                    {
                        tboxNumber.Text = "-0" + Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        tboxNumber.SelectionStart = tboxNumber.Text.Length;

                    }
                    decSeparator = tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }
            }
            else if (e.KeyChar == '-')
            {
                if (tboxNumber.Text.StartsWith("-"))
                {
                    tboxNumber.Text = tboxNumber.Text.Substring(1);
                    if (cursorPosition > 0) tboxNumber.SelectionStart = --cursorPosition;
                }
                else if (MinValue < 0)
                {
                    tboxNumber.Text = "-" + tboxNumber.Text;
                    tboxNumber.SelectionStart = ++cursorPosition;
                }
            }
            else if (e.KeyChar == '\u001b')
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else if (e.KeyChar == '\r')
            {
                //not ok if empty - just return
                if (tboxNumber.Text == "") return;

                //culture invariant parse to double
                double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);

                if (tryNumber > MaxValue)
                {
                    tboxNumber.Text = MaxValue.ToString();
                    tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    lblMax.ForeColor = Color.Red;
                    return;
                }
                else if (tryNumber < MinValue)
                {
                    tboxNumber.Text = MinValue.ToString();
                    tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    lblMin.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //all good, return the value
                    if (ChangeToCulture) ReturnValue = Math.Round(tryNumber * Unit2Mtr, Decimals, MidpointRounding.AwayFromZero);
                    else ReturnValue = tryNumber;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }

            bool Add = (decSeparator < 0 || tboxNumber.Text.Length - decSeparator <= Decimals);
            for (int i = 0; i < 10; i++)
            {
                decimal tryNumber = decimal.Parse(tboxNumber.Text + i.ToString(), CultureInfo.CurrentCulture);
                Controls["Btn" + i].Enabled = Add && (Divisible < 0 || tryNumber % Divisible == 0);
            }
            Controls["BtnSeparator"].Enabled = decSeparator == -1 && !WholeNumbers;

            tboxNumber.SelectionLength = 0;
            tboxNumber.Focus();
        }

        private void TboxNumber_Click(object sender, EventArgs e)
        {
            isFirstKey = false;
        }

        private void FormNumeric_Activated(object sender, EventArgs e)
        {
            Left = Owner.Left + Owner.Width / 2 - Width / 2;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;
        }
    }
}