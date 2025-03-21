using UnityEngine;
using System.Collections;

public class PikaxeScript : MonoBehaviour
{
    public float jumpHeight = 1.5f;  // The height of the jump
    public float jumpSpeed = 0.7f;   // Speed at which the pickaxe rises and falls
    public float waitTime = 0.5f;    // Time before the next jump
    public float rotationSpeed = 200f;  // Rotation speed of the pickaxe (degrees per second)

    Vector3 initialPosition;
    float time;

    // Define the AnimationCurve (can be modified in the Unity Inspector)
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    void Start()
    {
        initialPosition = transform.position;  // Store the initial position of the pickaxe
    }

    void Update()
    {
        // Increment time by deltaTime * speed
        time += Time.deltaTime * jumpSpeed;

        // Use the AnimationCurve to get a smooth jump
        float curveValue = curve.Evaluate(time % 1f);  // Loop the time for continuous jump effect

        // Apply the curve value to the Y position, scaled by jumpHeight
        float yOffset = curveValue * jumpHeight;

        // Update the pickaxe position
        transform.position = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
    }

    // This will be called when the pickaxe is clicked
    private void OnMouseDown()
    {
        // Start the rotation and disappearance coroutine
        StartCoroutine(RotateAndDisappear());
    }

    // Coroutine to rotate the pickaxe and make it disappear
    IEnumerator RotateAndDisappear()
    {
        float rotationAmount = 0f;  // Amount of rotation
        Quaternion initialRotation = transform.rotation;

        // Rotate the pickaxe smoothly
        while (rotationAmount < 360f)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, rotationStep, 0f);  // Rotate around Z axis (you can adjust the axis as needed)
            rotationAmount += rotationStep;
            yield return null;
        }

        // After rotation, wait for a short delay before disappearing
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds before hiding

        // Make the pickaxe disappear (could be destroyed or hidden)
        gameObject.SetActive(false);  // Hides the pickaxe
    }
}
