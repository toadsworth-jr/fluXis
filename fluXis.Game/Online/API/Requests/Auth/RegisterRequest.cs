using System.Net.Http;
using fluXis.Game.Online.API.Payloads.Auth;
using fluXis.Game.Online.API.Responses.Auth;
using fluXis.Game.Utils;
using osu.Framework.IO.Network;

namespace fluXis.Game.Online.API.Requests.Auth;

public class RegisterRequest : APIRequest<RegisterResponse>
{
    protected override string Path => "/auth/register";
    protected override HttpMethod Method => HttpMethod.Post;

    private string username { get; }
    private string password { get; }
    private string email { get; }

    public RegisterRequest(string username, string password, string email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }

    protected override WebRequest CreateWebRequest(string url)
    {
        var req = base.CreateWebRequest(url);
        var json = new RegisterPayload(username, password, email);
        req.AddRaw(json.Serialize());
        return req;
    }
}
