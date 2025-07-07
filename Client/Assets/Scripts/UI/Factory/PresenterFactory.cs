using Cysharp.Threading.Tasks;
using Project.PlayerInfo;
using Project.UI.Interface;
using Project.UI.Type;
using System;
using UnityEngine;

namespace Project.UI.Factory
{
    public class PresenterFactory
    {
        public async UniTask<IPresenter> CreatePresenterAsync(PresenterType presenterType)
        {
            try
            {
                return presenterType switch
                {
                    PresenterType.PlayerInfo => await PlayerInfoPresenter.CreateAndOpenAsync(),
                    PresenterType.Shop => await PlayerInfoPresenter.CreateAndOpenAsync(),
                    PresenterType.Inventory => await PlayerInfoPresenter.CreateAndOpenAsync(),
                    PresenterType.Settings => await PlayerInfoPresenter.CreateAndOpenAsync(),
                    _ => throw new ArgumentException($"Unknown presenter type: {presenterType}")
                };
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create presenter {presenterType}: {ex.Message}");
                return null;
            }
        }

        public bool CanCreate(PresenterType presenterType)
        {
            return presenterType switch
            {
                PresenterType.PlayerInfo => true,
                PresenterType.Shop => false,      // 아직 구현되지 않음
                PresenterType.Inventory => false, // 아직 구현되지 않음
                PresenterType.Settings => false,  // 아직 구현되지 않음
                _ => false
            };
        }
    }
} 