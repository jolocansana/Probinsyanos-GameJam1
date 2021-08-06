using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGoldberg : MonoBehaviour
{
    [SerializeField] private GameObject goldbergMachine;
    [SerializeField] private GameObject templateGoldberg;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject templateBall;

    // Start is called before the first frame update
    void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.GameJam.RESET_GOLDBERG, this.dropBall);
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.GameJam.RESET_GOLDBERG);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void dropBall()
    {
        Vector3 localPos = templateBall.transform.localPosition;
        ball.transform.localPosition = localPos;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void resetGoldberg()
    {
        Debug.Log("Reset");
        GameObject.Destroy(this.goldbergMachine);

        Vector3 localPos = templateGoldberg.transform.localPosition;

        goldbergMachine = ObjectUtils.SpawnDefault(this.templateGoldberg, this.transform, localPos);
        goldbergMachine.SetActive(true);
    }
}
