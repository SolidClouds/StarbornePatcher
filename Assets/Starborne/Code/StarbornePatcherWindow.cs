using System;
using System.IO;
using System.Linq;
using PatchKit.Unity.Patcher;
using PatchKit.Unity.Patcher.AppUpdater.Status;
using PatchKit.Unity.Utilities;
using StarbornePipes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StarbornePatcherWindow : MonoBehaviour
{
	public int width;
	public int height;

	public Button CloseButton;

	private void Awake()
	{
		var w = width * Screen.dpi / 100f;
		var h = height * Screen.dpi / 100f;

		Screen.SetResolution((int)w, (int)h, false);

		var patcher = Patcher.Instance;

		var updatesOnly = Environment.GetCommandLineArgs().Any(arg => arg.Equals("--updateOnly", StringComparison.OrdinalIgnoreCase));

		if (updatesOnly)
			Application.targetFrameRate = 10;
		else
			Application.targetFrameRate = 60;

		CloseButton.gameObject.SetActive(!updatesOnly);

		patcher.CanStartApp
			.ObserveOnMainThread()
			.Subscribe(canStartApp =>
			{
				if (canStartApp)
				{
					LauncherRelay.Send(PatcherMessageType.AppCanStart);
					if (updatesOnly)
						patcher.Quit();
				}
			})
			.AddTo(this);

		patcher.UpdaterStatus
			.SelectSwitchOrNull(u => u.LatestActiveOperation)
			.Select(s => s as IReadOnlyDownloadStatus)
			.SelectSwitchOrDefault(status => status.Bytes.CombineLatest(status.TotalBytes, status.BytesPerSecond, (b, tb, bps) => new PatcherDownloadStatus { Bytes = b, TotalBytes = tb, BytesPerSecond = bps }), default)
			.ObserveOnMainThread()
			.Subscribe(data => LauncherRelay.Send(PatcherMessageType.DownloadInfo, data))
			.AddTo(this);

		patcher.Warning
			.ObserveOnMainThread()
			.Subscribe(warning => LauncherRelay.Send(PatcherMessageType.Warning, warning))
			.AddTo(this);

		var prog = patcher.UpdaterStatus
			.SelectSwitchOrDefault(s => s.Progress, -1.0);

		patcher.State
			.CombineLatest(prog, (state, progress) => new PatcherProgressBarInfo { State = state, Progress = progress })
			.ObserveOnMainThread()
			.Subscribe(data => LauncherRelay.Send(PatcherMessageType.ProgressBar, data))
			.AddTo(this);

		//Status
		var operationStatus = patcher.UpdaterStatus.SelectSwitchOrNull(s => s.LatestActiveOperation);
		var statusDescription = operationStatus.SelectSwitchOrDefault(s => s.Description, string.Empty);
		var combined = patcher.State
			.CombineLatest(statusDescription, (state, description) => new PatcherStateInfo { State = state, Description = description })
			.ObserveOnMainThread()
			.Subscribe(data => LauncherRelay.Send(PatcherMessageType.PatcherState, data));

		try // Just logging, nothing critical if fails.
		{
			var process = System.Diagnostics.Process.GetCurrentProcess();
			var rootPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(process.MainModule.FileName), ".."));
			Debug.Log($"Patcher Root Path: {rootPath}");
		}
		finally { }
	}

	public void CloseButtonPressed()
	{
		Patcher.Instance.Quit();
	}
}

