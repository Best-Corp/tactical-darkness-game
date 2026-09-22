using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float damage = 100f;
    public float range = 5f;
    public Transform firePoint;
    public LightHalo lightHalo;

    private Vector2 aimDirection;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (lightHalo == null)
            lightHalo = GetComponent<LightHalo>();

        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = new Vector3(1f, 0, 0);
            firePoint = fp.transform;
        }
    }

    void Update()
    {
        if (mainCam == null) return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = 10f;
        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);

        aimDirection = (mouseWorld - (Vector2)transform.position).normalized;

        if (firePoint != null)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector2 origin = firePoint != null ? (Vector2)firePoint.position : (Vector2)transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, aimDirection, range);

        if (hit.collider != null)
        {
            float distance = Vector2.Distance(transform.position, hit.point);

            if (lightHalo != null && distance <= lightHalo.haloRadius)
            {
                Health target = hit.collider.GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }
        }

        Debug.DrawRay(origin, aimDirection * range, Color.red, 0.5f);
    }
}
