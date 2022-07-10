using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormMakeBndCon : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        public FormMakeBndCon(Form _mf)
        {
            mf = _mf as FormGPS;
            InitializeComponent();

            lblHz.Text = gStr.gsPass;
            label1.Text = gStr.gsSpacing;

            this.Text = gStr.gsMakeBoundaryContours;

            nudPass.Controls[0].Enabled = false;
            nudSpacing.Controls[0].Enabled = false;
        }

        private void BtnOk_Click(object sender, System.EventArgs e)
        {
            //convert to meters
            if (mf.gyd.BuildFenceContours((double)nudPass.Value - 0.5, (double)nudSpacing.Value * 0.01))
                mf.TimedMessageBox(1500, "Boundary Contour", "Contour Path Created");
            else
                mf.TimedMessageBox(1500, "Boundary Contour Error", "No Boundaries Made");
            Close();
        }

        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void nudPass_Click(object sender, System.EventArgs e)
        {
            nudPass.KeypadToNUD();
            btnCancel.Focus();
        }

        private void nudSpacing_Click(object sender, System.EventArgs e)
        {
            nudSpacing.KeypadToNUD();
            btnCancel.Focus();
        }

        private void FormMakeBndCon_Load(object sender, System.EventArgs e)
        {
            btnCancel.Focus();
        }
    }
}