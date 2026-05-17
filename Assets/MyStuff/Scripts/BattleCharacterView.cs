using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacterView : MonoBehaviour
{
    [Header("表示")]
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text hpText;

    [Header("HPバー")]
    [SerializeField] private Image hpFillImage;

    [Header("エフェクト生成位置")]
    [SerializeField] private Transform effectPoint;

    [Header("ダメージ時の点滅")]
    [SerializeField] private float flashTime = 0.08f;
    [SerializeField] private Color damageFlashColor = Color.red;

    private Color originalColor = Color.white;

    public Vector3 EffectPosition
    {
        get
        {
            if (effectPoint != null)
            {
                return effectPoint.position;
            }

            return transform.position;
        }
    }

    public void Setup(CharacterData data, int currentHP)
    {
        if (characterImage != null)
        {
            characterImage.sprite = data.characterSprite;
            originalColor = characterImage.color;
        }

        if (nameText != null)
        {
            nameText.text = data.characterName;
        }

        RefreshHP(currentHP, data.maxHP);
    }

    public void RefreshHP(int currentHP, int maxHP)
    {
        if (hpText != null)
        {
            hpText.text = currentHP + " / " + maxHP;
        }

        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = (float)currentHP / maxHP;
        }
    }

    public IEnumerator AnimateHP(int fromHP, int toHP, int maxHP, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            int displayHP = Mathf.RoundToInt(Mathf.Lerp(fromHP, toHP, t));
            RefreshHP(displayHP, maxHP);

            yield return null;
        }

        RefreshHP(toHP, maxHP);
    }

    public IEnumerator FlashDamage()
    {
        if (characterImage == null)
        {
            yield break;
        }

        characterImage.color = damageFlashColor;
        yield return new WaitForSeconds(flashTime);
        characterImage.color = originalColor;
    }
}