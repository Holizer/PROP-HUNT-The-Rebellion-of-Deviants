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
        // ������������� ����� �������� (���� ���������)
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
        // ��������������� ����� �������� (���� ���������)
        // shootAudioSource.Play();

        // ��������������� ������� ��������
        Muzzelflash.Play();

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            //Debug.Log("Hit " + hit.transform.name);

            // ���������� ���� � ������� � Rigidbody
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * Muzzelforce);
            }

            // �������� ������� ���������
            GameObject hitEffect = Instantiate(Flareeffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitEffect, 2f); // �������� ������� ����� 2 �������
        }
    }
}
