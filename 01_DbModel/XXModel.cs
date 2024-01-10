 
namespace _01_DbModel
{
    internal class XXModel
    {
    }

    public class XReturn
    {
        public bool Islem { get; set; }
        public string Fonksiyon { get; set; } = string.Empty;
        public string Kod { get; set; } = string.Empty;
        public string Mesaj { get; set; } = string.Empty;
        public Object Obje { get; set; } = string.Empty;
    }

    public class IpAdres
    {
        public string IpIdentityServer { get; set; } = String.Empty;
        public string IpApi { get; set; } = String.Empty;
        public string IpAdminApp { get; set; } = String.Empty;
        public string IpClientApp { get; set; } = String.Empty;
    }

}
