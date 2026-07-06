using System;
using System.Collections;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    [SerializeField] private float safeDuration = 1.5f;
    [SerializeField] private float dangerousDuration = 1.5f;
    private BoxCollider2D spikeCollider;
    private Animator animator;
    private Health_Player healthPlayer;

    private void Awake()
    {
        spikeCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(SpikeTrapRoutine());
    }

    IEnumerator SpikeTrapRoutine()
    {
        while (true)
        {
            spikeCollider.enabled = false;
            animator.SetBool("isActive", false);
            yield return new WaitForSeconds(safeDuration);

            animator.SetBool("isActive", true);
            spikeCollider.enabled = true;
            yield return new WaitForSeconds(dangerousDuration);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Player take Damage");
            healthPlayer = collision.gameObject.GetComponent<Health_Player>();
            healthPlayer.TakeDamage(10);
        }
    }
}
