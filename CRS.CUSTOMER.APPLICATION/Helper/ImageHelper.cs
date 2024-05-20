using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CRS.CUSTOMER.APPLICATION.Helper
{
    public class ImageHelper
    {
        private static HttpClient httpClient = new HttpClient();

        private static AmazonS3Configruation _AmazonS3Configruation = ApplicationUtilities.GetAppDataJsonConfigValue<AmazonConfigruation>("AmazonConfigruation").AmazonS3Configruation;

        public static async Task<bool> ImageUpload(string fileName, HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                    return false;
                var amazonS3Client = new AmazonS3Client(_AmazonS3Configruation.AccessKeyId, _AmazonS3Configruation.SecretAccessKey, RegionEndpoint.APNortheast1);
                var transferUtility = new TransferUtility(amazonS3Client);
                await transferUtility.UploadAsync(file.InputStream, _AmazonS3Configruation.BucketName, fileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> ImageUpload(string fileName, string fileFullLocalDirectoryPath)
        {
            var generatePresignedURLResponse = await GeneratePresignedURL(fileName, fileFullLocalDirectoryPath);
            return generatePresignedURLResponse.Item1;
        }

        public static string ProcessedImage(string imageURL, bool useDefaultImage = false)
        {
            string response = $"{_AmazonS3Configruation.BaseURL}/{_AmazonS3Configruation.BucketName}/{_AmazonS3Configruation.NoImageURL.TrimStart('/')}";
            if (!useDefaultImage && !string.IsNullOrEmpty(imageURL))
                if (imageURL.ToLower().Contains("/content/userupload"))
                    response = $"{ConfigurationManager.AppSettings["ImageVirtualPath"].ToString()}/{imageURL.TrimStart('/')}";
                else
                    response = $"{_AmazonS3Configruation.BaseURL}/{_AmazonS3Configruation.BucketName}/{imageURL.TrimStart('/')}";
            return response;
        }

        private static async Task<Tuple<bool, string>> GeneratePresignedURL(string ObjectKey, string filePath)
        {
            AWSConfigsS3.UseSignatureVersion4 = true;
            IAmazonS3 s3Client = new AmazonS3Client(_AmazonS3Configruation.AccessKeyId, _AmazonS3Configruation.SecretAccessKey, RegionEndpoint.APNortheast1);
            try
            {
                string urlString = string.Empty;
                var request = new GetPreSignedUrlRequest
                {
                    Verb = HttpVerb.PUT,
                    BucketName = _AmazonS3Configruation.BucketName,
                    Key = ObjectKey,
                    Expires = DateTime.UtcNow.AddMinutes(_AmazonS3Configruation.TimeoutDurationInMin)
                };
                urlString = s3Client.GetPreSignedURL(request);
                var uploadResponse = await UploadObject(urlString, filePath);
                if (!uploadResponse)
                    return new Tuple<bool, string>(false, "Failed");
                return new Tuple<bool, string>(true, "Success");
            }
            catch (AmazonS3Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        private static async Task<bool> UploadObject(string url, string filePath)
        {
            var streamContent = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read));
            var response = await httpClient.PutAsync(url, streamContent);
            return response.IsSuccessStatusCode;
        }
    }
}