using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Camera mainCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 move = moveInput.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotate toward mouse
        if (mainCam != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(mousePos);
            Vector2 direction = mouseWorld - rb.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle - 90f);
        }
    }
}
