using System;

namespace Project.EventStream
{
    public interface IEventStreamDisposable
    {
        void Add(IDisposable disposable);
    }
}
