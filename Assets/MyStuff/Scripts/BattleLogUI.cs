using System.Collections;
using TMPro;
using UnityEngine;

public class BattleLogUI : MonoBehaviour
{
    [SerializeField] private TMP_Text logText;
    [SerializeField] private float defaultWaitTime = 0.8f;

    public IEnumerator ShowLog(string message)
    {
        if (logText != null)
        {
            logText.text = message;
        }

        Debug.Log(message);

        yield return new WaitForSeconds(defaultWaitTime);
    }

    public void SetText(string message)
    {
        if (logText != null)
        {
            logText.text = message;
        }

        Debug.Log(message);
    }
}