using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    [Header("Stats")]
    public float Speed = 0.003f;
    public float Damage;
    public float Health;

    [Space] 
    [Header("Layer")]
    [SerializeField] private LayerMask monsterLayerMask;

    private RaycastHit2D isPlayerAtMonster;
    private Animator[] playerAnimControllers;
    private GameObject attackedMonster;

    void Start()
    {
        playerAnimControllers = GetComponentsInChildren<Animator>();
        UIStatsScript.UpdateStat(ref Stats.PlayersLeft, +1);
    }

    private void FixedUpdate()
    {
        isPlayerAtMonster = Physics2D.Raycast(transform.position, Vector2.left, 0.3f, monsterLayerMask);
    }

    void Update()
    {
        ManagePlayerStates();  
    }

    /// <summary>
    /// Manages if player is walking, dying or paused.
    /// </summary>
    private void ManagePlayerStates()
    {
        if (Time.timeScale == 1)
        {
            if (Health > 0)
            {
                PlayerWalks();
            }
            else
            {
                PlayerDies();
            }
        }
    }

    private void PlayerDies()
    {
        if (attackedMonster != null)
        {
            // Resets sprite color
            attackedMonster.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Resets layer + Calls death anim
        gameObject.layer = 0;
        UpdateAnimState("ANIM_IsPlayerDead", true);
    }

    private void PlayerWalks()
    {
        UpdateAnimState("ANIM_IsPlayerHitting", isPlayerAtMonster);

        // Checks if Player at monster base
        if (transform.position.x > -7.5f)
        {
            // Player walks ahead when no monster
            if (!isPlayerAtMonster)
            {
                transform.position = new Vector3(transform.position.x - Speed, transform.position.y, transform.position.z);
            }
        }
        else
        {
            // Player reached monster base
            UIStatsScript.UpdateStat(ref Stats.PlayersLeft, -1);
            UIStatsScript.UpdateStat(ref Stats.BaseHP, -20);
            Destroy(gameObject);
        }
    }

    private void UpdateAnimState(string _nameAnimBool, bool _boolState)
    {
        foreach (Animator _animController in playerAnimControllers)
        {
            _animController.SetBool(_nameAnimBool, _boolState);
        }
    }

    /// <summary>
    /// Animation event when player is attacking.
    /// Visually attacks monster + updates monster stats.
    /// </summary>
    public void Attack()
    {
        if (isPlayerAtMonster)
        {
            attackedMonster = isPlayerAtMonster.transform.gameObject;

            // Updates monster stats from attack.
            attackedMonster.GetComponent<SpriteRenderer>().color = Color.red;
            attackedMonster.GetComponent<Ab_BaseMonsterScript>().Health -= ((int)Damage);
            attackedMonster.GetComponent<Ab_BaseMonsterScript>().CheckMonsterHP();
            UIStatsScript.UpdateStat(ref Stats.DamageReceived, Damage);
        }
    }

    /// <summary>
    /// Animation event when player attack over.
    /// Checks for monster and calls attack animation.
    /// </summary>
    public void AttackOver()
    {
        if (isPlayerAtMonster)
        {
            attackedMonster.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            UpdateAnimState("ANIM_IsPlayerHitting", isPlayerAtMonster);
        }
    }

    /// <summary>
    /// Deletes player object.
    /// </summary>
    public void PlayerKilled()
    {
        Destroy(gameObject);
    }
}
