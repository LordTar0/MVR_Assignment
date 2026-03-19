using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : MonoBehaviour
{
    private static UIManagement instance;
    public static UIManagement _Instance { get => instance; }

    [SerializeField] PlayerUI Game_UI;
    [SerializeField] ScoreUI Score_UI;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateUIShown(UIShown Index)
    {
        Game_UI.gameObject.SetActive(false);
        Score_UI.gameObject.SetActive(false);

        switch (Index)
        {
            case UIShown.Game: Game_UI.gameObject.SetActive(true); break;
            case UIShown.Score: Score_UI.gameObject.SetActive(true);break;
            default: break;
        }
    }
}

public enum UIShown
{
    Game,
    Paused,
    Score
}