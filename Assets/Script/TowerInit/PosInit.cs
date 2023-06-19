using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosInit : MonoBehaviour
{

    public float del_pixel=125f;
    public GameObject emptyPrefab;
    public GameObject field;

    private string roadIndex = "701,601,501,401,301,201,101,001,702,703,708,709,104,204,304,404,504,604,704,707,607,507,407,307,207,107,106,105,112,111,110,210,310,410,510,610,710,713,613,513,413,313,213,113,714,715,716,616,516,416,316,216,116,016";
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos_parent = Vector3.zero;
        for (int i = 0; i <= 8; i++)
        {
            GameObject rowParent = Instantiate(emptyPrefab);
            
            rowParent.transform.SetParent(transform);
            rowParent.transform.localPosition = pos_parent;
            rowParent.transform.localScale = Vector3.one;
            rowParent.transform.name = i.ToString();
            pos_parent.y -= del_pixel;
        }

        Transform[] emptyParent = GetComponentsInChildren<Transform>();
        Vector3 pos_field = Vector3.zero;
        for (int j = 1;j<emptyParent.Length;j++)
        {
            for (int i = 0; i <= 17; i++)
            {
                GameObject emField = Instantiate(field);
                
                emField.transform.SetParent(emptyParent[j]);
                emField.transform.localPosition = pos_field;
                emField.transform.localScale = Vector3.one;
                if (i < 10)
                {
                    emField.transform.name = (j-1).ToString()+"0"+i.ToString();
                }
                else
                {
                    emField.transform.name = (j-1).ToString() + i.ToString();
                }
                pos_field.x += del_pixel;
            }
            pos_field=Vector3.zero;
        }
        SetRoad(roadIndex);
        //print(transform.childCount);
    }

    private void SetRoad(string roadString)
    {
        string[] road = roadString.Split(',');
        foreach (string index in road)
        {
            char rowChar = index[0];
            string colChar = index.Substring(index.Length - 2);
            int row = int.Parse(rowChar.ToString());
            int col = int.Parse(colChar);
            GameObject roadGameObject = transform.GetChild(row).GetChild(col).gameObject;
            roadGameObject.tag = "Road";
            Component fieldComponent = roadGameObject.GetComponent<FieldInit>();
            if (fieldComponent != null)
            {
                Destroy(fieldComponent);
            }
        }
    }
    
}
