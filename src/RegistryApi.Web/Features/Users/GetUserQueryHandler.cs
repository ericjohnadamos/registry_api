namespace RegistryApi.Web.Features.Users;

using RegistryApi.Core.Users;
using RegistryApi.Core.Users.Specifications;
using RegistryApi.Infrastructure.Data;
using RegistryApi.SharedKernel.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optional;
using System;
using System.Threading;
using System.Threading.Tasks;

public class GetUserQueryHandler(
    IUserRepository<User> userRepository,
    IPasswordHasher<User> passwordHasher
) : IRequestHandler<GetUserQuery, Option<User, Exception>>
{
    public async Task<Option<User, Exception>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (   string.IsNullOrWhiteSpace(request?.Username)
            || string.IsNullOrWhiteSpace(request?.Password))
        {
            var exception = new ArgumentNullException(nameof(request), "Username and password are required.");
            return await Task.FromResult(Option.None<User, Exception>(exception));
        }

        var expression = new MatchingUsernameSpecification(request.Username).ToExpression();
        var user = await userRepository
            .GetAllAsQueryable()
            .Include(u => u.Customer)
            .Include(u => u.UsersInRole).ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(expression);

        if (user == null)
            return Option.None<User, Exception>(new NotFoundException("User not found with specified credentials"));

        var result = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
        var verified = (   result == PasswordVerificationResult.Success
                        || result == PasswordVerificationResult.SuccessRehashNeeded);
        return verified
            ? Option.Some<User, Exception>(user)
            : Option.None<User, Exception>(
                new NotFoundException($"User with username: {request.Username} was not found."));
    }
}