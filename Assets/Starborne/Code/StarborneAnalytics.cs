using System;
using UnityEngine;
using UnityEngine.Analytics;

namespace PatchKit.Unity.Patcher.UI
{
	public static class LauncherAnalytics
	{
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
	}
}