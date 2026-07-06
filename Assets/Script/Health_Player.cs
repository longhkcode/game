using System.Collections;
using UnityEngine;


    public class Health_Player :  MonoBehaviour
    {
        [SerializeField] private float currentHp;
        [SerializeField] private float maxHp = 100;
        private Animator animator;
        [SerializeField] private Health_Bar healthBar;

        void Start()
        {
            animator = GetComponent<Animator>();
            currentHp = maxHp;
        }

        public void Heal(float healAmount)
        {
            currentHp += healAmount;
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
            healthBar.updateBar((int)currentHp, (int)maxHp);
        }

        public void TakeDamage(float damageAmount)
        {
            currentHp -= damageAmount;
            if (currentHp <= 0)
            {
                Debug.Log("Player was Died");
                animator.SetBool("isDead", true);
                StartCoroutine(WaitAndPauseGame());
            }
            healthBar.updateBar((int)currentHp, (int)maxHp);
        }

        IEnumerator WaitAndPauseGame()
        {
            yield return new WaitForSeconds(1.5f);
            Time.timeScale = 0;
        }
    }