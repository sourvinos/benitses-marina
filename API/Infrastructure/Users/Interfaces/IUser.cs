namespace API.Infrastructure.Users {

    public interface IUser {

        string Email { get; set; }
        string Username { get; set; }
        bool IsAdmin { get; set; }

    }

}