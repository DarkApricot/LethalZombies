using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDeathScript : MonoBehaviour
{
    private TextMeshProUGUI[] textComponents;
    private Button[] buttonComponents;

    void Start()
    {
        textComponents = GetComponentsInChildren<TextMeshProUGUI>();
        buttonComponents = GetComponentsInChildren<Button>();

        foreach (TextMeshProUGUI _text in textComponents)
        {
            _text.gameObject.SetActive(false);
        }

        foreach (Button _button in buttonComponents)
        {
            _button.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(StartEndSequence());
    }

    private void KillAllEntities()
    {
        Ab_BaseMonsterScript[] _monsterCount = FindObjectsOfType<Ab_BaseMonsterScript>();
        foreach (Ab_BaseMonsterScript _monster in _monsterCount)
        {
            Destroy(_monster.gameObject);
        }

        PlayerScript[] _playerCount = FindObjectsOfType<PlayerScript>();
        foreach (PlayerScript _player in _playerCount)
        {
            Destroy(_player.gameObject);
        }
    }
    private IEnumerator StartEndSequence()
    {
        float[] _savedStats = { Stats.PlayersKilled, Stats.MonstersLost, Stats.DamageGiven, Stats.DamageReceived, Stats.LootWon, Stats.Wave - 1};

        KillAllEntities();

        yield return new WaitForSecondsRealtime(1.5f);
        textComponents[0].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);

        for (int i = 1; i < _savedStats.Length; i++)
        {
            textComponents[i].gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.75f);
            textComponents[i].text += " " +_savedStats[i - 1].ToString();
            yield return new WaitForSecondsRealtime(0.75f);
        }

        foreach (Button button in buttonComponents)
        {
            button.gameObject.SetActive(true);
            button.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

}
