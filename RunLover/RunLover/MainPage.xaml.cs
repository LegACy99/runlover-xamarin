using Couchbase.Lite;
using Couchbase.Lite.Auth;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RunLover
{

	public partial class MainPage : ContentPage {

		private List<RunData> mHistory;

		public MainPage() {
			InitializeComponent();

			Title = "Run Lover";

			mHistory = new List<RunData>();
			listHistory.ItemsSource = mHistory;

			ReadHistoryData();

			Database database = Couchbase.Lite.Manager.SharedInstance.GetDatabase(RunData.LOCAL_DB_NAME);

			Replication downloader = database.CreatePullReplication(new Uri(DATABASE_URL + "/" + DATABASE_NAME));
			downloader.Authenticator = AuthenticatorFactory.CreateBasicAuthenticator(USERNAME, PASSWORD);
			downloader.Changed += OnReplicationChange;
			downloader.Start();
		}

		private async void OnTrackClick(object sender, EventArgs e) {
			if (DependencyService.Get<IPermissionManager>().IsLocationPermissionGranted()) {
				await Navigation.PushAsync(new TrackPage());
			} else {
				DependencyService.Get<IPermissionManager>().RequestLocationPermission();
			}
		}

		private async void OnItemSelect(object sender, SelectedItemChangedEventArgs e) {
			if (e.SelectedItem != null) {
				await Navigation.PushAsync(new DetailPage((RunData)e.SelectedItem));
			}
		}

        private void OnReplicationChange(object sender, EventArgs eventArgs) {
			ReadHistoryData();
		}

		private void ReadHistoryData() {
			Database database = Couchbase.Lite.Manager.SharedInstance.GetDatabase(RunData.LOCAL_DB_NAME);

			if (mHistory.Count != database.GetDocumentCount()) {
				mHistory = new List<RunData>();

				QueryEnumerator enumerator = database.CreateAllDocumentsQuery().Run();
				foreach (QueryRow row in enumerator) {
					mHistory.Add(new RunData(row.Document.Properties));
				}

				listHistory.ItemsSource = mHistory;
			}
		}
	}
}
