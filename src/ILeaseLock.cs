using System;
using System.Threading.Tasks;

namespace Timer
{
    public interface ILeaseLock
    {
        Task<bool> ExecuteIfLeaseAcquired(Func<Task> func);
    }
}