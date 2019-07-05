using PatchKit.Unity.Patcher;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class AppName : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		var text = GetComponent<TextMeshProUGUI>();

		Patcher.Instance.AppInfo
				.ObserveOnMainThread()
				.Where(app => app.Id != default)
				.Select(app => app.DisplayName)
				.Subscribe(app => text.text = "Updating: " + app);
	}
}
