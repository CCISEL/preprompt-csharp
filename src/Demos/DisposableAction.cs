using System;

namespace Demos
{
    public class DisposableAction : IDisposable
    {
        private readonly Action _callback;

        public DisposableAction(Action callback)
        {
            _callback = callback;
        }

        public void Dispose()
        {
            _callback();
        }
    }
}