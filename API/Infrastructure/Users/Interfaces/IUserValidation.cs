namespace API.Infrastructure.Users {

    public interface IUserValidation<T> where T : class {

        bool IsUserOwner(string userId);

    }

}