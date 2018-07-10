namespace PatchKit.Patching.Unity.Debug
{
    public class UnitySystemInfoProvider : ISystemInfoProvider
    {
        public string SystemInfo => UnityEngine.SystemInfo.operatingSystem;
    }
}