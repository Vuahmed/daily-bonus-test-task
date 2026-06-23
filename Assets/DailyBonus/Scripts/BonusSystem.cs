using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BonusSystem : MonoBehaviour
{
    private float dailyTimer;
    private string typeMode;
    private string bonusId;
    private BonusDefinition bonus;
    private DateTime lastBonusTime;

    [SerializeField] private BonusDatabase bonusDatabase;

    public event Action<TimeSpan> OnTimerChanged;
    public event Action<BonusDefinition> OnBonusChanged;
    public event Action<TimeSpan> OnBonusTimerChanged;

    public BonusDatabase GetDatabase() { return bonusDatabase; }
    void Start()
    {
        typeMode = ConfigLoader.Load().DailyBonusType;
        
        SetDailyTimer();
        
        lastBonusTime = GetTime();

        bonusId = GetBonusId();

        LoadBonus();

        DateTime currentTime = DateTime.Now;
        TimeSpan passedTime = currentTime - lastBonusTime;

        if(passedTime.TotalSeconds >= dailyTimer) GetBonus();

        StartCoroutine(TimeRoutine());
    }

    private IEnumerator TimeRoutine()
    {
        while (true)
        {
            UpdateTimer();

            yield return new WaitForSeconds(1f);
        }
    }

    private void LoadBonus()
    {
        if (!string.IsNullOrEmpty(bonusId))
        {
            bonus = bonusDatabase.GetBonusById(GetBonusId());
            OnBonusChanged?.Invoke(bonus);
        }
    }

    private void GetBonus()
    {
        bonus = bonusDatabase.GetRandomBonus();

        OnBonusChanged?.Invoke(bonus);
        
        Debug.Log("ID: " + bonus.id);

        SetTime();
        SetBonus();

        lastBonusTime = GetTime();
    }

    private void SetDailyTimer()
    {
        if(typeMode == "Test") dailyTimer = 1440f; // 24 minute
        else if (typeMode == "Release") dailyTimer = 86400f; // 24 hour
    }

    public void SetTime()
    {
        PlayerPrefs.SetString("LastBonusTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    public DateTime GetTime()
    {
        if (PlayerPrefs.HasKey("LastBonusTime"))
            return DateTime.Parse(PlayerPrefs.GetString("LastBonusTime")); 
        return DateTime.MinValue;
    }

    public void SetBonus()
    {
        PlayerPrefs.SetString("BonusID", bonus.id);
        PlayerPrefs.Save();
    }

    public string GetBonusId()
    {
        if (PlayerPrefs.HasKey("BonusID"))
            return PlayerPrefs.GetString("BonusID");
        return null;
    }

    private void UpdateTimer()
    {
        TimeSpan passedTime = DateTime.Now - lastBonusTime;

        TimeSpan cooldowm = TimeSpan.FromSeconds(dailyTimer);
        TimeSpan remainTime = cooldowm - passedTime;

        if (passedTime >= cooldowm)
            remainTime = new TimeSpan();    
        
        OnTimerChanged?.Invoke(remainTime);
        
        TimeSpan bonusCooldown = TimeSpan.FromMinutes(bonus.timeBonus);
        TimeSpan remainBonusTime = bonusCooldown - passedTime;
        
        if (passedTime >= bonusCooldown)
            remainTime = new TimeSpan();
        
        OnBonusTimerChanged?.Invoke(remainBonusTime);
    }
}
