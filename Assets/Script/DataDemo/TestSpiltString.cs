using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpiltString : MonoBehaviour
{
    public string input = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";

    public string[] mapmapStringList;
    public string[][] mapList;
    public int mapmapRowCount;
    public int mapmapColCount;
    public int rowIndex;
    void Start()
    {
        string[] rows = input.Split("/n");
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
        
        for (int i = 0; i < stringList.Length; i++)
        {
            if (stringList[i] != null)
            {
                string[] row = stringList[i];
                List<string> newRow = new List<string>();

                for (int j = 0; j < row.Length; j++)
                {
                    if (row[j] != null&&row[j] != "")
                    {
                        newRow.Add(row[j]);
                    }
                }
                // ¸²¸ÇÔ­Ê¼ÐÐ
                stringList[i] = newRow.ToArray();
            }
        }
        mapmapStringList = stringList[0];
        mapList = stringList;
    }

    // Update is called once per frame
    void Update()
    {
        mapmapStringList = mapList[rowIndex];
        mapmapRowCount = mapList.Length;
        mapmapColCount = mapList[0].Length;
    }
}
