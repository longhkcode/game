using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private EnemyController _enemyController;

    void Start()
    {
        // Lấy script cha nằm trên con Goblin
        _enemyController= GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu Player bước vào vùng check
        if (collision.CompareTag("Player"))
        {
            _enemyController.SetTarget(collision.transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Nếu Player ra khỏi vùng check
        if (collision.CompareTag("Player"))
        {
            _enemyController.SetTarget(null, false);
        }
    }
}