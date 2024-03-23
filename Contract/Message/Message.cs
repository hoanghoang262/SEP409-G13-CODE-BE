namespace Contract.Service.Message
{
    public static class Message
    {
        /// <summary>
        /// MSG01: Tài khoản không tồn tại
        /// </summary>
        public static readonly MessageModel MSG01 = new MessageModel
        {
            MsgCode = "MSG01",
            MsgTextVN = "Tài khoản không tồn tại",
            MsgTextEN = "Account does not exist"
        };

        /// <summary>
        /// MSG02: Đăng nhập thành công
        /// </summary>
        public static readonly MessageModel MSG02 = new MessageModel
        {
            MsgCode = "MSG02",
            MsgTextVN = "Đăng nhập thành công",
            MsgTextEN = "Login successfully"
        };

        /// <summary>
        /// MSG03: Email này chưa được xác nhận
        /// </summary>
        public static readonly MessageModel MSG03 = new MessageModel
        {
            MsgCode = "MSG03",
            MsgTextVN = "Email này chưa được xác nhận",
            MsgTextEN = "This email has not been confirmed"
        };

        /// <summary>
        /// MSG04: Mật khẩu không chính xác, vui lòng kiểm tra lại
        /// </summary>
        public static readonly MessageModel MSG04 = new MessageModel
        {
            MsgCode = "MSG04",
            MsgTextVN = "Mật khẩu không chính xác, vui lòng kiểm tra lại",
            MsgTextEN = "Incorrect password, please check again"
        };

        /// <summary>
        /// MSG05: Đăng ký thành công
        /// </summary>
        public static readonly MessageModel MSG05 = new MessageModel
        {
            MsgCode = "MSG05",
            MsgTextVN = "Đăng ký thành công",
            MsgTextEN = "Register successfully"
        };

        /// <summary>
        /// MSG06: Email này đã được đăng ký, vui lòng sử dụng một email khác
        /// </summary>
        public static readonly MessageModel MSG06 = new MessageModel
        {
            MsgCode = "MSG06",
            MsgTextVN = "Email này đã được đăng ký, vui lòng sử dụng một email khác",
            MsgTextEN = "This email has been registered, please use another email"
        };

        /// <summary>
        /// MSG07: Tên tài khoản này đã được đăng ký, vui lòng sử dụng một tài khoản khác
        /// </summary>
        public static readonly MessageModel MSG07 = new MessageModel
        {
            MsgCode = "MSG07",
            MsgTextVN = "Tên tài khoản này đã được đăng ký, vui lòng sử dụng một tài khoản khác",
            MsgTextEN = "This username has been registered, please use another username"
        };

        /// <summary>
        /// MSG08: Email xác nhận đã được gửi, vui lòng kiểm tra email để xác nhận tài khoản
        /// </summary>
        public static readonly MessageModel MSG08 = new MessageModel
        {
            MsgCode = "MSG08",
            MsgTextVN = "Email xác nhận đã được gửi, vui lòng kiểm tra email để xác nhận tài khoản",
            MsgTextEN = "Confirmation email has been sent, please check your email to confirm your account"
        };

        /// <summary>
        /// MSG09: Email không hợp lệ
        /// </summary>
        public static readonly MessageModel MSG09 = new MessageModel
        {
            MsgCode = "MSG09",
            MsgTextVN = "Email không hợp lệ",
            MsgTextEN = "Invalid email"
        };

        /// <summary>
        /// MSG10: Email không tồn tại
        /// </summary>
        public static readonly MessageModel MSG10 = new MessageModel
        {
            MsgCode = "MSG10",
            MsgTextVN = "Email không tồn tại",
            MsgTextEN = "Email does not exist"
        };

        /// <summary>
        /// MSG11: Không được để trống
        /// </summary>
        public static readonly MessageModel MSG11 = new MessageModel
        {
            MsgCode = "MSG11",
            MsgTextVN = "Không được để trống",
            MsgTextEN = "Not empty"
        };

        /// <summary>
        /// MSG12: Đổi mật khẩu thành công
        /// </summary>
        public static readonly MessageModel MSG12 = new MessageModel
        {
            MsgCode = "MSG12",
            MsgTextVN = "Đổi mật khẩu thành công",
            MsgTextEN = "Change password successfully"
        };

        /// <summary>
        /// MSG13: Mật khẩu cũ không chính xác
        /// </summary>
        public static readonly MessageModel MSG13 = new MessageModel
        {
            MsgCode = "MSG13",
            MsgTextVN = "Mật khẩu cũ không chính xác",
            MsgTextEN = "Incorrect old password"
        };

        /// <summary>
        /// MSG14: Gửi OTP thành công
        /// </summary>
        public static readonly MessageModel MSG14 = new MessageModel
        {
            MsgCode = "MSG14",
            MsgTextVN = "Gửi OTP thành công",
            MsgTextEN = "Send OTP successfully"
        };

        /// <summary>
        /// MSG15: Mã OTP không chính xác
        /// </summary>
        public static readonly MessageModel MSG15 = new MessageModel
        {
            MsgCode = "MSG15",
            MsgTextVN = "Mã OTP không chính xác",
            MsgTextEN = "Incorrect OTP code"
        };

        /// <summary>
        /// MSG16: Thành công
        /// </summary>
        public static readonly MessageModel MSG16 = new MessageModel
        {
            MsgCode = "MSG16",
            MsgTextVN = "Thành công",
            MsgTextEN = "Success"
        };

        /// <summary>
        /// MSG17: Mật khẩu không hợp lệ
        /// </summary>
        public static readonly MessageModel MSG17 = new MessageModel
        {
            MsgCode = "MSG17",
            MsgTextVN = "Mật khẩu không hợp lệ",
            MsgTextEN = "Invalid password"
        };

        /// <summary>
        /// MSG18: Mật khẩu cũ và mật khẩu mới không được giống nhau
        /// </summary>
        public static readonly MessageModel MSG18 = new MessageModel
        {
            MsgCode = "MSG18",
            MsgTextVN = "Mật khẩu cũ và mật khẩu mới không được giống nhau",
            MsgTextEN = "Old password and new password must not be the same"
        };

        /// <summary>
        /// MSG19: Sửa thông tin thành công
        /// </summary>
        public static readonly MessageModel MSG19 = new MessageModel
        {
            MsgCode = "MSG19",
            MsgTextVN = "Sửa thông tin thành công",
            MsgTextEN = "Update information successfully"
        };

        /// <summary>
        /// MSG20: Số điện thoại không hợp lệ
        /// </summary>
        public static readonly MessageModel MSG20 = new MessageModel
        {
            MsgCode = "MSG20",
            MsgTextVN = "Số điện thoại không hợp lệ",
            MsgTextEN = "Invalid phone number"
        };

        /// <summary>
        /// MSG21: Tên tài khoản không hợp lệ
        /// </summary>
        public static readonly MessageModel MSG21 = new MessageModel
        {
            MsgCode = "MSG21",
            MsgTextVN = "Tên tài khoản không hợp lệ",
            MsgTextEN = "Invalid username"
        };

        /// <summary>
        /// MSG22: Trống
        /// </summary>
        public static readonly MessageModel MSG22 = new MessageModel
        {
            MsgCode = "MSG22",
            MsgTextVN = "Trống",
            MsgTextEN = "Empty"
        };

        /// <summary>
        /// MSG23: Tên không hơp lệ
        /// </summary>
        public static readonly MessageModel MSG23 = new MessageModel
        {
            MsgCode = "MSG23",
            MsgTextVN = "Tên không hơp lệ",
            MsgTextEN = "Invalid name"
        };

        /// <summary>
        /// MSG24: Người dùng không tồn tại
        /// </summary>
        public static readonly MessageModel MSG24 = new MessageModel
        {
            MsgCode = "MSG24",
            MsgTextVN = "Người dùng không tồn tại",
            MsgTextEN = "User does not exist"
        };

        /// <summary>
        /// MSG25: Khóa học không tồn tại
        /// </summary>
        public static readonly MessageModel MSG25 = new MessageModel
        {
            MsgCode = "MSG25",
            MsgTextVN = "Khóa học không tồn tại",
            MsgTextEN = "Course does not exist"
        };

        /// <summary>
        /// MSG26: Số không hợp lệ
        /// </summary>
        public static readonly MessageModel MSG26 = new MessageModel
        {
            MsgCode = "MSG26",
            MsgTextVN = "Số không hợp lệ",
            MsgTextEN = "Invalid number"
        };

        /// <summary>
        /// MSG27: Độ dài không hợp lệ
        /// </summary>
        public static readonly MessageModel MSG27 = new MessageModel
        {
            MsgCode = "MSG27",
            MsgTextVN = "Độ dài không hợp lệ",
            MsgTextEN = "Invalid length"
        };

        /// <summary>
        /// MSG28: Chương không tồn tại
        /// </summary>
        public static readonly MessageModel MSG28 = new MessageModel
        {
            MsgCode = "MSG28",
            MsgTextVN = "Chương không tồn tại",
            MsgTextEN = "Chapter does not exist"
        };

        /// <summary>
        /// MSG29: Bài học không tồn tại
        /// </summary>
        public static readonly MessageModel MSG29 = new MessageModel
        {
            MsgCode = "MSG29",
            MsgTextVN = "Bài học không tồn tại",
            MsgTextEN = "Lesson does not exist"
        };

        /// <summary>
        /// MSG30: Lỗi
        /// </summary>
        public static readonly MessageModel MSG30 = new MessageModel
        {
            MsgCode = "MSG30",
            MsgTextVN = "Lỗi",
            MsgTextEN = "Error"
        };

        /// <summary>
        /// MSG31: Câu hỏi không tồn tại
        /// </summary>
        public static readonly MessageModel MSG31 = new MessageModel
        {
            MsgCode = "MSG31",
            MsgTextVN = "Câu hỏi không tồn tại",
            MsgTextEN = "Question does not exist"
        };

        /// <summary>
        /// MSG32: Bài kiểm tra không tồn tại
        /// </summary>
        public static readonly MessageModel MSG32 = new MessageModel
        {
            MsgCode = "MSG32",
            MsgTextVN = "Bài kiểm tra không tồn tại",
            MsgTextEN = "Exam does not exist"
        };

        /// <summary>
        /// MSG33: Tài khoản đã bị khóa
        /// </summary>
        public static readonly MessageModel MSG33 = new MessageModel
        {
            MsgCode = "MSG33",
            MsgTextVN = "Tài khoản đã bị khóa",
            MsgTextEN = "Account has been locked"
        };
    }
}
