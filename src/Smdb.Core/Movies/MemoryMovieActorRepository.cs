// Added by GitHub Copilot
// To implement in-memory repository for movie-actor relationships

using Smdb.Core.Db;
using Smdb.Core.Movies;
using Shared.Http;

namespace Smdb.Core.Movies;

public class MemoryMovieActorRepository(MemoryDatabase db) : IMovieActorRepository
{
    public async Task<PagedResult<MovieActor>> ReadMovieActors(int page, int size)
    {
        int totalCount = db.MovieActors.Count;
        int start = page * size;
        int length = Math.Min(size, totalCount - start);
        var values = db.MovieActors.Skip(start).Take(length).ToList();
        var result = new PagedResult<MovieActor>(totalCount, values);
        return result;
    }

    public async Task<MovieActor?> CreateMovieActor(MovieActor newMovieActor)
    {
        if (db.MovieActors.Any(ma => ma.MovieId == newMovieActor.MovieId && ma.ActorId == newMovieActor.ActorId))
        {
            return null; // Already exists
        }
        db.MovieActors.Add(newMovieActor);
        return newMovieActor;
    }

    public async Task<MovieActor?> ReadMovieActor(int movieId, int actorId)
    {
        return db.MovieActors.FirstOrDefault(ma => ma.MovieId == movieId && ma.ActorId == actorId);
    }

    public async Task<bool> DeleteMovieActor(int movieId, int actorId)
    {
        var ma = db.MovieActors.FirstOrDefault(ma => ma.MovieId == movieId && ma.ActorId == actorId);
        if (ma != null)
        {
            db.MovieActors.Remove(ma);
            return true;
        }
        return false;
    }

    public async Task<List<MovieActor>> ReadMovieActorsByMovie(int movieId)
    {
        return db.MovieActors.Where(ma => ma.MovieId == movieId).ToList();
    }

    public async Task<List<MovieActor>> ReadMovieActorsByActor(int actorId)
    {
        return db.MovieActors.Where(ma => ma.ActorId == actorId).ToList();
    }
}