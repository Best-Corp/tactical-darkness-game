using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float damage = 100f;
    public float range = 5f;
    public Transform firePoint;
    public LightHalo lightHalo;

    private Vector2 aimDirection;

    void Update()
    {
        // Mouse aim
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = (mousePos - (Vector2)transform.position).normalized;

        // Rotate fire point
        if (firePoint != null)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Shoot
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            firePoint != null ? firePoint.position : transform.position,
            aimDirection,
            range
        );

        if (hit.collider != null)
        {
            float distance = Vector2.Distance(transform.position, hit.point);

            if (distance <= lightHalo.haloRadius)
            {
                Health target = hit.collider.GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }
        }
    }
}
