namespace Smdb.Core.Actors;

using System.Net;
using Shared.Http;

public class DefaultActorService : IActorService
{
    private IActorRepository actorRepository;

    public DefaultActorService(IActorRepository actorRepository)
    {
        this.actorRepository = actorRepository;
    }

    public async Task<Result<PagedResult<Actor>>> ReadActors(int page, int size)
    {
        var result = await actorRepository.ReadActors(page, size);

        return new Result<PagedResult<Actor>>(
            result,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Result<Actor>> CreateActor(Actor actor)
    {
        var validation = ValidateActor(actor);
        if (validation != null) return validation;

        var result = await actorRepository.CreateActor(actor);

        return new Result<Actor>(
            result!,
            (int)HttpStatusCode.Created
        );
    }

    public async Task<Result<Actor>> ReadActor(int id)
    {
        var actor = await actorRepository.ReadActor(id);

        return actor == null
            ? new Result<Actor>(new Exception($"Could not find actor with id {id}."), (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> UpdateActor(int id, Actor newData)
    {
        var validation = ValidateActor(newData);
        if (validation != null) return validation;

        var actor = await actorRepository.UpdateActor(id, newData);

        return actor == null
            ? new Result<Actor>(new Exception($"Could not update actor with id {id}."), (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> DeleteActor(int id)
    {
        var actor = await actorRepository.DeleteActor(id);

        return actor == null
            ? new Result<Actor>(new Exception($"Could not delete actor with id {id}."), (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);
    }

    private static Result<Actor>? ValidateActor(Actor actorData)
    {
        if (string.IsNullOrWhiteSpace(actorData.Name))
        {
            return new Result<Actor>(
                new Exception("Name is required and cannot be empty."),
                (int)HttpStatusCode.BadRequest);
        }

        if (actorData.Biography.Length > 256)
        {
            return new Result<Actor>(
                new Exception("Biography cannot be longer than 256 characters."),
                (int)HttpStatusCode.BadRequest);
        }

        if (actorData.BirthYear < 1888 || actorData.BirthYear > DateTime.UtcNow.Year)
        {
            return new Result<Actor>(
                new Exception($"BirthYear must be between 1888 and {DateTime.UtcNow.Year}."),
                (int)HttpStatusCode.BadRequest);
        }

        return null;
    }
}