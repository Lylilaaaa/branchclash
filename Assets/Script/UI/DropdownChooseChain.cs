using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DropdownChooseChain : MonoBehaviour
{
    private TMP_Dropdown dropD;
    private RectTransform arrowRT;
    public WebLogin webLogin;

    public string[] chainName;
    // Start is called before the first frame update
    void Awake()
    {
        //UrLController._instance.ChangeServeDataSet("Polygon");
        dropD = GetComponent<TMP_Dropdown>();
        arrowRT = transform.GetChild(1).GetComponent<RectTransform>();
        Vector3 rota =  Vector3.zero;
        arrowRT.rotation = Quaternion.Euler(rota);
        dropD.onValueChanged.AddListener(ChoseCertainChain);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 4)
        {
            Vector3 rota = new Vector3(0f, 0f, 180f);
            arrowRT.rotation = Quaternion.Euler(rota);
        }
        else
        {
            Vector3 rota =  Vector3.zero;
            arrowRT.rotation = Quaternion.Euler(rota);
        }
    }

    public void ChoseCertainChain(int index)
    {
        print("chose netWork: "+index);
        dropD.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = chainName[index];
        if (index == 0)
        {
            webLogin.ChooseToChangeLine("AltLayer");
            UrLController._instance.ChangeServeDataSet("Sepolia");
        }
        else if(index == 1)
        {
            webLogin.ChooseToChangeLine("Polygon");
            UrLController._instance.ChangeServeDataSet("Polygon");
        }
        // else if(index == 2)
        // {
        //     webLogin.ChooseToChangeLine("AltLayer");
        //     UrLController._instance.ChangeServeDataSet("Sepolia");
        // }
    }
}
