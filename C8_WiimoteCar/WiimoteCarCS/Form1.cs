using System;
using System.Windows.Forms;
using Phidgets;
using WiimoteLib;

namespace WiimoteCar
{
	public partial class Form1 : Form
	{
		// instance of the connected Wiimote
		private Wiimote _wiimote;

		// instance of the connected Phidget Interface Kit
		private InterfaceKit _interfaceKit;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// create the InterfaceKit object and open access to the board
			_interfaceKit = new InterfaceKit();
			_interfaceKit.open();
			_interfaceKit.waitForAttachment(1000);

			// create the Wiimote object
			_wiimote = new Wiimote();

			// setup an event handler to be notified when the Wiimote sends a packet of data
			_wiimote.WiimoteChanged += Wiimote_WiimoteChanged;

			// connect the Wiimote
			_wiimote.Connect();

			// set the report type to Buttons and Accelerometer data only
			_wiimote.SetReportType(InputReport.ButtonsAccel, true);
		}

		void Wiimote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
		{
			// get the current Wiimote state
			WiimoteState ws = e.WiimoteState;

			// if button 1 is pressed, toggle the Forward output
			_interfaceKit.outputs[0] = ws.ButtonState.One;

			// if button 2 is pressed, toggle the Backward output
			_interfaceKit.outputs[1] = ws.ButtonState.Two;

			// if the Wiimote is tilted far enough to the left, toggle the Left output
			_interfaceKit.outputs[2] = (ws.AccelState.Values.Y < -0.07f);

			// if the Wiimote is tilted far enough to the right, toggle the Reft output
			_interfaceKit.outputs[3] = (ws.AccelState.Values.Y >  0.07f);
		}
	}
}
