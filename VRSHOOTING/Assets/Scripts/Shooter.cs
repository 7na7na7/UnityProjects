using UnityEngine;

public class Shooter : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunBarrelEnd;

    [SerializeField] ParticleSystem gunParticle;
    [SerializeField] AudioSource gunAudioSource;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            Shoot();
        }

    }
    void Shoot()
    {
        Instantiate(bulletPrefab, gunBarrelEnd.position, gunBarrelEnd.rotation);

        gunParticle.Play();
        gunAudioSource.Play();
    }
}
