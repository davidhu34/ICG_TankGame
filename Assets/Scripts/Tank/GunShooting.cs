using UnityEngine;
using UnityEngine.UI;

public class GunShooting : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.
    public AudioSource gunAudio, LAudio;                           // Reference to the audio source.
    public AudioClip Lcharge, Lblast;
    public GameObject cyl1, cyl2;
    public Light LLight;
    public Slider LCDSlider;

    float timer, Ltimer;                                    // A timer to determine when to fire.
    Ray shootRay;                                   // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.

    Light gunLight;                                 // Reference to the light component.
    float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.

    void Awake()
    {
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        Ltimer = 0f;
    }

    void Update()
    {
        if (!LLight.enabled)
        { Ltimer -= Time.deltaTime; }
        timer += Time.deltaTime;

        if (Input.GetButton("Fire2") && timer >= timeBetweenBullets)
        {
            Shoot();
        }
        if (Input.GetButton("Fire3") && Ltimer < 0f)
        {
            LShoot();
            Ltimer = 10f;
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
        LCDSlider.value = Ltimer;
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
        LLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range))
        {
			TankHealth targetHealth = shootHit.collider.GetComponent<TankHealth>();
			if (targetHealth != null)
			{
				targetHealth.TakeDamage(5);
			}
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
    void LShoot ()
    {
        Ltimer = 10f;
        LAudio.clip = Lcharge;
        LAudio.Play();
        Invoke("Blast", 1f);
        GameObject cylobj = Instantiate(cyl1, transform.position + transform.forward * 50, transform.rotation * Quaternion.Euler(90f, 0f, 0f)) as GameObject;
        cylobj.transform.parent = transform;
        LLight.enabled = true;
    }
    void Blast()
    {
        LAudio.clip = Lblast;
        LAudio.Play();
        GameObject cylobj = Instantiate(cyl2, transform.position + transform.forward * 50, transform.rotation * Quaternion.Euler(90f, 0f, 0f)) as GameObject;
        cylobj.transform.parent = transform;
        Invoke("off", 1f);
    }
    void off()
    {
        LLight.enabled = false;
    }
}