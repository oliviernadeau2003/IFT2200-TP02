using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float minAngle = -3f;  // Angle min d'oscillation
    public float maxAngle = 2f;   // Angle max d'oscillation
    public float speed = 1f;      // Vitesse de l'oscillation

    public float openDistance = 1.3f; // Distance d'ouverture de la porte
    public float openSpeed = 0.5f;  // Vitesse d'ouverture

    private Quaternion startRotation;
    private Vector3 startPosition;
    private bool isOpening = false;
    private float openTime = 0f;

    void Start()
    {
        startRotation = transform.rotation;
        startPosition = transform.position;
    }

    void Update()
    {
        if (isOpening)
        {
            // Interpolation vers l'ouverture
            openTime += Time.deltaTime * openSpeed;
            float distance = Mathf.SmoothStep(0, openDistance, openTime);
            transform.position = startPosition + new Vector3(0, 0, -distance);
        }
        else
        {
            // Oscillation normale (effet du vent)
            float t = Mathf.PingPong(Time.time * speed, 1);
            float angle = Mathf.SmoothStep(minAngle, maxAngle, t);
            transform.rotation = startRotation * Quaternion.Euler(angle, 0, 0);
        }
    }

    void OnMouseDown()
    {
        if (!isOpening)
        {
            isOpening = true;
            openTime = 0f;
        }
    }
}