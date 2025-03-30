using UnityEngine;

public class NextStagePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            StageManager.Instance.NextStage();
    }
}
