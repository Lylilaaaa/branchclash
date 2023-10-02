using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BroadCastURL_TMPNetWork : MonoBehaviour,IPointerClickHandler
{
    public TextMeshProUGUI netWorkText;

    public string url;
    // Start is called before the first frame update


    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 pos = new Vector3(eventData.position.x, eventData.position.y, 0);
        Application.OpenURL(url);
    }
}
