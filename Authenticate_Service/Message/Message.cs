namespace AuthenticateService.API.Message
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
        /// MSG11: Email không được để trống
        /// </summary>
        public static readonly MessageModel MSG11 = new MessageModel
        {
            MsgCode = "MSG11",
            MsgTextVN = "Email không được để trống",
            MsgTextEN = "Email cannot be empty"
        };

    }
}
