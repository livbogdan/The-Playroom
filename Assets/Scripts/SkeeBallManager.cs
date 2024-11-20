  using UnityEngine;
  using TMPro;

  public class SkeeBallManager : MonoBehaviour
  {
      public static SkeeBallManager Instance { get; private set; }
    
      [System.Serializable]
      public class ScoreZone
      {
          public string zoneName;
          public int scoreValue;
          public Collider zoneCollider;
      }

      public ScoreZone[] scoreZones;
      public string ballTag = "SkeeBall";
      public TextMeshProUGUI scoreText;
      public int maxBalls = 9;
    
      private int currentScore;
      private int ballsRemaining;

      private void Awake()
      {
          Instance = this;
          ResetGame();
      }

      private void Start()
      {
          // Ensure all zone colliders are triggers
          foreach (ScoreZone zone in scoreZones)
          {
              zone.zoneCollider.isTrigger = true;
          }
      }

      public void ResetGame()
      {
          currentScore = 0;
          ballsRemaining = maxBalls;
          UpdateUI();
      }

      // Change to OnCollisionEnter for physical collisions
      private void OnCollisionEnter(Collision collision)
      {
          if (collision.gameObject.CompareTag(ballTag))
          {
              // Find which zone was hit
              foreach (ScoreZone zone in scoreZones)
              {
                  if (collision.contacts[0].thisCollider == zone.zoneCollider)
                  {
                      AddScore(zone.scoreValue, zone.zoneName);
                      break;
                  }
              }
          }
      }

      public void AddScore(int points, string zoneName)
      {
          currentScore += points;
          ballsRemaining--;
        
          Debug.Log($"Scored {points} points in {zoneName}! Total: {currentScore}");
          UpdateUI();

          if (ballsRemaining <= 0)
          {
              EndGame();
          }
      }

      private void UpdateUI()
      {
          if (scoreText != null)
          {
              scoreText.text = $"Score: {currentScore}\nBalls: {ballsRemaining}";
          }
      }

      private void EndGame()
      {
          Debug.Log($"Game Over! Final Score: {currentScore}");
      }
  }