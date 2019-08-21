using PatchKit.Unity.Patcher.AppUpdater.Status;
using PatchKit.Unity.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

namespace PatchKit.Unity.Patcher.UI
{
	public class DownloadStatus : MonoBehaviour
	{

		private void Start()
		{
			var downloadStatusText = GetComponent<TextMeshProUGUI>();

			Patcher.Instance.UpdaterStatus
				.SelectSwitchOrNull(u => u.LatestActiveOperation)
				.Select(s => s as IReadOnlyDownloadStatus)
				.SelectSwitchOrDefault(GetDownloadStatus, string.Empty)
				.ObserveOnMainThread()
				.SubscribeToText(downloadStatusText)
				.AddTo(this);
		}

		private static IObservable<string> GetDownloadStatus(IReadOnlyDownloadStatus status)
		{
			return status.Bytes.CombineLatest(
					status.TotalBytes,
					(bytes, totalBytes) => string.Format("{0:0.0} MB of {1:0.0} MB", bytes / 1024.0 / 1024.0, totalBytes / 1024.0 / 1024.0)
				);
		}
	}
}
