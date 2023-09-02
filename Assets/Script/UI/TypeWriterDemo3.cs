using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeWriterDemo3 : MonoBehaviour
{
    public float Speed = 15;
    public string stringToRun = "You have refined the debuff against the worm race. \n What ripples shall thy choice engender? Await the passage of time to grant humanity its answer. \n Constraining the elusive worm horde is no fleeting endeavor; \n a multitude of strategies will pave the way for humanity's future. \n Oh, sagacious harbinger, advance with unwavering determination, \n carving out a broader landscape for the preservation of humanity's existence.";
    //public AudioClip typingSound;
    public TextMeshProUGUI text;

    void Start( )
    {
        text = this.GetComponent<TextMeshProUGUI>();
        Run(stringToRun, text);
    }
    public void Run(string textToType, TextMeshProUGUI textLabel)
    {
        StartCoroutine(TypeText(textToType, textLabel));
    }
    IEnumerator TypeText(string textToType, TextMeshProUGUI textLabel)
    {
        float t = 0;//������ʱ��
        int charIndex = 0;//�ַ�������ֵ
        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * Speed;//�򵥼�ʱ����ֵ��t
            charIndex = Mathf.FloorToInt(t);//��tתΪint���͸�ֵ��charIndex
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            textLabel.text = textToType.Substring(0, charIndex);
            //SoundManager._instance.PlayEffectSound(typingSound);
            yield return null;
        }
        textLabel.text = textToType;
    }
}

