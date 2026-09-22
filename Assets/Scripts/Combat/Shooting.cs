using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float damage = 100f;
    public float range = 5f;
    public float fireRate = 1f;
    
    public LightHalo lightHalo;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    
    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Play muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Raycast from camera
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            // Check if target is in halo
            float distanceToTarget = Vector3.Distance(transform.position, hit.point);
            
            if (distanceToTarget <= lightHalo.haloRadius)
            {
                // Target is in light - can hit
                Health target = hit.transform.GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }

                // Spawn impact effect
                if (impactEffect != null)
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            else
            {
                // Target is in darkness - bullet stops
                Debug.Log("Target in darkness - cannot hit!");
            }
        }
    }
}
