
using Android.App;
using Android.Content.PM;
using Android.Support.V4.App;
using Xamarin.Forms;

[assembly: Dependency(typeof(RunLover.Droid.PermissionManager))]
namespace RunLover.Droid {

	class PermissionManager : IPermissionManager {

		public static Activity activity;

		public bool IsLocationPermissionGranted() {
			if (activity != null) {
				return ActivityCompat.CheckSelfPermission(activity, Android.Manifest.Permission.AccessFineLocation) == Permission.Granted;
			}

			return false;
		}

		public void RequestLocationPermission() {
			if (activity != null) {
				ActivityCompat.RequestPermissions(activity, new string[] { Android.Manifest.Permission.AccessFineLocation }, 1);
			}
		}
	}
}