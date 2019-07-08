using PatchKit.Unity.Patcher.Cancellation;
using PatchKit.Unity.Utilities;
using TMPro;
using UnityEngine.UI;

namespace PatchKit.Unity.Patcher.UI.Dialogs
{
    public class ErrorDialog : Dialog<ErrorDialog>
    {
        public TextMeshProUGUI ErrorText;

        public void Confirm()
        {
            OnDisplayed();
        }

        public void Display(PatcherErrorMessage error, CancellationToken cancellationToken)
        {
            UnityDispatcher.Invoke(() => UpdateMessage(error)).WaitOne();

            Display(cancellationToken);
        }

        private void UpdateMessage(PatcherErrorMessage error)
        {
            ErrorText.text = error.Message;
        }
    }
}
