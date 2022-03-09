using System;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormCommTool : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        //constructor
        public FormCommTool(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormCommSet_Load(object sender, EventArgs e)
        {
            if (mf.spGPS2.IsOpen)
            {
                cboxBaud2.Enabled = false;
                cboxPort2.Enabled = false;
                btnCloseGPS2.Enabled = true;
                btnOpenGPS2.Enabled = false;
            }
            else
            {
                cboxBaud2.Enabled = true;
                cboxPort2.Enabled = true;
                btnCloseGPS2.Enabled = false;
                btnOpenGPS2.Enabled = true;
            }



            if (mf.spModule3.IsOpen)
            {
                cboxModule3Port.Enabled = false;
                btnCloseSerialModule3.Enabled = true;
                btnOpenSerialModule3.Enabled = false;
            }
            else
            {
                cboxModule3Port.Enabled = true;
                btnCloseSerialModule3.Enabled = false;
                btnOpenSerialModule3.Enabled = true;
            }

            //load the port box with valid port names
            cboxPort2.Items.Clear();
            cboxModule3Port.Items.Clear();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxPort2.Items.Add(s);
                cboxModule3Port.Items.Add(s);
            }

            lblCurrentPort2.Text = mf.spModule1.PortName;
            lblCurrentModule3Port.Text = mf.spModule3.PortName;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //GPS phrase
            lblSteer.Text = mf.spModule1.PortName;
            lblGPS.Text = mf.spGPS.PortName;
            lblGPS2.Text = mf.spGPS2.PortName;
            lblIMU.Text = mf.spIMU.PortName;
            lblMachine.Text = mf.spModule2.PortName;

            lblFromGPS.Text = mf.traffic.cntrGPSIn == 0 ? "--" : (mf.traffic.cntrGPSIn).ToString();
            lblFromGPS2.Text = mf.traffic.cntrGPS2In == 0 ? "--" : (mf.traffic.cntrGPS2In).ToString();

            lblFromModule1.Text = mf.traffic.cntrModule1In == 0 ? "--" : (mf.traffic.cntrModule1In).ToString();

            lblFromModule2.Text = mf.traffic.cntrModule2In == 0 ? "--" : (mf.traffic.cntrModule2In).ToString();

            lblFromMU.Text = mf.traffic.cntrIMUIn == 0 ? "--" : (mf.traffic.cntrIMUIn).ToString();

        }

        private void btnSerialOK_Click(object sender, EventArgs e)
        {
            //save
            //DialogResult = DialogResult.OK;
            Close();
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            cboxPort2.Items.Clear();
            cboxModule3Port.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxPort2.Items.Add(s);
                cboxModule3Port.Items.Add(s);
            }
        }

        private void cboxModule3Port_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf.spModule3.PortName = cboxModule3Port.Text;
            FormLoop.portNameModule3 = cboxModule3Port.Text;
            lblCurrentModule3Port.Text = cboxModule3Port.Text;
        }

        private void btnOpenSerialModule3_Click(object sender, EventArgs e)
        {
            mf.OpenModule3Port();
            if (mf.spModule3.IsOpen)
            {
                cboxModule3Port.Enabled = false;
                btnCloseSerialModule3.Enabled = true;
                btnOpenSerialModule3.Enabled = false;
                lblCurrentModule3Port.Text = mf.spModule3.PortName;
            }
            else
            {
                cboxModule3Port.Enabled = true;
                btnCloseSerialModule3.Enabled = false;
                btnOpenSerialModule3.Enabled = true;
            }
        }

        private void btnCloseSerialModule3_Click(object sender, EventArgs e)
        {
            mf.CloseModule3Port();
            if (mf.spModule3.IsOpen)
            {
                cboxModule3Port.Enabled = false;
                btnCloseSerialModule3.Enabled = true;
                btnOpenSerialModule3.Enabled = false;
            }
            else
            {
                cboxModule3Port.Enabled = true;
                btnCloseSerialModule3.Enabled = false;
                btnOpenSerialModule3.Enabled = true;
            }
        }

        private void btnOpenGPS2_Click(object sender, EventArgs e)
        {
            mf.OpenGPS2Port();
            if (mf.spGPS2.IsOpen)
            {
                cboxBaud2.Enabled = false;
                cboxPort2.Enabled = false;
                btnCloseGPS2.Enabled = true;
                btnOpenGPS2.Enabled = false;
                lblCurrentBaud2.Text = mf.spGPS.BaudRate.ToString();
                lblCurrentPort2.Text = mf.spGPS.PortName;
            }
            else
            {
                cboxBaud2.Enabled = true;
                cboxPort2.Enabled = true;
                btnCloseGPS2.Enabled = false;
                btnOpenGPS2.Enabled = true;
                MessageBox.Show("Unable to connect to Port");
            }
        }

        private void btnCloseGPS2_Click(object sender, EventArgs e)
        {
            mf.CloseGPS2Port();
            if (mf.spGPS2.IsOpen)
            {
                cboxBaud2.Enabled = false;
                cboxPort2.Enabled = false;
                btnCloseGPS2.Enabled = true;
                btnOpenGPS2.Enabled = false;
            }
            else
            {
                cboxBaud2.Enabled = true;
                cboxPort2.Enabled = true;
                btnCloseGPS2.Enabled = false;
                btnOpenGPS2.Enabled = true;
            }
        }

        private void cboxBaud2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboxPort2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    } //class
} //namespace
