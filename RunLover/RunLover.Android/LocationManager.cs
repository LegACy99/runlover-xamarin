
using Android.App;
using Android.Gms.Location;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

[assembly: Dependency(typeof(RunLover.Droid.LocationManager))]
namespace RunLover.Droid {

	class LocationManager : ILocationManager {

		public class LocationManagerCallback : LocationCallback {
			private LocationManager mManager;

			public LocationManagerCallback(LocationManager manager) {
				mManager = manager;
			}

			public override void OnLocationResult(LocationResult result) {
				base.OnLocationResult(result);

				mManager.OnLocationResult(result);
			}
		}

		public static Activity activity;

		private bool mPositionAvailable;
		private Position mLatestPosition;
		private LocationManagerCallback mCallback;

		public LocationManager() {
			mPositionAvailable = false;
			mLatestPosition = new Position();
			mCallback = new LocationManagerCallback(this);
		}

		public bool IsLocationAvailable() {
			return mPositionAvailable;
		}

		public Position GetLatestPosition() {
			return mLatestPosition;
		}

		public float CalculateDistance(Position position1, Position position2) {
			float[] results = new float[] { 0.0f };
			Android.Locations.Location.DistanceBetween(position1.Latitude, position1.Longitude, position2.Latitude, position2.Longitude, results);

			return results[0];
		}

		public void StartLocationRequest() {
			if (activity != null) {
				LocationRequest request = new LocationRequest()
					.SetPriority(LocationRequest.PriorityHighAccuracy)
					.SetFastestInterval(1000)
					.SetInterval(10000);

				LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
				builder.AddLocationRequest(request);
				LocationServices.GetSettingsClient(activity).CheckLocationSettings(builder.Build());

				LocationServices.GetFusedLocationProviderClient(activity).RequestLocationUpdates(request, mCallback, Looper.MyLooper());
			}
		}

		public void StopLocationRequest() {
			if (activity != null) {
				LocationServices.GetFusedLocationProviderClient(activity).RemoveLocationUpdates(mCallback);
			}
		}

		public void OnLocationResult(LocationResult result) {
			mPositionAvailable = true;
			mLatestPosition = new Position(result.LastLocation.Latitude, 
				result.LastLocation.Longitude);
		}
	}
}