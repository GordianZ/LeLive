using System;
using System.Net;
using AVFoundation;
using AVKit;
using Foundation;
using UIKit;

namespace LeLive
{
	public partial class ViewController : UIViewController
	{
		NSUrl mediaUrl;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			btnMain.PrimaryActionTriggered += LoadPlayer;
			var url = new Uri ("http://static.api.sports.letv.com/sms/v1/carousels/channels?clientId=1050701004&caller=1001");
			var wc = new WebClient ();
			wc.DownloadStringCompleted += (sender, e) => {
				NSError err = null;
				var res = NSJsonSerialization.Deserialize (e.Result, NSJsonReadingOptions.MutableContainers, out err);
				var data = res.ValueForKey ((NSString)"data") as NSArray;
				var firstChannel = data.GetItem<NSObject> (0);
				var channelName = firstChannel.ValueForKey ((NSString)"channelName") as NSString;
				var channelStreams = firstChannel.ValueForKey ((NSString)"streams") as NSArray;
				var lastStream = channelStreams.GetItem<NSObject> (channelStreams.Count - 1);
				var streamUrl = lastStream.ValueForKey ((NSString)"streamUrl") as NSString;
				mediaUrl = new NSUrl (streamUrl + "&sign=live_tv&format=letv&platid=10&splatid=1009");
				Console.WriteLine (mediaUrl);
				btnMain.SetTitle (channelName, UIControlState.Normal);
			};
			wc.DownloadStringAsync (url);

			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		void LoadPlayer (object sender, EventArgs e)
		{
			//var mediaUrl = new NSUrl ("");
			//var asset = AVAsset.FromUrl (mediaUrl);
			//var playerItem = AVPlayerItem.FromUrl (mediaUrl);
			var pvc = new AVPlayerViewController ();
			var avplayer = AVPlayer.FromUrl (mediaUrl);
			pvc.Player = avplayer;
			PresentViewController (pvc, true, avplayer.Play);
		}
	}
}
