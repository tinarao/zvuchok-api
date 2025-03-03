using api.Models;

namespace api.Utils
{
    public class Utilities
    {
        public enum AvailabilityStatus
        {
            Public,
            OnModeration,
            Rejected,
            OnAnalysis
        }

        public static string NormalizeString(string str)
        {
            return str.ToLower();
        }

        public async static void SendSampleToAnalysisService(Sample sample)
        {
            var client = new HttpClient();
            var formdata = new MultipartFormDataContent();

            client.BaseAddress = new Uri("http://localhost:4200");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "storage");
            var sampleFilePath = Path.Combine(uploadsFolder, sample.SampleFilePath);

            var audioStream = new FileStream(sampleFilePath, FileMode.Open);
            var audioContent = new StreamContent(audioStream);
            audioContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data");

            formdata.Add(audioContent, "file", sample.Name);
            formdata.Add(new StringContent($"http://localhost:5129/api/sample/metadata/{sample.UpdateMetadataToken}"), "callback_api_url");

            var response = await client.PostAsync("/generate", formdata);
            response.EnsureSuccessStatusCode();
        }
    }
}