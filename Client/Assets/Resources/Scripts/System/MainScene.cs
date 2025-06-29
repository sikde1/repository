using UnityEngine;

public class MainScene : MonoBehaviour
{
    private static MainScene _instance = new MainScene();

    public MainScene Instance
    {
        get
        {
            if(_instance == null)
                _instance = new MainScene();
            return _instance;
        }
    }
}
