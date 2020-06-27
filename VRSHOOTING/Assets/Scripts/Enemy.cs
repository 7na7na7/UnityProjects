
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject hpSlider;
    [SerializeField] AudioClip spawnClip;
    [SerializeField] AudioClip hitClip;
    [SerializeField] Collider enemyCollider;
    [SerializeField] Renderer enemyRenderer;

    public float HP = 3;

    private int hp;
    AudioSource audioSource;

    private float hpDownValue = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(spawnClip);
        hpDownValue = 1 / HP;
    }
    void OnHitBullet()
    {
        audioSource.PlayOneShot(hitClip);
        HP-=0.5f;
        hpSlider.GetComponent<Slider>().value -= hpDownValue/2f;
        if (HP == 0)
        {
           hpSlider.SetActive(false);
           enemyCollider.enabled = false;
            enemyRenderer.enabled = false;
            FindObjectOfType<ScoreManager>().scoreValue += 10;
            Destroy(gameObject, 1f);
        }
    }
}
