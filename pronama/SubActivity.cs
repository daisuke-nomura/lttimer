using System;
using System.Threading;
using System.Timers;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace jp.nomula.pronama.lttimer
{
	[Activity (Label = "LTタイマー", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, HardwareAccelerated = false)]
	public class SubActivity : BaseActivity
	{
		const ushort interval = 1000;
		ushort duration = 0;
		ushort count = 0;
		bool pause = false;
		static System.Timers.Timer timer = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Sub);

			//インテント受け取り
			duration = (ushort)(Intent.GetIntExtra (GetString(Resource.String.intent_duration), 0) * 60);
			play = Intent.GetBooleanExtra (GetString(Resource.String.intent_soundon), true);
			count = duration;

			//スリープ不可
			Window.AddFlags (WindowManagerFlags.KeepScreenOn);

			//画面設定
			var textView = FindViewById<TextView> (Resource.Id.textView2);
			textView.Text = TrimDurationFormat(count);

			var chkBox = FindViewById<CheckBox> (Resource.Id.checkBox2);
			chkBox.Checked = play;
			chkBox.CheckedChange +=	(object sender, CompoundButton.CheckedChangeEventArgs e) =>
			{
				play = chkBox.Checked;
			};

			//タイマー
			timer = new System.Timers.Timer();
			timer.Interval = (double)interval;

			//タイマー動作
			timer.Elapsed += TickTimer;

			var button = FindViewById<Button> (Resource.Id.pauseButton);
			button.Click += (object sender, EventArgs e) => 
			{
				if (pause)
				{
					//一時停止から再開する
					timer.Start();
					button.Text = GetString(Resource.String.lt_pause);
					pause = false;
				}
				else
				{
					//一時停止する
					timer.Stop();
					timer.Close();
					button.Text = GetString(Resource.String.lt_resume);
					pause = true;
				}
			};

			timer.Start ();
		}

		protected override void OnDestroy()
		{
			//スリープ許可	
			Window.ClearFlags (WindowManagerFlags.KeepScreenOn);

			base.OnDestroy();

			timer.Stop();
			timer.Close();
		}

		public override bool OnKeyDown (Keycode keyCode, KeyEvent e)
		{
			if (keyCode == Keycode.Back)
			{
				ShowDialog (0);
			}

			return base.OnKeyDown (keyCode, e);
		}

		protected override Dialog OnCreateDialog (int id)
		{
			if (id == 0 && timer.Enabled)
			{
				var diag = new AlertDialog.Builder (this);
				diag.SetTitle (GetString(Resource.String.app_name));
				diag.SetMessage (GetString(Resource.String.back_confirm));
				diag.SetNegativeButton (GetString(Resource.String.no), new EventHandler<Android.Content.DialogClickEventArgs> ((object sender, Android.Content.DialogClickEventArgs e) => {
					//何もしない
				}));
				diag.SetPositiveButton (GetString(Resource.String.yes), new EventHandler<Android.Content.DialogClickEventArgs> ((object sender, Android.Content.DialogClickEventArgs e) => {
					//アクティビティを終了
					this.Finish();
				}));

				return diag.Create ();
			}

			return base.OnCreateDialog (id);
		}

		private void TickTimer(object sender, System.Timers.ElapsedEventArgs e)
		{
			//一時停止又はカウントが0以下
			if (pause || count < 0)
				return;

			count--;
			CountDown();

			//画面のカウントを更新
			RunOnUiThread(() => 
				{
					var textView = FindViewById<TextView> (Resource.Id.textView2);
					textView.Text = TrimDurationFormat(count);
				});

			if (count == 0)
			{
				timer.Stop();
				timer.Close();
				Thread.Sleep(interval);
				PlaySound(Resource.Raw.end);

				RunOnUiThread(() =>
					{
						//一時停止不可にする
						var button = FindViewById<Button> (Resource.Id.pauseButton);
						button.Enabled = false;
					});
			}
		}

		private void CountDown()
		{
			//音声の再生と画像の更新
			switch (count)
			{
				case 0: PlaySound(Resource.Raw.c0); Blink (Resource.Drawable.kei0); break;
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
				case 29: break;
				case 30: PlaySound(Resource.Raw.c30); Blink(Resource.Drawable.kei30); break;
				case 60: PlaySound(Resource.Raw.c60); break;
				default: Blink(Resource.Drawable.kei1); break;
			}
		}
	}
}

