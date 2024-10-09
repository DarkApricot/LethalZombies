using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour
{
    public MonsterInfoScript SelectedMonsterScript;

    private TextMeshProUGUI[] monsterInfoTexts;
    private Image[] monsterImage;

    private string[] takenMonsterInfo;
    private Sprite takenMonsterImage;

    [Header("Fill In GameObjects")]
    [SerializeField] private GameObject SF_monsterInfoPanel;
    [SerializeField] private Transform SF_placeMonster;

    void Start()
    {
        monsterImage = SF_monsterInfoPanel.GetComponentsInChildren<Image>();
        monsterInfoTexts = SF_monsterInfoPanel.GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void GetMonsterInfoButtonClicked()
    {
        // Check if takenMonsterInfo has been filled in before, if so check if it's the same info as the new info, then if the panel is already on
        if (takenMonsterInfo != null && takenMonsterInfo[0] == SelectedMonsterScript.MonsterInfo[0] && SF_monsterInfoPanel.activeInHierarchy)
        {
            SF_monsterInfoPanel.SetActive(false);
        }
        else
        {
            UpdateInfoPanel();
        }

        FillMonsterInfo();
    }

    private void UpdateInfoPanel()
    {
        takenMonsterInfo = SelectedMonsterScript.MonsterInfo;
        takenMonsterImage = SelectedMonsterScript.MonsterImage;
        SF_monsterInfoPanel.SetActive(true);
    }

    private void FillMonsterInfo()
    {
        for (int i = 0; i < takenMonsterInfo.Length; i++)
        {
            monsterInfoTexts[i].text = takenMonsterInfo[i];
        }

        monsterImage[1].sprite = takenMonsterImage;
    }

    public void BuyMonster()
    {
        float _Price = float.Parse(takenMonsterInfo[5]);

        if (Stats.Loot >= _Price)
        {
            FindAnyObjectByType<UIManagerScript>().ChangeUIScreen("PlayUI");
            UIStatsScript.UpdateStat(ref Stats.Loot, -_Price);

            GameObject _enemyPrefab = Resources.Load<GameObject>("Prefabs/" + takenMonsterInfo[0]);
            FindObjectOfType<PlaceMonsterScript>().enabled = true;

            Instantiate(_enemyPrefab, SF_placeMonster);
        }
    }
}