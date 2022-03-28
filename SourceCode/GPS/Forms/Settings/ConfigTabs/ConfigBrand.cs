using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class ConfigBrand : UserControl2
    {
        private readonly FormGPS mf;

        TBrand brand;
        HBrand brandH;
        WDBrand brand4WD;

        public ConfigBrand(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void ConfigBrand_Load(object sender, EventArgs e)
        {
            //Brand constructor
            brand = Properties.Settings.Default.setBrand_TBrand;

            if (brand == TBrand.Case)
                rbtnBrandTCase.Checked = true;
            else if (brand == TBrand.Claas)
                rbtnBrandTClaas.Checked = true;
            else if (brand == TBrand.Deutz)
                rbtnBrandTDeutz.Checked = true;
            else if (brand == TBrand.Fendt)
                rbtnBrandTFendt.Checked = true;
            else if (brand == TBrand.JDeere)
                rbtnBrandTJDeere.Checked = true;
            else if (brand == TBrand.Kubota)
                rbtnBrandTKubota.Checked = true;
            else if (brand == TBrand.Massey)
                rbtnBrandTMassey.Checked = true;
            else if (brand == TBrand.NewHolland)
                rbtnBrandTNH.Checked = true;
            else if (brand == TBrand.Same)
                rbtnBrandTSame.Checked = true;
            else if (brand == TBrand.Steyr)
                rbtnBrandTSteyr.Checked = true;
            else if (brand == TBrand.Ursus)
                rbtnBrandTUrsus.Checked = true;
            else if (brand == TBrand.Valtra)
                rbtnBrandTValtra.Checked = true;
            else
                rbtnBrandTAoG.Checked = true;


            brandH = Properties.Settings.Default.setBrand_HBrand;

            if (brandH == HBrand.Case)
                rbtnBrandHCase.Checked = true;
            else if (brandH == HBrand.Claas)
                rbtnBrandHClaas.Checked = true;
            else if (brandH == HBrand.JDeere)
                rbtnBrandHJDeere.Checked = true;
            else if (brandH == HBrand.NewHolland)
                rbtnBrandHNH.Checked = true;
            else
                rbtnBrandHAoG.Checked = true;


            brand4WD = Properties.Settings.Default.setBrand_WDBrand;

            if (brand4WD == WDBrand.Case)
                rbtnBrand4WDCase.Checked = true;
            else if (brand4WD == WDBrand.Challenger)
                rbtnBrand4WDChallenger.Checked = true;
            else if (brand4WD == WDBrand.JDeere)
                rbtnBrand4WDJDeere.Checked = true;
            else if (brand4WD == WDBrand.NewHolland)
                rbtnBrand4WDNH.Checked = true;
            else
                rbtnBrand4WDAoG.Checked = true;
        }

        public override void Close()
        {
            if (rbtnBrandTCase.Checked)
                brand = TBrand.Case;
            else if (rbtnBrandTClaas.Checked)
                brand = TBrand.Claas;
            else if (rbtnBrandTDeutz.Checked)
                brand = TBrand.Deutz;
            else if (rbtnBrandTFendt.Checked)
                brand = TBrand.Fendt;
            else if (rbtnBrandTJDeere.Checked)
                brand = TBrand.JDeere;
            else if (rbtnBrandTKubota.Checked)
                brand = TBrand.Kubota;
            else if (rbtnBrandTMassey.Checked)
                brand = TBrand.Massey;
            else if (rbtnBrandTNH.Checked)
                brand = TBrand.NewHolland;
            else if (rbtnBrandTSame.Checked)
                brand = TBrand.Same;
            else if (rbtnBrandTSteyr.Checked)
                brand = TBrand.Steyr;
            else if (rbtnBrandTUrsus.Checked)
                brand = TBrand.Ursus;
            else if (rbtnBrandTValtra.Checked)
                brand = TBrand.Valtra;
            else
                brand = TBrand.AGOpenGPS;

            Properties.Settings.Default.setBrand_TBrand = brand;

            if (rbtnBrandHCase.Checked)
                brandH = HBrand.Case;
            else if (rbtnBrandHClaas.Checked)
                brandH = HBrand.Claas;
            else if (rbtnBrandHJDeere.Checked)
                brandH = HBrand.JDeere;
            else if (rbtnBrandHNH.Checked)
                brandH = HBrand.NewHolland;
            else
                brandH = HBrand.AGOpenGPS;

            Properties.Settings.Default.setBrand_HBrand = brandH;


            if (rbtnBrand4WDCase.Checked)
                brand4WD = WDBrand.Case;
            else if (rbtnBrand4WDChallenger.Checked)
                brand4WD = WDBrand.Challenger;
            else if (rbtnBrand4WDJDeere.Checked)
                brand4WD = WDBrand.JDeere;
            else if (rbtnBrand4WDNH.Checked)
                brand4WD = WDBrand.NewHolland;
            else
                brand4WD = WDBrand.AGOpenGPS;

            Properties.Settings.Default.setBrand_WDBrand = brand4WD;


            Bitmap bitmap = mf.GetTractorBrand(brand);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[13]);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);


            bitmap = mf.GetHarvesterBrand(brandH);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[18]);
            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);


            bitmap = mf.Get4WDBrandFront(brand4WD);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[16]);
            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);


            bitmap = mf.Get4WDBrandRear(brand4WD);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[17]);
            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);
        }
    }
}
