namespace NRZ.Models.Settings
{
    public class EPaymentSettings
    {
        public bool UseTestEnv { get; set; }
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public string Url { get; set; }
        public string SendJsonPaymentRequestUri { get; set; }
        public string TestEnvClientId { get; set; }
        public string TestEnvSecretKey { get; set; }
        public string TestEnvUrl { get; set; }
        public string AdministrativeServiceSupplierUri { get; set; }
        public string AdministrativeServiceNotificationUrl { get; set; }
    }
}
