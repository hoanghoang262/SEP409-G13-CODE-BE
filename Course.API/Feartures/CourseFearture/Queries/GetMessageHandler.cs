


using EventBus.Message.IntegrationEvent.Event;
using MediatR;

using System.Globalization;

namespace CourseService.API.Feartures.CourseFearture.Queries
{

    public class GetMessageHandler : IRequestHandler<MessageCommand, LoginEvent>
    {

        public async Task<LoginEvent> Handle(MessageCommand request, CancellationToken cancellation)
        {
            if (request == null)
            {
                return null;
            }
            LoginEvent loginModels = new LoginEvent();

            // Giả sử rằng bạn lấy dữ liệu từ một nguồn nào đó, ví dụ: cơ sở dữ liệu, API,...
            // Đây là một ví dụ giả định:
            loginModels.UserName = request.UserName;
            loginModels.PassWord = request.PassWord;

            return loginModels;

        }



    }
}
