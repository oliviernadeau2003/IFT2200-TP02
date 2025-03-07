using System;
using UnityEngine;

public class Parcour : MonoBehaviour
{
    private Transform positionInitial;
    public Transform[] Bornes = new Transform[14];

    public GameObject objetParcour;
    public GameObject objetAccueil;

    public float[] vitessesDeplacement = new float[14]; // Tableau des vitesses de déplacement
    public float[] vitessesRotation = new float[14]; // Tableau des vitesses de rotation
    public float distanceSeuil = 0.3f; // Distance pour changer de borne
    private int indexBorne = 0;

    void Start()
    {
        positionInitial = transform;
    }

    void Update()
    {
        if (indexBorne >= Bornes.Length)
        {
            RetourAccueil();
            return;
        }

        Transform borneSuivante = Bornes[indexBorne];
        float vitesseDeplacement = vitessesDeplacement[indexBorne];
        float vitesseRotation = vitessesRotation[indexBorne];

        // Rotation vers la borne suivante
        Vector3 direction = (borneSuivante.position - transform.position).normalized;
        Quaternion rotationCible = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationCible, vitesseRotation * Time.deltaTime);

        // Déplacement vers la borne suivante
        transform.position += transform.forward * vitesseDeplacement * Time.deltaTime;

        // Vérifier si la caméra est proche de la borneSuivante
        if (Vector3.Distance(transform.position, borneSuivante.position) < distanceSeuil)
        {
            indexBorne++;
        }
    }

    void RetourAccueil()
    {
        objetAccueil.SetActive(true);
        transform.position = positionInitial.position;
        transform.rotation = positionInitial.rotation;
        indexBorne = 0;
        objetParcour.SetActive(false);
    }
}
