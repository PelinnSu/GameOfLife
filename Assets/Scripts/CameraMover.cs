using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float panSpeed = 5f; // Drag speed
    [SerializeField] private float zoomSpeed = 5f; // Zooming speed
    [SerializeField] private float minZoom = 2f, maxZoom = 10f; // Zoom limits
    [SerializeField] private float inertiaDamping = 0.95f; // Inertia effect

    private Vector3 dragOrigin;
    private Vector3 velocity = Vector3.zero;
    private bool isDragging = false;

    private void Update()
    {
        HandlePan();
    }

    private void HandlePan()
    {
        Vector3 move = Vector3.zero;

        // PC: Middle Mouse Button Drag
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
            velocity = Vector3.zero; // Reset inertia
        }
        else if (Input.GetMouseButton(2) && isDragging)
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            move = difference;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
            velocity = move / Time.deltaTime; // Store velocity for inertia
        }

        // Mobile: Touch Drag (1 Finger)
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
            move = new Vector3(-touchDelta.x, -touchDelta.y, 0) * panSpeed * Time.deltaTime;
            velocity = move / Time.deltaTime; // Update velocity
        }

        // Apply movement
        Camera.main.transform.position += move;

        // Inertia Effect (if not dragging)
        if (!isDragging)
        {
            Camera.main.transform.position += velocity * Time.deltaTime;
            velocity *= inertiaDamping; // Slowly reduce velocity
        }
    }
}
