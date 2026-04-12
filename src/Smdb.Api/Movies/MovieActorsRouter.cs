// Added by GitHub Copilot
// To route API endpoints for movie-actor relationships

using Shared.Http;
using Smdb.Api.Movies;

namespace Smdb.Api.Movies;

public class MovieActorsRouter : HttpRouter
{
    public MovieActorsRouter(MovieActorsController controller)
    {
        UseParametrizedRouteMatching();
        MapGet("/movie-actors", controller.GetMovieActors);
        MapPost("/movie-actors", HttpUtils.ReadRequestBodyAsText, controller.CreateMovieActor);
        MapGet("/movie-actors/:movieId/:actorId", controller.GetMovieActor);
        MapDelete("/movie-actors/:movieId/:actorId", controller.DeleteMovieActor);
        MapGet("/movies/:movieId/actors", controller.GetMovieActorsByMovie);
        MapGet("/actors/:actorId/movies", controller.GetMovieActorsByActor);
    }
}