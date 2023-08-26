using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TmpFlicker : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public float fadeDuration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        // 启动协程，开始闪烁效果
        StartCoroutine(FlickerCoroutine());
    }

    private System.Collections.IEnumerator FlickerCoroutine()
    {
        while (true)
        {
            // 淡入效果
            yield return FadeImage(0f, 1f, fadeDuration);

            // 等待一段时间
            yield return new WaitForSeconds(0.5f);

            // 淡出效果
            yield return FadeImage(1f, 0f, fadeDuration);

            // 等待一段时间
            yield return new WaitForSeconds(0.5f);
        }
    }
    private System.Collections.IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        Color startColor = tmp.color;
        Color endColor = tmp.color;
        startColor.a = startAlpha;
        endColor.a = endAlpha;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 插值计算当前的颜色值
            float t = elapsedTime / duration;
            tmp.color = Color.Lerp(startColor, endColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 设置最终的颜色值
        tmp.color = endColor;
    }
}
