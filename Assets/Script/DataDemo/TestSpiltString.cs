using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpiltString : MonoBehaviour
{
    public string input = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";

    public string[] mapmapStringList;
    public string[][] mapList;
    public int rowIndex;
    void Start()
    {
        string[] rows = input.Split('\n');
        string[][] stringList = new string[rows.Length][];
        for (int i = 0; i < rows.Length; i++)
        {
            stringList[i] = rows[i].Split(',');
        }
        for (int i = 0; i < stringList.Length / 2; i++)
        {
            string[] temp = stringList[i];
            stringList[i] = stringList[stringList.Length - 1 - i];
            stringList[stringList.Length - 1 - i] = temp;
        }

        string[][] withoutNull;
        withoutNull = new string[rows.Length][];
        for (int i = 0; i < stringList.Length; i++)
        {
            if (stringList[i] != null)
            {
                string[] row = stringList[i];
                List<string> newRow = new List<string>();
                int newIndex = 0;

                for (int j = 0; j < row.Length; j++)
                {
                    if (row[j] != null)
                    {
                        newRow[newIndex] = row[j];
                        newIndex++;
                    }
                }

                // ¸²¸ÇÔ­Ê¼ÐÐ
                //stringList[i] = newRow;
            }
        }
        mapmapStringList = stringList[0];
        mapList = stringList;
    }

    // Update is called once per frame
    void Update()
    {
        mapmapStringList = mapList[rowIndex];
    }
}
