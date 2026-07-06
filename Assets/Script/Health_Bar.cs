using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    public Image fillBar;
    public TextMeshProUGUI healthText;

    [Header("Color Settings")]
    public Color maxHealthColor = Color.green; // Màu khi đầy máu (Xanh)
    public Color minHealthColor = Color.red;   // Màu khi sắp hết máu (Đỏ)

    private void Start()
    {
        fillBar.color = Color.green;
    }

    public void updateBar(int curHealth, int maxHealth)
    {
        float healthNormalized = (float)curHealth / maxHealth;
        fillBar.fillAmount = healthNormalized;

        // 3. Đổi màu mượt mà dựa trên tỷ lệ máu bằng Color.Lerp
        fillBar.color = Color.Lerp(minHealthColor, maxHealthColor, healthNormalized);

        // 4. Cập nhật Text hiển thị
        healthText.text = curHealth.ToString() + "/" + maxHealth.ToString();
    }
}