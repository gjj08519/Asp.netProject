namespace PizzaWebsite.Services.SendGrid
{
    public class SendGridEmailSenderOptions
    {
        public string ApiKey { get; set; }

        public string SenderEmail { get; set; }

        public string SenderName { get; set; }

        public string CompanyEmail { get; set; }
    }
}
