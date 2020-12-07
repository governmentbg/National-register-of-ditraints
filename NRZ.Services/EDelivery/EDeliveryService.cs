using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.EDelivery
{
    public static class EDeliveryService
    {
        static BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);

        //https://edelivery.egov.bg/Services/EDeliveryIntegrationService.svc
        //https://edelivery-test.egov.bg/Services/EDeliveryIntegrationService.svc
        static EDeliveryServiceReference.EDeliveryIntegrationServiceClient client = new EDeliveryServiceReference.EDeliveryIntegrationServiceClient(binding, new EndpointAddress("https://edelivery.egov.bg/Services/EDeliveryIntegrationService.svc"));

        static EDeliveryService()
        {
            
            binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;

            client.ConfigureEndpoint(client.Endpoint);
            client.ClientCredentials.ClientCertificate.SetCertificate(
                            StoreLocation.LocalMachine,
                            StoreName.My,
                            X509FindType.FindByThumbprint,
                            "7f15b06b2641ad8635f9968d5a4b76c20ecbc1a2");
        }

        public static async Task<EDeliveryServiceReference.DcPersonRegistrationInfo> CheckPersonHasRegistration(string personEGN)
        {
            return await client.CheckPersonHasRegistrationAsync(personEGN);
        }

        public static async Task<EDeliveryServiceReference.DcInstitutionInfo[]> GetRegisteredInstitutions()
        {
            return await client.GetRegisteredInstitutionsAsync();
        }

        public static async Task<EDeliveryServiceReference.DcMessageDetails> GetSentDocumentStatusByRegNum(string documentRegistrationNumber, string operatorEGN)
        {
            return await client.GetSentDocumentStatusByRegNumAsync(documentRegistrationNumber, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcMessageDetails> GetSentMessageStatus(int messageId, string operatorEGN)
        {
            return await client.GetSentMessageStatusAsync(messageId, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcDocument> GetSentDocumentContentByRegNum(string documentRegistrationNumber, string operatorEGN)
        {
            return await client.GetSentDocumentContentByRegNumAsync(documentRegistrationNumber, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcDocument> GetSentDocumentContent(int documentId, string operatorEGN)
        {
            return await client.GetSentDocumentContentAsync(documentId, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcDocument[]> GetSentDocumentsContent(int documentId, string operatorEGN)
        {
            return await client.GetSentDocumentsContentAsync(documentId, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcMessage[]> GetSentMessagesList(string operatorEGN)
        {
            return await client.GetSentMessagesListAsync(operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcPartialListOfDcMessageHR29gRRX> GetSentMessagesListPaged(int pageNumber, int pageSize, string operatorEGN)
        {
            return await client.GetSentMessagesListPagedAsync(pageNumber, pageSize, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcMessage[]> GetReceivedMessagesList(bool onlyNew, string operatorEGN)
        {
            return await client.GetReceivedMessagesListAsync(onlyNew, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcPartialListOfDcMessageHR29gRRX> GetReceivedMessagesListPaged(bool onlyNew, int pageNumber, int pageSize, string operatorEGN)
        {
            return await client.GetReceivedMessagesListPagedAsync(onlyNew, pageNumber, pageSize, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcMessageDetails> GetReceivedMessageContent(int messageId, string operatorEGN)
        {
            return await client.GetReceivedMessageContentAsync(messageId, operatorEGN);
        }

        public static async Task<EDeliveryServiceReference.DcSubjectInfo> GetSubjectInfo(Guid electronicSubjectId, string operatorEGN)
        {
            return await client.GetSubjectInfoAsync(electronicSubjectId, operatorEGN);
        }

        public static async Task<int> SendElectronicDocument(string subject, byte[] docBytes, string docNameWithExtension, string docRegNumber, EDeliveryServiceReference.eProfileType receiverType, string receiverUniqueIdentifier, string receiverPhone, string receiverEmail, string serviceOID, string operatorEGN)
        {
            return await client.SendElectronicDocumentAsync(subject, docBytes, docNameWithExtension, docRegNumber, receiverType, receiverUniqueIdentifier, receiverPhone, receiverEmail, serviceOID, operatorEGN);
        }

        public static async Task<int> SendElectronicDocumentOnBehalfOf(string subject, byte[] docBytes, string docNameWithExtension, string docRegNumber, EDeliveryServiceReference.eProfileType senderType, string senderUniqueIdentifier, string senderPhone, string senderEmail, string senderFirstName, string senderLastName, EDeliveryServiceReference.eProfileType receiverType, string receiverUniqueIdentifier, string serviceOID, string operatorEGN)
        {
            return await client.SendElectronicDocumentOnBehalfOfAsync(subject, docBytes, docNameWithExtension, docRegNumber, senderType, senderUniqueIdentifier, senderPhone, senderEmail, senderFirstName, senderLastName, receiverType, receiverUniqueIdentifier, serviceOID, operatorEGN);
        }

        public static async Task<int> SendMessage(EDeliveryServiceReference.DcMessageDetails message, EDeliveryServiceReference.eProfileType receiverType, string receiverUniqueIdentifier, string receiverPhone, string receiverEmail, string serviceOID, string operatorEGN)
        {
            return await client.SendMessageAsync(message, receiverType, receiverUniqueIdentifier, receiverPhone, receiverEmail, serviceOID, operatorEGN);
        }

        public static async Task<int> SendMessageOnBehalfOf(EDeliveryServiceReference.DcMessageDetails message, EDeliveryServiceReference.eProfileType senderType, string senderUniqueIdentifier, string senderPhone, string senderEmail, string senderFirstName, string senderLastName, EDeliveryServiceReference.eProfileType receiverType, string receiverUniqueIdentifier, string serviceOID, string operatorEGN)
        {
            return await client.SendMessageOnBehalfOfAsync(message, senderType, senderUniqueIdentifier, senderPhone, senderEmail, senderFirstName, senderLastName, receiverType, receiverUniqueIdentifier, serviceOID, operatorEGN);
        }

        public static async Task<int> SendMessageInReplyTo(EDeliveryServiceReference.DcMessageDetails message, int replayToMessageId, string serviceOID, string operatorEGN)
        {
            return await client.SendMessageInReplyToAsync(message, replayToMessageId, serviceOID, operatorEGN);
        }


    }
}
