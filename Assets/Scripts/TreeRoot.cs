//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TreeRoot : MonoBehaviour
//{
//    //层数，层内序号，父节点层内序号，子节点数量
//    public static int[] nodes;
//    public GameObject prefab;

//    public List<int> children;

//    private void Start()
//    {
//        nodes = new int[] {0,0,0,17,1,0,0,0,1,1,0,0,1,2,0,0,1,3,0,0,1,4,0,0,1,5,0,0,1,6,0,0,1,7,0,0,1,8,0,0,1,9,0,0,
//        1,10,0,0,1,11,0,0,1,12,0,0};

//        FindChildren();
//        SpawnChildren();

//    }


//    private void FindChildren()
//    {
//        for(int i=0; i<(nodes.Length/4); i++)
//        {
//            if(nodes[4 * i]==1)
//            {
//                if(nodes[4 * i+2]==0)
//                {
//                    children.Add(nodes[4 * i + 1]);
//                }
//            }
//        }
//    }


//    private void SpawnChildren()
//    {
//        int sideLength = children.Count / 4;
//        int offset;
//        float middle;
//        if((children.Count % 4) != 0)
//        {
//            sideLength += 1;
//        }
//        offset = sideLength - 1;
//        middle = (float)sideLength / 2f;

//        Debug.Log(children.Count);
//        Debug.Log(sideLength);
//        for(int i = 1; i <= children.Count; i++)
//        {
//            if(i % 4 == 1)  //俯视图左
//            {
//                if(i / 4 == 0)
//                {
//                    Vector3 posOffset = new Vector3((offset + 2), 3, -offset);
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 1;
//                }
//                else if(i == children.Count || i / 4 == (sideLength - 1))
//                {
//                    Vector3 posOffset = new Vector3((offset + 2), 3, offset);
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 2;
//                }
//                else
//                {
//                    Vector3 posOffset;
//                    if((i / 4) < middle)
//                    {
//                        posOffset = new Vector3((offset + 2), 3 + (i / 4), (-offset + (i / 4) * 2));
//                    }
//                    else
//                    {
//                        posOffset = new Vector3((offset + 2), 2 + sideLength - (i / 4), (-offset + (i / 4) * 2));
//                    }
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 5;
//                }
//            }
//            else if (i % 4 == 2)    //下
//            {
//                if (i / 4 == 0)
//                {
//                    Vector3 posOffset = new Vector3(offset, 3, (offset + 2));
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 2;
//                }
//                else if (i == children.Count || i / 4 == (sideLength - 1))
//                {
//                    Vector3 posOffset = new Vector3(-offset, 3, (offset + 2));
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 3;
//                }
//                else
//                {
//                    Vector3 posOffset;
//                    if ((i / 4) < middle)
//                    {
//                        posOffset = new Vector3((offset - (i / 4) * 2), 3 + (i / 4), (offset + 2));
//                    }
//                    else
//                    {
//                        posOffset = new Vector3((offset - (i / 4) * 2), 2 + sideLength - (i / 4), (offset + 2));
//                    }
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 6;
//                }
//            }
//            else if (i % 4 == 3)  //右
//            {
//                if (i / 4 == 0)
//                {
//                    Vector3 posOffset = new Vector3(-(offset + 2), 3, offset);
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 3;
//                }
//                else if (i == children.Count || i / 4 == (sideLength - 1))
//                {
//                    Vector3 posOffset = new Vector3(-(offset + 2), 3, -offset);
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 4;
//                }
//                else
//                {
//                    Vector3 posOffset;
//                    if ((i / 4) < middle)
//                    {
//                        posOffset = new Vector3(-(offset + 2), 3 + (i / 4), (offset - (i / 4) * 2));
//                    }
//                    else
//                    {
//                        posOffset = new Vector3(-(offset + 2), 2 + sideLength - (i / 4), (offset - (i / 4) * 2));
//                    }
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 5;
//                }
//            }
//            else   //上
//            {
//                if (i / 4 == 0)
//                {
//                    Vector3 posOffset = new Vector3(-offset, 3, -(offset + 2));
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 4;
//                }
//                else if (i == children.Count || i / 4 == (sideLength - 1))
//                {
//                    Vector3 posOffset = new Vector3(offset, 3, -(offset + 2));
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 1;
//                }
//                else
//                {
//                    Vector3 posOffset;
//                    if ((i / 4) < middle)
//                    {
//                        posOffset = new Vector3((-offset + (i / 4) * 2), 3 + (i / 4), -(offset + 2));
//                    }
//                    else
//                    {
//                        posOffset = new Vector3((-offset + (i / 4) * 2), 2 + sideLength - (i / 4), -(offset + 2));
//                    }
//                    Vector3 newPos = transform.position + posOffset;
//                    GameObject child = Instantiate(prefab, newPos, transform.rotation).gameObject;
//                    TreeNode tn = child.GetComponent<TreeNode>();
//                    tn.layer = 1;
//                    tn.num = children[i - 1];
//                    tn.type = 6;
//                }
//            }

//        }
//    }
//}
