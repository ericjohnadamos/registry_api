using System;
using System.Linq.Expressions;
using RegistryApi.SharedKernel;

namespace RegistryApi.Core.Users.Specifications;

public class MatchingUsernameSpecification : Specification<User>
{
    private readonly string _username;

    public MatchingUsernameSpecification(string username)
        => _username = username;

    public override Expression<Func<User, bool>> ToExpression()
        => user => user.Username == _username;
}