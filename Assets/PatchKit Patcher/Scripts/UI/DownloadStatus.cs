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

		private static IObservable<string> GetDownloadStatus(IReadOnlyDownloadStatus status) =>
			status.Bytes.CombineLatest(
				status.TotalBytes,
				(bytes, totalBytes) => $"{bytes / 1024.0 / 1024.0:0.0} MB of {totalBytes / 1024.0 / 1024.0:0.0} MB"
			);
	}
}
