using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUtils
{
    public static GameObject SpawnDefault (GameObject toSpawn, Transform parent, Vector3 localPos)
    {
        GameObject myObj = GameObject.Instantiate(toSpawn, parent);
        myObj.SetActive(true);
        myObj.transform.localPosition = localPos;
        return myObj;
    }
}

