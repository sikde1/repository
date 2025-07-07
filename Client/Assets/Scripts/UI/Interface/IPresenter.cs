using Cysharp.Threading.Tasks;
using Project.EventStream;
using System;

namespace Project.UI.Interface
{
    public interface IPresenter : IDisposable
    {
        UniTask OpenPopupAsync();
        void ClosePopup();
        void Refresh();
    }
}

