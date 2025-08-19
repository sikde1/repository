using TMPro;
using UnityEngine;

public class HUDMonsterCount : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    public static HUDMonsterCount Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void UpdateMonsterCount(int count)
    {
        if (countText != null)
        {
            countText.text = $"{count}";
        }
    }
}
