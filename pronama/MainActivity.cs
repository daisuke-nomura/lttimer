using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;

namespace jp.nomula.pronama.lttimer
{
	[Activity (Label = "LTタイマー", MainLauncher = true)]
	public class MainActivity : Activity
	{
		const int min = 1;
		const int max = 15;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			var numberPicker = FindViewById<NumberPicker> (Resource.Id.numberPicker);
			numberPicker.MinValue = min;
			numberPicker.MaxValue = max;

			var button = FindViewById<Button> (Resource.Id.startButton);
			button.Click += delegate {
				var chkBox = FindViewById<CheckBox> (Resource.Id.checkBox1);
				if (chkBox.Checked)
				{
					var mp = MediaPlayer.Create (this, Resource.Raw.start);
					mp.Start ();
				}

				var intent = new Intent(this, typeof(SubActivity));
				intent.PutExtra(GetString(Resource.String.intent_duration), numberPicker.Value);
				intent.PutExtra(GetString(Resource.String.intent_soundon), chkBox.Checked);
				StartActivity(intent);
			};
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			menu.Add(0,0,0,GetString(Resource.String.app_info));
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case 0:
					var intent = new Intent (Intent.ActionView, Android.Net.Uri.Parse(GetString(Resource.String.web_site)));
					StartActivity (intent);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
	}
}