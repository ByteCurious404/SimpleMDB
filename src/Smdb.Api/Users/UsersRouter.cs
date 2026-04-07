namespace Smdb.Api.Users;

using Shared.Http;

public class UsersRouter : HttpRouter
{
    public UsersRouter(UsersController controller)
    {
        UseParametrizedRouteMatching();

        MapGet("/", controller.ReadUsers);

        MapPost("/", HttpUtils.ReadRequestBodyAsText,
            controller.CreateUser);

        MapGet("/:id", controller.ReadUser);

        MapPut("/:id", HttpUtils.ReadRequestBodyAsText,
            controller.UpdateUser);

        MapDelete("/:id", controller.DeleteUser);
    }
}