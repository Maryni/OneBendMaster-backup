using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IdleGame : MonoBehaviour
{
    [SerializeField] private Image mainImage;
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private TMP_Text textMoney;
    [SerializeField] private TMP_Text textTime;
    [SerializeField] private List<Sprite> sprites;
    [Space, SerializeField] private float minRandom;
    [SerializeField] private float maxRandom;
    [SerializeField] private float damage;

    [SerializeField] private Stats currentEnemy;
 
    private float score;
    private float money;
    private float timer;
    private int index;

    public void StartIdle()
    {
        currentEnemy = GetStats();
        StartTimer(currentEnemy.time);
    }

    private void SetValueToUI()
    {
        textScore.text = score.ToString();
        textMoney.text = money.ToString();
    }

    private IEnumerator Timer(float value)
    {
        value -= Time.deltaTime;
        timer = value;
        UpdateTime();
        yield return Timer(value);
    }

    private Stats GetStats() => new Stats() { hp = GetRandom(), time = GetRandom(), money = GetRandom() };

    private float GetRandom() => Random.Range(minRandom + (minRandom / index), maxRandom * index);

    private void UpdateTime() => textTime.text = timer.ToString();

    private void StartTimer(float value) => StartCoroutine(Timer(value));

    public void DealDamage()
    {
        currentEnemy.hp -= damage;
        if(currentEnemy.hp <= 0)
        {
            money += currentEnemy.money;
            score += index;
            index++;
            SetValueToUI();
        }
    }
}

[System.Serializable]
public struct Stats
{
    public float hp;
    public float time;
    public float money;
}
