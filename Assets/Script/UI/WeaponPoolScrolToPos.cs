using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPoolScrolToPos : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 5f;
    public float scrollAmount = 120f;

    private float targetXPosition = 0f;
    private int childCount;
    

    private void Start()
    {
        childCount = transform.GetChild(0).childCount-1;
        scrollRect = GetComponent<ScrollRect>();
        UpdateScrollPosition();
    }

    private void LateUpdate()
    {
        childCount = transform.GetChild(0).childCount-1;
        UpdateScrollPosition();
    }

    private void UpdateScrollPosition()
    {
        float normalizedTargetPosition = targetXPosition / scrollRect.content.rect.width;
        //print("targetXPosition / scrollRect.content.rect.width: "+normalizedTargetPosition);
        float currentNormalizedPosition = scrollRect.horizontalNormalizedPosition;
        //print("scrollRect.horizontalNormalizedPosition: "+currentNormalizedPosition);

        if (Mathf.Approximately(currentNormalizedPosition, normalizedTargetPosition))
        {
            return;
        }

        scrollRect.horizontalNormalizedPosition = Mathf.MoveTowards(currentNormalizedPosition, normalizedTargetPosition, scrollSpeed * Time.deltaTime);
    }

    public void ScrollRight()
    {
        float scrollAmount = scrollRect.content.rect.width / childCount;
        targetXPosition += scrollAmount;
        //print("targetXposition: "+targetXPosition);

        // 限制目标位置不超过content的宽度
        targetXPosition = Mathf.Clamp(targetXPosition, 0f, scrollRect.content.rect.width);
    }
    public void ScrollLeft()
    {
        float scrollAmount = scrollRect.content.rect.width / childCount;
        targetXPosition -= scrollAmount;

        // 限制目标位置不超过content的宽度
        targetXPosition = Mathf.Clamp(targetXPosition, 0f, scrollRect.content.rect.width);
        
    }
}
