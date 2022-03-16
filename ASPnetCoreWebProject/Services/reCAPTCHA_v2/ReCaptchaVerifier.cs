using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace PizzaWebsite.Services.reCAPTCHA_v2
{
    public interface IReCaptchaVerifier
    {
        /// <summary>
        /// Retrieves the reCAPTCHA site key.
        /// </summary>
        /// <returns>The reCAPTCHA site key.</returns>
        public string GetSiteKey();

        /// <summary>
        /// Validates the given encoded reCAPTCHA response.
        /// </summary>
        /// <param name="encodedReCaptchaResponse">The encoded reCAPTCHA response.</param>
        /// <returns>true if valid, false otherwise.</returns>
        public bool Validate(string encodedReCaptchaResponse);
    }

    public class ReCaptchaVerifier : IReCaptchaVerifier
    {
        public ReCaptchaVerifier(IOptions<ReCaptchaOptions> options)
        {
            Options = options.Value;
        }

        private ReCaptchaOptions Options { get; set; }

        public string GetSiteKey() { return Options.SiteKey; }

        public bool Validate(string encodedReCaptchaResponse)
        {
            var client = new System.Net.WebClient();

            string privateKey = Options.SecretKey;

            var googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", privateKey, encodedReCaptchaResponse));

            var captchaResponse = JsonConvert.DeserializeObject<ReCaptchaResult>(googleReply);

            return captchaResponse.Success.ToLower() == "true";
        }
    }
}
