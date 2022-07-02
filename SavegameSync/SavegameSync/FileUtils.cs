using System;
using System.IO;
using System.Linq;

namespace SavegameSync
{
    public static class FileUtils
    {
        /*
         * Based on this MSDN example: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories.
         */
        public static void CopyDirectory(string originalDir, string destDir, string[] filter = null)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(originalDir);
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            string searchPattern = ProcessFilter(filter);

            FileInfo[] files = dirInfo.GetFiles(searchPattern);
            foreach (FileInfo file in files)
            {
                string destFilePath = Path.Combine(destDir, file.Name);
                file.CopyTo(destFilePath);
            }

            DirectoryInfo[] subdirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo subdir in subdirs)
            {
                string destSubdirPath = Path.Combine(destDir, subdir.Name);
                CopyDirectory(subdir.FullName, destSubdirPath, filter);
            }
        }

        /// <summary>
        /// Get the latest LastWriteTime of any file in the given directory or its subdirectories.
        /// </summary>
        public static DateTime GetLatestFileWriteTime(string dir, string[] filter = null)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            DateTime latestFileWriteTime = new DateTime(1900, 1, 1);

            string searchPattern = ProcessFilter(filter);

            FileInfo[] files = dirInfo.GetFiles(searchPattern);
            foreach (FileInfo file in files)
            {
                DateTime curDateTime = file.LastWriteTimeUtc;
                if (curDateTime > latestFileWriteTime)
                {
                    latestFileWriteTime = curDateTime;
                }
            }

            DirectoryInfo[] subDirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                DateTime curDateTime = GetLatestFileWriteTime(subDir.FullName, filter);
                if (curDateTime > latestFileWriteTime)
                {
                    latestFileWriteTime = curDateTime;
                }
            }

            return latestFileWriteTime;
        }

        public static void DeleteIfExists(string path, string[] filter = null)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else if (Directory.Exists(path))
            {
                if (filter == null)
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    string searchPattern = ProcessFilter(filter);

                    string[] filesToDelete = Directory.EnumerateFiles(path, searchPattern).ToArray();

                    foreach (string file in filesToDelete)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        private static string ProcessFilter(string[] filter)
        {
            string searchPattern = "";

            if (filter != null)
            {
                foreach (string s in filter)
                {
                    if (s.StartsWith("."))
                        searchPattern += "*" + s;
                    else
                        searchPattern += s;

                    searchPattern += "|";
                }

                searchPattern = searchPattern.Trim('|');
            }
            else
            {
                searchPattern = "*.*";
            }

            return searchPattern;
        }
    }
}
