using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RunLover {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : ContentPage {

		public DetailPage (RunData data) {
			InitializeComponent ();

			Title = "Run Detail";

			textDistance.Text = "Distance: " + data.GetDistance().ToString("0.00") + " m";
			textDuration.Text = "Duration: " + data.DurationString;
			textDate.Text = data.DateString;

			map.Pins.Add(new Pin { Position = data.GetStartCoordinate(), Label = "Start" });
			map.Pins.Add(new Pin { Position = data.GetFinishCoordinate(), Label = "Finish" });

			Position center = new Position((data.GetStartCoordinate().Latitude + data.GetFinishCoordinate().Latitude) / 2f,
											(data.GetStartCoordinate().Longitude + data.GetFinishCoordinate().Longitude) / 2f);
			map.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(1)));
		}
	}
}