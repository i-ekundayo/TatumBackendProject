namespace TatumBackendProject.Auth
{
    public class EmailSettings
    {
        /// <summary>
        /// Email provider
        /// "Smtp" or "SendGrid"
        /// </summary>
        public string Provider { get; set; } = "Smtp";

        /// <summary>
        /// Email address that appears as the sender.
        /// </summary>
        public string FromEmail { get; set; } = null!;

        public string FromName { get; set; } = null!;

        /// <summary>
        /// Frontend RL used for password setup.
        /// Example:
        /// https://app.tatumconnect.com
        /// </summary>
        public string FrontendBaseUrl { get; set; } = null!;
        public SmtpSettings Smtp { get; set; } = new();
        public SendGridSettings SendGrid { get; set; } = new();
    }

    public class SmtpSettings
    {
        public string Host { get; set;  } = null!;
        public int Port { get; set; } = 587;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool EnableSsl { get; set; } = true;
    }
    
    public class SendGridSettings
    {
        public string ApiKey { get; set; } = null;
        public string ApiUrl { get; set; } = "https://api.sendgrid.com/v3/mail/send";
    }
}
