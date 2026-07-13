using System;
using System.Collections;
using UnityEngine;

namespace Script
{
    public class SpamEnemy : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private GameObject _enemySpam; // Prefab con Ong
        [SerializeField] private Transform _spamPos;    // Vị trí tổ ong
        
        [SerializeField] private int totalEnemies = 25;       // Tổng số ong trong 1 đợt xả
        [SerializeField] private float spawnInterval = 0.8f;  // Khoảng thời gian sinh giữa mỗi con (sinh cực nhanh)

        private bool _isSpawning = false;

        // Hàm này được LineController của bạn gọi khi nhấc chuột Up(0)
        public void StartSpawning()
        {
            if (!_isSpawning)
            {
                _isSpawning = true;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.StartCountdown();
                }
                StartCoroutine(_SpamEnemyCoroutine());
            }
        }

        private IEnumerator _SpamEnemyCoroutine()
        {
            // 1. Phun toàn bộ đàn ong ra ngoài màn hình
            for (int i = 0; i < totalEnemies; i++)
            {
                Instantiate(_enemySpam, _spamPos.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}