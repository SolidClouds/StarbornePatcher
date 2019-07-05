using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using StarbornePipes;
using UnityEngine;

public static class LauncherRelay
{
	private static readonly string _pipeName;

	private static readonly Thread _relayThread;

	private static readonly SemaphoreSlim _messageCounter = new SemaphoreSlim(0);

	private static readonly ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();

	public static bool IsDoneSendingData => _relayThread == null || _messageCounter.CurrentCount == 0;

	static LauncherRelay()
	{
		Debug.Log("LauncherRelay Constructor Called.");
		var args = Environment.GetCommandLineArgs();
		var i = Array.IndexOf(args, "--pipe");
		if (i >= 0)
		{
			_pipeName = args[i + 1];
			Debug.Log("Starting Launcher RelayThread with pipeName " + _pipeName);
			_relayThread = new Thread(RelayThread);
			_relayThread.Start();
		}
		else
		{
			Debug.Log("Not starting Launcher RelayThread. No pipe provied");
		}
	}

	public static void RelayThread()
	{
		try
		{
			using (var client = new NamedPipeClientStream(
				".",
				_pipeName,
				PipeDirection.Out,
				PipeOptions.WriteThrough))
			{
				Debug.LogWarning("LauncherRelay Connecting");

				client.Connect();

				if (client.IsConnected)
				{
					Debug.LogWarning("LauncherRelay connected!");
					using (var stream = new StreamWriter(client))
					{
						stream.AutoFlush = true;
						while (true)
						{
							_messageCounter.Wait();
							if (_messages.TryDequeue(out var msg))
							{
								//Debug.LogWarning("LauncherRelay Sending: " + msg);
								stream.WriteLine(msg);
							}
						}
					}
				}
				else
				{
					Debug.LogError("LauncherRelay is not connected!");
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	public static void Send(PatcherMessageType type)
	{
		//Debug.Log("LauncherRelay Enqueued: " + type);
		var message = JsonUtility.ToJson((type, ""));
		_messages.Enqueue(message);
		_messageCounter.Release();
	}

	public static void Send<T>(PatcherMessageType type, T data)
	{
		//Debug.Log("LauncherRelay Enqueued: " + (type, data));
		var message = JsonUtility.ToJson((type, JsonUtility.ToJson(data)));
		_messages.Enqueue(message);
		_messageCounter.Release();
	}
}

