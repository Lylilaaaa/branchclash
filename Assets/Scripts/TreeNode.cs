using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 Description:       Each node in upward tree finds a way, prepares for connection
 Unity Version:     2020.3.15f2c1
 Author:            ZHUANG Yan
 Date:              01/01/2023
 Last Modified:     06/12/2023
 */

public class TreeNode : MonoBehaviour
{
    public int layer;
    public int num;
    public int father;

    public bool isMajor = false;
    private GameObject line;
    private GameObject corner;
    private Transform fatherTransform;
    private TreeNode fatherNode;
    public Material yellowMat;
    public Material blueMat;
    private List<GameObject> _matCore;

    private void Start()
    {
        _matCore = new List<GameObject>();
        if (layer != 0)
        {
            fatherTransform = transform.parent;
            fatherNode = fatherTransform.gameObject.GetComponent<TreeNode>();
        }
        
        // if(num == 0)
        // {
        //     isMajor = true;
        // }
        // if (transform.name == "0-0"|| transform.name == "1-1" || transform.name == "2-3" || transform.name == "3-1" ||
        //     transform.name == "4-2" || transform.name == "5-1")
        // {
        //     isMajor = true;
        // }

        foreach (string majorName in GlobalVar._instance.MajorNodeList)
        {
            if (transform.name == majorName)
            {
                isMajor = true;
            }
        }
        FindObjectsWithTag(transform, "blueMat");
        if (isMajor == true)
        {
            foreach (GameObject child in _matCore)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                
                if (renderer != null && yellowMat != null)
                {
                    renderer.material = yellowMat;
                }
            }
        }
        else
        {
            foreach (GameObject child in _matCore)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                
                if (renderer != null && blueMat != null)
                {
                    renderer.material = blueMat;
                }
            }
        }
        
        GenerateLine();
    }
    
    private void FindObjectsWithTag(Transform parentTransform, string tag)
    {
        // int childCount = parentTransform.childCount;
        // for (int i = 0; i < childCount; i++)
        // {
        //     if (parentTransform.GetChild(i).CompareTag(tag))
        //     {
        //         _matCore.Add
        //     }
        // }
        foreach (Transform child in parentTransform)
        {
            if (child.CompareTag(tag))
            {
                _matCore.Add(child.gameObject);
            }
            
            FindObjectsWithTag(child, tag);
        }
    }

    private void AddMajorPos(Vector3 pos)
    {
        if(!isMajor)
        {
            return;
        }

        if(LineGenerator.majorChain.Contains(pos))
        {
            return;
        }
        else
        {
            LineGenerator.majorChain.Add(pos);
        }
    }


    private void GenerateLine()
    {
        if (layer == 0)
        {
            return;
        }

        Vector3 pos = transform.position;
        Vector3 fatherPos = fatherTransform.position;

        //上下生成线
        Vector3 yOffset = new Vector3(0, -0.5f, 0);
        Vector3 currentPos = pos + yOffset;
        float yTarget = (fatherPos - 2 * yOffset).y;
        while (currentPos.y != yTarget)
        {
            currentPos += yOffset;
            AddMajorPos(currentPos);
            if(!LineGenerator.lineMap.ContainsKey(currentPos))
            {
                LineGenerator.lineMap.Add(currentPos,12);
            }
            else
            {
                LineGenerator.lineMap[currentPos] |= 0b001100;
            }
            //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
        }

        int ran = Random.Range(0, 2);
        if(ran == 0)            //先调整x
        {
            if (currentPos.x > fatherPos.x)
            {
                currentPos += yOffset;
                AddMajorPos(currentPos);
                if (!LineGenerator.lineMap.ContainsKey(currentPos))
                {
                    LineGenerator.lineMap.Add(currentPos, 12);
                }
                else
                {
                    LineGenerator.lineMap[currentPos] |= 0b001100;
                }
                //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                currentPos += yOffset;
                //y+,x-,011000
                AddMajorPos(currentPos);
                if (!LineGenerator.lineMap.ContainsKey(currentPos))
                {
                    LineGenerator.lineMap.Add(currentPos, 24);
                }
                else
                {
                    LineGenerator.lineMap[currentPos] |= 0b011000;
                }
                //Instantiate(corner, currentPos, Quaternion.Euler(0, 180, 0));

                Vector3 xOffset = new Vector3(-0.5f, 0, 0);
                float xTarget = (fatherPos - 2 * xOffset).x;

                while (currentPos.x != xTarget)
                {
                    currentPos += xOffset;
                    //110000
                    AddMajorPos(currentPos);
                    if (!LineGenerator.lineMap.ContainsKey(currentPos))
                    {
                        LineGenerator.lineMap.Add(currentPos, 48);
                    }
                    else
                    {
                        LineGenerator.lineMap[currentPos] |= 0b110000;
                    }
                    //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                }
            }
            else if(currentPos.x < fatherPos.x)
            {
                currentPos += yOffset;
                AddMajorPos(currentPos);
                if (!LineGenerator.lineMap.ContainsKey(currentPos))
                {
                    LineGenerator.lineMap.Add(currentPos, 12);
                }
                else
                {
                    LineGenerator.lineMap[currentPos] |= 0b001100;
                }
                //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                currentPos += yOffset;
                AddMajorPos(currentPos);
                if (!LineGenerator.lineMap.ContainsKey(currentPos))
                {
                    LineGenerator.lineMap.Add(currentPos, 40);
                }
                else
                {
                    LineGenerator.lineMap[currentPos] |= 0b101000;
                }
                //Instantiate(corner, currentPos, Quaternion.Euler(0, 0, 0));

                Vector3 xOffset = new Vector3(0.5f, 0, 0);
                float xTarget = (fatherPos - 2 * xOffset).x;

                while (currentPos.x != xTarget)
                {
                    currentPos += xOffset;
                    AddMajorPos(currentPos);
                    if (!LineGenerator.lineMap.ContainsKey(currentPos))
                    {
                        LineGenerator.lineMap.Add(currentPos, 48);
                    }
                    else
                    {
                        LineGenerator.lineMap[currentPos] |= 0b110000;
                    }
                    //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                }
            }


            if (currentPos.z == fatherPos.z)
            {
                if (currentPos.x > fatherPos.x)
                {
                    fatherNode.Connect(1);
                }
                else if (currentPos.x < fatherPos.x)
                {
                    fatherNode.Connect(2);
                }
            }
            else
            {
                if (currentPos.z > fatherPos.z)
                {
                    if (currentPos.x > fatherPos.x)
                    {
                        currentPos += new Vector3(-0.5f, 0, 0);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                        currentPos += new Vector3(-0.5f, 0, 0);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 33);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b100001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(-90, 0, 0));
                    }
                    else if (currentPos.x < fatherPos.x)
                    {
                        currentPos += new Vector3(0.5f, 0, 0);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                        currentPos += new Vector3(0.5f, 0, 0);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 17);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b010001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(-90, 0, 90));
                    }
                    else
                    {
                        currentPos += yOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 12);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b001100;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                        currentPos += yOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 9);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b001001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(0, 90, 0));
                    }

                    Vector3 zOffset = new Vector3(0, 0, -0.5f);
                    float zTarget = (fatherPos - 2 * zOffset).z;

                    while (currentPos.z != zTarget)
                    {
                        currentPos += zOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                    }

                    fatherNode.Connect(3);
                }
                else
                {
                    if (currentPos.x > fatherPos.x)
                    {
                        currentPos += new Vector3(-0.5f, 0, 0);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                        currentPos += new Vector3(-0.5f, 0, 0);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 34);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b100010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(90, 0, 0));
                    }
                    else if (currentPos.x < fatherPos.x)
                    {
                        currentPos += new Vector3(0.5f, 0, 0);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                        currentPos += new Vector3(0.5f, 0, 0);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 18);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b010010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(90, 0, 90));
                    }
                    else
                    {
                        currentPos += yOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 12);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b001100;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                        currentPos += yOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 10);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b001010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(0, -90, 0));
                    }

                    Vector3 zOffset = new Vector3(0, 0, 0.5f);
                    float zTarget = (fatherPos - 2 * zOffset).z;

                    while (currentPos.z != zTarget)
                    {
                        currentPos += zOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                    }

                    fatherNode.Connect(4);
                }

                
            }


            
        }
        else                    //先调整z
        { 
            if (currentPos.z > fatherPos.z)
            {
                currentPos += yOffset;
                AddMajorPos(currentPos);
                if (!LineGenerator.lineMap.ContainsKey(currentPos))
                {
                    LineGenerator.lineMap.Add(currentPos, 12);
                }
                else
                {
                    LineGenerator.lineMap[currentPos] |= 0b001100;
                }
                //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                currentPos += yOffset;
                AddMajorPos(currentPos);
                if (!LineGenerator.lineMap.ContainsKey(currentPos))
                {
                    LineGenerator.lineMap.Add(currentPos, 9);
                }
                else
                {
                    LineGenerator.lineMap[currentPos] |= 0b001001;
                }
                //Instantiate(corner, currentPos, Quaternion.Euler(0, 90, 0));

                Vector3 zOffset = new Vector3(0, 0, -0.5f);
                float zTarget = (fatherPos - 2 * zOffset).z;
                while (currentPos.z != zTarget)
                {
                    currentPos += zOffset;
                    AddMajorPos(currentPos);
                    if (!LineGenerator.lineMap.ContainsKey(currentPos))
                    {
                        LineGenerator.lineMap.Add(currentPos, 3);
                    }
                    else
                    {
                        LineGenerator.lineMap[currentPos] |= 0b000011;
                    }
                    //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                }
            }
            else if (currentPos.z < fatherPos.z)
            {
                currentPos += yOffset;
                AddMajorPos(currentPos);
                if (!LineGenerator.lineMap.ContainsKey(currentPos))
                {
                    LineGenerator.lineMap.Add(currentPos, 12);
                }
                else
                {
                    LineGenerator.lineMap[currentPos] |= 0b001100;
                }
                //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                currentPos += yOffset;
                AddMajorPos(currentPos);
                if (!LineGenerator.lineMap.ContainsKey(currentPos))
                {
                    LineGenerator.lineMap.Add(currentPos, 10);
                }
                else
                {
                    LineGenerator.lineMap[currentPos] |= 0b001010;
                }
                //Instantiate(corner, currentPos, Quaternion.Euler(0, -90, 0));

                Vector3 zOffset = new Vector3(0, 0, 0.5f);
                float zTarget = (fatherPos - 2 * zOffset).z;
                while (currentPos.z != zTarget)
                {
                    currentPos += zOffset;
                    AddMajorPos(currentPos);
                    if (!LineGenerator.lineMap.ContainsKey(currentPos))
                    {
                        LineGenerator.lineMap.Add(currentPos, 3);
                    }
                    else
                    {
                        LineGenerator.lineMap[currentPos] |= 0b000011;
                    }
                    //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                }
            }


            if (currentPos.x == fatherPos.x)
            {
                if (currentPos.z > fatherPos.z)
                {
                    fatherNode.Connect(3);
                }
                else if (currentPos.z < fatherPos.z)
                {
                    fatherNode.Connect(4);
                }
            }
            else
            {
                if (currentPos.x > fatherPos.x)
                {
                    if (currentPos.z > fatherPos.z)
                    {
                        currentPos += new Vector3(0, 0, -0.5f);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                        currentPos += new Vector3(0, 0, -0.5f);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 18);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b010010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(90, 0, 90));
                    }
                    else if (currentPos.z < fatherPos.z)
                    {
                        currentPos += new Vector3(0, 0, 0.5f);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                        currentPos += new Vector3(0, 0, 0.5f);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 17);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b010001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(-90, 0, 90));
                    }
                    else
                    {
                        currentPos += yOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 12);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b001100;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                        currentPos += yOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 24);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b011000;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(0, 180, 0));
                    }

                    Vector3 xOffset = new Vector3(-0.5f, 0, 0);
                    float xTarget = (fatherPos - 2 * xOffset).x;

                    while (currentPos.x != xTarget)
                    {
                        currentPos += xOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                    }

                    fatherNode.Connect(1);
                }
                else
                {
                    if (currentPos.z > fatherPos.z)
                    {
                        currentPos += new Vector3(0, 0, -0.5f);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                        currentPos += new Vector3(0, 0, -0.5f);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 34);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b100010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(90, 0, 0));
                    }
                    else if (currentPos.z < fatherPos.z)
                    {
                        currentPos += new Vector3(0, 0, 0.5f);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                        currentPos += new Vector3(0, 0, 0.5f);
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 33);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b100001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(-90, 0, 0));
                    }
                    else
                    {
                        currentPos += yOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 12);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b001100;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                        currentPos += yOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 40);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b101000;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(0, 0, 0));
                    }

                    Vector3 xOffset = new Vector3(0.5f, 0, 0);
                    float xTarget = (fatherPos - 2 * xOffset).x;

                    while (currentPos.x != xTarget)
                    {
                        currentPos += xOffset;
                        AddMajorPos(currentPos);
                        if (!LineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            LineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            LineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                    }

                    fatherNode.Connect(2);
                }
            }
        }
    }

    public void Connect(int type)
    {
        if (type == 1)          //x+
        {
            GameObject target = transform.Find("X+").gameObject;
            target.SetActive(true);
        }
        else if (type == 2)     //x-
        {
            GameObject target = transform.Find("X-").gameObject;
            target.SetActive(true);
        }
        else if (type == 3)     //z+
        {
            GameObject target = transform.Find("Z+").gameObject;
            target.SetActive(true);
        }
        else if (type == 4)     //z-
        {
            GameObject target = transform.Find("Z-").gameObject;
            target.SetActive(true);
        }
    }
}
