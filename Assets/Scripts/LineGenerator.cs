using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    //六位二进制表示有无，x+,x-,y+,y-,z+,z-
    public static Dictionary<Vector3, int> lineMap = new Dictionary<Vector3, int>();
    public GameObject type2_1;
    public GameObject type2_2;
    public GameObject type3_1;
    public GameObject type3_2;
    public GameObject type4_1;
    public GameObject type4_2;
    public GameObject type4_3;
    public GameObject type5_1;
    public GameObject type6_1;


    private void Start()
    {
        type2_1 = Resources.Load<GameObject>("2.1");
        type2_2 = Resources.Load<GameObject>("2.2");
        type3_1 = Resources.Load<GameObject>("3.1");
        type3_2 = Resources.Load<GameObject>("3.2");
        type4_1 = Resources.Load<GameObject>("4.1");
        type4_2 = Resources.Load<GameObject>("4.2");
        type4_3 = Resources.Load<GameObject>("4.3");
        type5_1 = Resources.Load<GameObject>("5.1");
        type6_1 = Resources.Load<GameObject>("6.1");

        Invoke("GenerateLine", 3);
    }



    public void GenerateLine()
    {
        foreach(var temp in lineMap)
        {
            switch(temp.Value)
            {
                //000011
                case 3:
                    Instantiate(type2_1, temp.Key, Quaternion.Euler(90, 0, 0));
                    break;

                //000101
                case 5:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(0, 90, -90));
                    break;

                //000110
                case 6:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(0, -90, -90));
                    break;

                //000111
                case 7:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(180, 90, 0));
                    break;

                //001001
                case 9:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(0, 90, 0));
                    break;

                //001010
                case 10:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(0, -90, 0));
                    break;

                //001011
                case 11:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(0, 90, 0));
                    break;

                //001100
                case 12:
                    Instantiate(type2_1, temp.Key, Quaternion.Euler(0, 0, 0));
                    break;

                //001101
                case 13:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(0, 90, -90));
                    break;

                //001110
                case 14:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(0, 90, 90));
                    break;

                //001111
                case 15:
                    Instantiate(type4_2, temp.Key, Quaternion.Euler(0, 0, 90));
                    break;

                //010001
                case 17:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(-90, 0, 90));
                    break;

                //010010
                case 18:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(90, 0, 90));
                    break;

                //010011
                case 19:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(90, 0, 90));
                    break;

                //010100
                case 20:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(180, 0, 90));
                    break;

                //010101
                case 21:
                    Instantiate(type3_2, temp.Key, Quaternion.Euler(-90, 0, 0));
                    break;

                //010110
                case 22:
                    Instantiate(type3_2, temp.Key, Quaternion.Euler(180, 00, 0));
                    break;

                //010111
                case 23:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(0, 90, 180));
                    break;

                //011000
                case 24:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(0, 0, 90));
                    break;

                //011001
                case 25:
                    Instantiate(type3_2, temp.Key, Quaternion.Euler(0, 0, 0));
                    break;

                //011010
                case 26:
                    Instantiate(type3_2, temp.Key, Quaternion.Euler(90, 0, 0));
                    break;

                //011011
                case 27:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(90, 90, 180));
                    break;

                //011100
                case 28:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(0, 0, 90));
                    break;

                //011101
                case 29:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(0, 0, 90));
                    break;

                //011110
                case 30:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(0, 90, 90));
                    break;

                //011111
                case 31:
                    Instantiate(type5_1, temp.Key, Quaternion.Euler(0, 0, -90));
                    break;

                //100001
                case 33:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(-90, 0, 0));
                    break;

                //100010
                case 34:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(90, 0, 0));
                    break;

                //100011
                case 35:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(-90, -90, 0));
                    break;

                //100100
                case 36:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(180, 0, 0));
                    break;

                //100101
                case 37:
                    Instantiate(type3_2, temp.Key, Quaternion.Euler(0, 0, 180));
                    break;

                //100110
                case 38:
                    Instantiate(type3_2, temp.Key, Quaternion.Euler(0, -90, 180));
                    break;

                //100111
                case 39:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(-90, -90, 0));
                    break;

                //101000
                case 40:
                    Instantiate(type2_2, temp.Key, Quaternion.Euler(0, 0, 0));
                    break;

                //101001
                case 41:
                    Instantiate(type3_2, temp.Key, Quaternion.Euler(0, 0, -90));
                    break;

                //101010
                case 42:
                    Instantiate(type3_2, temp.Key, Quaternion.Euler(0, 180, 0));
                    break;

                //101011
                case 43:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(0, -90, 0));
                    break;

                //101100
                case 44:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(0, 0, -90));
                    break;

                //101101
                case 45:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(0, -90, 90));
                    break;

                //101110
                case 46:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(0, 180, 90));
                    break;

                //101111
                case 47:
                    Instantiate(type5_1, temp.Key, Quaternion.Euler(0, 0, 90));
                    break;

                //110000
                case 48:
                    Instantiate(type2_1, temp.Key, Quaternion.Euler(0, 0, 90));
                    break;

                //110001
                case 49:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(-90, 0, 0));
                    break;

                //110010
                case 50:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(90, 0, 0));
                    break;

                //110011
                case 51:
                    Instantiate(type4_2, temp.Key, Quaternion.Euler(0, 0, 0));
                    break;

                //110100
                case 52:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(180, 0, 0));
                    break;

                //110101
                case 53:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(-90, 0, 0));
                    break;

                //110110
                case 54:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(180, 0, 0));
                    break;

                //110111
                case 55:
                    Instantiate(type5_1, temp.Key, Quaternion.Euler(0, 0, 0));
                    break;

                //111000
                case 56:
                    Instantiate(type3_1, temp.Key, Quaternion.Euler(0, 0, 0));
                    break;

                //111001
                case 57:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(0, 0, 0));
                    break;

                //111010
                case 58:
                    Instantiate(type4_1, temp.Key, Quaternion.Euler(90, 0, 0));
                    break;

                //111011
                case 59:
                    Instantiate(type5_1, temp.Key, Quaternion.Euler(180, 0, 0));
                    break;

                //111100
                case 60:
                    Instantiate(type4_2, temp.Key, Quaternion.Euler(90, 0, 0));
                    break;

                //111101
                case 61:
                    Instantiate(type5_1, temp.Key, Quaternion.Euler(90, 0, 0));
                    break;

                //111110
                case 62:
                    Instantiate(type5_1, temp.Key, Quaternion.Euler(-90, 0, 0));
                    break;

                //111111
                case 63:
                    Instantiate(type6_1, temp.Key, Quaternion.Euler(0, 0, 0));
                    break;
            }
        }
    }
    
}
