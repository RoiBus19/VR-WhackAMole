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
   public TextMeshProUGUI scoreText;
   public TextMeshProUGUI highscoreText;
   public TextMeshProUGUI stateText;
   [Header("Worms")]
   public List<Worm> worms = new List<Worm>();
   [Header("Audio")]
   public AudioSource sfxSource;
   public AudioClip hitClip;
   public GameState State { get; private set; } = GameState.Idle;
   public int Score { get; private set; }
   private float _timeLeft;
   private int _highscore;
   public static event Action<int> OnScoreChanged;
   public static event Action<GameState> OnStateChanged;
   void Start()
   {
       _highscore = PlayerPrefs.GetInt("Highscore", 0);
       SetState(GameState.Idle);
       SetScore(0);
       UpdateUI();
   }
   void Update()
   {
       if (State != GameState.Running) return;
       _timeLeft -= Time.deltaTime;
       if (_timeLeft <= 0f)
       {
           EndRound();
       }
   }
   public void StartRound()
   {
       if (State == GameState.Running) return;
       SetScore(0);
       _timeLeft = roundDurationSeconds;
       SetState(GameState.Running);
       foreach (var w in worms)
           w.SetActiveForRound(true);
   }
   public void EndRound()
   {
       if (State != GameState.Running) return;
       foreach (var w in worms)
           w.SetActiveForRound(false);
       SetState(GameState.Ended);
       if (Score > _highscore)
       {
           _highscore = Score;
           PlayerPrefs.SetInt("Highscore", _highscore);
           PlayerPrefs.Save();
       }
       UpdateUI();
   }
   public void ResetToIdle()
   {
       foreach (var w in worms)
           w.SetActiveForRound(false);
       SetScore(0);
       SetState(GameState.Idle);
       UpdateUI();
   }
   public void RegisterHit()
   {
       if (State != GameState.Running) return;
       SetScore(Score + 1);
       if (sfxSource && hitClip)
           sfxSource.PlayOneShot(hitClip);
   }
   private void SetScore(int value)
   {
       Score = value;
       OnScoreChanged?.Invoke(Score);
       UpdateUI();
   }
   private void SetState(GameState s)
   {
       State = s;
       OnStateChanged?.Invoke(State);
       UpdateUI();
   }
   private void UpdateUI()
   {
       if (scoreText) scoreText.text = $"Score: {Score}";
       if (highscoreText) highscoreText.text = $"Highscore: {_highscore}";
       if (stateText) stateText.text = $"State: {State}";
   }
}