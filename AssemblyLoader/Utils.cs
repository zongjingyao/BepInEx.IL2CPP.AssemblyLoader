using Il2CppSystem.Security.Cryptography;
using Il2CppSystem.Text;

namespace AssemblyLoader
{
    public static class Utils
    {
        public static string Md5FromBytes(byte[] bytes)
        {
            if (bytes == null)
                return null;

            MD5 md5 = MD5.Create();
            StringBuilder sb = new StringBuilder();
            foreach (byte b in md5.ComputeHash(bytes))
                sb.Append(b.ToString("x2"));
            md5.Dispose();

            return sb.ToString();
        }
    }
}
