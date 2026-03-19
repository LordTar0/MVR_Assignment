using UnityEngine;

public class TransitionScreenManager : MonoBehaviour
{
    private static TransitionScreenManager _instance;
    public static TransitionScreenManager Instance { get => _instance; }

    protected Animator transitonAnimator;

    private void Awake()
    {
        transitonAnimator = GetComponentInChildren<Animator>();
        Singleton();
    }

    //Makes sure there's only one of this object
    private void Singleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(_instance);
    }

    //Updates the animator to trigger the transition screen.
    public void Transition(bool isLoaded)
    {
        transitonAnimator.SetBool("IsLoaded", isLoaded);
    }

    //Checks to see if the animation is completed (mainly used in IEnumerators to sequence it into another part)
    public bool TransitionFinishCheck(string stateName)
    {
        return IsTransitionFinishedPlaying(transitonAnimator.GetCurrentAnimatorStateInfo(0)) && transitonAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    //Timer check on the animation state. Used in the check above.
    private bool IsTransitionFinishedPlaying(AnimatorStateInfo stateInfo)
    {
        float c_Time = stateInfo.normalizedTime % 1;

        return c_Time >= 0.9f;
    }
}
