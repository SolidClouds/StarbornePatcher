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
		public string Message { get; }
		public PatcherError PatcherError { get; }

		public PatcherErrorMessage(string message, PatcherError patcherError)
		{
			Message = message;
			PatcherError = patcherError;
		}

		public static PatcherErrorMessage NoInternetConnection() => new PatcherErrorMessage("Please check your internet connection.", PatcherError.NoInternetConnection);

		public static PatcherErrorMessage NoPermissions() => new PatcherErrorMessage("Please check write permissions in application's directory.", PatcherError.NoPermissions);

		public static PatcherErrorMessage NotEnoughDiskSpace(long additionalBytesRequired)
			=> new PatcherErrorMessage(
				$"Not enough disk space to install this application. Additional {additionalBytesRequired / (1024 * 1024 * 1024.0):0.00} GB of disk space is required.",
				PatcherError.NotEnoughDiskSpace
			);

		public static PatcherErrorMessage NonLauncherExecution() => new PatcherErrorMessage("Patcher has to be started using the launcher.", PatcherError.NonLauncherExecution);

		public static PatcherErrorMessage Other() => new PatcherErrorMessage("Unknown error, please try again.", PatcherError.Other);
	}
}