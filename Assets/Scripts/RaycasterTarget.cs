using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasterTarget : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private bool enableObjectSelection = true;
    [SerializeField] private bool enableObjectHighlighting = true;
    [SerializeField] private Material highlightMaterial;

    private new MeshRenderer renderer;

    bool highlighted = false;

    // Start is called before the first frame update
    void Start()
    {
        this.renderer = obj.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (highlighted)
        {
            this.renderer.material = this.defaultMaterial;
            highlighted = false;
        }
    }
    public void RayTargetHit()
    {
        if (enableObjectSelection)
        {
            Debug.Log("Target hit");
            obj.SetActive(false);
        }
    }

    public void RayTargetHighlight()
    {
        if (enableObjectHighlighting)
        {
            Debug.Log("Target highlighted");
            this.renderer.material = this.highlightMaterial;
            this.highlighted = true;
        }
    }
}
