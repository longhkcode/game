using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Bắt buộc để dùng Coroutine

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float survivalTime = 3f; // Thời gian cần thủ để thắng
    
    [Header("UI Panels")]
    [SerializeField] private GameObject winPanel;  // Kéo GameWinUI vào đây
    [SerializeField] private GameObject losePanel; // Kéo GameOverUI vào đây
    
    [Header("UI Animation Settings")]
    [SerializeField] private float slideDuration = 3f; // Tốc độ trượt (giây)
    [SerializeField] private float startYPosition = 1200f; // Độ cao bắt đầu trượt ở ngoài màn hình

    private float _timer;
    private bool _isGamePlaying = false;
    private bool _isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // Đảm bảo tốc độ game bình thường khi mới vào màn chơi
        Time.timeScale = 1f;

        // Ẩn các panel khi mới vào game
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    // Hàm này được gọi từ SpamEnemy khi bắt đầu thả ong
    public void StartCountdown()
    {
        if (_isGameOver) return;
        
        _timer = survivalTime;
        _isGamePlaying = true;
        Debug.Log("Bắt đầu đếm ngược sinh tồn: " + survivalTime + " giây!");
    }

    void Update()
    {
        if (_isGamePlaying && !_isGameOver)
        {
            _timer -= Time.deltaTime;
            
            // Nếu hết thời gian đếm ngược mà chưa thua -> Chiến thắng!
            if (_timer <= 0)
            {
                WinGame();
            }
        }
    }

    public void WinGame()
    {
        if (_isGameOver) return;
        _isGameOver = true;
        _isGamePlaying = false;
        
        Debug.Log("CHÚC MỪNG! BẠN ĐÃ CHIẾN THẮNG!");
        if (winPanel != null) 
        {
            StartCoroutine(SlideInPanel(winPanel)); // Kích hoạt hiệu ứng trượt bảng thắng
        }
    }

    public void LoseGame()
    {
        if (_isGameOver) return;
        _isGameOver = true;
        _isGamePlaying = false;

        Debug.Log("GAME OVER! DOGE BỊ ĐỐT RỒI!");
        if (losePanel != null) 
        {
            StartCoroutine(SlideInPanel(losePanel)); // Kích hoạt hiệu ứng trượt bảng thua
        }
    }

    // Coroutine xử lý trượt UI mượt mà từ trên xuống
    private IEnumerator SlideInPanel(GameObject panel)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            panel.SetActive(true);
            Time.timeScale = 0f; // Dừng game lập tức nếu không tìm thấy RectTransform
            yield break;
        }

        // 1. Đặt vị trí ban đầu của Panel ở trên cao ngoài tầm nhìn màn hình
        Vector2 targetPosition = Vector2.zero; 
        rectTransform.anchoredPosition = new Vector2(0, startYPosition);
        
        // 2. Bật Panel lên
        panel.SetActive(true);

        // 3. Thực hiện trượt mượt mà bằng vòng lặp thời gian thực (UnscaledDeltaTime để tránh bị ảnh hưởng bởi Time.timeScale nếu có dùng ở nơi khác)
        float elapsedTime = 0f;
        Vector2 startPosition = rectTransform.anchoredPosition;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Dùng unscaledDeltaTime để đảm bảo UI trượt mượt mà
            float t = elapsedTime / slideDuration;
            
            t = Mathf.SmoothStep(0, 1, t); // Làm mượt chuyển động ở điểm đầu và điểm cuối

            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // Đảm bảo panel khớp chuẩn vị trí tâm (0,0)
        rectTransform.anchoredPosition = targetPosition;

        // 4. SAU KHI UI TRƯỢT XUỐNG XONG -> DỪNG GAME TOÀN BỘ
        Time.timeScale = 0f; 
        Debug.Log("Đã dừng game!");
    }

    public void RestartLevel()
    {
        // Quan trọng: Trả lại tốc độ game về 1 trước khi load cảnh mới để màn sau không bị đóng băng
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        // 3. Kiểm tra xem Index tiếp theo có hợp lệ trong Build Settings không
        // (Nếu nextSceneIndex nhỏ hơn tổng số Scene hiện có thì load tiếp)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Nếu là Map cuối cùng rồi thì quay về Menu chính
            Debug.Log("Bạn đã phá đảo game! Quay lại Menu.");
            SceneManager.LoadScene("Menu");
        }
    }
}