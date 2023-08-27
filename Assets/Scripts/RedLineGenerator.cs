using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 Description:       Instantiate all connecting lines for downward tree
 Unity Version:     2020.3.15f2c1
 Author:            ZHUANG Yan
 Date:              01/01/2023
 Last Modified:     06/12/2023
 */

public class RedLineGenerator : MonoBehaviour
{
    public static RedLineGenerator _instance;
    public Transform parent;
    //六位二进制表示有无，x+,x-,y+,y-,z+,z-
    public static Dictionary<Vector3, int> lineMap = new Dictionary<Vector3, int>();
    public static List<Vector3> majorChain = new List<Vector3>();
    public GameObject type2_1;
    public GameObject type2_2;
    public GameObject type3_1;
    public GameObject type3_2;
    public GameObject type4_1;
    public GameObject type4_2;
    public GameObject type4_3;
    public GameObject type5_1;
    public GameObject type6_1;

    public GameObject type2_1_m;
    public GameObject type2_2_m;
    public GameObject type3_1_m;
    public GameObject type3_2_m;
    public GameObject type4_1_m;
    public GameObject type4_2_m;
    public GameObject type4_3_m;
    public GameObject type5_1_m;
    public GameObject type6_1_m;
    private void Awake()
    {
        _instance = this;
        
    }
    private void Start()
    {
        ReStart();
    }

    public void ReStart()
    {
        type2_1 = Resources.Load<GameObject>("2.1_red");
        type2_2 = Resources.Load<GameObject>("2.2_red");
        type3_1 = Resources.Load<GameObject>("3.1_red");
        type3_2 = Resources.Load<GameObject>("3.2_red");
        type4_1 = Resources.Load<GameObject>("4.1_red");
        type4_2 = Resources.Load<GameObject>("4.2_red");
        type4_3 = Resources.Load<GameObject>("4.3_red");
        type5_1 = Resources.Load<GameObject>("5.1_red");
        type6_1 = Resources.Load<GameObject>("6.1_red");

        type2_1_m = Resources.Load<GameObject>("2.1_pink");
        type2_2_m = Resources.Load<GameObject>("2.2_pink");
        type3_1_m = Resources.Load<GameObject>("3.1_pink");
        type3_2_m = Resources.Load<GameObject>("3.2_pink");
        type4_1_m = Resources.Load<GameObject>("4.1_pink");
        type4_2_m = Resources.Load<GameObject>("4.2_pink");
        type4_3_m = Resources.Load<GameObject>("4.3_pink");
        type5_1_m = Resources.Load<GameObject>("5.1_pink");
        type6_1_m = Resources.Load<GameObject>("6.1_pink");

        Invoke("GenerateLine", 3);
    }



    public void GenerateLine()
    {
        foreach (var temp in lineMap)
        {
            switch (temp.Value)
            {
                //000011
                case 3:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_1_m : type2_1, temp.Key, Quaternion.Euler(90, 0, 0),parent);
                    break;

                //000101
                case 5:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(0, 90, -90),parent);
                    break;

                //000110
                case 6:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(0, -90, -90),parent);
                    break;

                //000111
                case 7:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(180, 90, 0),parent);
                    break;

                //001001
                case 9:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(0, 90, 0),parent);
                    break;

                //001010
                case 10:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(0, -90, 0),parent);
                    break;

                //001011
                case 11:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(0, 90, 0),parent);
                    break;

                //001100
                case 12:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_1_m : type2_1, temp.Key, Quaternion.Euler(0, 0, 0),parent);
                    break;

                //001101
                case 13:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(0, 90, -90),parent);
                    break;

                //001110
                case 14:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(0, 90, 90),parent);
                    break;

                //001111
                case 15:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_2_m : type4_2, temp.Key, Quaternion.Euler(0, 0, 90),parent);
                    break;

                //010001
                case 17:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(-90, 0, 90),parent);
                    break;

                //010010
                case 18:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(90, 0, 90),parent);
                    break;

                //010011
                case 19:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(90, 0, 90),parent);
                    break;

                //010100
                case 20:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(180, 0, 90),parent);
                    break;

                //010101
                case 21:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_2_m : type3_2, temp.Key, Quaternion.Euler(-90, 0, 0),parent);
                    break;

                //010110
                case 22:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_2_m : type3_2, temp.Key, Quaternion.Euler(180, 0, 0),parent);
                    break;

                //010111
                case 23:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(0, 90, 180),parent);
                    break;

                //011000
                case 24:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(0, 0, 90),parent);
                    break;

                //011001
                case 25:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_2_m : type3_2, temp.Key, Quaternion.Euler(0, 0, 0),parent);
                    break;

                //011010
                case 26:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_2_m : type3_2, temp.Key, Quaternion.Euler(90, 0, 0),parent);
                    break;

                //011011
                case 27:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(90, 90, 180),parent);
                    break;

                //011100
                case 28:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(0, 0, 90),parent);
                    break;

                //011101
                case 29:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(0, 0, 90),parent);
                    break;

                //011110
                case 30:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(0, 90, 90),parent);
                    break;

                //011111
                case 31:
                    Instantiate(majorChain.Contains(temp.Key) ? type5_1_m : type5_1, temp.Key, Quaternion.Euler(0, 0, -90),parent);
                    break;

                //100001
                case 33:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(-90, 0, 0),parent);
                    break;

                //100010
                case 34:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(90, 0, 0),parent);
                    break;

                //100011
                case 35:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(-90, -90, 0),parent);
                    break;

                //100100
                case 36:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(180, 0, 0),parent);
                    break;

                //100101
                case 37:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_2_m : type3_2, temp.Key, Quaternion.Euler(0, 0, 180),parent);
                    break;

                //100110
                case 38:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_2_m : type3_2, temp.Key, Quaternion.Euler(0, -90, 180),parent);
                    break;

                //100111
                case 39:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(-90, -90, 0),parent);
                    break;

                //101000
                case 40:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_2_m : type2_2, temp.Key, Quaternion.Euler(0, 0, 0),parent);
                    break;

                //101001
                case 41:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_2_m : type3_2, temp.Key, Quaternion.Euler(0, 0, -90),parent);
                    break;

                //101010
                case 42:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_2_m : type3_2, temp.Key, Quaternion.Euler(0, 180, 0),parent);
                    break;

                //101011
                case 43:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(0, -90, 0),parent);
                    break;

                //101100
                case 44:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(0, 0, -90),parent);
                    break;

                //101101
                case 45:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(0, -90, 90),parent);
                    break;

                //101110
                case 46:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(0, 180, 90),parent);
                    break;

                //101111
                case 47:
                    Instantiate(majorChain.Contains(temp.Key) ? type5_1_m : type5_1, temp.Key, Quaternion.Euler(0, 0, 90),parent);
                    break;

                //110000
                case 48:
                    Instantiate(majorChain.Contains(temp.Key) ? type2_1_m : type2_1, temp.Key, Quaternion.Euler(0, 0, 90),parent);
                    break;

                //110001
                case 49:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(-90, 0, 0),parent);
                    break;

                //110010
                case 50:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(90, 0, 0),parent);
                    break;

                //110011
                case 51:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_2_m : type4_2, temp.Key, Quaternion.Euler(0, 0, 0),parent);
                    break;

                //110100
                case 52:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(180, 0, 0),parent);
                    break;

                //110101
                case 53:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(-90, 0, 0),parent);
                    break;

                //110110
                case 54:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(180, 0, 0),parent);
                    break;

                //110111
                case 55:
                    Instantiate(majorChain.Contains(temp.Key) ? type5_1_m : type5_1, temp.Key, Quaternion.Euler(0, 0, 0),parent);
                    break;

                //111000
                case 56:
                    Instantiate(majorChain.Contains(temp.Key) ? type3_1_m : type3_1, temp.Key, Quaternion.Euler(0, 0, 0),parent);
                    break;

                //111001
                case 57:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(0, 0, 0),parent);
                    break;

                //111010
                case 58:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_1_m : type4_1, temp.Key, Quaternion.Euler(90, 0, 0),parent);
                    break;

                //111011
                case 59:
                    Instantiate(majorChain.Contains(temp.Key) ? type5_1_m : type5_1, temp.Key, Quaternion.Euler(180, 0, 0),parent);
                    break;

                //111100
                case 60:
                    Instantiate(majorChain.Contains(temp.Key) ? type4_2_m : type4_2, temp.Key, Quaternion.Euler(90, 0, 0),parent);
                    break;

                //111101
                case 61:
                    Instantiate(majorChain.Contains(temp.Key) ? type5_1_m : type5_1, temp.Key, Quaternion.Euler(90, 0, 0),parent);
                    break;

                //111110
                case 62:
                    Instantiate(majorChain.Contains(temp.Key) ? type5_1_m : type5_1, temp.Key, Quaternion.Euler(-90, 0, 0),parent);
                    break;

                //111111
                case 63:
                    Instantiate(majorChain.Contains(temp.Key) ? type6_1_m : type6_1, temp.Key, Quaternion.Euler(0, 0, 0),parent);
                    break;
            }
        }
    }
    
}
