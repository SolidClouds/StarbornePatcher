using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PatchKit.Unity.Patcher.UI
{
	public class WarningText : MonoBehaviour
	{
		private void Start()
		{
			var warningText = GetComponent<TextMeshProUGUI>();
			Patcher.Instance.Warning.ObserveOnMainThread().Subscribe(warning =>
			{
				warningText.text = warning;
			}).AddTo(this);
		}
	}
}
