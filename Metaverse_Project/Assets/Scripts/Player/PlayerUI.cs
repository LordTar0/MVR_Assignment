using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private static PlayerUI instance;
    public static PlayerUI _Instance { get => instance; }

    [Header("UI Settings")]
    [SerializeField] private float LerpSpeed = 2f;

    [Header("Compass")]
    [SerializeField] private Image Compass_Icon;

    [Header("Throttle")]
    [SerializeField] private RectTransform Throttle_Rect;

    [Header("CountDownTimer")]
    [SerializeField] private Image Timer_Icon;
    [Header("Icons")]
    [SerializeField] private Sprite Three;
    [SerializeField] private Sprite Two;
    [SerializeField] private Sprite One;

    [Header("Game Timer")]
    [SerializeField] private TextMeshProUGUI Timer_Text;

    private void Awake()
    {
        instance = this;
    }

    //Updates the UI Compass based on the inputted angle direction
    public void UpdateCompassDirection(float Angle)
    {
        Quaternion rotationLerp = Quaternion.Euler(Compass_Icon.rectTransform.rotation.eulerAngles.x, Compass_Icon.rectTransform.rotation.eulerAngles.y, Angle);
        Compass_Icon.rectTransform.rotation = Quaternion.Slerp(Compass_Icon.rectTransform.rotation, rotationLerp, LerpSpeed * Time.deltaTime);
    }

    //Updates the count down timer icon (used for start of game & checking player's finish condition)
    public void UpdateCountDownTimer(int Number)
    {
        Number = Mathf.Clamp(Number, 0, 3);
        Timer_Icon.gameObject.SetActive(true);

        switch (Number)
        {
            case 3: Timer_Icon.sprite = Three; break;
            case 2: Timer_Icon.sprite = Two; break;
            case 1: Timer_Icon.sprite = One; break;
            default: Timer_Icon.sprite = null; Timer_Icon.gameObject.SetActive(false); break;
        }
    }

    //Updates the bar on the left hand side which displays the speed the players going
    public void UpdateThrottleBar(float SpeedPercentage)
    {
        SpeedPercentage = Mathf.Clamp01(SpeedPercentage);

        Throttle_Rect.anchorMax = new Vector2(Throttle_Rect.anchorMax.x, SpeedPercentage);
        Throttle_Rect.sizeDelta = Vector2.Lerp(Throttle_Rect.sizeDelta, Vector2.zero, LerpSpeed * Time.deltaTime);
    }

    //Timer text in the top right corner for how long the player has played that particular level for.
    //It displays seconds and microseconds.
    public void UpdateTimerText(int Second, int Microsecond)
    {
        Timer_Text.text = $"{Second}.<size=60%>{Microsecond}";
    }
}