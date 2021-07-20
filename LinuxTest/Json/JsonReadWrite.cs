using System.IO;
using Newtonsoft.Json;

namespace MapExtractor
{
    public class JsonReadWrite
    {
        public static void SaveJson<T>(T data, string path)
        {
            using (StreamWriter w = new StreamWriter(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                string contents = JsonConvert.SerializeObject(data);
                var jtw = new JsonTextWriter(w);
                jtw.Formatting = Formatting.Indented;
                serializer.Serialize(jtw, data);
                w.Close();
            }
        }
        public static T LoadJson<T>(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                T items = JsonConvert.DeserializeObject<T>(json);
                r.Close();
                return items;
            }
        }
    }
}