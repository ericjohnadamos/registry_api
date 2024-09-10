﻿namespace RegistryApi.Web;

using RegistryApi.Core;
using RegistryApi.SharedKernel.Exceptions;
using RegistryApi.Web.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IMediator mediator;

    public BasicAuthenticationHandler(
        IMediator mediator,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        this.mediator = mediator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        string username;
        string password;

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader?.Parameter ?? "");
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            username = credentials[0];
            password = credentials[1];
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var userOption = await this.mediator.Send(new GetUserQuery(username, password));
        return userOption.Match(
            user =>
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ApiUserClaimTypes.Username, user.Username),
                    new Claim(ApiUserClaimTypes.Password, password),
                    new Claim(ApiUserClaimTypes.CustomerKey, user.Customer.Key),
                    new Claim(ApiUserClaimTypes.TimeZoneId, user.Customer?.TimeZoneId ?? ""),
                };

                foreach (var userRole in user.UsersInRole.Select(u => u.Role))
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Name));

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            },
            ex => AuthenticateResult.Fail(
                ex is NotFoundException ? "Invalid Username or Password" : "Invalid Authorization Header"));
    }
}