using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace jp.nomula.pronama.lttimer
{	
	public class BaseActivity : Activity
	{
		protected static bool play = true;
		int image = Resource.Drawable.kei1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
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
					intent.SetFlags (ActivityFlags.NewTask);
					StartActivity (intent);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		protected void PlaySound(int resId)
		{
			if (play)
			{
				var mp = MediaPlayer.Create (this, resId);
				mp.Start ();
			}
		}

		protected void Blink(int resId)
		{
			//表示中画像と同じ場合は更新しない
			if (image != resId)
			{
				image = resId;

				RunOnUiThread (() => {
					var imageView = FindViewById<ImageView> (Resource.Id.imageView2);
					imageView.SetImageResource (resId);
				});
			}
		}

		protected string TrimDurationFormat(int duration)
		{
			var time = TimeSpan.FromSeconds(duration);
			return string.Format(GetString (Resource.String.duration_format), time.Minutes, time.Seconds);
		}
	}
}

