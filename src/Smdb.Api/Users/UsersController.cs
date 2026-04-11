namespace Smdb.Api.Users;

using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Users;

public class UsersController
{
    private IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    // GET /api/v1/users?page=1&size=10
    public async Task ReadUsers(HttpListenerRequest req, HttpListenerResponse res,
        Hashtable props, Func<Task> next)
    {
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 9;
        var result = await userService.ReadUsers(page, size);
        await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
        await next();
    }

    // POST /api/v1/users
    public async Task CreateUser(HttpListenerRequest req,
        HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text = (string)props["req.text"]!;
        var user = JsonSerializer.Deserialize<User>(text, JsonSerializerOptions.Web);
        
        // COPILOT FIX: Added validation for user fields - check if user is null, or if username/email/fullName are empty or exceed length limits
        if (user == null || string.IsNullOrEmpty(user.Username) || user.Username.Length > 50 || 
            string.IsNullOrEmpty(user.Email) || user.Email.Length > 100 ||
            string.IsNullOrEmpty(user.FullName) || user.FullName.Length > 100)
        {
            var errorResponse = new { error = "Username (max 50 chars), Email (max 100 chars), and FullName (max 100 chars) are all required." };
            await HttpUtils.SendResponse(req, res, props, (int)HttpStatusCode.BadRequest, 
                JsonSerializer.Serialize(errorResponse, JsonSerializerOptions.Web));
            await next();
            return;
        }
        
        var result = await userService.CreateUser(user!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // GET /api/v1/users/5
    public async Task ReadUser(HttpListenerRequest req, HttpListenerResponse res,
        Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;
        var result = await userService.ReadUser(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // PUT /api/v1/users/5
    public async Task UpdateUser(HttpListenerRequest req,
        HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;
        var text = (string)props["req.text"]!;
        var user = JsonSerializer.Deserialize<User>(text, JsonSerializerOptions.Web);
        var result = await userService.UpdateUser(id, user!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // DELETE /api/v1/users/5
    public async Task DeleteUser(HttpListenerRequest req,
        HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;
        var result = await userService.DeleteUser(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}