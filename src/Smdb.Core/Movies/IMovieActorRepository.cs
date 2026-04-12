// Added by GitHub Copilot
// To define repository interface for movie-actor relationships

using Shared.Http;

namespace Smdb.Core.Movies;

public interface IMovieActorRepository
{
    Task<PagedResult<MovieActor>> ReadMovieActors(int page, int size);
    Task<MovieActor?> CreateMovieActor(MovieActor newMovieActor);
    Task<MovieActor?> ReadMovieActor(int movieId, int actorId);
    Task<bool> DeleteMovieActor(int movieId, int actorId);
    Task<List<MovieActor>> ReadMovieActorsByMovie(int movieId);
    Task<List<MovieActor>> ReadMovieActorsByActor(int actorId);
}