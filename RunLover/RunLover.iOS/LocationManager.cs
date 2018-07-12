using CoreLocation;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

[assembly: Dependency(typeof(RunLover.iOS.LocationManager))]
namespace RunLover.iOS {

	class LocationManager : ILocationManager {

		private static CLLocationManager manager = null;

		private bool mPositionAvailable;
		private Position mLatestPosition;

		public LocationManager() {
			mPositionAvailable = false;
			mLatestPosition = new Position();

			if (manager == null) {
				manager = new CLLocationManager();
				manager.LocationsUpdated += (sender, e) => {
					if (e.Locations.Length > 0) {
						mPositionAvailable = true;
						mLatestPosition = new Position(e.Locations[0].Coordinate.Latitude, 
								e.Locations[0].Coordinate.Latitude);
					}
				};
			}
		}

		public bool IsLocationAvailable() {
			return mPositionAvailable;
		}

		public Position GetLatestPosition() {
			return mLatestPosition;
		}

		public float CalculateDistance(Position position1, Position position2) {
			CLLocation location1 = new CLLocation(position1.Latitude, position1.Longitude);
			CLLocation location2 = new CLLocation(position1.Latitude, position1.Longitude);

			return (float)location1.DistanceFrom(location2);
		}

		public void StartLocationRequest() {
			manager.StartUpdatingLocation();
		}

		public void StopLocationRequest() {
			mPositionAvailable = false;
			manager.StopUpdatingLocation();
		}
	}
}