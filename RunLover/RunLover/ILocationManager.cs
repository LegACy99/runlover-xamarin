
using Xamarin.Forms.Maps;

namespace RunLover {

    public interface ILocationManager {
		bool IsLocationAvailable();
		Position GetLatestPosition();
		float CalculateDistance(Position position1, Position position2);

		void StartLocationRequest();
		void StopLocationRequest();
	}
}
