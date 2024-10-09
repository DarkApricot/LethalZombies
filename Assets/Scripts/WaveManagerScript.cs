using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerScript : MonoBehaviour
{
    private GameObject playerPrefab;
    [SerializeField] private Transform SF_placedPlayers;

    void Start()
    {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        StartCoroutine(SpawnNewWave());
    }

    /// <summary>
    /// If base survived, spawn a new wave.
    /// </summary>
    public void WaveComplete()
    {
        if (Stats.BaseHP > 0)
        {
            StartCoroutine(SpawnNewWave());
        }
    }

    /// <summary>
    /// Spawns new wave with randomized amount of players * row. (1 wave = 1 row)
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnNewWave()
    {
        // Amount rows + amount players per row
        for (int x = 0; x < Stats.Wave; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                // If there's a player spawned yes or no (chance increases per wave)
                if (Random.Range(0, 1 + Stats.Wave) >= 1) 
                {
                    // Adds +10 hp, +5 damage and speed to the enemies every wave
                    playerPrefab.GetComponent<PlayerScript>().Health = 90 + (Stats.Wave * 10);
                    playerPrefab.GetComponent<PlayerScript>().Damage = 10 + (Stats.Wave * 5);
                    playerPrefab.GetComponent<PlayerScript>().Speed = 0.003f + (Stats.Wave * 0.0005f);
                    Instantiate(playerPrefab, new Vector3(playerPrefab.transform.position.x, playerPrefab.transform.position.y - y), transform.rotation, SF_placedPlayers);
                }
            }

            yield return new WaitForSeconds(1.5f);
        }


        PlayerScript[] _playerCount = FindObjectsOfType<PlayerScript>();

        if (_playerCount.Length == 0)
        {
            // No players spawned, retry
            StartCoroutine(SpawnNewWave());
        }
        else if (Stats.Wave == 1)
        {
            // 1 enemy = +200 starting coins
            UIStatsScript.UpdateStat(ref Stats.Loot, _playerCount.Length * 200);
        }
    }
}
