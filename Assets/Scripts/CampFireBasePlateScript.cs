using System;
using UnityEngine;

public class BasePlateClick : MonoBehaviour
{
    public CampFireScript campfire; // Assign this in Inspector

    void OnMouseDown()
    {
        if (campfire != null)
        {
            campfire.OnBasePlateClicked();
        }
    }
}
