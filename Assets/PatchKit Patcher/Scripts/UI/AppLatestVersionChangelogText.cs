﻿using System.Collections;
using PatchKit.Unity.Patcher.Cancellation;
using PatchKit.Unity.Utilities;
using TMPro;
using UnityEngine.UI;

namespace PatchKit.Unity.UI
{
    public class AppLatestVersionChangelogText : AppCompontent
    {
        public TextMeshProUGUI Text;

        protected override IEnumerator LoadCoroutine()
        {
            yield return Threading.StartThreadCoroutine(() => MainApiConnection.GetAppLatestAppVersion(AppSecret, CancellationToken.Empty),
                response =>
                {
                    Text.text = response.Changelog;
                });
        }

        private void Reset()
        {
            if (Text == null)
            {
                Text = GetComponent<TextMeshProUGUI>();
            }
        }
    }
}