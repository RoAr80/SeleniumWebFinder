using System.Threading.Tasks;
using System.Windows.Input;

namespace SeleniumWebFinder.Commands.AsyncCommand
{
    public interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T parameter);
        bool CanExecute(T parameter);
    }
}
