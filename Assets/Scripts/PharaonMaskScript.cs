using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class PharaonMaskScript : MonoBehaviour
{
    Volume volume;
    GameObject localVolume;

    ColorAdjustments colorAdjustments;

    public Light SunLight;
    public Material newSkybox;

    public AudioClip thunderSound;
    private AudioSource audioSource;

    // Spotlight
    Vector3 initialSpotlightPosition;
    public Light spotLight;

    public float spotlightSpeed = .5f;
    public float spotlightMoveDistance = 2.5f;

    private Vector3 targetSpotlightPosition;
    private bool isSpotlightMovingToTarget = true;

    // Mask
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isMaskMoving = false;
    private bool movingUp = true;

    public float moveSpeed = 1f;    
    public float moveDistance = 0.5f;


    void Start()
    {
        localVolume = GameObject.Find("Box Volume");
        volume = localVolume.GetComponent<Volume>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = thunderSound;

        initialSpotlightPosition = spotLight.transform.position;
        targetSpotlightPosition = initialSpotlightPosition + new Vector3(0, spotlightMoveDistance, 0);

        initialPosition = transform.position;
        targetPosition = initialPosition + new Vector3(0, moveDistance, 0); 
    }

    void Update()
    {
        SpotlightMovement();
    }

    void OnMouseDown()
    {
        if (!isMaskMoving)
        {
            StartCoroutine(SlideMask());
        }
    }

    private IEnumerator SlideMask()
    {
        Console.WriteLine("Mask has been moved");
        isMaskMoving = true;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = movingUp ? targetPosition : initialPosition;

        while (elapsedTime < 1f) 
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = endPos; 

        audioSource.Play();
        spotLight.GetComponent<Light>().intensity = 0;
        SunLight.GetComponent<Light>().intensity = 0;
        RenderSettings.skybox = newSkybox;
        DynamicGI.UpdateEnvironment();

        // Post Exposure
        if (volume.sharedProfile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = -3f;
        }

        movingUp = !movingUp;
        isMaskMoving = false;
    }

    private void SpotlightMovement()
    {
        if (isSpotlightMovingToTarget)
        {
            spotLight.transform.position = Vector3.Lerp(spotLight.transform.position, targetSpotlightPosition, Time.deltaTime * spotlightSpeed);
        }
        else
        {
            spotLight.transform.position = Vector3.Lerp(spotLight.transform.position, initialSpotlightPosition, Time.deltaTime * spotlightSpeed);
        }

        if (Vector3.Distance(spotLight.transform.position, targetSpotlightPosition) < 0.1f)
        {
            isSpotlightMovingToTarget = !isSpotlightMovingToTarget;
            targetSpotlightPosition = isSpotlightMovingToTarget ? initialSpotlightPosition + new Vector3(0, spotlightMoveDistance, 0) : initialSpotlightPosition;
        }
    }
}