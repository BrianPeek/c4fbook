using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WiimoteWhiteboard
{
	public partial class CalibrationForm : Form
	{
		private Bitmap _bmpCalibration;
		private Graphics _gfxCalibration;
	  
		public CalibrationForm()
		{
			InitializeComponent();

			Rectangle rect = Screen.GetWorkingArea(this);

			this.Size = new Size(rect.Width, rect.Height);
			this.Text = "Calibration - Working area:" + Screen.GetWorkingArea(this).ToString() + " || Real area: " + Screen.GetBounds(this).ToString();
	  
			_bmpCalibration = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
			_gfxCalibration = Graphics.FromImage(_bmpCalibration);
			pbCalibrate.Left = 0;
			pbCalibrate.Top = 0;
			pbCalibrate.Size = new Size(rect.Width, rect.Height);
			pbCalibrate.Image = _bmpCalibration;

			_gfxCalibration.Clear(Color.White);
		}

		public void ShowCalibration(int x, int y, int size, Pen p)
		{
			_gfxCalibration.Clear(Color.White);

			// draw crosshair
			_gfxCalibration.DrawEllipse(p, x - size / 2, y - size / 2, size, size);
			_gfxCalibration.DrawLine(p, x-size, y, x+size, y);
			_gfxCalibration.DrawLine(p, x, y-size, x, y+size);

			BeginInvoke((MethodInvoker)delegate() { pbCalibrate.Image = _bmpCalibration; });
		}

		private void CalibrationForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.Close();
		}
	}
}