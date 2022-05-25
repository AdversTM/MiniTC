using System;
using System.IO;

namespace MiniTC.util {
    public class IO {

        public static void CopyFilesRecursively(string source, string target, bool overwrite = false) {
            CopyFilesRecursively(new DirectoryInfo(source), new DirectoryInfo(target), overwrite);
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, bool overwrite = false) {
            Directory.CreateDirectory(target.FullName);
            foreach (var dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name), overwrite);
            foreach (var file in source.GetFiles())
                try {
                    file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
                } catch (Exception e) {
                    // ignored
                }
        }
    }
}