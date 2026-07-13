using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float noiseIntensity = 0.8f;
    public float acceleration = 50f; // Độ nhạy khi đổi hướng bay

    [Header("Collision Bounce")]
    [SerializeField] private float bounceForceMultiplier = 8f; 

    private Transform dogeTransform;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Đảm bảo bắt va chạm liên tục để không bị xuyên lưới
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        GameObject doge = GameObject.FindWithTag("Player");
        if (doge != null) dogeTransform = doge.transform;
    }

    void FixedUpdate()
    {
        if (dogeTransform == null) return;

        // 1. Tính toán hướng bay mong muốn hướng về phía Doge
        Vector2 targetDirection = (dogeTransform.position - transform.position).normalized;
        Vector2 noise = Random.insideUnitCircle * noiseIntensity;
        Vector2 desiredVelocity = (targetDirection + noise).normalized * speed;

        // 2. Thay vì gán thẳng, ta dùng lực gia tốc để thay đổi vận tốc dần dần 
        // Điều này giúp nét vẽ khi đẩy Ong ra, Ong sẽ thực sự bị bật lại rồi mới từ từ bay vòng lại sau
        Vector2 velocityError = desiredVelocity - rb.linearVelocity;
        Vector2 movementForce = velocityError * acceleration;
        
        rb.AddForce(movementForce * rb.mass);

        // Giới hạn tốc độ tối đa để tránh Ong bị gia tốc quá đà
        if (rb.linearVelocity.magnitude > speed * 1.5f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (speed * 1.5f);
        }

        // Lật mặt Sprite
        if (rb.linearVelocity.x > 0.05f) transform.localScale = new Vector3(-1, 1, 1);
        else if (rb.linearVelocity.x < -0.05f) transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null) GameManager.Instance.LoseGame();
            return;
        }

        if (collision.gameObject.name.Contains("DrawLine") || collision.gameObject.CompareTag("DrawLine"))
        {
            Rigidbody2D lineRb = collision.gameObject.GetComponent<Rigidbody2D>();
        
            // Lấy hướng phản lực từ điểm va chạm
            Vector2 contactNormal = collision.GetContact(0).normal; 
        
            // Lực phản hồi đẩy Ong văng ngược ra ngay lập tức
            float calculatedForce = speed * bounceForceMultiplier;
            rb.AddForce(contactNormal * calculatedForce, ForceMode2D.Impulse);

            if (lineRb != null && lineRb.simulated)
            {
                // Ép thêm một chút lực hướng lên trên để nét vẽ có xu hướng "bật nảy lên"
                Vector2 bounceDirection = (contactNormal + Vector2.up * 0.5f).normalized;
                lineRb.AddForce(bounceDirection * (calculatedForce * 0.5f), ForceMode2D.Impulse);
            }
        }
    }
}