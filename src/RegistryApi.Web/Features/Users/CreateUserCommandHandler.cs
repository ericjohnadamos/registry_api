namespace RegistryApi.Web.Features.Users;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RegistryApi.Core.Users;
using RegistryApi.Core.Users.Exceptions;
using RegistryApi.Core.Users.Specifications;
using RegistryApi.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RegistryApi.Infrastructure.Data;

public class CreateUserCommandHandler(
	IUserRepository<User> userRepo,
	IPasswordHasher<User> passwordHasher,
    ILogger<CreateUserCommandHandler> logger
) : IRequestHandler<CreateUserCommand, Response<Unit>>
{
    public async Task<Response<Unit>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
	{
		logger.LogInformation($"Handling create user command for user: {command.Username}.");

		var existingUser = await userRepo.GetDetachedAsync(new MatchingUsernameSpecification(command.Username));
		if (existingUser.Any()) 
		{
			logger.LogError($"Create user command failed. User with username: {command.Username} already exists.");
			return new FailureResponse<Unit>(new UserExistsException($"User with username {command.Username} already exists."));
		}

		var user = User.CreateNewUser(command.Username, command.Password, command.CustomerId);
		var hashedPassword = passwordHasher.HashPassword(user, user.Password);
		user.SetPassword(hashedPassword);

		await userRepo.AddAsync(user);
		await userRepo.SaveChangesAsync(user);

		logger.LogInformation($"User: {user.Username} was created successfully.");

		await userRepo.SetupNewMySqlUser(user, command.Password);

		logger.LogInformation($"MySql user with username: {user.Username} was created and configured successfully.");

		return new SuccessResponse<Unit>(Unit.Value);
	}
}