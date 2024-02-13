

namespace CourseService
{
    public class CloudinaryService
    {
    //    private readonly Cloudinary _cloudinary;

    //    public CloudinaryService(string cloudName, string apiKey, string apiSecret)
    //    {
    //        Account account = new Account(cloudName, apiKey, apiSecret);
    //        _cloudinary = new Cloudinary(account) { Api = { Secure = true } };

    //    }

    //    public async Task<string> UploadImageasync(IFormFile imageFile)
    //    {
    //        if (imageFile?.Length > 0)
    //        {
    //            var uploadParams = new ImageUploadParams
    //            {
    //                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
    //                // Các cài đặt khác nếu cần
    //            };

    //            var uploadResult = await _cloudinary.Uploadasync(uploadParams);
    //            return uploadResult.SecureUrl?.AbsoluteUri;
    //        }

    //        return null;
    //    }
    //     public async Task<string> UploadVideoasync(IFormFile videoFile)
    //   {
    //    if (videoFile?.Length > 0)
    //    {
    //        var uploadParams = new VideoUploadParams
    //        {
    //            File = new FileDescription(videoFile.Name, videoFile.OpenReadStream())
    //            // Các cài đặt khác nếu cần, ví dụ: Resource Type, Public ID
    //        };

    //        var uploadResult = await _cloudinary.Uploadasync(uploadParams);
    //        return uploadResult.SecureUrl?.AbsoluteUri;
    //    }

    //    return null;
    //}
    }
}
