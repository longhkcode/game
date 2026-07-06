using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    
    [Header("Patrol Settings")]
    public Transform posA;
    public Transform posB;
    
    private Transform currentPatrolTarget;
    private Transform playerTransform;
    private bool isChasing = false;
    void Start()
    {
        // Ban đầu, đặt mục tiêu đi tuần là điểm A
        if (posA != null)
        {
            currentPatrolTarget = posA;
        }
    }
    void Update()
    {
        if (isChasing && playerTransform != null)
        {
            // Trạng thái 1: Đuổi theo Player
            MoveTowards(playerTransform.position);
        }
        else
        {
            // Trạng thái 2: Đi tuần tra giữa posA và posB
            Patrol();
        }
    }

    private void Patrol()
    {
        if (posA == null || posB == null) return;
        // Di chuyển tới điểm tuần tra hiện tại
        MoveTowards(currentPatrolTarget.position);
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
        {
            if (currentPatrolTarget == posA)
            {
                currentPatrolTarget = posB;
            }
            else
            {
                currentPatrolTarget = posA;
            }
        }
    }

    private void MoveTowards(Vector2 target)
    {
        // 1. Di chuyển Goblin về phía mục tiêu
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    
        // 2. Lấy scale hiện tại của Goblin ra để xử lý
        Vector3 scale = transform.localScale;

        // 3. So sánh tọa độ X để quyết định hướng quay mặt
        if (target.x > transform.position.x)
        {
            // Nếu mục tiêu ở bên phải -> Đảm bảo scale.x mang dấu dương (quay sang phải)
            scale.x = Mathf.Abs(scale.x);
        }
        else if (target.x < transform.position.x)
        {
            // Nếu mục tiêu ở bên trái -> Đảm bảo scale.x mang dấu âm (quay sang trái)
            scale.x = -Mathf.Abs(scale.x);
        }

        // 4. Áp dụng scale mới ngược trở lại cho transform
        transform.localScale = scale;
    }
    
    // Hàm nhận dữ liệu từ DetectionZone (Object con)
    public void SetTarget(Transform player, bool chase)
    {
        playerTransform = player; // 1. Lưu lại thông tin Player
        isChasing = chase;        // 2. Cập nhật trạng thái (Đuổi theo hay Thôi)
    
        // 3. Nếu không đuổi nữa (chase == false), tìm điểm tuần tra gần nhất
        if (!chase)
        {
            ChooseClosestPatrolPoint();
        }
    }

    private void ChooseClosestPatrolPoint()
    {
        if (posA == null || posB == null) return;

        float distanceToA = Vector2.Distance(transform.position, posA.position);
        float distanceToB = Vector2.Distance(transform.position, posB.position);

        // Điểm nào gần hơn thì chọn điểm đó làm mục tiêu tiếp theo
        currentPatrolTarget = (distanceToA < distanceToB) ? posA : posB;
    }
}
