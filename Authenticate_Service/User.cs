namespace Account.API
{
    public class User
    {
        public string UserName { get; set; }
        public string Pass { get; set; }

        public string Role { get; set; }

        public User(string userName, string pass, string role)
        {
            UserName = userName;
            Pass = pass;
            Role = role;
        }
    }
}
