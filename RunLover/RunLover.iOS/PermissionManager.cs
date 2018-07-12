﻿
using Xamarin.Forms;

[assembly: Dependency(typeof(RunLover.iOS.PermissionManager))]
namespace RunLover.iOS {

	class PermissionManager : IPermissionManager {
		public bool IsLocationPermissionGranted() {
			return true;
		}

		public void RequestLocationPermission() {}
	}
}