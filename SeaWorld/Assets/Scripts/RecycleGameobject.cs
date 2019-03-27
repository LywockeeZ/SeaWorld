using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleGameobject : MonoBehaviour
{
    public void Restart()
    {
        //使z坐标保持为0
        var temp = gameObject.transform.position;
        gameObject.transform.position = new Vector3(temp.x, temp.y, 0);
        gameObject.SetActive(true);
    }

    public void Shutdown()
    {
        gameObject.SetActive(false);
    }

}
