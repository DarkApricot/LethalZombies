using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventHelper : MonoBehaviour
{
    private PlayerScript playerScript;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
    }

    /// <summary>
    /// Anim event, calls PlayerKilled.
    /// </summary>
    public void OnAnimationDeath()
    {
        playerScript.PlayerKilled();
    }

    /// <summary>
    /// Anim event, add winnings from DeadPlayer to Main Stats.
    /// </summary>
    public void UpdateStats()
    {
        UIStatsScript.UpdateStat(ref Stats.Loot, +25);
        UIStatsScript.UpdateStat(ref Stats.LootWon, +25);
        UIStatsScript.UpdateStat(ref Stats.PlayersKilled, +1);
        UIStatsScript.UpdateStat(ref Stats.PlayersLeft, -1);
    }
}
