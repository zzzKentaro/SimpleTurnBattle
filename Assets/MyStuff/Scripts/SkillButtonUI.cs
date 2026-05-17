using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private Image skillIconImage;

    private int skillIndex;

    public void Setup(int index, SkillData skillData, Action<int> onClick)
    {
        skillIndex = index;

        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.RemoveAllListeners();

        if (skillData == null)
        {
            if (skillNameText != null)
            {
                skillNameText.text = "-";
            }

            if (skillIconImage != null)
            {
                skillIconImage.enabled = false;
            }

            button.interactable = false;
            return;
        }

        if (skillNameText != null)
        {
            skillNameText.text = skillData.skillName;
        }

        if (skillIconImage != null)
        {
            skillIconImage.sprite = skillData.skillIcon;
            skillIconImage.enabled = skillData.skillIcon != null;
        }

        button.interactable = true;

        button.onClick.AddListener(() =>
        {
            onClick?.Invoke(skillIndex);
        });
    }

    public void SetInteractable(bool interactable)
    {
        if (button != null)
        {
            button.interactable = interactable;
        }
    }
}