using BepInEx;
using BepInEx.Logging;
using System.Reflection;

namespace AssemblyLoader
{
    public class AssemblyLoader
    {
        private const string DirectoryName = "assembly";
        private const string DllName = "Payload.dll";

        private const string MainMethod = "Main";
        private const string UnloadMethod = "Unload";

        private readonly string assemblyPath;
        private readonly ManualLogSource log;

        private FileSystemWatcher fileSystemWatcher;
        private Assembly previousAssembly;
        private string previousAssemblyHash;

        public AssemblyLoader(ManualLogSource log)
        {
            this.log = log;

            string assemblyDirectory = Path.Combine(Paths.BepInExRootPath, DirectoryName);
            assemblyPath = Path.Combine(assemblyDirectory, DllName);

            fileSystemWatcher = new FileSystemWatcher(assemblyDirectory);
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            fileSystemWatcher.Filter = DllName;
            fileSystemWatcher.Changed += OnAssemblyFileUpdated;
            fileSystemWatcher.Created += OnAssemblyFileUpdated;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Dispose();
            fileSystemWatcher = null;

            InvokeMethod(previousAssembly, UnloadMethod);
            previousAssembly = null;
            previousAssemblyHash = null;
        }

        private void OnAssemblyFileUpdated(object sender, FileSystemEventArgs args)
        {
            if (args.FullPath != assemblyPath)
                return;

            log.LogInfo($"File {Path.GetFileName(args.FullPath)} {args.ChangeType}. Reloading...");
            byte[] bytes = File.ReadAllBytes(assemblyPath);
            string newHash = Utils.Md5FromBytes(bytes);
            if (newHash == previousAssemblyHash)
            {
                log.LogInfo("File not changed.");
                return;
            }

            previousAssemblyHash = newHash;
            InvokeMethod(previousAssembly, UnloadMethod);
            previousAssembly = Assembly.Load(bytes);
            InvokeMethod(previousAssembly, MainMethod, log);
        }

        private void InvokeMethod(Assembly assembly, string methodName, params object[] parameters)
        {
            if (assembly == null)
                return;

            int paramLength = parameters?.Length ?? 0;
            foreach (Type type in assembly.GetTypes())
            {
                MethodInfo method = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == paramLength);

                if (method == null)
                    continue;

                log.LogInfo($"Running method {method.Name}");
                method.Invoke(null, parameters);
                break;
            }
        }
    }
}
