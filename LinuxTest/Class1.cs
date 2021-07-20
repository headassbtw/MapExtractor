using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Newtonsoft.Json;

namespace MapExtractor
{
    public class Class1
    {
        public static string ExecutablePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static string SettingsJsonPath = Path.Combine(ExecutablePath, "settings.json");

        public static Settings settings;
        public static void Main(string[] args)
        {
            if (!File.Exists(Path.Combine(ExecutablePath, "settings.json")))
            {
                File.Create(SettingsJsonPath).Close();
                JsonReadWrite.SaveJson<Settings>(settings, SettingsJsonPath);
            }
            settings = JsonReadWrite.LoadJson<Settings>(SettingsJsonPath);
            if (settings == null)
                settings = new Settings();
            string CustomSongsFolder = Path.Combine(settings.BeatSaberDirectory, "Beat Saber_Data", "CustomLevels");

            if (settings.BeatSaberDirectory.Length.Equals(0))
            {
                Console.WriteLine("Beat Saber directory not set, please enter it here:");
                settings.BeatSaberDirectory = Console.ReadLine();
                JsonReadWrite.SaveJson(settings, SettingsJsonPath);
            }

            if (Directory.Exists(Path.Combine(CustomSongsFolder, Path.GetFileNameWithoutExtension(args[0]))))
            {
                Console.WriteLine("Map is already in custom songs folder");
                Console.ReadKey();
                return;
            }

            string destination = Path.Combine(CustomSongsFolder, Path.GetFileNameWithoutExtension(args[0]));
            try
            {
                ZipFile.ExtractToDirectory(args[0], destination);
            }
            catch (Exception)
            {
                Console.WriteLine("Zip was not a valid custom song, didn't copy");
                Console.ReadKey();
            }
            if (!File.Exists(Path.Combine(destination, "info.dat")))
            {
                Directory.Delete(destination, true);
                Console.WriteLine("Zip was not a valid custom song, didn't copy");
                Console.ReadKey();
            }
            JsonReadWrite.SaveJson(settings, SettingsJsonPath);
        }
    }
}