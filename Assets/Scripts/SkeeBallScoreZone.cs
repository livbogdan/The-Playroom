using UnityEngine;

public class SkeeBallScoreZone : MonoBehaviour
{
    public int scoreValue;
    public string zoneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SkeeBall"))
        {
            SkeeBallManager.Instance.AddScore(scoreValue, zoneName);
        }
    }

}