
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
   public enum GameState { Idle, Running, Ended }
   [Header("Round")]
   public float roundDurationSeconds = 45f;
   [Header("UI")]
   public TMP_Text scoreText;
   public TMP_Text highscoreText;
   public TMP_Text stateText;
   public TMP_Text timerText;
   [Header("Worms")]
   public List<Worm> worms = new List<Worm>();
   public GameState State { get; private set; } = GameState.Idle;
   public int Score { get; private set; }
   private float timeLeft;
   private int highscore;
   public static event Action<int> OnScoreChanged;
   public static event Action<GameState> OnStateChanged;
   void Start()
   {
       highscore = PlayerPrefs.GetInt("Highscore", 0);
       SetScore(0);
       SetState(GameState.Idle);
       UpdateUI();
   }
   void Update()
   {
       if (State != GameState.Running) return;
       timeLeft -= Time.deltaTime;
       if (timeLeft <= 0f)
       {
           timeLeft = 0f;
           EndRound();
       }
       UpdateUI();
   }
   public void StartRound()
   {
       if (State == GameState.Running) return;
       SetScore(0);
       timeLeft = roundDurationSeconds;
       SetState(GameState.Running);
       foreach (var worm in worms)
       {
           if (worm != null)
               worm.SetActiveForRound(true);
       }
       UpdateUI();
   }
   public void EndRound()
   {
       if (State != GameState.Running) return;
       foreach (var worm in worms)
       {
           if (worm != null)
               worm.SetActiveForRound(false);
       }
       if (Score > highscore)
       {
           highscore = Score;
           PlayerPrefs.SetInt("Highscore", highscore);
           PlayerPrefs.Save();
       }
       SetState(GameState.Ended);
       UpdateUI();
   }
   public void RegisterHit()
   {
       if (State != GameState.Running) return;
       SetScore(Score + 1);
   }
   private void SetScore(int value)
   {
       Score = value;
       OnScoreChanged?.Invoke(Score);
   }
   private void SetState(GameState newState)
   {
       State = newState;
       OnStateChanged?.Invoke(State);
   }
   private void UpdateUI()
   {
       if (scoreText != null) scoreText.text = "Score: " + Score;
       if (highscoreText != null) highscoreText.text = "Highscore: " + highscore;
       if (stateText != null) stateText.text = "State: " + State;
       if (timerText != null) timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
   }
}