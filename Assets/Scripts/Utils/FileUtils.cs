using System;
using System.IO;
using UnityEngine;

namespace Utils
{
    public enum PathStatus
    {
        ContainsData,
        Empty,
        DoesNotExist
    }
    public static class FileUtils
    {
        public static void WriteToFile(string fileName, string json)
        {
            string path = GetFilePath(fileName);
            FileStream stream = new FileStream(path, FileMode.Create);

            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(json);
        }

        public static PathStatus ReadFromFile(string fileName, out string retrievedFile)
        {
            retrievedFile = null;
            
            string path = GetFilePath(fileName);
            if (File.Exists(path))
            {
                using StreamReader reader = new StreamReader(path);
                retrievedFile = reader.ReadToEnd();
            }

            PathStatus pathStatus = retrievedFile == null ? PathStatus.DoesNotExist :
                retrievedFile.Length == 0 ? PathStatus.Empty : PathStatus.ContainsData;

            return pathStatus;
        }

        public static string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}
