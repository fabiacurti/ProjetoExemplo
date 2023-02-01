namespace Data.Configuration
{
    public class SmtpEmailConfig
    {
        public bool Authentication { get; set; }
        public string UserCredential { get; set; }
        public string PassCredential { get; set; }
        public string SmtpClient { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public string Nickname { get; set; }
    }
}
