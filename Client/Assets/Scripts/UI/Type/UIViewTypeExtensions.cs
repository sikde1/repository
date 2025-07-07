using FlatBufferData;
using Project.Data;
using Project.UI.Type;
using UnityEngine;

public static class UIViewTypeExtensions
{

    public static string GetPrefabPath(this UIViewType viewType)
    {
        var dataUIPopup = MainSystem.Instance.GameDataRepository.GetData<DataUIPopup>(DataType.UIPopup);
        if (dataUIPopup?.UIPopups.TryGetValue(viewType, out var popupData) == true)
        {
            return popupData.PrefabPath;
        }

        Debug.LogError($"Repository에서 UIViewType {viewType}에 해당하는 PrefabPath를 찾을 수 없습니다.");
        return null;
    }

    public static FBUIPopup? GetPopupData(this UIViewType viewType)
    {
        var dataUIPopup = MainSystem.Instance.GameDataRepository.GetData<DataUIPopup>(DataType.UIPopup);
        if (dataUIPopup?.UIPopups.TryGetValue(viewType, out var popupData) == true)
        {
            return popupData;
        }

        Debug.LogError($"Repository에서 UIViewType {viewType}에 해당하는 PopupData를 찾을 수 없습니다.");
        return null;
    }
} 