using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RegiXServiceReference;

namespace NRZ.RegiX.Client
{
    public static class RegiXUtility
    {
        public static async Task<RegiXResponse> CallAsync(RegiXEntryPointClient client, ServiceRequestData request)
        {
            Guid callId;
            //string rawResponse;
            ServiceResultData result;

            RawMessage rawRequestMessage = new RawMessage();
            RawMessage rawResponseMessage = new RawMessage();

            try
            {
                callId = RegiXMessageInspector.BeforeCall();

                if (client == null)
                    client = new RegiXEntryPointClient();
                await client.OpenAsync();
                result = await client.ExecuteSynchronousAsync(request);
                await client.CloseAsync();
                
            }
            catch (Exception ex)
            {
                client.Abort();
                throw ex;
            }
            finally
            {
                //rawResponse = RegiXMessageInspector.AfterCall(callId);
                RegiXMessageInspector.AfterCallAll(callId, ref rawRequestMessage, ref rawResponseMessage);
            }

            return new RegiXResponse(result, rawRequestMessage, rawResponseMessage);
        }



    }
}
