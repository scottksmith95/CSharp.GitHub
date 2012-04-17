#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using CSharp.GitHub.Api.Impl;
using CSharp.GitHub.Api.Interfaces;
using Spring.Social.OAuth2;

namespace CSharp.GitHub.Connect
{
	/// <summary>
	/// GitHub <see cref="IServiceProvider"/> implementation.
	/// </summary>
	/// <author>Scott Smith</author>
    public class GitHubServiceProvider : AbstractOAuth2ServiceProvider<IGitHub>
    {
		/// <summary>
		/// Creates a new instance of <see cref="GitHubServiceProvider"/>.
		/// </summary>
		/// <param name="clientId">The application's API client id.</param>
		/// <param name="clientSecret">The application's API client secret.</param>
        public GitHubServiceProvider(String clientId, String clientSecret)
            : base(new OAuth2Template(clientId, clientSecret,
                "https://github.com/login/oauth/authorize",
                "https://github.com/login/oauth/access_token"))
        {
        }

		/// <summary>
		/// Returns an API interface allowing the client application to access unprotected resources.
		/// </summary>
		/// <returns>A binding to the service provider's API.</returns>
		public IGitHub GetAPi()
		{
			return new GitHubTemplate();
		}

		/// <summary>
		/// Returns an API interface allowing the client application to access protected resources on behalf of a user.
		/// </summary>
		/// <param name="accessToken">The API access token.</param>
		/// <returns>A binding to the service provider's API.</returns>
		public override IGitHub GetApi(string accessToken)
		{
			return new GitHubTemplate(accessToken);
		}
    }
}
