using System;
using UnityEngine;
using System.Collections;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    
    public float alertDistance = 1.0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckDistanceToBees();
    }

    void CheckDistanceToBees()
    {
        // Tìm tất cả các GameObject có component EnemyAI (tất cả con ong trong scene)
        EnemyAI[] allBees = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        
        bool isAnyBeeTooClose = false;

        foreach (EnemyAI bee in allBees)
        {
            if (bee != null)
            {
                float distance = Vector2.Distance(transform.position, bee.transform.position);
                if (distance < alertDistance)
                {
                    isAnyBeeTooClose = true;
                    break;
                }
            }
        }
        // Cập nhật trạng thái Animator dựa trên kết quả
        if (isAnyBeeTooClose)
        {
            _animator.SetBool("SoHai", true);
        }
        else
        {
            // Nếu ong bay đi xa (hoặc chưa tới), quay lại trạng thái bình thường
            _animator.SetBool("SoHai", false); 
        }
    }
}