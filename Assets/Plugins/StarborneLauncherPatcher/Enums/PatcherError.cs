namespace PatchKit.Unity.Patcher
{
	public enum PatcherError
	{
		NoInternetConnection,
		NoPermissions,
		NotEnoughDiskSpace,
		NonLauncherExecution,
		Other
	}
	public class PatcherErrorMessage
	{
		public string Message { get; private set; }
		public PatcherError PatcherError { get; private set; }

		public PatcherErrorMessage(string message, PatcherError patcherError)
		{
			Message = message;
			PatcherError = patcherError;
		}

		public static PatcherErrorMessage NoInternetConnection()
		{
			return new PatcherErrorMessage("Please check your internet connection.", PatcherError.NoInternetConnection);
		}
		public static PatcherErrorMessage NoPermissions()
		{
			return new PatcherErrorMessage("Please check write permissions in application's directory.", PatcherError.NoPermissions);
		}

		public static PatcherErrorMessage NotEnoughDiskSpace(long additionalBytesRequired)
		{
			return new PatcherErrorMessage(
				string.Format("Not enough disk space to install this application. Additional {0:0.00} GB of disk space is required.", additionalBytesRequired / (1024 * 1024 * 1024.0)),
				PatcherError.NotEnoughDiskSpace
			);
		}
		public static PatcherErrorMessage NonLauncherExecution()
		{
			return new PatcherErrorMessage("Patcher has to be started using the launcher.", PatcherError.NonLauncherExecution);
		}
		public static PatcherErrorMessage Other()
		{
			return new PatcherErrorMessage("Unknown error, please try again.", PatcherError.Other);
		}
	}
}
