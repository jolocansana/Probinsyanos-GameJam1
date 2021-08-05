using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // highlighting
        RaycastHit hit;
        bool objectHighlighted = Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hit,
                10f
            );
        if (objectHighlighted)
        {
            hit.collider.SendMessage("RayTargetHighlight", SendMessageOptions.DontRequireReceiver);
        }


        // selection
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("bang!");
            if (objectHighlighted)
            {
                hit.collider.SendMessage("RayTargetHit", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
