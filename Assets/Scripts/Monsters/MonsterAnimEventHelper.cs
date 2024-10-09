using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimEventHelper : MonoBehaviour
{
    [SerializeField] private bool doesMonsterShoot;
    private Ab_BaseMonsterScript baseMonsterScript;

    private void Start()
    {
        baseMonsterScript = GetComponentInParent<Ab_BaseMonsterScript>();
    }

    /// <summary>
    /// Anim event. Calls monster's Attack Function.
    /// </summary>
    public void OnAnimationShoot()
    {
        baseMonsterScript.Attack(doesMonsterShoot);
    }

    /// <summary>
    /// Anim event. Calls monster's Death Function.
    /// </summary>
    public void OnAnimationDead()
    {
        baseMonsterScript.MonsterKilled();
    }
}