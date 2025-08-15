﻿namespace DentalHealthFollow_Up.API.Options
{
    public class SmtpOptions
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string From { get; set; } = "";
        public string DisplayName { get; set; } = "Dental Sağlık";
    }
}

