using System.Threading.Tasks;
using System.Windows.Input;

namespace SeleniumWebFinder.Commands.AsyncCommand
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }
}
