using BepInEx;
using BepInEx.Unity.IL2CPP;

namespace AssemblyLoader
{
    [BepInPlugin("BepInEx.IL2CPP.AssemblyLoader", "AssemblyLoader", "1.0.0")]
    public class BepInExLoader : BasePlugin
    {
        public override void Load()
        {
            Log.LogInfo($"Plugin AssemblyLoader is loaded!");

            Bootstrap.Log = Log;
            AddComponent<Bootstrap>();
        }
    }
}
