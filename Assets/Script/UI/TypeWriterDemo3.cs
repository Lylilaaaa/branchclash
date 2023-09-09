using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeWriterDemo3 : MonoBehaviour
{
    public float charactersPerSecond = 10f;
    public string[] stringToRun; 
    public AudioClip typingSound;
    public EndGameReloader egReloader;
    
    private TextMeshProUGUI textComponent;

    private bool isTyping;
    private string[] currentText;
    private int currentLineIndex = 0; // 当前行索引
    private int currentCharIndex = 0; // 当前字符索引
    //private AudioSource audioSource;
    
    private void Awake()
    {
        Debug.Log("==========end scene reloaded!============");
        SoundManager._instance.StopMusicSound();
        isTyping = false;
        egReloader.endButton.interactable = false;
        textComponent = transform.GetComponent<TextMeshProUGUI>();
        textComponent.text = ""; // 清空TextMeshProUGUI文本内容
        stringToRun = getEndString();
        StartTyping(stringToRun);
    }

    private string[] getEndString()
    {
        string[] _stringToRun = new string[4];
        if (GlobalVar._instance.role == 0) //士兵
        {
            if (GlobalVar._instance.gameResult == 0)
            {
                _stringToRun = new string[6];
                _stringToRun[0] = "Through your cunning tactics and the valiant struggles of our warriors,";
                _stringToRun[1] = "humanity has once more emerged triumphant against the relentless worm horde.";
                _stringToRun[2] = "Against all odds, you thwarted onslaughts from a horde of ravenous creatures,";
                _stringToRun[3] = "and our bastion remained unscathed.";
                _stringToRun[4] = "Stay resolute, for on this steadfast path,";
                _stringToRun[5] = "humanity shall forge an even grander future amidst the annals of history.";
            }
            else if (GlobalVar._instance.gameResult == 1)
            {
                _stringToRun = new string[6];
                _stringToRun[0] = "Through your cunning tactics and the valiant struggles of our warriors,";
                _stringToRun[1] = "humanity has once more emerged triumphant against the relentless worm horde.";
                _stringToRun[2] = "Against all odds, you thwarted onslaughts from a horde of ravenous creatures,";
                _stringToRun[3] = "and though our bastion suffered "+CurNodeDataSummary._instance.homeDestroyData+" points of grievous harm, it stands defiant.";
                _stringToRun[4] = "Stay resolute, for on this steadfast path,";
                _stringToRun[5] = "humanity shall forge an even grander future amidst the annals of history.";
            }
            else if (GlobalVar._instance.gameResult == 2)
            {
                _stringToRun = new string[8];
                _stringToRun[0] = "In the midst of your valiant struggle alongside the stalwart warriors, destiny chose another path,";
                _stringToRun[1] = "and triumph slipped through your grasp.";
                _stringToRun[2] = "Following the onslaught launched by the worm horde,";
                _stringToRun[3] = "the bastion bore the weight of "+CurNodeDataSummary._instance.homeDestroyData+" points of unrelenting devastation.";
                _stringToRun[4] =
                    "A shadow appeared to fall upon humanity's tomorrows, casting doubt upon the horizon.";
                _stringToRun[5] = "But despair not, for within the crucible of perseverance lies the ember of hope.";
                _stringToRun[6] = "Forge ahead, for even amidst the darkest of hours,";
                _stringToRun[7] = "the flame of humanity's potential still flickers with an undying radiance.";
            }
            
        }
        else //研究者
        {
            _stringToRun = new string[6];
            _stringToRun[0] = "You have refined the debuff against the worm race. ";
            _stringToRun[1] = "What ripples shall thy choice engender? Await the passage of time to grant humanity its answer. ";
            _stringToRun[2] = "Constraining the elusive worm horde is no fleeting endeavor; ";
            _stringToRun[3] = "a multitude of strategies will pave the way for humanity's future. ";
            _stringToRun[4] = "Oh, sagacious harbinger, advance with unwavering determination, ";
            _stringToRun[5] = "carving out a broader landscape for the preservation of humanity's existence.";
        }
        return _stringToRun;
    }

    public void StartTyping(string[] newText)
    {
        if (!isTyping)
        {
            currentText = newText;
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {        
        isTyping = true;
        int countText = 0;
        
        while (currentLineIndex < currentText.Length)
        {
            string line = currentText[currentLineIndex];
            while (currentCharIndex < line.Length)
            {
                textComponent.text += line[currentCharIndex];
                countText += 1;
                // 播放打字机音效
                int randomCount = Random.Range(2, 6);
                if (typingSound != null&& countText==randomCount)
                {
                    countText = 0;
                    SoundManager._instance.PlayEffectSound(typingSound);

                }
                if (typingSound != null&&countText >= 6)
                {
                    countText = 0;
                    SoundManager._instance.PlayEffectSound(typingSound);
                }

                currentCharIndex++;
                yield return new WaitForSeconds(1f / charactersPerSecond);
            }

            // 增加当前行索引，重置字符索引
            currentLineIndex++;
            currentCharIndex = 0;

            // 换行
            if (currentLineIndex < currentText.Length)
            {
                textComponent.text += "\n";
                yield return new WaitForSeconds(0.3f); // 在换行后等待一段时间
            }
        }
        isTyping = false;
        egReloader.endButton.interactable = true;
    }
}

