namespace Smdb.Api.Actors;

using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Actors;

public class ActorsController
{
    private IActorService actorService;

    public ActorsController(IActorService actorService)
    {
        this.actorService = actorService;
    }

    // GET /api/v1/actors?page=1&size=10
    public async Task ReadActors(HttpListenerRequest req, HttpListenerResponse res,
        Hashtable props, Func<Task> next)
    {
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 9;
        var result = await actorService.ReadActors(page, size);
        await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
        await next();
    }

    // POST /api/v1/actors
    public async Task CreateActor(HttpListenerRequest req,
        HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text = (string)props["req.text"]!;
        var actor = JsonSerializer.Deserialize<Actor>(text, JsonSerializerOptions.Web);
        var result = await actorService.CreateActor(actor!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // GET /api/v1/actors/5
    public async Task ReadActor(HttpListenerRequest req, HttpListenerResponse res,
        Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;
        var result = await actorService.ReadActor(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // PUT /api/v1/actors/5
    public async Task UpdateActor(HttpListenerRequest req,
        HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;
        var text = (string)props["req.text"]!;
        var actor = JsonSerializer.Deserialize<Actor>(text, JsonSerializerOptions.Web);
        var result = await actorService.UpdateActor(id, actor!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // DELETE /api/v1/actors/5
    public async Task DeleteActor(HttpListenerRequest req,
        HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;
        var result = await actorService.DeleteActor(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}