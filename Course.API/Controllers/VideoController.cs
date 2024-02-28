using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
    //    private readonly Cloudinary _cloudinary;
    //    private readonly CloudinaryService _cloudinaryService;

    //    public VideoController(CloudinaryService cloudinaryService)
    //    {
    //        // Cấu hình Cloudinary với thông tin API của bạn
    //        var account = new Account(
    //            "dcduktpij",     // Thay thế bằng tên cloud của bạn
    //            "592561579458269",        // Thay thế bằng khóa API của bạn
    //            "rriM4lqd8uNQ9FtUd11NjTq50ac");    // Thay thế bằng mã bí mật API của bạn

    //        // Khởi tạo đối tượng Cloudinary bằng tài khoản của bạn
    //        _cloudinary = new Cloudinary(account);
    //        _cloudinaryService = cloudinaryService;
    //    }

    //    [HttpPost("upload-video")]
        
    //        public async Task<IActionResult> UploadVideo(IFormFile videoFile)
    //        {
    //            try
    //            {
    //                string videoUrl = await _cloudinaryService.UploadVideoasync(videoFile);

    //                // Xử lý kết quả, trả về thông báo, URL, hoặc thông tin khác
    //                return Ok(new { VideoUrl = videoUrl });
    //            }
    //            catch (Exception ex)
    //            {
    //                // Xử lý lỗi và trả về lỗi
    //                return BadRequest(new { ErrorMessage = ex.Message });
    //            }

    //        }
        
      }
}
