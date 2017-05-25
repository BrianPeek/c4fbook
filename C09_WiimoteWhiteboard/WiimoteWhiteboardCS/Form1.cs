using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using WiimoteLib;

namespace WiimoteWhiteboard
{
	public partial class Form1 : Form
	{
        private const string CalibrationFilename = "calibration.dat";

        //instance of the wii remote
		private Wiimote _wm = new Wiimote();
        private WiimoteState _lastWiiState = new WiimoteState();//helps with event firing

        private bool _cursorControl;
		private Size _screenSize;

        private int _calibrationState;
        private float _calibrationMargin = .1f; //specifies how far in to draw the crosshairs

		int _smoothingCount = 0; //track the number of points stored
		int _smoothingPoints = 5; //store the number of points to average out (1-10)
		float[] _smoothingX = new float[10]; //store the past coordinates for smoothing
		float[] _smoothingY = new float[10];

        private CalibrationForm cf;

        private Warper _warper = new Warper();
        private float[] _srcX = new float[4];
        private float[] _srcY = new float[4];
        private float[] _dstX = new float[4];
        private float[] _dstY = new float[4];


        //declare consts for mouse messages
        public const int INPUT_MOUSE			= 0;
        public const int MOUSEEVENTF_MOVE		= 0x01;
        public const int MOUSEEVENTF_LEFTDOWN	= 0x02;
        public const int MOUSEEVENTF_LEFTUP		= 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN	= 0x08;
        public const int MOUSEEVENTF_RIGHTUP	= 0x10;
        public const int MOUSEEVENTF_ABSOLUTE	= 0x8000;

        //declare consts for key scan codes
        public const byte VK_LEFT	= 0x25;
        public const byte VK_UP		= 0x26;
        public const byte VK_RIGHT 	= 0x27;
        public const byte VK_DOWN 	= 0x28;

        public const int KEYEVENTF_KEYUP = 0x02;

        public struct MOUSEINPUT	//24 bytes
        {
            public int dx;			//4
            public int dy;			//4
            public uint mouseData;	//4
            public uint dwFlags;	//4
            public uint time;		//4
            public IntPtr dwExtraInfo;//4
        }

		public struct INPUT			//28 bytes
        {
            public int type;		// 4 bytes
            public MOUSEINPUT mi;	//24 bytes
        }

        private const int INPUT_SIZE = 28;

        private INPUT[] _buffer = new INPUT[2];

        //for firing mouse events
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        //imports keybd_event function from user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, long dwFlags, long dwExtraInfo);

		// delegate for updating the UI on the original thread
		private delegate void UpdateUIDelegate(WiimoteState args);

		// delegate for updating tracking info
		private delegate void UpdateTrackingUtilizationDelegate(string utilStatus);

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
            _screenSize.Width = Screen.GetBounds(this).Width;
            _screenSize.Height= Screen.GetBounds(this).Height;
            
            try
            {
                //connect to wii remote
                _wm.Connect();

                //set what features you want to enable for the remote, look at Wiimote.InputReport for options
                _wm.SetReportType(InputReport.IRAccel, true);
                
                //set wiiremote LEDs with this enumerated ID
                _wm.SetLEDs(true, false, false, false);
            }
            catch (Exception x)
            {
                MessageBox.Show("Exception: " + x.Message);
                this.Close();
            }

            //add event listeners to changes in the wiiremote
            //fired for every input report - usually 100 times per second if accelerometer is enabled
			_wm.WiimoteChanged += wm_WiimoteChanged;

            LoadCalibrationData();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
            //disconnect the wiimote
			_wm.Disconnect();
		}

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
			if(cf == null || cf.IsDisposed)
				cf = new CalibrationForm();

            cf.Show();
            _cursorControl = false;
            _calibrationState = 1;
            DoCalibration();
        }

        private void cbCursorControl_CheckedChanged(object sender, EventArgs e)
        {
            _cursorControl = cbCursorControl.Checked;
        }

		void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
		{
            //extract the wiimote state
            WiimoteState ws = e.WiimoteState;

            if (ws.IRState.IRSensors[0].Found)
            {
                int x = ws.IRState.IRSensors[0].RawPosition.X;
                int y = ws.IRState.IRSensors[0].RawPosition.Y;
                float warpedX = x;
                float warpedY = y;

                _warper.Warp(x, y, ref warpedX, ref warpedY);

                if (!_lastWiiState.IRState.IRSensors[0].Found)//mouse down
                {
                    _lastWiiState.IRState.IRSensors[0].Found = ws.IRState.IRSensors[0].Found;

                    if (_cursorControl)
                    {
                        _buffer[0].type = INPUT_MOUSE;
                        _buffer[0].mi.dx = (int)(warpedX * 65535.0f / _screenSize.Width);
                        _buffer[0].mi.dy = (int)(warpedY * 65535.0f / _screenSize.Height);
                        _buffer[0].mi.mouseData = 0;
                        _buffer[0].mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;
                        _buffer[0].mi.time = 0;
                        _buffer[0].mi.dwExtraInfo = (IntPtr)0;

                        _buffer[1].type = INPUT_MOUSE;
                        _buffer[1].mi.dx = 0;
                        _buffer[1].mi.dy = 0;
                        _buffer[1].mi.mouseData = 0;
                        _buffer[1].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
                        _buffer[1].mi.time = 10;
                        _buffer[1].mi.dwExtraInfo = (IntPtr)0;

                        SendInput(2, _buffer, INPUT_SIZE);

                    }//cusor control

                    switch (_calibrationState)
                    {
                        case 1:
                            _srcX[_calibrationState - 1] = x;
                            _srcY[_calibrationState - 1] = y;
                            _calibrationState = 2;
                            DoCalibration();
                            break;
                        case 2:
                            _srcX[_calibrationState - 1] = x;
                            _srcY[_calibrationState - 1] = y;
                            _calibrationState = 3;
                            DoCalibration();
                            break;
                        case 3:
                            _srcX[_calibrationState - 1] = x;
                            _srcY[_calibrationState - 1] = y;
                            _calibrationState = 4;
                            DoCalibration();
                            break;
                        case 4:
                            _srcX[_calibrationState - 1] = x;
                            _srcY[_calibrationState - 1] = y;
                            _calibrationState = 5;
                            DoCalibration();
                            break;
                        default:
                            break;
                    }//calibtation state
                }//mouse down                
                else
                {
					if (_cursorControl)//dragging
					{
						float sumX = warpedX, sumY = warpedY; //initialize the sum
						_smoothingCount += 1; //track number of stored values

						if (_smoothingCount > _smoothingPoints)
							_smoothingCount = _smoothingPoints;

						for (int i = 0; i < _smoothingPoints - 1; i++)
						{
							//shift stored points
							_smoothingX[i] = _smoothingX[i + 1]; 
							_smoothingY[i] = _smoothingY[i + 1];

							sumX += _smoothingX[i]; //adding sum
							sumY += _smoothingY[i];
						}

						//add new points to the last position
						_smoothingX[_smoothingPoints - 1] = warpedX; 
						_smoothingY[_smoothingPoints - 1] = warpedY;

						//calculate the averaged values
						float smoothWarpedX = sumX / (_smoothingCount); 
						float smoothWarpedY = sumY / (_smoothingCount);

						//mouse drag with the averaged coordinate
						_buffer[0].type = INPUT_MOUSE;
						_buffer[0].mi.dx = (int)(smoothWarpedX * 65535.0f / _screenSize.Width);
						_buffer[0].mi.dy = (int)(smoothWarpedY * 65535.0f / _screenSize.Height);
						_buffer[0].mi.mouseData = 0;
						_buffer[0].mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;
						_buffer[0].mi.time = 0;
						_buffer[0].mi.dwExtraInfo = (IntPtr)0;
						SendInput(1, _buffer, INPUT_SIZE);
					}
                }
            }//ir visible
            else
            {
                if (_lastWiiState.IRState.IRSensors[0].Found)//mouse up
                {
					if (_cursorControl)
					{
						_buffer[0].type = INPUT_MOUSE;
						_buffer[0].mi.dx = 0;
						_buffer[0].mi.dy = 0;
						_buffer[0].mi.mouseData = 0;
						_buffer[0].mi.dwFlags = MOUSEEVENTF_LEFTUP;
						_buffer[0].mi.time = 0;
						_buffer[0].mi.dwExtraInfo = (IntPtr)0;

						_buffer[1].type = INPUT_MOUSE;
						_buffer[1].mi.dx = 0;
						_buffer[1].mi.dy = 0;
						_buffer[1].mi.mouseData = 0;
						_buffer[1].mi.dwFlags = MOUSEEVENTF_MOVE;
						_buffer[1].mi.time = 0;
						_buffer[1].mi.dwExtraInfo = (IntPtr)0;
						SendInput(2, _buffer, INPUT_SIZE);

						//clear smoothing data when mouse up
						ResetCursorSmoothing();
					}
                }//ir lost
            }

            _lastWiiState.IRState.IRSensors[0].Found = ws.IRState.IRSensors[0].Found;

            if (!_lastWiiState.ButtonState.A && ws.ButtonState.A)
                BeginInvoke((MethodInvoker)delegate() { btnCalibrate.PerformClick(); });

            _lastWiiState.ButtonState.A = ws.ButtonState.A;

            if (!_lastWiiState.ButtonState.Up && ws.ButtonState.Up)
                keybd_event(VK_UP, 0x45, 0, 0);
            if (_lastWiiState.ButtonState.Up && !ws.ButtonState.Up)
                keybd_event(VK_UP, 0x45, KEYEVENTF_KEYUP, 0);
            _lastWiiState.ButtonState.Up = ws.ButtonState.Up;

            if (!_lastWiiState.ButtonState.Down && ws.ButtonState.Down)
                keybd_event(VK_DOWN, 0x45, 0, 0);
            if (_lastWiiState.ButtonState.Down && !ws.ButtonState.Down)
                keybd_event(VK_DOWN, 0x45, KEYEVENTF_KEYUP, 0);
            _lastWiiState.ButtonState.Down = ws.ButtonState.Down;

            if (!_lastWiiState.ButtonState.Left && ws.ButtonState.Left)
                keybd_event(VK_LEFT, 0x45, 0, 0);
            if (_lastWiiState.ButtonState.Left && !ws.ButtonState.Left)
                keybd_event(VK_LEFT, 0x45, KEYEVENTF_KEYUP, 0);
            _lastWiiState.ButtonState.Left = ws.ButtonState.Left;

            if (!_lastWiiState.ButtonState.Right && ws.ButtonState.Right)
                keybd_event(VK_RIGHT, 0x45, 0, 0);
            if (_lastWiiState.ButtonState.Right && !ws.ButtonState.Right)
                keybd_event(VK_RIGHT, 0x45, KEYEVENTF_KEYUP, 0);
            _lastWiiState.ButtonState.Right = ws.ButtonState.Right;

			BeginInvoke(new UpdateUIDelegate(UpdateUI), e.WiimoteState);
        }

		private void UpdateUI(WiimoteState ws)
		{
            //draw battery value on GUI
            pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
            float f = (((100.0f * 48.0f * (float)(ws.Battery / 48.0f))) / 192.0f);
            lblBattery.Text = f.ToString("F");

            //check the GUI check boxes if the IR dots are visible
            String irstatus = "Visible IR dots: ";
            if (ws.IRState.IRSensors[0].Found)
                irstatus += "1 ";
            if (ws.IRState.IRSensors[1].Found)
                irstatus += "2 ";
            if (ws.IRState.IRSensors[2].Found)
                irstatus += "3 ";
            if (ws.IRState.IRSensors[3].Found)
                irstatus += "4 ";

            lblIRVisible.Text = irstatus;
		}

        void UpdateTrackingUtilization()
        {
            //area of ideal calibration coordinates (to match the screen)
            float idealArea = (1 - 2*_calibrationMargin) * 1024 * (1 - 2*_calibrationMargin) * 768;
            
            //area of quadrliatera
            float actualArea = 0.5f * Math.Abs((_srcX[1] - _srcX[2]) * (_srcY[0] - _srcY[3]) - (_srcX[0] - _srcX[3]) * (_srcY[1] - _srcY[2]));

            float util = (actualArea / idealArea)*100;
			string utilstatus = "Tracking Utilization: " + util.ToString("f0")+"%";

            BeginInvoke(new UpdateTrackingUtilizationDelegate(UpdateTrackingUtilizationUI), utilstatus);
        }

		private void UpdateTrackingUtilizationUI(string utilStatus)
		{
			lblTrackingUtil.Text = utilStatus;
		}

		public void LoadCalibrationData()
        {
            // create reader & open file
            try
            {
                TextReader tr = new StreamReader(CalibrationFilename);
                for (int i = 0; i < 4; i++)
                {
                    _srcX[i] = float.Parse(tr.ReadLine());
                    _srcY[i] = float.Parse(tr.ReadLine());
                }

                // close the stream
                tr.Close();
            }
            catch (FileNotFoundException)
            {
                //no prexsting calibration
                return;
            }

            _warper.SetDestination( _screenSize.Width  * _calibrationMargin,
                                    _screenSize.Height * _calibrationMargin,
                                    _screenSize.Width  * (1.0f-_calibrationMargin),
                                    _screenSize.Height * _calibrationMargin,
                                    _screenSize.Width  * _calibrationMargin,
                                    _screenSize.Height * (1.0f - _calibrationMargin),
                                    _screenSize.Width  * (1.0f - _calibrationMargin),
                                    _screenSize.Height * (1.0f - _calibrationMargin));
            _warper.SetSource(_srcX[0], _srcY[0], _srcX[1], _srcY[1], _srcX[2], _srcY[2], _srcX[3], _srcY[3]);

            _warper.ComputeWarp();
            _cursorControl = true;

            cbCursorControl.Checked = _cursorControl;

            UpdateTrackingUtilization();
        }

        public void SaveCalibrationData()
        {
            TextWriter tw = new StreamWriter(CalibrationFilename);

            // write a line of text to the file
            for (int i = 0; i < 4; i++)
            {
                tw.WriteLine(_srcX[i]);
                tw.WriteLine(_srcY[i]);
            }

            // close the stream
            tw.Close();
        }

        public void DoCalibration()
		{
            if (cf == null)
                return;

            int x;
            int y;
            int size = 25;
            Pen p = new Pen(Color.Red);

            switch (_calibrationState)
            {
                case 1:
                    x = (int)(_screenSize.Width * _calibrationMargin);
                    y = (int)(_screenSize.Height * _calibrationMargin);
                    cf.ShowCalibration(x, y, size, p);
                    _dstX[_calibrationState - 1] = x;
                    _dstY[_calibrationState - 1] = y;
                    break;
                case 2:
                    x = _screenSize.Width - (int)(_screenSize.Width * _calibrationMargin);
                    y = (int)(_screenSize.Height * _calibrationMargin);
                    cf.ShowCalibration(x, y, size, p);
                    _dstX[_calibrationState - 1] = x;
                    _dstY[_calibrationState - 1] = y;
                    break;
                case 3:
                    x = (int)(_screenSize.Width * _calibrationMargin);
                    y = _screenSize.Height -(int)(_screenSize.Height * _calibrationMargin);
                    cf.ShowCalibration(x, y, size, p);
                    _dstX[_calibrationState - 1] = x;
                    _dstY[_calibrationState - 1] = y;
                    break;
                case 4:
                    x = _screenSize.Width - (int)(_screenSize.Width * _calibrationMargin);
                    y = _screenSize.Height -(int)(_screenSize.Height * _calibrationMargin);
                    cf.ShowCalibration(x, y, size, p);
                    _dstX[_calibrationState - 1] = x;
                    _dstY[_calibrationState - 1] = y;
                    break;
                case 5:
                    //compute warp
                    _warper.SetDestination(_dstX[0], _dstY[0], _dstX[1], _dstY[1], _dstX[2], _dstY[2], _dstX[3], _dstY[3]);
                    _warper.SetSource(_srcX[0], _srcY[0], _srcX[1], _srcY[1], _srcX[2], _srcY[2], _srcX[3], _srcY[3]);
                    _warper.ComputeWarp();
                    _calibrationState = 0;
                    _cursorControl = true;
                    BeginInvoke((MethodInvoker)delegate() { cf.Close(); cbCursorControl.Checked = _cursorControl; });
                    SaveCalibrationData();
                    UpdateTrackingUtilization();
                    break;
                default:
                    break;
            }
        }

		private void ResetCursorSmoothing() //reset smoothing data
		{
			_smoothingCount = 0;
			for (int i = 0; i < 10; i++)
			{
			   _smoothingX[i] = 0;
			   _smoothingY[i] = 0;
			}
		}
	}
}