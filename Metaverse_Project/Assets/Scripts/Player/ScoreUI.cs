using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private static ScoreUI instance;
    public static ScoreUI _Instance { get => instance; }

    [SerializeField] private TextMeshProUGUI SummaryText;

    [SerializeField] private Image Rank_Icon;

    [SerializeField] private Sprite S_Icon;
    [SerializeField] private Sprite A_Icon;
    [SerializeField] private Sprite B_Icon;
    [SerializeField] private Sprite C_Icon;
    [SerializeField] private Sprite D_Icon;
    [SerializeField] private Sprite F_Icon;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateSummary(int Seconds, int Miliseconds, int Score, Rank Rank)
    {
        SummaryText.text = $"<align=left><line-height=0>Time:\n" +
                           $"<align=right><line-height=1em>{Seconds}.{Miliseconds}\n" +
                           $"<align=left><line-height=0>Score:\n" +
                           $"<align=right><line-height=1em>{Score}";

        switch (Rank)
        {
            case Rank.S: Rank_Icon.sprite = S_Icon; break;
            case Rank.A: Rank_Icon.sprite = A_Icon; break;
            case Rank.B: Rank_Icon.sprite = B_Icon; break;
            case Rank.C: Rank_Icon.sprite = C_Icon; break;
            case Rank.D: Rank_Icon.sprite = D_Icon; break;
                default: Rank_Icon.sprite = F_Icon; break;
        }
    }
}
public enum Rank
{
    S,
    A,
    B,
    C,
    D,
    F
}