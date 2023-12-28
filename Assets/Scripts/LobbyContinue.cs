using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyContinue : MonoBehaviour
{
    public GameObject blackScreen;
    public ThirdPersonMovement movement;
    bool continued = false;

    void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Continue();
            continued = true;
            movement.vertical = 1;
        }
    }

    void Continue()
    {
        if (continued) return;
        blackScreen.SetActive(true);
    }
}
