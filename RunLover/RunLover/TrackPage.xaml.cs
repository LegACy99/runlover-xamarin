using Couchbase.Lite;
using Couchbase.Lite.Auth;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RunLover {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TrackPage : ContentPage {

		private bool mTracking;

		private TimeSpan mDuration;
		private DateTime mStartTime;

		private bool mStarted;
		private float mDistance;
		private Position mStartCoordinate;
		private Position mFinishCoordinate;

		public TrackPage () {
			InitializeComponent ();

			Title = "Track Run";

			mTracking = false;

			mDuration = TimeSpan.Zero;
			mStartTime = DateTime.Now;

			mStarted = false;
			mStartCoordinate = new Position();
			mFinishCoordinate = new Position();
			mDistance = 0f;
		}

		private void OnTrackClick() {
			if (mTracking) { 
				buttonTrack.IsVisible = false;
				buttonReset.IsVisible = true;
				buttonSave.IsVisible = true;

				DependencyService.Get<ILocationManager>().StopLocationRequest();

			} else {
				buttonTrack.Text = "STOP";

				mDistance = 0f;
				mStarted = false;
				mStartCoordinate = new Position();
				mFinishCoordinate = new Position();
				mStartTime = DateTime.Now;

				DependencyService.Get<ILocationManager>().StartLocationRequest();

				Device.StartTimer(new TimeSpan(0, 0, 0, 0, 30), () => {
					mDuration = DateTime.Now.Subtract(mStartTime);
					textDuration.Text = mDuration.ToString(@"mm\:ss\:FFF");					
					
					if (DependencyService.Get<ILocationManager>().IsLocationAvailable()) {
						Position previousCoordinate = mFinishCoordinate;
						mFinishCoordinate = DependencyService.Get<ILocationManager>().GetLatestPosition();

						map.MoveToRegion(MapSpan.FromCenterAndRadius(mFinishCoordinate, Distance.FromKilometers(1)));

						if (!mStarted) {
							mStarted = true;
							mStartCoordinate = mFinishCoordinate;

						} else if (!previousCoordinate.Equals(mFinishCoordinate)) {
							mDistance += DependencyService.Get<ILocationManager>().CalculateDistance(previousCoordinate, mFinishCoordinate);
							textDistance.Text = mDistance.ToString("0.00") + " m";
						}
					}

					return mTracking;
				});
			}

			mTracking = !mTracking;
		}

		private void OnResetClick() {
			buttonTrack.IsVisible = true;
			buttonTrack.Text = "START";

			buttonSave.IsVisible = false;
			buttonReset.IsVisible = false;

			textDuration.Text = new TimeSpan(0, 0, 0, 0, 0).ToString(@"mm\:ss\:FFF");
			textDistance.Text = "0.00 m";
		}

		private async void OnSaveClick() {
			Dictionary<string, object> data = RunData.CreateDictionary(
				new DateTimeOffset(mStartTime).ToUnixTimeMilliseconds(), (long)mDuration.TotalMilliseconds,
				mDistance, mStartCoordinate, mFinishCoordinate);

			Database database = Couchbase.Lite.Manager.SharedInstance.GetDatabase(RunData.LOCAL_DB_NAME);

			Document document = database.CreateDocument();
			document.PutProperties(data);

			Replication uploader = database.CreatePushReplication(new Uri(DATABASE_URL + "/" + DATABASE_NAME));
			uploader.Authenticator = AuthenticatorFactory.CreateBasicAuthenticator(USERNAME, PASSWORD);
			uploader.Start();

			await Navigation.PushAsync(new DetailPage(new RunData(data)));
			//Navigation.RemovePage(this);
		}
	}
}