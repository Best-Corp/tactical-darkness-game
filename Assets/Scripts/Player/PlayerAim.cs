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

        // Validate screen position
        if (mouseScreen.x < 0 || mouseScreen.x > Screen.width ||
            mouseScreen.y < 0 || mouseScreen.y > Screen.height)
            return;

        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        Vector2 dir = mouseWorld - (Vector2)transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        visual.rotation = Quaternion.Euler(0, 0, angle);

        if (firePoint != null)
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
