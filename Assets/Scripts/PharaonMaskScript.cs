using UnityEngine;
using UnityEngine.Animations;

public class PharaonMaskScript : MonoBehaviour
{
    Vector3 initialPosition;
    public Light spotLight;

    public float moveSpeed = .5f;      // Speed at which the spotlight moves
    public float moveDistance = 2.5f;   // Maximum distance the spotlight will move back and forth

    private Vector3 targetPosition;
    private bool movingToTarget = true;

    void Start()
    {
        initialPosition = spotLight.transform.position;  // Store the initial position of the spotlight
        targetPosition = initialPosition + new Vector3(0, moveDistance, 0);  // Set the target position to the right
    }

    void Update()
    {
        // Move the spotlight back and forth between the initial position and the target position
        if (movingToTarget)
        {
            spotLight.transform.position = Vector3.Lerp(spotLight.transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            spotLight.transform.position = Vector3.Lerp(spotLight.transform.position, initialPosition, Time.deltaTime * moveSpeed);
        }

        // When the spotlight reaches the target position, change direction
        if (Vector3.Distance(spotLight.transform.position, targetPosition) < 0.1f)
        {
            movingToTarget = !movingToTarget;
            targetPosition = movingToTarget ? initialPosition + new Vector3(0, moveDistance, 0) : initialPosition;
        }
    }
}
