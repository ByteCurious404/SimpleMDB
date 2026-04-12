// Added by GitHub Copilot
// To handle API requests for movie-actor relationships

using Shared;
using Shared.Http;
using Smdb.Core.Movies;
using System.Net;
using System.Text.Json;
using System.Collections;
using System.Collections.Specialized;

namespace Smdb.Api.Movies;

public class MovieActorsController(IMovieActorService service)
{
    public async Task GetMovieActors(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 0;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 10;

        var result = await service.ReadMovieActors(page, size);

        await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
        await next();
    }

    public async Task CreateMovieActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text = (string)props["req.text"]!;
        var movieActor = JsonSerializer.Deserialize<MovieActor>(text, JsonSerializerOptions.Web);

        if (movieActor == null)
        {
            var errorResponse = new { error = "Invalid JSON" };
            await HttpUtils.SendResponse(req, res, props, (int)HttpStatusCode.BadRequest, 
                JsonSerializer.Serialize(errorResponse, JsonSerializerOptions.Web));
            await next();
            return;
        }

        var result = await service.CreateMovieActor(movieActor);
        if (result == null)
        {
            var errorResponse = new { error = "MovieActor already exists" };
            await HttpUtils.SendResponse(req, res, props, (int)HttpStatusCode.Conflict, 
                JsonSerializer.Serialize(errorResponse, JsonSerializerOptions.Web));
            await next();
            return;
        }

        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task GetMovieActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        var movieId = int.Parse(uParams["movieId"]!);
        var actorId = int.Parse(uParams["actorId"]!);

        var result = await service.ReadMovieActor(movieId, actorId);
        if (result == null)
        {
            await HttpUtils.SendResponse(req, res, props, (int)HttpStatusCode.NotFound, "{}");
            await next();
            return;
        }

        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task DeleteMovieActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        var movieId = int.Parse(uParams["movieId"]!);
        var actorId = int.Parse(uParams["actorId"]!);

        var result = await service.DeleteMovieActor(movieId, actorId);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task GetMovieActorsByMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        var movieId = int.Parse(uParams["movieId"]!);

        var result = await service.ReadMovieActorsByMovie(movieId);

        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task GetMovieActorsByActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        var actorId = int.Parse(uParams["actorId"]!);

        var result = await service.ReadMovieActorsByActor(actorId);

        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}