using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGoldberg : MonoBehaviour
{
    [SerializeField] private GameObject goldbergMachine;
    [SerializeField] private GameObject templateGoldberg;

    // Start is called before the first frame update
    void Start()
    {
        this.StartCoroutine(this.testRespawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator testRespawn()
    {
        yield return new WaitForSeconds(10.0f);

        GameObject.Destroy(this.goldbergMachine);

        Vector3 localPos = templateGoldberg.transform.localPosition;

        goldbergMachine = ObjectUtils.SpawnDefault(this.templateGoldberg, this.transform, localPos);
        goldbergMachine.SetActive(true);

        this.StartCoroutine(this.testRespawn());
    }
}
