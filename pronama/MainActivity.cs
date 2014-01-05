using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace jp.nomula.pronama.lttimer
{
	[Activity (Label = "LTタイマー", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, MainLauncher = true)]
	public class MainActivity : BaseActivity
	{
		const ushort min = 1;
		const ushort max = 15;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			var numberPicker = FindViewById<NumberPicker> (Resource.Id.numberPicker);
			numberPicker.MinValue = (int)min;
			numberPicker.MaxValue = (int)max;


			var button = FindViewById<Button> (Resource.Id.startButton);
			button.Click += (object sender, EventArgs e) => 
			{
				var chkBox = FindViewById<CheckBox> (Resource.Id.checkBox1);
				play = chkBox.Checked;
				PlaySound(Resource.Raw.start);

				var intent = new Intent(this, typeof(SubActivity));
				intent.PutExtra(GetString(Resource.String.intent_duration), numberPicker.Value);
				intent.PutExtra(GetString(Resource.String.intent_soundon), chkBox.Checked);
				StartActivity(intent);
			};
		}

		protected override void OnStart ()
		{
			base.OnStart ();

			//音声あり・なしを適用
			var chkBox = FindViewById<CheckBox> (Resource.Id.checkBox1);
			chkBox.Checked = play;
		}
	}
}