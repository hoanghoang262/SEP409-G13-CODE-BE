using ForumService.API.Common.DTO;
using GrpcServices;

namespace CommentService.API.Fearture.Queries
{
    public class GetUserInfomation
    {
        private readonly GetUserPostGrpcService _service;
        
        public GetUserInfomation(GetUserPostGrpcService service)
        {
            _service = service; 
        }
        //public CommentDTO GetInfo(int userId)
        //{


        //}
    }
}
