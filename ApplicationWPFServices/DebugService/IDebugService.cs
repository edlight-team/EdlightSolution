using System.Threading.Tasks;

namespace ApplicationWPFServices.DebugService
{
    public interface IDebugService
    {
        void ConfigureDebugWindow();
        void Log(string message);
        void Clear();
    }
}
