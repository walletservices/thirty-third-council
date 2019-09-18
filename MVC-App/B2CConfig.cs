

namespace MVC_App
{
    public class B2CConfig
    {
        public const string PolicyAuthenticationProperty = "Policy";

        public B2CConfig()
        {
            B2CConfigInstance = "login.microsoftonline.com/tfp";
        }

        public string ClientId { get; set; }
        public string B2CConfigInstance { get; set; }
        public string Tenant { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public string SignInPolicyId { get; set; }
        public string SignUpPolicyId { get; set; }
        public string ResetPasswordPolicyId { get; set; }
        public string EditProfilePolicyId { get; set; }
        public string RedirectUri { get; set; }

        public string DefaultPolicy => SignUpSignInPolicyId;
        public string Authority => $"https://{B2CConfigInstance}/{Tenant}/{DefaultPolicy}/v2.0";

        public string ClientSecret { get; set; }
        public string ApiUrl { get; set; }
        public string ApiScopes { get; set; }
    }
}
