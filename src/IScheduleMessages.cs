using System.Threading.Tasks;

namespace Timer
{
    public interface IScheduleMessages
    {
        void Schedule(string body);
        void Start();
        void Stop();
    }
}