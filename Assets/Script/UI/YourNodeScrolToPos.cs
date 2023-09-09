using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YourNodeScrolToPos : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 3f;

    private float targetXPosition = 0f;
    private int childCount;

    public HomePageSelectUI hpUI;
    

    private void Awake()
    {
        childCount = transform.GetChild(0).childCount-1;
        scrollRect = GetComponent<ScrollRect>();
        UpdateScrollPosition();

    }

    private void Update()
    {
        if (GlobalVar._instance.finalNodePrepared && hpUI._nodeFinish)
        {
            childCount = transform.GetChild(0).childCount-1;
            UpdateScrollPosition();
        }

    }

    private void UpdateScrollPosition()
    {
        float normalizedTargetPosition = targetXPosition / scrollRect.content.rect.height;
        //print("targetXPosition / scrollRect.content.rect.width: "+normalizedTargetPosition);
        float currentNormalizedPosition = scrollRect.verticalNormalizedPosition;
        //print("scrollRect.horizontalNormalizedPosition: "+currentNormalizedPosition);

        if (Mathf.Approximately(currentNormalizedPosition, normalizedTargetPosition))
        {
            return;
        }

        scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(currentNormalizedPosition, normalizedTargetPosition, scrollSpeed * Time.deltaTime);
    }

    public void ScrollUp()
    {
        float scrollAmount = (scrollRect.content.rect.height / (childCount))*5;
        targetXPosition += scrollAmount;
        //print("targetXposition: "+targetXPosition);

        // 限制目标位置不超过content的宽度
        targetXPosition = Mathf.Clamp(targetXPosition, 0f, scrollRect.content.rect.height);
    }
    public void ScrollDown()
    {
        float scrollAmount = (scrollRect.content.rect.height /( childCount))*5;
        targetXPosition -= scrollAmount;

        // 限制目标位置不超过content的宽度
        targetXPosition = Mathf.Clamp(targetXPosition, 0f, scrollRect.content.rect.height);
        
    }
}
