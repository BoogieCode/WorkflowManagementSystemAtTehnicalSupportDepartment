using licenta.Helpers.Dtos;
using Newtonsoft.Json;
using System.IO;

namespace licenta.Helpers
{
    public static class JsonHelper
    {
        public static SmtpCredentials ReadJson(string pathToJson)
        {
            using (StreamReader r = new StreamReader(pathToJson))
            {
                string json = r.ReadToEnd();
                var smtpCredentials = JsonConvert.DeserializeObject<SmtpCredentials>(json);
                return smtpCredentials;
            }
        }
    }
}