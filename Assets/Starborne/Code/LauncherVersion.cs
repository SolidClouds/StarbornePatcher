using PatchKit.Unity.Patcher;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LauncherVersion : MonoBehaviour
{
	public TextMeshProUGUI Text;

	// Use this for initialization
	void Start()
	{
		Text.text = "Launcher version: " + Version.Value;
	}
}
