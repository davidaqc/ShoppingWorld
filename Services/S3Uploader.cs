using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;

public class S3Uploader
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Uploader(IOptions<AWSOptions> awsOptions)
    {
        var options = awsOptions.Value;
        if (options == null)
        {
            throw new ArgumentNullException(nameof(awsOptions), "AWS options are not configured properly.");
        }

        if (string.IsNullOrWhiteSpace(options.Region))
        {
            throw new ArgumentException("AWS region is not configured properly.", nameof(options.Region));
        }

        _s3Client = new AmazonS3Client(options.AccessKey, options.SecretKey, RegionEndpoint.GetBySystemName(options.Region));
        _bucketName = options.BucketName;
    }

    public async Task<string> UploadFileAsync(string filePath, string fileName)
    {
        try
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(filePath, _bucketName, fileName);

            string fileUrl = $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
            return fileUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        try
        {
            var deleteObjectRequest = new Amazon.S3.Model.DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex.Message}");
            return false;
        }
    }

}
