using BepInEx.Logging;
using UnityEngine;

namespace AssemblyLoader
{
    public class Bootstrap : MonoBehaviour
    {
        public static ManualLogSource Log { get; set; }

        private AssemblyLoader assemblyLoader;

        public Bootstrap(IntPtr value) : base(value)
        {
        }

        private void Awake()
        {
            Log.LogInfo("Bootstrap.Awake");
            assemblyLoader = new AssemblyLoader(Log);
        }

        private void OnDestroy()
        {
            Log.LogInfo("Bootstrap.OnDestroy");
            assemblyLoader.Dispose();
            assemblyLoader = null;
        }
    }
}
