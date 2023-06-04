using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTreeNode : MonoBehaviour
{
    public int layer;
    public int num;
    public int father;

    private GameObject line;
    private GameObject corner;
    private Transform fatherTransform;
    private RedTreeNode fatherNode;

    private void Start()
    {
        if (layer != 1)
        {
            fatherTransform = transform.parent;
            fatherNode = fatherTransform.gameObject.GetComponent<RedTreeNode>();
        }



        GenerateLine();
    }


    private void GenerateLine()
    {
        if (layer == 1)
        {
            return;
        }

        Vector3 pos = transform.position;
        Vector3 fatherPos = fatherTransform.position;

        //上下生成线
        Vector3 yOffset = new Vector3(0, 0.5f, 0);
        Vector3 currentPos = pos + yOffset;
        float yTarget = (fatherPos - 2 * yOffset).y;
        while (currentPos.y != yTarget)
        {
            currentPos += yOffset;
            if(!RedLineGenerator.lineMap.ContainsKey(currentPos))
            {
                RedLineGenerator.lineMap.Add(currentPos,12);
            }
            else
            {
                RedLineGenerator.lineMap[currentPos] |= 0b001100;
            }
            //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
        }

        int ran = Random.Range(0, 2);
        if(ran == 0)            //先调整x
        {
            if (currentPos.x > fatherPos.x)
            {
                currentPos += yOffset;
                if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                {
                    RedLineGenerator.lineMap.Add(currentPos, 12);
                }
                else
                {
                    RedLineGenerator.lineMap[currentPos] |= 0b001100;
                }
                //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                currentPos += yOffset;
                //y+,x-,011000
                if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                {
                    RedLineGenerator.lineMap.Add(currentPos, 24);
                }
                else
                {
                    RedLineGenerator.lineMap[currentPos] |= 0b011000;
                }
                //Instantiate(corner, currentPos, Quaternion.Euler(0, 180, 0));

                Vector3 xOffset = new Vector3(-0.5f, 0, 0);
                float xTarget = (fatherPos - 2 * xOffset).x;

                while (currentPos.x != xTarget)
                {
                    currentPos += xOffset;
                    //110000
                    if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                    {
                        RedLineGenerator.lineMap.Add(currentPos, 48);
                    }
                    else
                    {
                        RedLineGenerator.lineMap[currentPos] |= 0b110000;
                    }
                    //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                }
            }
            else if(currentPos.x < fatherPos.x)
            {
                currentPos += yOffset;
                if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                {
                    RedLineGenerator.lineMap.Add(currentPos, 12);
                }
                else
                {
                    RedLineGenerator.lineMap[currentPos] |= 0b001100;
                }
                //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                currentPos += yOffset;
                if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                {
                    RedLineGenerator.lineMap.Add(currentPos, 40);
                }
                else
                {
                    RedLineGenerator.lineMap[currentPos] |= 0b101000;
                }
                //Instantiate(corner, currentPos, Quaternion.Euler(0, 0, 0));

                Vector3 xOffset = new Vector3(0.5f, 0, 0);
                float xTarget = (fatherPos - 2 * xOffset).x;

                while (currentPos.x != xTarget)
                {
                    currentPos += xOffset;
                    if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                    {
                        RedLineGenerator.lineMap.Add(currentPos, 48);
                    }
                    else
                    {
                        RedLineGenerator.lineMap[currentPos] |= 0b110000;
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
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                        currentPos += new Vector3(-0.5f, 0, 0);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 33);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b100001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(-90, 0, 0));
                    }
                    else if (currentPos.x < fatherPos.x)
                    {
                        currentPos += new Vector3(0.5f, 0, 0);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                        currentPos += new Vector3(0.5f, 0, 0);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 17);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b010001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(-90, 0, 90));
                    }
                    else
                    {
                        currentPos += yOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 12);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b001100;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                        currentPos += yOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 9);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b001001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(0, 90, 0));
                    }

                    Vector3 zOffset = new Vector3(0, 0, -0.5f);
                    float zTarget = (fatherPos - 2 * zOffset).z;

                    while (currentPos.z != zTarget)
                    {
                        currentPos += zOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b000011;
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
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                        currentPos += new Vector3(-0.5f, 0, 0);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 34);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b100010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(90, 0, 0));
                    }
                    else if (currentPos.x < fatherPos.x)
                    {
                        currentPos += new Vector3(0.5f, 0, 0);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b110000;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 90));
                        currentPos += new Vector3(0.5f, 0, 0);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 18);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b010010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(90, 0, 90));
                    }
                    else
                    {
                        currentPos += yOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 12);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b001100;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                        currentPos += yOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 10);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b001010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(0, -90, 0));
                    }

                    Vector3 zOffset = new Vector3(0, 0, 0.5f);
                    float zTarget = (fatherPos - 2 * zOffset).z;

                    while (currentPos.z != zTarget)
                    {
                        currentPos += zOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b000011;
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
                if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                {
                    RedLineGenerator.lineMap.Add(currentPos, 12);
                }
                else
                {
                    RedLineGenerator.lineMap[currentPos] |= 0b001100;
                }
                //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                currentPos += yOffset;
                if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                {
                    RedLineGenerator.lineMap.Add(currentPos, 9);
                }
                else
                {
                    RedLineGenerator.lineMap[currentPos] |= 0b001001;
                }
                //Instantiate(corner, currentPos, Quaternion.Euler(0, 90, 0));

                Vector3 zOffset = new Vector3(0, 0, -0.5f);
                float zTarget = (fatherPos - 2 * zOffset).z;
                while (currentPos.z != zTarget)
                {
                    currentPos += zOffset;
                    if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                    {
                        RedLineGenerator.lineMap.Add(currentPos, 3);
                    }
                    else
                    {
                        RedLineGenerator.lineMap[currentPos] |= 0b000011;
                    }
                    //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                }
            }
            else if (currentPos.z < fatherPos.z)
            {
                currentPos += yOffset;
                if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                {
                    RedLineGenerator.lineMap.Add(currentPos, 12);
                }
                else
                {
                    RedLineGenerator.lineMap[currentPos] |= 0b001100;
                }
                //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                currentPos += yOffset;
                if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                {
                    RedLineGenerator.lineMap.Add(currentPos, 10);
                }
                else
                {
                    RedLineGenerator.lineMap[currentPos] |= 0b001010;
                }
                //Instantiate(corner, currentPos, Quaternion.Euler(0, -90, 0));

                Vector3 zOffset = new Vector3(0, 0, 0.5f);
                float zTarget = (fatherPos - 2 * zOffset).z;
                while (currentPos.z != zTarget)
                {
                    currentPos += zOffset;
                    if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                    {
                        RedLineGenerator.lineMap.Add(currentPos, 3);
                    }
                    else
                    {
                        RedLineGenerator.lineMap[currentPos] |= 0b000011;
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
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                        currentPos += new Vector3(0, 0, -0.5f);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 18);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b010010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(90, 0, 90));
                    }
                    else if (currentPos.z < fatherPos.z)
                    {
                        currentPos += new Vector3(0, 0, 0.5f);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                        currentPos += new Vector3(0, 0, 0.5f);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 17);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b010001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(-90, 0, 90));
                    }
                    else
                    {
                        currentPos += yOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 12);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b001100;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                        currentPos += yOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 24);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b011000;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(0, 180, 0));
                    }

                    Vector3 xOffset = new Vector3(-0.5f, 0, 0);
                    float xTarget = (fatherPos - 2 * xOffset).x;

                    while (currentPos.x != xTarget)
                    {
                        currentPos += xOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b110000;
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
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                        currentPos += new Vector3(0, 0, -0.5f);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 34);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b100010;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(90, 0, 0));
                    }
                    else if (currentPos.z < fatherPos.z)
                    {
                        currentPos += new Vector3(0, 0, 0.5f);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 3);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b000011;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(90, 0, 0));
                        currentPos += new Vector3(0, 0, 0.5f);
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 33);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b100001;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(-90, 0, 0));
                    }
                    else
                    {
                        currentPos += yOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 12);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b001100;
                        }
                        //Instantiate(line, currentPos, Quaternion.Euler(0, 0, 0));
                        currentPos += yOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 40);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b101000;
                        }
                        //Instantiate(corner, currentPos, Quaternion.Euler(0, 0, 0));
                    }

                    Vector3 xOffset = new Vector3(0.5f, 0, 0);
                    float xTarget = (fatherPos - 2 * xOffset).x;

                    while (currentPos.x != xTarget)
                    {
                        currentPos += xOffset;
                        if (!RedLineGenerator.lineMap.ContainsKey(currentPos))
                        {
                            RedLineGenerator.lineMap.Add(currentPos, 48);
                        }
                        else
                        {
                            RedLineGenerator.lineMap[currentPos] |= 0b110000;
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
