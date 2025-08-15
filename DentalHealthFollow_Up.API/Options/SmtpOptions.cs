namespace DentalHealthFollow_Up.API.Options
{
    public class SmtpOptions
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } 
        public bool EnableSsl { get; set; } 
        public string User { get; set; } = "";
        public string Password { get; set; } = "";
        public string FromName { get; set; } = "";
        public string FromAddress { get; set; } = "";
        public string DisplayName { get; set; } = "Dental Sağlık";
    }
}

