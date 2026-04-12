// Added by GitHub Copilot
// To define service interface for movie-actor relationships

using Smdb.Core.Movies;
using Shared;
using Shared.Http;

namespace Smdb.Core.Movies;

public interface IMovieActorService
{
    Task<Result<PagedResult<MovieActor>>> ReadMovieActors(int page, int size);
    Task<Result<MovieActor?>> CreateMovieActor(MovieActor newMovieActor);
    Task<Result<MovieActor?>> ReadMovieActor(int movieId, int actorId);
    Task<Result<bool>> DeleteMovieActor(int movieId, int actorId);
    Task<Result<List<MovieActor>>> ReadMovieActorsByMovie(int movieId);
    Task<Result<List<MovieActor>>> ReadMovieActorsByActor(int actorId);
}