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
        // ����Э�̣���ʼ��˸Ч��
        StartCoroutine(FlickerCoroutine());
    }

    private System.Collections.IEnumerator FlickerCoroutine()
    {
        while (true)
        {
            // ����Ч��
            yield return FadeImage(0f, 1f, fadeDuration);

            // �ȴ�һ��ʱ��
            yield return new WaitForSeconds(0.5f);

            // ����Ч��
            yield return FadeImage(1f, 0f, fadeDuration);

            // �ȴ�һ��ʱ��
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
            // ��ֵ���㵱ǰ����ɫֵ
            float t = elapsedTime / duration;
            tmp.color = Color.Lerp(startColor, endColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �������յ���ɫֵ
        tmp.color = endColor;
    }
}
