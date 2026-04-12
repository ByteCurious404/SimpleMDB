// Added by GitHub Copilot
// To support movie-actor relationships for bonus CRUD+L operations

namespace Smdb.Core.Movies;

public class MovieActor
{
    public int MovieId { get; set; }
    public int ActorId { get; set; }

    public MovieActor(int movieId, int actorId)
    {
        MovieId = movieId;
        ActorId = actorId;
    }

    public override string ToString()
    {
        return $"MovieActor[MovieId={MovieId}, ActorId={ActorId}]";
    }
}