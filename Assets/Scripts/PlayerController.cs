using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // D�placement
    private const float vitesseDeplacement = 10.0f;

    // Rotation
    float vitesseRotation = 5.0f;
    public bool controleVerticalInverse = true;
    public bool controleHorizontalInverse = false;

    // Terrain
    public float distanceDuSol = 1.5f;

    void Update()
    {
        // Rotation
        GestionBoutonDroit();

        if(!Cursor.visible)
        {
            transform.localRotation = GestionRotation();
        }

        // D�placement
        transform.Translate(Acceleration() * Time.deltaTime * vitesseDeplacement * Deplacement());

        if (Terrain.activeTerrain)
        {
            transform.position = GestionTerrain();
        }
    }

    Vector3 Deplacement()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.up;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += Vector3.down;
        }
        return direction;
    }

    float Acceleration()
    {
        float acceleration = 1.0f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            acceleration = 2.0f;
        }
        return acceleration;
    }

    void GestionBoutonDroit()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    Quaternion GestionRotation()
    {
        float rotationY = Input.GetAxis("Mouse X");
        float rotationX = Input.GetAxis("Mouse Y");

        float limite = 1f;
        rotationY = Mathf.Clamp(rotationY, -limite, limite);
        rotationX = Mathf.Clamp(rotationX, -limite, limite);

        if (controleHorizontalInverse)
        {
            rotationY *= -1;
        }
        if (controleVerticalInverse)
        {
            rotationX *= -1;
        }

        Vector3 rotationSouris = new Vector3(rotationX, rotationY, 0) * vitesseRotation;
        Vector3 rotationActuelle = transform.eulerAngles;

        Vector3 rotationNouvelle = new Vector3();
        rotationNouvelle = rotationActuelle + rotationSouris;

        if (rotationNouvelle.x > 180f)
        {
            rotationNouvelle.x = Mathf.Max(rotationNouvelle.x - 360f, -90f);
        }
        else
        {
            rotationNouvelle.x = Mathf.Min(rotationNouvelle.x, 90f);
        }

        return Quaternion.Euler(rotationNouvelle);
    }

    // OLD
    //Vector3 GestionTerrain()
    //{
    //    float hauteurTerrain = Terrain.activeTerrain.transform.position.y + Terrain.activeTerrain.SampleHeight(transform.position) + distanceDuSol;

    //    return new Vector3(transform.position.x, Mathf.Max(transform.position.y, hauteurTerrain),transform.position.z);
    //}

    Vector3 GestionTerrain()
    {
        float hauteurTerrain = Terrain.activeTerrain.transform.position.y + Terrain.activeTerrain.SampleHeight(transform.position) + distanceDuSol;

        // Vérifier s'il y a un objet solide sous le joueur avec un Raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            // Nouvelle hauteur basée de l'objet touché
            float hauteurObjet = hit.point.y + distanceDuSol;
            return new Vector3(transform.position.x, Mathf.Max(transform.position.y, hauteurTerrain, hauteurObjet), transform.position.z);
        }

        // Si aucun objet n'est détecté, on utilise la hauteur du terrain
        return new Vector3(transform.position.x, Mathf.Max(transform.position.y, hauteurTerrain), transform.position.z);
    }

}
