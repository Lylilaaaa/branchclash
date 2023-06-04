using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static Counter _instance;
    private string[] map_map_array;
    public List<string> map_map;

    public List<int> showRankMap1;

    public string showRanKMapString1;

    private string[] showRanKMapString1Count;

    public float AVERAGERANK;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _instance = this;
    }
    
    void Start()
    {
        map_map_array = new string[200];
        map_map = new List<string>(map_map_array);
        for (int i=1; i <= 19*9; i++){
            map_map[i] = "xx";
        }
        map_map[2] = "H";  map_map[17] = "R"; map_map[19] = "/n";
        map_map[21] = "R"; map_map[24] = "R"; map_map[25] = "R"; map_map[26] = "R"; map_map[27] = "R"; map_map[30] = "R";map_map[31] = "R";map_map[32] = "R";map_map[33] = "R"; map_map[36] = "R"; map_map[38] = "/n";
        map_map[40] = "R"; map_map[43] = "R"; map_map[46] = "R"; map_map[49] = "R"; map_map[52] = "R"; map_map[55] = "R"; map_map[57] = "/n";
        map_map[59] = "R"; map_map[62] = "R"; map_map[65] = "R"; map_map[68] = "R"; map_map[71] = "R"; map_map[74] = "R"; map_map[76] = "/n";
        map_map[78] = "R"; map_map[81] = "R"; map_map[84] = "R"; map_map[87] = "R"; map_map[90] = "R"; map_map[93] = "R"; map_map[95] = "/n";
        map_map[97] = "R"; map_map[100] = "R"; map_map[103] = "R"; map_map[106] = "R"; map_map[109] = "R"; map_map[112] = "R"; map_map[114] = "/n";
        map_map[116] = "R"; map_map[119] = "R"; map_map[122] = "R"; map_map[125] = "R"; map_map[128] = "R";map_map[131] = "R"; map_map[133] = "/n";
        map_map[135] = "R"; map_map[136] = "R"; map_map[137] = "R"; map_map[138] = "R"; map_map[141] = "R";map_map[142] = "R"; map_map[143] = "R"; map_map[144] = "R"; map_map[147] = "R"; map_map[148] = "R"; map_map[149] = "R"; map_map[150] = "R"; map_map[152] = "/n";
        map_map[171] = "/n";
        
        //rank for wood1 
        List<int> rankCheckIndex;
        //rankCheckIndex = new List<int>(){-20,-19,-18,-1,1,18,19,20};
        //rankCheckIndex = new List<int>(){-40,-39,-38,-37,-36,-21,-20,-19,-18,-17,-2,-1,1,2,17,18,19,20,21,36,37,38,39,40};
        //rankCheckIndex = new List<int>(){-60,-59,-58,-57,-56,-55,-54,-41,-40,-39,-38,-37,-36,-35,-22,-21,-20,-19,-18,-17,-16,-3,-2,-1,1,2,3,16,17,18,19,20,21,22,35,36,37,38,39,40,41,54,55,56,57,58,59,60};
        //rankCheckIndex = new List<int>() { -57,-38,-19,-3,-2,-1,1,2,3,19,38,57 };
        //rankCheckIndex = new List<int>() { -95,-76,-57,-38,-19,-5,-4,-3,-2,-1,1,2,3,4,5,19,38,57,76,95 };
        //rankCheckIndex = new List<int>() { -95,-76,-57,-38,-19,-10,-9,-8,-7,-6,-5,-4,-3,-2,-1,1,2,3,4,5,6,7,8,9,10,19,38,57,76,95 };
        //rankCheckIndex = new List<int>() { -20,-19,-18,-17,-1,2,18,19,20,21 };
        //rankCheckIndex = new List<int>() { -40,-39,-38,-37,-36,-35,-21,-20,-19,-18,-17,-16,-2,-1,2,3,17,18,19,20,21,22,36,37,38,39,40,41 };
        rankCheckIndex = new List<int>() { -60,-59,-58,-57,-56,-55,-54,-53,-41,-40,-39,-38,-37,-36,-35,-34,-22,-21,-20,-19,-18,-17,-16,-15,-3,-2,-1,2,3,4,16,17,18,19,20,21,22,23,35,36,37,38,39,40,41,42,54,55,56,57,58,59,60,61 };
        for (int i=1; i <= 19*9; i++){
            int tempRankThisPos = 0;
            if (map_map[i] == "xx")
            {
                foreach (int j in rankCheckIndex)
                {
                    if (i + j < 1 || i + j > 19*9)
                    {
                        continue;
                    }
                    if (map_map[i + j] == "R" || map_map[i + j] == "H")
                    {
                        tempRankThisPos += 1;
                    }
                }
                showRankMap1.Add(tempRankThisPos);
            }
            else
            {
                showRankMap1.Add(tempRankThisPos);
            }
        }

        foreach (int k in showRankMap1)
        {
            showRanKMapString1 += k.ToString();
            showRanKMapString1 += ",";
        }
        print(showRanKMapString1);
        showRanKMapString1Count = showRanKMapString1.Split(',');
        print(showRanKMapString1Count.Length);
        calculateAverage();
    }
    
    //check the first

    void calculateAverage()
    {
        int totalrank = 0;
        int biggerZeroCounter = 0;
        foreach (int k in showRankMap1)
        {
            if (k > 0)
            {
                totalrank += k;
                biggerZeroCounter += 1;
            }
        }

        AVERAGERANK = (float)(totalrank)  / (float)biggerZeroCounter;
    }

}
