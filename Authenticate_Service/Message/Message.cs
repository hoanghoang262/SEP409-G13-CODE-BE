namespace AuthenticateService.API.Message
{
    public static class Message
    {
        /// <summary>
        /// MG01: Tài khoản không tồn tại
        /// </summary>
        public static readonly MessageModel MG01 = new MessageModel
        {
            MgCode = "MG01",
            MgTextVN = "Tài khoản không tồn tại",
            MgTextEN = "Account does not exist"
        };

        /// <summary>
        /// MG02: Đăng nhập thành công
        /// </summary>
        public static readonly MessageModel MG02 = new MessageModel
        {
            MgCode = "MG02",
            MgTextVN = "Đăng nhập thành công",
            MgTextEN = "Login successfully"
        };

        /// <summary>
        /// MG03: Email này chưa được xác nhận
        /// </summary>
        public static readonly MessageModel MG03 = new MessageModel
        {
            MgCode = "MG03",
            MgTextVN = "Email này chưa được xác nhận",
            MgTextEN = "This email has not been confirmed"
        };

        /// <summary>
        /// MG04: Mật khẩu không chính xác, vui lòng kiểm tra lại
        /// </summary>
        public static readonly MessageModel MG04 = new MessageModel
        {
            MgCode = "MG04",
            MgTextVN = "Mật khẩu không chính xác, vui lòng kiểm tra lại",
            MgTextEN = "Incorrect password, please check again"
        };

        /// <summary>
        /// MG05: Đăng ký thành công
        /// </summary>
        public static readonly MessageModel MG05 = new MessageModel
        {
            MgCode = "MG05",
            MgTextVN = "Đăng ký thành công",
            MgTextEN = "Register successfully"
        };

        /// <summary>
        /// MG06: Email này đã được đăng ký, vui lòng sử dụng một email khác
        /// </summary>
        public static readonly MessageModel MG06 = new MessageModel
        {
            MgCode = "MG06",
            MgTextVN = "Email này đã được đăng ký, vui lòng sử dụng một email khác",
            MgTextEN = "This email has been registered, please use another email"
        };

        /// <summary>
        /// MG07: Tên tài khoản này đã được đăng ký, vui lòng sử dụng một tài khoản khác
        /// </summary>
        public static readonly MessageModel MG07 = new MessageModel
        {
            MgCode = "MG07",
            MgTextVN = "Tên tài khoản này đã được đăng ký, vui lòng sử dụng một tài khoản khác",
            MgTextEN = "This username has been registered, please use another username"
        };

        /// <summary>
        /// MG08: Email xác nhận đã được gửi, vui lòng kiểm tra email để xác nhận tài khoản
        /// </summary>
        public static readonly MessageModel MG08 = new MessageModel
        {
            MgCode = "MG08",
            MgTextVN = "Email xác nhận đã được gửi, vui lòng kiểm tra email để xác nhận tài khoản",
            MgTextEN = "Confirmation email has been sent, please check your email to confirm your account"
        };

        /// <summary>
        /// MG09: Email không hợp lệ
        /// </summary>
        public static readonly MessageModel MG09 = new MessageModel
        {
            MgCode = "MG09",
            MgTextVN = "Email không hợp lệ",
            MgTextEN = "Invalid email"
        };

        /// <summary>
        /// MG10: Email không tồn tại
        /// </summary>
        public static readonly MessageModel MG10 = new MessageModel
        {
            MgCode = "MG10",
            MgTextVN = "Email không tồn tại",
            MgTextEN = "Email does not exist"
        };

        /// <summary>
        /// MG11: Email không được để trống
        /// </summary>
        public static readonly MessageModel MG11 = new MessageModel
        {
            MgCode = "MG11",
            MgTextVN = "Email không được để trống",
            MgTextEN = "Email cannot be empty"
        };

    }
}
