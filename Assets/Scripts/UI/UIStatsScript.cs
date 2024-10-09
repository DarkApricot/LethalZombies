using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats
{
    public static float Loot, BaseHP = 100, PlayersLeft, Wave = 1, PlayersKilled, MonstersLost, DamageGiven, DamageReceived, LootWon = Loot;
}

public class UIStatsScript : MonoBehaviour
{
    private static TextMeshProUGUI[] texts;

    private void Awake()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();

        ResetStats();
        UpdateUI();
    }

    /// <summary>
    /// Updates a stat with given input.
    /// </summary>
    /// <param name="_whichStat"></param>
    /// <param name="_changeWith"></param>
    public static void UpdateStat(ref float _whichStat, float _changeWith)
    {
        _whichStat += _changeWith;
        UpdateUI();

        if (_whichStat == Stats.BaseHP && Stats.BaseHP <= 0)
        {
            FindObjectOfType<UIManagerScript>().YouDied();
        }

        if (_whichStat == Stats.PlayersLeft && Stats.PlayersLeft == 0)
        {
            FindObjectOfType<WaveManagerScript>().WaveComplete();
            UpdateStat(ref Stats.Wave, +1);
        }
    }

    /// <summary>
    /// Corrects UI visuals with current stats.
    /// </summary>
    public static void UpdateUI()
    {
        float[] _savedStats = { Stats.Loot, Stats.BaseHP, Stats.PlayersLeft, Stats.Wave };

        for (int i = 0; i < _savedStats.Length; i++)
        {
            texts[i + 1].text = _savedStats[i].ToString();
        }
    }

    private void ResetStats()
    {
        Stats.Loot = 0;
        Stats.BaseHP = 100;
        Stats.PlayersLeft = 0;
        Stats.Wave = 1;
        Stats.PlayersKilled = 0;
        Stats.MonstersLost = 0;
        Stats.DamageGiven = 0;
        Stats.DamageReceived = 0;
        Stats.LootWon = 0;

        Time.timeScale = 1;
    }
}
