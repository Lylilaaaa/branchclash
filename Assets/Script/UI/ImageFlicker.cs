using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFlicker : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 0.5f;

    private void Start()
    {
        image = GetComponent<Image>();
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
        Color startColor = image.color;
        Color endColor = image.color;
        startColor.a = startAlpha;
        endColor.a = endAlpha;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 插值计算当前的颜色值
            float t = elapsedTime / duration;
            image.color = Color.Lerp(startColor, endColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 设置最终的颜色值
        image.color = endColor;
    }


}
