using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class RecaptchaValidator
    {
        private const string RecaptchaApiUrl = "https://www.google.com/recaptcha/api/siteverify";
        private readonly string secretKey;

        public RecaptchaValidator(string secretKey)
        {
            this.secretKey = secretKey;
        }

        public async Task<bool> ValidateAsync(string responseToken)
        {
            using (var client = new HttpClient())
            {
                var parameters = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("secret", secretKey),
                new KeyValuePair<string, string>("response", responseToken)
            });

                var response = await client.PostAsync(RecaptchaApiUrl, parameters);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<RecaptchaResponse>(responseContent);
                    return result.Success;
                }

                return false;
            }
        }

    }
}
