using System;
using System.ServiceModel;
using WHSMailCommon.Contracts;

namespace WHSMailWeb
{
	public partial class BasePage : System.Web.UI.Page
	{
		ChannelFactory<IWHSMailService> _factory = null;
		private IWHSMailService _channel = null;

		protected void Page_Init(object sender, EventArgs e)
		{
			// create a channel factory and then instantiate a proxy channel
			_factory = new ChannelFactory<IWHSMailService>("WHSMailService");
			_channel = _factory.CreateChannel();
		}

		protected void Page_Unload(object sender, EventArgs e)
		{
			try
			{
				_factory.Close();
			}
			catch
			{
				// for the moment, we don't really care what happens here...if it fails, so be it
			}
		}

		public IWHSMailService WHSMailService
		{
			get { return _channel; }
		}
	
	}
}
