using Shared.Http;
// Added by GitHub Copilot
// To implement service for movie-actor relationships

using Smdb.Core.Movies;
using Shared;

namespace Smdb.Core.Movies;

public class DefaultMovieActorService(IMovieActorRepository repo) : IMovieActorService
{
    public async Task<Result<PagedResult<MovieActor>>> ReadMovieActors(int page, int size)
    {
        var pagedResult = await repo.ReadMovieActors(page, size);
        return new Result<PagedResult<MovieActor>>(pagedResult, (int)System.Net.HttpStatusCode.OK);
    }

    public async Task<Result<MovieActor?>> CreateMovieActor(MovieActor newMovieActor)
    {
        var result = await repo.CreateMovieActor(newMovieActor);
        return new Result<MovieActor?>(result, result != null ? (int)System.Net.HttpStatusCode.Created : (int)System.Net.HttpStatusCode.Conflict);
    }

    public async Task<Result<MovieActor?>> ReadMovieActor(int movieId, int actorId)
    {
        var result = await repo.ReadMovieActor(movieId, actorId);
        return new Result<MovieActor?>(result, result != null ? (int)System.Net.HttpStatusCode.OK : (int)System.Net.HttpStatusCode.NotFound);
    }

    public async Task<Result<bool>> DeleteMovieActor(int movieId, int actorId)
    {
        var deleted = await repo.DeleteMovieActor(movieId, actorId);
        return new Result<bool>(deleted, deleted ? (int)System.Net.HttpStatusCode.NoContent : (int)System.Net.HttpStatusCode.NotFound);
    }

    public async Task<Result<List<MovieActor>>> ReadMovieActorsByMovie(int movieId)
    {
        var result = await repo.ReadMovieActorsByMovie(movieId);
        return new Result<List<MovieActor>>(result, (int)System.Net.HttpStatusCode.OK);
    }

    public async Task<Result<List<MovieActor>>> ReadMovieActorsByActor(int actorId)
    {
        var result = await repo.ReadMovieActorsByActor(actorId);
        return new Result<List<MovieActor>>(result, (int)System.Net.HttpStatusCode.OK);
    }
}