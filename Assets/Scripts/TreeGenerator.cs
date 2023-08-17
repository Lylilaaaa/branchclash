using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 Description:       Randonly instantiate nodes of upward tree, set nodes propety
 Unity Version:     2020.3.15f2c1
 Author:            ZHUANG Yan
 Date:              01/01/2023
 Last Modified:     06/12/2023
 */

public class TreeGenerator : MonoBehaviour
{
    #region
    ////层数，层内序号，父节点层内序号，子节点数量
    //public static int[] nodes;
    //public GameObject prefab;
    //public int layerHeight = 5;

    //public int curLayer;
    //public List<int> curLayerNodes;


    //private void Start()
    //{
    //    nodes = new int[] {0,0,0,17,1,0,0,0,1,1,0,0,1,2,0,0,1,3,0,0,1,4,0,0,1,5,0,0,1,6,0,0,1,7,0,0,1,8,0,0,1,9,0,0,
    //    1,10,0,0,1,11,0,0,2,0,1,0};
    //    GetLayer();

    //    for(int l = curLayer; l >= 0; l--)
    //    {
    //        SpawnNodes(l);
    //    }
    //}


    //private void GetLayer()
    //{
    //    for(int i = 0; i < nodes.Length / 4; i++)
    //    {
    //        if(nodes[4 * i] > curLayer)
    //        {
    //            curLayer = nodes[4 * i];
    //        }
    //    }
    //    Debug.Log(curLayer);
    //}


    //private void SpawnNodes(int layer)
    //{
    //    string name;
    //    Vector3 pos;

    //    for (int i = 0; i < (nodes.Length / 4); i++)
    //    {
    //        if(nodes[4 * i] == layer)
    //        {
    //            curLayerNodes.Add(nodes[4 * i + 1]);
    //        }
    //    }

    //    for (int j = 0; j < curLayerNodes.Count; j++)
    //    {
    //        pos = new Vector3(Random.Range(-layer, layer), layerHeight * layer, Random.Range(-layer, layer));
    //        name = layer.ToString() + "-" + j.ToString();
    //        GameObject node = Instantiate(prefab, pos, transform.rotation);
    //        node.name = name;
    //    }

    //    curLayerNodes.Clear();
    //}


    #endregion

    public static TreeGenerator _instance;
    //层数，层内序号，父节点层内序号，子节点数量
    public static int[] nodes;
    public GameObject prefab;
    public GameObject link;
    public int layerHeight = 5;

    private int layerNum;
    private List<Vector3> nodePos;
    //每一层节点平面范围的半边长
    private int[] layerWidth;
    private void Awake()
    {
        _instance = this;
        
    }
    private void Start()
    {
        
    }

    public void InitTree()
    {
        //nodes = new int[] {0,0,0,5,1,0,0,2,1,1,0,0,1,2,0,3,1,3,0,5,1,4,0,0,2,0,0,3,2,1,2,3,2,2,2,2,2,3,2,0,2,4,3,4,2,5,3,0,2,6,3,2,2,7,3,0,2,8,3,3,
        //3,0,0,0,3,1,1,0,3,2,1,0,3,3,1,0,3,4,2,0,3,5,2,0,3,6,4,0,3,7,4,0,3,8,4,0,3,9,4,0,3,10,8,0,3,11,8,0,3,12,8,0,3,13,6,0,3,14,6,0,3,15,0,0,3,16,0,0};
        nodes =GlobalVar._instance.TreeGen;    
        layerNum = GetLayer();
        nodePos = new List<Vector3>();
        layerWidth = new int[layerNum + 1];

        if(layerNum == -1)
        {
            Debug.Log("error");
            return;
        }

        nodePos.Add(new Vector3(0, 1, 0));
        layerWidth[0] = 0;

        FindPos();

        DistributePos();
    }


    private int GetLayer()
    {
        int layerNum = -1;

        for (int i = 0; i < nodes.Length / 4; i++)
        {
            if (nodes[4 * i] > layerNum)
            {
                layerNum = nodes[4 * i];
            }
        }

        return layerNum;
    }

    private void FindPos()
    {
        //每一层节点数量
        int nodeNum;
        Vector3 curPos;

        for (int i = 1; i <= layerNum; i++)
        {
            nodeNum = 0;
            for (int j = 0; j < nodes.Length / 4; j++)
            {
                if (nodes[4 * j] == i)
                {
                    nodeNum++;
                }
            }

            //取定合适的边长
            int width = 2 * (int)Mathf.Log(nodeNum) + 1;
            layerWidth[i] = width;

            for (int k = 0; k < nodeNum; k++)
            {
                curPos = new Vector3(Random.Range(-width, width + 1), i * layerHeight + 1, Random.Range(-width, width + 1));
                
                while (nodePos.Contains(curPos))
                {
                    curPos = new Vector3(Random.Range(-width, width + 1), i * layerHeight + 1, Random.Range(-width, width + 1));
                }

                nodePos.Add(curPos);
                
            }
        }

        
    }

    private void InsCube()
    {
        for(int i = 0; i < nodePos.Count; i++)
        {
            Instantiate(prefab, nodePos[i], transform.rotation);
        }
    }


    private List<int> NodeSort(List<GameObject> nodes)
    {
        int[] sortArray = new int[2 * nodes.Count];
        List<int> returnArray = new List<int>();

        for(int i = 0; i < nodes.Count; i++)
        {
            TreeNode tn = nodes[i].GetComponent<TreeNode>();
            sortArray[2 * i] = tn.num;
            sortArray[2 * i + 1] = OutPriority(nodes[i].transform.position);
        }

        int temp1;
        int temp2;
        for(int j = 0; j < nodes.Count; j++)
        {
            for(int k = 0; k < (nodes.Count - 1); k++)
            {
                if(sortArray[2 * k + 1] < sortArray[2 * (k + 1) + 1])
                {
                    temp1 = sortArray[2 * k];
                    temp2 = sortArray[2 * k + 1];
                    sortArray[2 * k] = sortArray[2 * (k + 1)];
                    sortArray[2 * k + 1] = sortArray[2 * (k + 1) + 1];
                    sortArray[2 * (k + 1)] = temp1;
                    sortArray[2 * (k + 1) + 1] = temp2;
                }
            }
        }

        for(int m = 0; m < nodes.Count; m++)
        {
            returnArray.Add(sortArray[2 * m]);
        }

        return returnArray;
    }

    private int OutPriority(Vector3 pos)
    {
        int xmax = (int)(pos.x < 0 ? -pos.x : pos.x);
        int zmax = (int)(pos.z < 0 ? -pos.z : pos.z);

        if(xmax > zmax)
        {
            return xmax;
        }
        else
        {
            return zmax;
        }
    }

    private void DistributePos()
    {
        int ran;
        //父节点层排序后顺序
        List<int> lastLayerOrder = new List<int>();

        //一层所有生成的节点，用于该层节点从内到外排序
        List<GameObject> currentLayerNodes = new List<GameObject>();

        //一层所有节点，用于生成和寻找父节点     层内序号，父节点层内序号
        List<int> certainLayer = new List<int>();

        for(int i = 0; i <= layerNum; i++)
        {
            if(i == 0)
            {
                GameObject n = Instantiate(prefab, nodePos[0], transform.rotation);
                Instantiate(link, new Vector3(0, 0.125f, 0), transform.rotation);
                TreeNode tn = n.GetComponent<TreeNode>();
                tn.layer = 0;
                tn.num = 0;
                tn.father = 0;
                n.name = i.ToString() + "-" + 0.ToString();
                nodePos.RemoveAt(0);
                print("init"+ n.name);
                tn.ReStart();
            }
            else if(i == 1)
            {
                for(int j = 0; j < nodes.Length / 4; j++)
                {
                    if(nodes[4 * j] == i)
                    {
                        ran = Random.Range(-1, 2);
                        GameObject n = Instantiate(prefab, nodePos[0] + new Vector3(0, ran, 0), transform.rotation);
                        n.name = i.ToString() + "-" + nodes[4 * j + 1].ToString();
                        GameObject father = GameObject.Find("0-0");
                        n.transform.SetParent(father.transform);
                        nodePos.RemoveAt(0);
                        TreeNode tn = n.GetComponent<TreeNode>();
                        tn.layer = i;
                        tn.num = nodes[4 * j + 1];
                        tn.father = 0;
                        currentLayerNodes.Add(n);
                        tn.ReStart();
                        print("init"+ n.name);
                    }
                }
                lastLayerOrder = NodeSort(currentLayerNodes);
                currentLayerNodes.Clear();
            }
            else
            {
                //找到该层所有节点
                for(int j = 0; j < nodes.Length / 4; j++)
                {
                    if(nodes[4 * j] == i)
                    {
                        certainLayer.Add(nodes[4 * j + 1]);//自己序号
                        certainLayer.Add(nodes[4 * j + 2]);//父节点序号
                    }
                }

                for(int m = 0; m < lastLayerOrder.Count; m++)               //外层循环父节点，从外而内遍历
                {
                    GameObject father = GameObject.Find((i - 1).ToString() + "-" + lastLayerOrder[m].ToString());
                    int fatherWidth = layerWidth[i - 1];
                    int childWidth = layerWidth[i];

                    Vector3 mapPoint;
                    float mapX = father.transform.position.x * (float)childWidth / fatherWidth;
                    float mapY = father.transform.position.y + layerHeight;
                    float mapZ = father.transform.position.z * (float)childWidth / fatherWidth;
                    mapPoint = new Vector3(mapX, mapY, mapZ);

                    for (int n = 0; n < certainLayer.Count / 2; n++)         //遍历该层节点，找到该父节点的每一个子节点，每个子结点找一个距父节点最近的位置，然后删掉该位置
                    {
                        if(certainLayer[2 * n + 1] == lastLayerOrder[m])
                        {
                            ran = Random.Range(-1, 2);
                            Vector3 pos = FindChildrenPos(i, mapPoint);
                            GameObject c = Instantiate(prefab, pos + new Vector3(0, ran, 0), transform.rotation);
                            c.name = i.ToString() + "-" + certainLayer[2 * n].ToString();
                            c.transform.SetParent(father.transform);
                            currentLayerNodes.Add(c);
                            TreeNode tn = c.GetComponent<TreeNode>();
                            tn.layer = i;
                            tn.num = certainLayer[2 * n];
                            tn.father = lastLayerOrder[m];
                            nodePos.Remove(pos);
                            tn.ReStart();
                            print("init"+ c.name);
                        }
                    }
                }

                lastLayerOrder = NodeSort(currentLayerNodes);
                currentLayerNodes.Clear();
                certainLayer.Clear();
            }
        }
    }


    private Vector3 FindChildrenPos(int layer, Vector3 mapPoint)
    {
        Vector3 temp = nodePos[0];
        float min = Vector3.Distance(mapPoint, nodePos[0]);

        for(int i = 0; i < nodePos.Count; i++)
        {
            if(nodePos[i].y < layerHeight * layer - 1 || nodePos[i].y > layerHeight * layer + 1)
            {
                break;
            }

            if(Vector3.Distance(mapPoint,nodePos[i]) < min)
            {
                min = Vector3.Distance(mapPoint, nodePos[i]);
                temp = nodePos[i];
            }
        }

        return temp;
        //GameObject self = GameObject.Find(layer.ToString() + "-" + num.ToString());
        //TreeNode tn = self.GetComponent<TreeNode>();
        //int childNum = tn.childNum;
        //if(childNum == 0)
        //{
        //    return new List<Vector3>();
        //}

        //int currentWidth = layerWidth[layer];
        //int nextWidth = layerWidth[layer + 1];
        //Vector3 mapPoint;
        //float mapX = (float)self.transform.position.x * nextWidth / currentWidth;
        //float mapY = (float)self.transform.position.y + layerHeight;
        //float mapZ = (float)self.transform.position.z * nextWidth / currentWidth;
        //mapPoint = new Vector3(mapX, mapY, mapZ);

        //List<Vector3> pos = new List<Vector3>();
        //for(int i = 0; i < nodePos.Count; i++)
        //{
        //    if(nodePos[i].y >= (mapY - 1) && nodePos[i].y <= (mapY + 1))
        //    {
        //        pos.Add(nodePos[i]);
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}

        //for(int m = 0; m < childNum; m++)
        //{
        //    for(int n = pos.Count - 1; n > 0; n--)
        //    {
        //        if(Vector3.Distance(mapPoint,pos[n]) < Vector3.Distance(mapPoint, pos[n - 1]))
        //        {
        //            Vector3 temp = pos[n];
        //            pos[n] = pos[n + 1];
        //            pos[n + 1] = temp;
        //        }
        //    }
        //}

        //List<Vector3> returnPos = new List<Vector3>();
        //returnPos = pos.GetRange(0, childNum);
        //return returnPos;
    }



}
