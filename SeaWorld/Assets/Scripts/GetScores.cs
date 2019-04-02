using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetScores : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        text.text = string.Format("+{0}", FlockManager.Instance.Flocks.Count);
    }
}
