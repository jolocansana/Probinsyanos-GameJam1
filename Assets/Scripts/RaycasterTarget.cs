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
    [SerializeField] private string action;

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
            Parameters parameters = new Parameters();

            parameters.PutExtra("trigger", false);

            EventBroadcaster.Instance.PostEvent(EventNames.GameJam.TRIGGER_PROMPT, parameters);
            this.renderer.material = this.defaultMaterial;
            highlighted = false;
        }
    }
    public void RayTargetHit()
    {
        Parameters parameters = new Parameters();

        if (enableObjectSelection)
        {
            if (action.Equals("check"))
            {
                EventBroadcaster.Instance.PostEvent(EventNames.GameJam.CHECK_INVENTORY, parameters);
            } 
            else
            {
                string name = obj.name;
                parameters.PutExtra("itemName", name);

                EventBroadcaster.Instance.PostEvent(EventNames.GameJam.ADD_INVENTORY, parameters);
                obj.SetActive(false);
            }

            parameters.PutExtra("trigger", false);
            EventBroadcaster.Instance.PostEvent(EventNames.GameJam.TRIGGER_PROMPT, parameters);
        } 
    }

    public void RayTargetHighlight()
    {
        Parameters parameters = new Parameters();

        if (enableObjectHighlighting)
        {
            // Debug.Log("Target highlighted");
            parameters.PutExtra("prompt_text", "Press E to Click or Get Item");
            parameters.PutExtra("trigger", true);

            EventBroadcaster.Instance.PostEvent(EventNames.GameJam.TRIGGER_PROMPT, parameters);
            this.renderer.material = this.highlightMaterial;
            this.highlighted = true;
        }
    }
}
