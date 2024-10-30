using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HunterShoot : MonoBehaviour
{
    public float damage = 10f;
    public float range = 30f;
    public ThirdPersonCamera camera;
    public ParticleSystem Muzzelflash;
    public GameObject Flareeffect;
    public float Muzzelforce = 30f;
    public AudioSource shootAudioSource;
    //public SoundClips SoundClips;

    private void Start()
    {
        // Инициализация звука выстрела (если требуется)
        // shootAudioSource.clip = SoundClips.shootSound;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && camera.currentState is AimingCameraState)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Воспроизведение звука выстрела (если требуется)
        // shootAudioSource.Play();

        // Воспроизведение вспышки выстрела
        Muzzelflash.Play();

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            //Debug.Log("Hit " + hit.transform.name);

            // Применение силы к объекту с Rigidbody
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * Muzzelforce);
            }

            // Создание эффекта попадания
            GameObject hitEffect = Instantiate(Flareeffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitEffect, 2f); // Удаление эффекта через 2 секунды
        }
    }
}
