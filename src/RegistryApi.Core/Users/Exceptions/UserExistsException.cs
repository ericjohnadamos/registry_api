using System;

namespace RegistryApi.Core.Users.Exceptions;

public class UserExistsException : Exception
{
    public UserExistsException(string message)
        : base(message)
    {
    }
}