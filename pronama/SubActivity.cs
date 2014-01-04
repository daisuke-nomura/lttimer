using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using System.Timers;
using System.Threading;

namespace jp.nomula.pronama.lttimer
{
	[Activity (Label = "LTタイマー")]			
	public class SubActivity : Activity
	{
		const int interval = 1000;
		int duration = 0;
		int count = 0;
		bool play = true;
		bool pause = false;
		static System.Timers.Timer timer = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Sub);

			duration = Intent.GetIntExtra (GetString(Resource.String.intent_duration), 0) * 60;
			play = Intent.GetBooleanExtra (GetString(Resource.String.intent_soundon), true);
			count = duration;

			var textView = FindViewById<TextView> (Resource.Id.textView2);
			var time = TimeSpan.FromSeconds(count);
			textView.Text = string.Format(GetString(Resource.String.duration_format), time.Minutes, time.Seconds);

			var chkBox = FindViewById<CheckBox> (Resource.Id.checkBox2);
			chkBox.Checked = play;
			chkBox.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => 
			{
				play = chkBox.Checked;
			};

			//タイマー
			timer = new System.Timers.Timer ();
			timer.Interval = interval;

			timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) => 
			{
				if (pause || count < 0)
					return;

				count--;
				CountDown();

				//画面のカウントを更新
				time = TimeSpan.FromSeconds(count);
				RunOnUiThread(() => textView.Text = string.Format(GetString(Resource.String.duration_format), time.Minutes, time.Seconds));

				if (count == 0)
				{
					timer.Stop();
					timer.Close();
					Thread.Sleep(interval);
					PlaySound(Resource.Raw.end);

					RunOnUiThread(() =>
						{
							var button2 = FindViewById<Button> (Resource.Id.pauseButton);
							button2.Enabled = false;
						});
				}
			};

			var button = FindViewById<Button> (Resource.Id.pauseButton);
			button.Click += (object sender, EventArgs e) => 
			{
				if (pause)
				{
					//一時停止から再開する
					timer.Start();
					button.Text = GetString(Resource.String.LTstop);
					pause = false;
				}
				else
				{
					//一時停止する
					timer.Stop();
					timer.Close();
					button.Text = GetString(Resource.String.LTresume);
					pause = true;
				}
			};

			timer.Start ();
		}

		private void CountDown()
		{
			//音声の再生
			switch (count)
			{
				case 0:	PlaySound(Resource.Raw.c0); break;
				case 1:	PlaySound(Resource.Raw.c1);	break;
				case 2:	PlaySound(Resource.Raw.c2);	break;
				case 3:	PlaySound(Resource.Raw.c3);	break;
				case 4:	PlaySound(Resource.Raw.c4);	break;
				case 5:	PlaySound(Resource.Raw.c5);	break;
				case 6:	PlaySound(Resource.Raw.c6);	break;
				case 7:	PlaySound(Resource.Raw.c7);	break;
				case 8:	PlaySound(Resource.Raw.c8);	break;
				case 9:	PlaySound(Resource.Raw.c9); break;
				case 10: PlaySound(Resource.Raw.c10); break;
				case 30: PlaySound(Resource.Raw.c30); break;
				case 60: PlaySound(Resource.Raw.c60); break;
				default: break;
			}

			//画像の更新
			switch (count)
			{
				case 30: Blink(Resource.Drawable.kei30); break;
				case 0:	Blink(Resource.Drawable.kei0); break;
				default: Blink (Resource.Drawable.kei1); break;
			}
		}

		private void PlaySound(int resId)
		{
			if (play)
			{
				var mp = MediaPlayer.Create (this, resId);
				mp.Start ();
			}
		}

		private void Blink(int resId)
		{
			RunOnUiThread (() =>
				{
					var imageView = FindViewById<ImageView> (Resource.Id.imageView1);
					imageView.SetImageResource (resId);
				});
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

