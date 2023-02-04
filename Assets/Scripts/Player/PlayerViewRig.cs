using UnityEngine;

public class PlayerViewRig : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed of movement along the forward axis
    public float lerpSpeed = 3.0f; // Speed of the lerp animation

    private Vector3 targetPosition; // Target position to move towards

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            targetPosition = targetPosition + transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetPosition = targetPosition - transform.forward * moveSpeed * Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }
}
