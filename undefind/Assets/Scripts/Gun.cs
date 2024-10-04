using UnityEngine;
using static UnityEngine.GraphicsBuffer;
//using static AutomaticGunScriptLPFP;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 30f;
    public Camera Maincamera;
    public ParticleSystem Muzzelflash;
    public GameObject Flareeffect;
    public float Muzzelforce = 30f;
    public AudioSource shootAudioSource;
    //public soundClips SoundClips;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }

        void Start()
        {
            //shootAudioSource.clip = SoundClips.shootSound;
        }

        void shoot()
        {
            //shootAudioSource.Play();
            
            Muzzelflash.Play();
            RaycastHit hit;
            if (Physics.Raycast(Maincamera.transform.position, Maincamera.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    //target.TakeDamage(damage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(hit.normal * Muzzelforce);
                }
                GameObject MuzzelGO = Instantiate(Flareeffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(MuzzelGO, 2f);
            }
        }
    }
}