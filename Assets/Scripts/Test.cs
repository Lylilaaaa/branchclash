using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startColor = Color.red;
        line.startWidth = 0.2f;
        line.positionCount = 3;
        line.SetPosition(0, new Vector3(0, 0, 0));
        line.SetPosition(1, new Vector3(0, 1, 0));
        line.SetPosition(2, new Vector3(0, 1, 1));
    }
}
