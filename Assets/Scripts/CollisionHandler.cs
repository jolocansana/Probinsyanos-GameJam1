using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private Material redMat;
    [SerializeField] private Material greenMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collide");
        if (other.GetComponent<Collider>().name == "FINAL_DOMINO")
        {
            GetComponent<MeshRenderer>().material = greenMat;
            EventBroadcaster.Instance.PostEvent(EventNames.GameJam.MACHINE_COMPLETE);
        }
    }
}
