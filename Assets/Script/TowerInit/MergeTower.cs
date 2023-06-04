using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeTower : MonoBehaviour
{
    private Collider selectedCollider;
    public LevelData globalVar;

    // Start is called before the first frame update
    void Start()
    {
        selectedCollider = transform.GetComponent<BoxCollider>();
        globalVar.MergeMem.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider == selectedCollider)
                {
                    gameObject.GetComponent<Image>().color = Color.red;
                    FieldInit firstmerge = transform.parent.GetComponent<FieldInit>();
                    firstmerge.checkState = 3;
                    globalVar.MergeMem.Add(transform.name.Substring(0,3));
                }
            }
        }
    }
}
