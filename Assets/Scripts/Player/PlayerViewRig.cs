using UnityEngine;

public class PlayerViewRig : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed of movement along the forward axis
    public float zoomSpeed = 2.0f; // Speed of movement along the forward axis
    public float lerpSpeed = 3.0f; // Speed of the lerp animation

    public float maxMovePos = 0;
    public float minMovePos = -22;

    public float maxZoom = 40;
    public float minZoom = 9;

    public Camera cam;

    private Vector3 targetPosition; // Target position to move towards
    private float targetZoom;

    private void Awake()
    {
        targetPosition = transform.position;
        targetZoom = cam.fieldOfView;
    }

    private void Update()
    {
        targetZoom = Mathf.Clamp(targetZoom - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime, minZoom, maxZoom);

        if (Input.GetKey(KeyCode.W))
        {
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, Mathf.Clamp(targetPosition.z + moveSpeed * Time.deltaTime, minMovePos, maxMovePos));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, Mathf.Clamp(targetPosition.z - moveSpeed * Time.deltaTime, minMovePos, maxMovePos));
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, lerpSpeed * Time.deltaTime);
    }
}
