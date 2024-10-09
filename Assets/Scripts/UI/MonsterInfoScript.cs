using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfoScript : MonoBehaviour
{
    [Header("Monster Info / Stats")]
    public string[] MonsterInfo = { "Name", "Description", "Speed", "Damage", "Range", "Price" };
    public Sprite MonsterImage;

    private StoreScript updaterScript;

    private void Start()
    {
        updaterScript = FindObjectOfType<StoreScript>();
    }

    /// <summary>
    /// Used in UI Store buttons. Updates store's Monster Info panel.
    /// </summary>
    public void GetScript()
    {
        updaterScript.SelectedMonsterScript = this;
    }
}
