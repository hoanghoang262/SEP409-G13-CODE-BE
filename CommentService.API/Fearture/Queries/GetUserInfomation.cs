using ForumService.API.Common.DTO;
using GrpcServices;

namespace CommentService.API.Fearture.Queries
{
    public class GetUserInfomation
    {
        private readonly GetUserInfoGrpcService _service;
        
        public GetUserInfomation(GetUserInfoGrpcService service)
        {
            _service = service; 
        }
        //public CommentDTO GetInfo(int userId)
        //{
        //    var

        //}
    }
}
