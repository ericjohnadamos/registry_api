namespace RegistryApi.Web.Features.Shared;

using System;
using System.Linq;
using System.Security.Claims;
using Ardalis.ApiEndpoints;
using RegistryApi.Core;
using RegistryApi.Core.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public abstract class RegistryControllerBase : Controller
{
    private const string DefaultTimeZoneId = "Eastern Standard Time";
    private const string DefaultCustomerKey = "_demo";

    protected RegistryControllerBase(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        var accessor = httpContextAccessor;
        Mediator = mediator;
            
        var userId = Convert.ToInt32(accessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value);
        var username = accessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        var customerKey = accessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ApiUserClaimTypes.CustomerKey)?.Value ??
                          DefaultCustomerKey;
        var timeZoneId = accessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ApiUserClaimTypes.TimeZoneId)?.Value ??
                         DefaultTimeZoneId;
        UserInfo = new UserInfo(userId, username, customerKey, timeZoneId);
    }

    protected UserInfo UserInfo { get; }
    protected IMediator Mediator { get; }
}

public abstract class RegistryEndpointBase : EndpointBase
{
    private const string DefaultTimeZoneId = "Eastern Standard Time";
    private const string DefaultCustomerKey = "_demo";

    public RegistryEndpointBase(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        var accessor = httpContextAccessor;
        Mediator = mediator;
            
        var userId = Convert.ToInt32(accessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value);
        var username = accessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        var customerKey = accessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ApiUserClaimTypes.CustomerKey)?.Value ??
                          DefaultCustomerKey;
        var timeZoneId = accessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ApiUserClaimTypes.TimeZoneId)?.Value ??
                         DefaultTimeZoneId;
        UserInfo = new UserInfo(userId, username, customerKey, timeZoneId);
    }

    protected UserInfo UserInfo { get; }
    protected IMediator Mediator { get; }
}