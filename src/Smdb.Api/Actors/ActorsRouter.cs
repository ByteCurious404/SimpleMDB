namespace Smdb.Api.Actors;

using Shared.Http;

public class ActorsRouter : HttpRouter
{
    public ActorsRouter(ActorsController controller)
    {
        UseParametrizedRouteMatching();

        MapGet("/", controller.ReadActors);

        MapPost("/", HttpUtils.ReadRequestBodyAsText,
            controller.CreateActor);

        MapGet("/:id", controller.ReadActor);

        MapPut("/:id", HttpUtils.ReadRequestBodyAsText,
            controller.UpdateActor);

        MapDelete("/:id", controller.DeleteActor);
    }
}