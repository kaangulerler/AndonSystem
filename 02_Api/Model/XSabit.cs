using Newtonsoft.Json;

namespace _02_Api.Model
{
    public class XSabit
    {
        public static string XSerialize(object obje)
        {
            var dönen = JsonConvert.SerializeObject(obje, Formatting.Indented,
                                                          new JsonSerializerSettings
                                                                        {
                                                                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                        });

            return dönen.ToString();
        }
    }

    public class IpAdres
    {
        public string IpIdentityServer { get; set; } = String.Empty;
        public string IpApi { get; set; } = String.Empty;
        public string IpAdminApp { get; set; } = String.Empty;
        public string IpClientApp { get; set; } = String.Empty;
    }
}
