using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public GameObject prefab = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(prefab);
        }
    }
}
