namespace Smdb.Core.Users;

using System.Net;
using Shared.Http;

public class DefaultUserService : IUserService
{
    private IUserRepository userRepository;

    public DefaultUserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<Result<PagedResult<User>>> ReadUsers(int page, int size)
    {
        var result = await userRepository.ReadUsers(page, size);

        return new Result<PagedResult<User>>(
            result,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Result<User>> CreateUser(User user)
    {
        var validation = ValidateUser(user);
        if (validation != null) return validation;

        var result = await userRepository.CreateUser(user);

        return new Result<User>(
            result!,
            (int)HttpStatusCode.Created
        );
    }

    public async Task<Result<User>> ReadUser(int id)
    {
        var user = await userRepository.ReadUser(id);

        return user == null
            ? new Result<User>(new Exception($"Could not find user with id {id}."), (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> UpdateUser(int id, User newData)
    {
        var validation = ValidateUser(newData);
        if (validation != null) return validation;

        var user = await userRepository.UpdateUser(id, newData);

        return user == null
            ? new Result<User>(new Exception($"Could not update user with id {id}."), (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> DeleteUser(int id)
    {
        var user = await userRepository.DeleteUser(id);

        return user == null
            ? new Result<User>(new Exception($"Could not delete user with id {id}."), (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);
    }

    private static Result<User>? ValidateUser(User userData)
    {
        if (string.IsNullOrWhiteSpace(userData.Username))
        {
            return new Result<User>(
                new Exception("Username is required and cannot be empty."),
                (int)HttpStatusCode.BadRequest);
        }

        if (string.IsNullOrWhiteSpace(userData.Email))
        {
            return new Result<User>(
                new Exception("Email is required and cannot be empty."),
                (int)HttpStatusCode.BadRequest);
        }

        if (userData.Bio.Length > 256)
        {
            return new Result<User>(
                new Exception("Bio cannot be longer than 256 characters."),
                (int)HttpStatusCode.BadRequest);
        }

        if (userData.BirthYear < 1900 || userData.BirthYear > DateTime.UtcNow.Year)
        {
            return new Result<User>(
                new Exception($"BirthYear must be between 1900 and {DateTime.UtcNow.Year}."),
                (int)HttpStatusCode.BadRequest);
        }

        return null;
    }
}