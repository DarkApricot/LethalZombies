using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMonsterScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float frameCounter;

    [SerializeField] private Transform placedMonsters;
    private LayerMask layerMaskWalls;
    private LayerMask layerMaskMonster;

    private Vector3 newSpawnPos;

    private void Start()
    {
        layerMaskWalls = LayerMask.GetMask("Walls");
        layerMaskMonster = LayerMask.GetMask("Monster");
    }

    private void OnEnable()
    {
        SpawnNewMonster();
    }

    /// <summary>
    /// Spawns newly bought monster on center or nearest empty tile
    /// </summary>
    private void SpawnNewMonster()
    {
        // Turn on selection square
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

        while (!CheckIfTileHasMonster())
        {
            // Check if the monster would land on a tile outside of the map
            if (!CheckIfTouchWall())
            {
                // Center on map or nearest free tile
                transform.position = new Vector3(0.5f, -0.5f, 0);
                while (!CheckIfTileHasMonster())
                {
                    CreateNewSpawnPosition();
                }

                return;
            }

            CreateNewSpawnPosition();
        }
    }

    private void CreateNewSpawnPosition()
    {
        newSpawnPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        transform.position += newSpawnPos;
    }

    private void Update()
    {
        MoveMonster();
        PlaceMonster();
    }

    /// <summary>
    /// Moves monster with 0.2s delay on input.
    /// </summary>
    private void MoveMonster()
    {
        // Pauses enemies
        Time.timeScale = 0;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // When key held, move monster every 0.2 sec
        if (Input.anyKey)
        {
            frameCounter += 1;

            // Stops coroutine stacking
            if (frameCounter == 1)
            {
                StartCoroutine(MoveMonsterDelayed());
            }
        }
        else
        {
            frameCounter = 0;
            StopAllCoroutines();
        }
    }

    private IEnumerator MoveMonsterDelayed()
    {
        if (CheckIfTouchWall())
        {
            transform.position = new Vector3(transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);
            yield return new WaitForSecondsRealtime(0.2f);
        }

        frameCounter = 0;
    }

    /// <summary>
    /// Checks if next tile is the border.
    /// </summary>
    /// <returns> If monster is at edge of map. </returns>
    private bool CheckIfTouchWall()
    {
        if (CheckIfTileHasMonster())
        {
            // Check is tile border
            return !Physics2D.Raycast(transform.position, new Vector2(horizontal, vertical), 1.3f, layerMaskWalls);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if next tile is empty.
    /// </summary>
    /// <returns> If tile is empty. </returns>
    private bool CheckIfTileHasMonster()
    {
        return !Physics2D.Raycast(transform.position, new Vector2(horizontal, vertical), 1.3f, layerMaskMonster);
    }

    /// <summary>
    /// Place monster on current tile when "Space" is pressed.
    /// </summary>
    private void PlaceMonster()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.GetChild(1).GetComponent<IUpdateMonsterStartPosition>().PlaceMonsterFinal(placedMonsters, transform.position); //script of placed monster + update start pos
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

            // Turns on enemies movement + disables selection square
            Time.timeScale = 1;
            enabled = false;
        }
    }
}
