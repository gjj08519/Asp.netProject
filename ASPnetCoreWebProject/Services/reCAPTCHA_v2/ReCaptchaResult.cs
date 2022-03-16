using Newtonsoft.Json;
using System.Collections.Generic;

namespace PizzaWebsite.Services.reCAPTCHA_v2
{
    public class ReCaptchaResult
    {
        private string m_Success;
        private List<string> m_ErrorCodes;

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }
    }
}
