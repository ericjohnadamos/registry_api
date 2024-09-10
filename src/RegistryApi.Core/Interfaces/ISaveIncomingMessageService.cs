namespace RegistryApi.Core.Interfaces;

using System;
using System.Threading.Tasks;

public interface ISaveIncomingMessageService
{
    Task SaveIncomingMessage(string eventName, int customerId, string message, string contact, DateTime timestamp);
}
