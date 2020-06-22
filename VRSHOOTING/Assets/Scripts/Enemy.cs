
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] AudioClip spawnClip;
    [SerializeField] AudioClip hitClip;
    [SerializeField] Collider enemyCollider;
    [SerializeField] Renderer enemyRenderer;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(spawnClip);

    }
    void OnHitBullet() {

        audioSource.PlayOneShot(hitClip);

        enemyCollider.enabled = false;
        enemyRenderer.enabled = false;

        Destroy(gameObject, 1f);
    }
}
