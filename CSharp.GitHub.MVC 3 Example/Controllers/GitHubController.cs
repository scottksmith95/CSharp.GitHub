using System.Web.Mvc;
using CSharp.GitHub.Api.Interfaces;
using CSharp.GitHub.Connect;
using Spring.Json;
using Spring.Social.OAuth2;

namespace CSharp.GitHub.MVC_3_Example.Controllers
{
	public class GitHubController : Controller
	{
		// Register your own GitHub app at http://developer.github.com/

		// Configure the Callback URL
		private const string CallbackUrl = "http://localhost/GitHub/Callback";

		// Set your consumer key & secret here
		private const string GitHubApiClientId = "ENTER YOUR CLIENT ID HERE";
		private const string GitHubApiSecret = "ENTER YOUR SECRET HERE";

		readonly IOAuth2ServiceProvider<IGitHub> _gitHubProvider = new GitHubServiceProvider(GitHubApiClientId, GitHubApiSecret);

		public ActionResult Index()
		{
			var accessGrant = Session["AccessGrant"] as AccessGrant;
			if (accessGrant != null)
			{
				//Until explict GitHub endpoints have API bindings, the following
				//method can be employed to consume GitHub endpoints
				var gitHubClient = _gitHubProvider.GetApi(accessGrant.AccessToken);
				var result = gitHubClient.RestOperations.GetForObject<JsonValue>("https://api.github.com/user/repos");

				ViewBag.AccessToken = accessGrant.AccessToken;
				ViewBag.ResultText = result.ToString();

				return View();
			}

			var parameters = new OAuth2Parameters
			{
				RedirectUrl = CallbackUrl,
				Scope = "repo"
			};
			return Redirect(_gitHubProvider.OAuthOperations.BuildAuthorizeUrl(GrantType.AuthorizationCode, parameters));
		}

		public ActionResult Callback(string code)
		{
			AccessGrant accessGrant = _gitHubProvider.OAuthOperations.ExchangeForAccessAsync(code, CallbackUrl, null).Result;

			Session["AccessGrant"] = accessGrant;

			return RedirectToAction("Index");
		}
	}
}
