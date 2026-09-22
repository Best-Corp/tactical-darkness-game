using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float range = 15f;
    public LayerMask hitMask;

    private PlayerAim aim;
    private DarknessOverlay fog;

    void Start()
    {
        aim = GetComponent<PlayerAim>();
        fog = GetComponent<DarknessOverlay>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (aim == null || aim.firePoint == null) return;

        float maxRange = fog != null ? Mathf.Min(range, fog.haloRadius) : range;

        Vector2 origin = aim.firePoint.position;
        Vector2 dir = aim.firePoint.right;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxRange, hitMask);

        if (hit.collider != null)
        {
            GameObject target = hit.collider.gameObject;

            // Check if it's a combatant
            Health health = target.GetComponent<Health>();
            if (health == null)
                health = target.GetComponentInParent<Health>();

            if (health != null)
            {
                health.TakeDamage(1);
            }
        }

        Debug.DrawRay(origin, dir * maxRange, Color.red, 0.3f);
    }
}
