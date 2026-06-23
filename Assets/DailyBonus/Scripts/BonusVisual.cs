using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusVisual : MonoBehaviour
{
    [SerializeField] private BonusSystem bonusSystem;
    
    [Header("Daily Timmer")]
    [SerializeField] private TMP_Text timerText;
    
    [Header("Active Bonus")]
    [SerializeField] private TMP_Text bonusText;
    [SerializeField] private TMP_Text bonusTimer;
    [SerializeField] private Image bonusImage;

    [Header("Bonus List")]
    [SerializeField] private TMP_Text bonusList;

    private void Start()
    {
        ShowBonusList();
    }

    private void OnEnable()
    {
        bonusSystem.OnTimerChanged += UpdateTimerText;
        bonusSystem.OnBonusChanged += UpdateBonusText;
        bonusSystem.OnBonusTimerChanged += UpdateBonusTimerText;
    }

    private void OnDisable()
    {
        bonusSystem.OnTimerChanged -= UpdateTimerText;
        bonusSystem.OnBonusChanged -= UpdateBonusText;
        bonusSystem.OnBonusTimerChanged -= UpdateBonusTimerText;
    }

    private void UpdateTimerText(TimeSpan timeSpan)
    {
        timerText.text = timeSpan.ToString(@"hh\:mm\:ss");
    }

    private void UpdateBonusText(BonusDefinition bonus)
    {
        bonusText.text = bonus.id;
        bonusImage.sprite = bonus.avatarBonus;
    }

    private void UpdateBonusTimerText(TimeSpan timeSpan)
    {
        bonusTimer.text = timeSpan.ToString(@"hh\:mm\:ss");
    }

    private void ShowBonusList()
    {
        bonusList.text = "";

        foreach (var bonus in bonusSystem.GetDatabase().bonuses)
        {
            bonusList.text += $"{bonus.id} ({bonus.timeBonus} сек)\n";
        }
    }
}
