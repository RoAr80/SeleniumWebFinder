using System;

namespace SeleniumWebFinder.Commands.AsyncCommand

{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}