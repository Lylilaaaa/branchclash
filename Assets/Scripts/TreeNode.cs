using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public bool isDead = false;
    private GameObject line;
    private GameObject corner;
    private Transform fatherTransform;
    private TreeNode fatherNode;
    public Material yellowMat;
    public Material blueMat;
    public Material deadMat;
    public Material currentMaterial;
    private List<GameObject> _matCore;
    private int generateTime=0;
    
    private bool isTransitioning = false;
    
    public float transitionDuration = 2.0f;
    
    private float transitionTimer = 0.0f;

    

    public void Start()
    {
        currentMaterial = deadMat;
        //print("be restart!");
        _matCore = new List<GameObject>();
        if (layer != 0)
        {
            fatherTransform = transform.parent;
            fatherNode = fatherTransform.gameObject.GetComponent<TreeNode>();
        }
        
        foreach (string majorName in GlobalVar._instance.MajorNodeList)
        {
            if (transform.name == majorName)
            {
                isMajor = true;
            }
        }

        foreach (string deadName in GlobalVar._instance.deadNodeList)
        {
            if (transform.name == deadName)
            {
                isDead = true;
            }
        }
        
        FindObjectsWithTag(transform, "blueMat");
        
        if (isMajor && !isDead)
        {
            currentMaterial = yellowMat;
            foreach (GameObject child in _matCore)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                
                if (renderer != null && yellowMat != null)
                {
                    renderer.material = yellowMat;
                    
                }
            }
        }
        else if(!isMajor && !isDead)
        {
            currentMaterial = blueMat;
            foreach (GameObject child in _matCore)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                
                if (renderer != null && blueMat != null)
                {
                    renderer.material = blueMat;
                }
            }
        }
        else
        {
            currentMaterial = deadMat;
            foreach (GameObject child in _matCore)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                
                if (renderer != null && blueMat != null)
                {
                    renderer.material = deadMat;
                }
            }
        }
        GenerateLine();
    }

    private void Update()
    {
        //print(transform.name);
        if (GlobalVar.CurrentGameState != GlobalVar.GameState.MainStart &&
            GlobalVar.CurrentGameState != GlobalVar.GameState.Viewing)
        {
            //print("destroy"+transform.name);
            Destroy(gameObject);
        }
        
        // for performance!!!!!
        if (Input.GetKeyDown(KeyCode.P) && (transform.name.Substring(0,1) == "4"||transform.name.Substring(0,1) == "5") && !isTransitioning)
        {
            print("is deading!!");
            isTransitioning = true;
        }
        
        if (isTransitioning)
        {
            foreach (GameObject child in _matCore)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                
                if (renderer != null)
                {
                    if (transitionTimer < transitionDuration)
                    {
                        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
                        renderer.material.Lerp(currentMaterial, deadMat, t);
                    }
                }
            }
            transitionTimer += Time.deltaTime;
        }
    }

    private void FindObjectsWithTag(Transform parentTransform, string tag)
    {
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
        generateTime++;
        //print(transform.name+": "+generateTime);
        if (layer == 0)
        {
            return;
        }

        Vector3 pos = transform.position;
        Vector3 fatherPos = fatherTransform.position;

        //??????????
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
        if(ran == 0)            //?????x
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
        else                    //?????z
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
