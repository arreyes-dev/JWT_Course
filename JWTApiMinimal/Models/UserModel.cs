namespace JWTApiMinimal.Models
{
    public class UserModel
    {

        public int Id { get; set; }
        public string? userName { get; set; }
        public string? userPwd{ get; set; }
        public string? office { get; set; }
        public static List<UserModel> GetUserDB()
        {
            return new List<UserModel>
                {
                    new UserModel { Id = 1, userName = "admin", userPwd = "admin123", office = "Headquarters" },
                    new UserModel { Id = 2, userName = "jsmith", userPwd = "pass123", office = "New York" },
                    new UserModel { Id = 3, userName = "mbrown", userPwd = "secure456", office = "Chicago" },
                    new UserModel { Id = 4, userName = "lgarcia", userPwd = "mypassword", office = "Los Angeles" },
                    new UserModel { Id = 5, userName = "dlee", userPwd = "letmein", office = "Houston" }
                };
        }
    }
}
