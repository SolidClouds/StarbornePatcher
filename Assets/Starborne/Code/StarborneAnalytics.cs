using System;
using UnityEngine;
using UnityEngine.Analytics;

namespace PatchKit.Unity.Patcher.UI
{
	public static class LauncherAnalytics
	{
		public static void PlayButtonVisible()
		{
			SendEvent("Play Button Visible");
		}

		public static void PlayButtonPressed()
		{
			SendEvent("Play Button Pressed");
		}

		public static void CheckForUpdatesPressed()
		{
			SendEvent("Check for Updates Pressed");
		}

		private static void SendEvent(string eventName)
		{
			if (CheckAndMarkDone(eventName))
				Analytics.CustomEvent(eventName + " (First Time)");

			Analytics.CustomEvent(eventName);
		}

		private static bool CheckAndMarkDone(string eventName)
		{
			var firstTimePressing = PlayerPrefs.GetInt(eventName, 0) == 0;
			PlayerPrefs.SetInt(eventName, 1);
			PlayerPrefs.Save();
			return firstTimePressing;
		}

		internal static void LoadingData()
		{
			SendEvent("Loading Data");
		}

		internal static void StartingApp()
		{
			SendEvent("Starting App");
		}

		internal static void UpdatingApp(string description)
		{
			SendEvent("Updating: " + description);
		}

		internal static void LoadingConfiguration()
		{
			SendEvent("Loading Configuration");
		}
	}
}