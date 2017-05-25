//////////////////////////////////////////////////////////////////////////////////
//	PhidgetHandler.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Phidgets;
using Phidgets.Events;
using System.ComponentModel;

namespace LightSequencer
{
	public static class PhidgetHandler
	{
		// hashtable mapping serial number to interface kit
		public static Dictionary<int, InterfaceKit> IFKits = new Dictionary<int,InterfaceKit>();
		private static Manager _phidgetsManager;

		public static event EventHandler PhidgetsChanged;

		public static void Init()
		{
			// create a new phidgets manager to find the devices connected
			_phidgetsManager = new Manager();
            _phidgetsManager.Attach += new AttachEventHandler(_phidgetsManager_Attach);
			_phidgetsManager.Detach += new DetachEventHandler(_phidgetsManager_Detach);
			_phidgetsManager.open();
		}

        static void _phidgetsManager_Attach(object sender, AttachEventArgs e)
        {
            if (e.Device.Type == "PhidgetInterfaceKit")
            {
                // if we haven't opened it already, do so now
                if (!IFKits.ContainsKey(e.Device.SerialNumber))
                {
                    InterfaceKit ik = new InterfaceKit();
                    ik.Attach += new AttachEventHandler(ik_Attach);
                    ik.open(e.Device.SerialNumber);
                    IFKits[e.Device.SerialNumber] = ik;

                }
            }
        }
        
        static void ik_Attach(object sender, AttachEventArgs e)
        {
            if (PhidgetsChanged != null)
            {
                foreach (EventHandler AttachHandler in PhidgetsChanged.GetInvocationList())
                {
                    ISynchronizeInvoke syncInvoke = AttachHandler.Target as ISynchronizeInvoke;
                    if ((syncInvoke != null) && (syncInvoke.InvokeRequired))
                        syncInvoke.Invoke(AttachHandler, new object[] { null, null });
                    else
                        AttachHandler(null, null);
                }
            }
        }

		static void _phidgetsManager_Detach(object sender, DetachEventArgs e)
		{
            int serial = e.Device.SerialNumber;
            IFKits[serial].close();
            IFKits.Remove(serial);

            if (PhidgetsChanged != null)
            {
                foreach (EventHandler AttachHandler in PhidgetsChanged.GetInvocationList())
                {
                    ISynchronizeInvoke syncInvoke = AttachHandler.Target as ISynchronizeInvoke;
                    if ((syncInvoke != null) && (syncInvoke.InvokeRequired))
                        syncInvoke.Invoke(AttachHandler, new object[] { null, null });
                    else
                        AttachHandler(null, null);
                }
            }
     	}
	}
}
