using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private bool isSizeUp = false;

    public void bigsize()
    {
        if (!isSizeUp)
        {
            // x, y 방향에 대해 크기를 2배로 한다.
            this.transform.localScale = new Vector3(2.0f, 2.0f, 0.5f);
            isSizeUp = true;
        }
        else
        {
            // 방패를 원래대로 돌려놓는다.
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.1f);
            isSizeUp = false;
        }
            
    }
}
