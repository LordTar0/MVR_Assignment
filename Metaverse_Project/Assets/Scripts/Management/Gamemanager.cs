using System.Collections;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager _Instance { get => instance; }

    Timer timer;
    [SerializeField] LevelData levelData;
    Level level;

    bool LevelCreated;
    bool PlayerPositioned;

    private void Awake()
    {
        instance = this;
        StartGame();
    }

    private void FixedUpdate()
    {
        while (!timer.IsTimerUp()) //Timer counts down and updates the onscreen UI instance.
        {
            timer.GetSecondsMiliseconds(out int Seconds, out int Miliseconds);

            PlayerUI._Instance?.UpdateTimerText(Seconds, Miliseconds);

            return;
        }

        LevelFinished();
    }

    private bool InitialiseLevel()
    {
        if (level == null)
        {
            //Creates the level, finds the goal and subscribes to the finished level action.
            GameObject levelOBJ = Instantiate(levelData.LevelPrefab);
            level = levelOBJ.GetComponent<Level>();
            Goal goal = level.GetGoalObj();
            goal.Finish_Action += LevelFinished;
        }

        LevelCreated = true;
        return LevelCreated;
    }

    private bool InitialisePlayer()
    {
        if (PlayerMovement._Instance == null) return false;

        PlayerMovement player = PlayerMovement._Instance;

        //Moves the player to the level's stated position & rotation.
        player.transform.position = levelData.PlayerSpawnPosition;
        player.transform.rotation = Quaternion.Euler(0,levelData.PlayerSpawnDirection,0);

        //Turns on the kinematic trigger for the player so it is still when getting ready
        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        playerRB.isKinematic = true;

        PlayerPositioned = true;

        return PlayerPositioned;
    }

    public void StartGame()
    {
        StartCoroutine(StartLevel());
    }
    public void RestartGame()
    {
        StartCoroutine(RestartLevel());
    }

    //TEMP (Remove in full game)
    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    private IEnumerator RestartLevel()
    {
        TransitionScreenManager.Instance.Transition(false);

        while (!TransitionScreenManager.Instance.TransitionFinishCheck("Transition_Start"))
        {
            yield return null;
        }

        StartGame();
    }

    private IEnumerator StartLevel()
    {
        //Creates the timer
        timer = new();
        timer.SetReverseTimer_Active();
        timer.SetStartTime(99);

        //Initialises the selected level
        while (!InitialiseLevel())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        //Resets level objects
        level.ResetObjects();

        //Sets shown UI to game UI
        UIManagement._Instance.UpdateUIShown(UIShown.Game);

        while (!InitialisePlayer())
        {
            yield return null;
        }

        //Giving the player the Goalscript
        PlayerMovement._Instance.GetGoal(level.GetGoalObj());

        //Ready to show the scene, so setting the transition manager load to true
        TransitionScreenManager.Instance.Transition(true);

        while (!TransitionScreenManager.Instance.TransitionFinishCheck("Transition_Finished"))
        {
            yield return null;
        }

        timer.SetCurrentTime(0);
        PlayerUI._Instance.UpdateCountDownTimer(0);

        yield return new WaitForSeconds(2f);

        //CountDown Sequence
        PlayerUI._Instance.UpdateCountDownTimer(3);
        yield return new WaitForSeconds(1f);
        PlayerUI._Instance.UpdateCountDownTimer(2);
        yield return new WaitForSeconds(1f);
        PlayerUI._Instance.UpdateCountDownTimer(1);
        yield return new WaitForSeconds(1f);
        PlayerUI._Instance.UpdateCountDownTimer(0);

        Rigidbody playerRB = PlayerMovement._Instance.GetComponent<Rigidbody>();
        playerRB.isKinematic = false;

        level.StartObjects();

        PlayerMovement._Instance.EnableInput();
        timer.EnableTimer();
    }

    private void LevelFinished()
    {
        timer.DisableTimer();
        PlayerMovement._Instance.DisableInput();
        UIManagement._Instance.UpdateUIShown(UIShown.Score);

        timer.GetSecondsMiliseconds(out int Seconds, out int Miliseconds);

        int score = GetTimeScore();

        ScoreUI._Instance.UpdateSummary(Seconds, Miliseconds, score, GetRank(score));

        Debug.Log($"Finished!");
    }

    //Gets the player's rank after finishing the level
    private Rank GetRank(int Score)
    {
        ScoreData scores = levelData.ScoreData;

        if (Score >= scores.S_Rank) { return Rank.S; }
        if (Score >= scores.A_Rank) { return Rank.A; }
        if (Score >= scores.B_Rank) { return Rank.B; }
        if (Score >= scores.C_Rank) { return Rank.C; }
        if (Score >= scores.C_Rank) { return Rank.D; }

        return Rank.F;
    }

    //Gets a score based on the time completed
    private int GetTimeScore()
    {
        TimeScore[] timeScores = levelData.TimeScores;

        foreach (TimeScore timeScore in timeScores)
        {
            if (timer.CheckTimer() < timeScore.time) { return timeScore.Score; }
        }

        return 0;
    }


    //TEMP (Remove in full game)
    private IEnumerator Quit()
    {
        TransitionScreenManager.Instance.Transition(false);

        while (!TransitionScreenManager.Instance.TransitionFinishCheck("Transition_Start"))
        {
            yield return null;
        }

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif

        Application.Quit();
    }
}