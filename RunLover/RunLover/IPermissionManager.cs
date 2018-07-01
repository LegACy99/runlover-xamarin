
namespace RunLover {

    public interface IPermissionManager {
		bool IsLocationPermissionGranted();
		void RequestLocationPermission();
    }
}
