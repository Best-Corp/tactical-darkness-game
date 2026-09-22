using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform visual;
    public Transform firePoint;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (mainCam == null || visual == null) return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -mainCam.transform.position.z;
        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        Vector2 dir = mouseWorld - (Vector2)transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        visual.rotation = Quaternion.Euler(0, 0, angle);

        if (firePoint != null)
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
