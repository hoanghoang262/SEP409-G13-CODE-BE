//using ForumService.API.GrpcServices;
//using ForumService.API.Models;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace ForumService.API.Fearture.Queries
//{
//    public class GetAllPostByUserId : IRequest<IActionResult>
//    {
//        public int UserId { get; set; }

//        public class GetAllPostByUserIdHandler : IRequestHandler<GetAllPostByUserId, IActionResult>
//        {
//            private readonly ForumContext _context;
//            private readonly GetUserInfoService _service;

//            public GetAllPostByUserIdHandler(ForumContext context, GetUserInfoService service)
//            {
//                _context = context;
//                _service=service;
//            }
//            public async Task<IActionResult> Handle(GetAllPostByUserId request, CancellationToken cancellationToken)
//            {
//                var user = await _service.SendUserId(request.UserId);
//                if(user.Id == 0)
//                {
//                    return new BadRequestObjectResult("Not found");
//                }
//                var query= await _context.F
               
//            }
//        }
//    }
//}
