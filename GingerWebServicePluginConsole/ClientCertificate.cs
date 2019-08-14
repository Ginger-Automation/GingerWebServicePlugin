using System;

namespace GingerWebServicePluginConsole
{
    public class ClientCertificate
    {
        public String CertificateFilePath { get; set; }
        public String CertificatePassword { get; set; }

        public ClientCertificate(string certificateFilePath, string certificatePassword)
        {
            this.CertificateFilePath = certificateFilePath;
            this.CertificatePassword = certificatePassword;
        }
    }

}
