// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using BepInEx.Logging;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Payload
{
    private static ManualLogSource log;

    // UnityEngine.Debug causes System.AccessViolationException exception, inject ManualLogSource as a workaround
    public static void Main(ManualLogSource log)
    {
        log.LogInfo("Payload.Main invoked.");
        Payload.log = log;
        DumpActiveSceneRootGameObjects();
    }

    public static void Unload()
    {
        // Unload and unpatch everything before reloading the script
        log.LogInfo("Payload.Unload invoked.");
    }

    private static void DumpActiveSceneRootGameObjects()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        Il2CppReferenceArray<GameObject> objects = activeScene.GetRootGameObjects();
        log.LogInfo("////////////////////////////");
        log.LogInfo($"Active scene {activeScene.name} with {objects.Count} root objects.");
        for (int i = 0; i < objects.Count; i++)
        {
            GameObject go = objects[i];
            Il2CppArrayBase<Component> components = go.GetComponents<Component>();
            log.LogInfo($"{i}. {go.name} with {components.Count} components.");
            for (int j = 0; j < components.Count; j++)
            {
                Component component = components[j];
                log.LogInfo($"  {j}. {component.GetIl2CppType().FullName}");
            }
            log.LogInfo("////////////////////////////");
        }
    }
}
