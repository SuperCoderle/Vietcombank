namespace VietcombankDTO
{
    public class AccountDTO
    {
        private string? iD;
        private string? username;
        private string? password;
        private string? role;

        public string? ID { get => iD; set => iD = value; }
        public string? Username { get => username; set => username = value; }
        public string? Password { get => password; set => password = value; }
        public string? Role { get => role; set => role = value; }
    }
}