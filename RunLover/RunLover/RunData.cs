
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace RunLover {

	public class RunData {
		public const string LOCAL_DB_NAME = "local_database";

		private const string KEY_DATE = "date";
		private const string KEY_START = "start";
		private const string KEY_FINISH = "finish";
		private const string KEY_DURATION = "duration";
		private const string KEY_DISTANCE = "distance";
		private const string KEY_LATITUDE = "latitude";
		private const string KEY_LONGITUDE = "longitude";

		private long mDate;
		private long mDuration;
		private float mDistance;
		private Position mStart;
		private Position mFinish;

		public static Dictionary<string, object> CreateDictionary(long date, 
			long duration, float distance, Position start, Position finish) {

			return new Dictionary<string, object>() {
				{ KEY_DATE, date },
				{ KEY_DURATION, duration },
				{ KEY_DISTANCE, distance },
				{ KEY_START, new Dictionary<string, object>() {
					{ KEY_LATITUDE, start.Latitude },
					{ KEY_LONGITUDE, start.Longitude}
				}},
				{ KEY_FINISH, new Dictionary<string, object>() {
					{ KEY_LATITUDE, finish.Latitude },
					{ KEY_LONGITUDE, finish.Longitude}
				}}
			};
		}

		public RunData(IDictionary<string, object> dictionary) {
			mDate = Convert.ToInt64(dictionary[KEY_DATE]);
			mDuration = Convert.ToInt64(dictionary[KEY_DURATION]);
			mDistance = Convert.ToSingle(dictionary[KEY_DISTANCE]);
			
			if (dictionary[KEY_START] is IDictionary<string, JToken>) {
				IDictionary<string, JToken> start = (IDictionary<string, JToken>)dictionary[KEY_START];
				IDictionary<string, JToken> finish = (IDictionary<string, JToken>)dictionary[KEY_FINISH];
				mFinish = new Position(finish[KEY_LATITUDE].ToObject<double>(), finish[KEY_LONGITUDE].ToObject<double>());
				mStart = new Position(start[KEY_LATITUDE].ToObject<double>(), start[KEY_LONGITUDE].ToObject<double>());
			} else {
				IDictionary<string, object> start = (IDictionary<string, object>)dictionary[KEY_START];
				IDictionary<string, object> finish = (IDictionary<string, object>)dictionary[KEY_FINISH];
				mFinish = new Position(Convert.ToDouble(finish[KEY_LATITUDE]), Convert.ToDouble(finish[KEY_LONGITUDE]));
				mStart = new Position(Convert.ToDouble(start[KEY_LATITUDE]), Convert.ToDouble(start[KEY_LONGITUDE]));
			}
		}

		public string DateString {
			get { return DateTimeOffset.FromUnixTimeMilliseconds(mDate).DateTime.ToString("dd/MM/yyyy"); }
		}

		public string DurationString {
			get { return TimeSpan.FromMilliseconds(mDuration).ToString(@"mm\:ss\:FFF"); }
		}

		public float GetDistance() {
			return mDistance;
		}

		public Position GetStartCoordinate() {
			return mStart;
		}

		public Position GetFinishCoordinate() {
			return mFinish;
		}
	}
}