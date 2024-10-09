using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUpdateMonsterStartPosition
{
    void PlaceMonsterFinal(Transform _newParent, Vector3 _newPosition);
}

public abstract class Ab_BaseMonsterScript : MonoBehaviour
{
    [Header("Stats")]
    public int Health;
    public int Damage;

    private bool isPlayerInView;

    private LayerMask playerLayerMask;
    private LayerMask monsterLayerMask;
    private GameObject bulletPrefab;
    private GameObject hpBarPrefab;
    private Slider associatedHPBar;

    public void Start()
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Mon_Bullet");
        hpBarPrefab = Resources.Load<GameObject>("Prefabs/HPBarPrefab");

        monsterLayerMask = gameObject.layer;
        gameObject.layer = 0;
    }

    public void PlaceMonsterFinal(Transform _newParent, Vector3 _newPosition)
    {
        transform.position = _newPosition;
        transform.parent = _newParent;

        playerLayerMask = LayerMask.GetMask("Player");
        gameObject.layer = monsterLayerMask;

        // Create new HP bar for monster
        GameObject _hpBar = Instantiate(hpBarPrefab, GameObject.Find("HPCanvas/Mon_HealthBars").transform);
        _hpBar.transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y + 0.5f); //Update HP bar pos
        associatedHPBar = _hpBar.GetComponent<Slider>();
        UpdateHPBar(Health);
    }

    /// <summary>
    /// Checks for player within given vision
    /// </summary>
    /// <param name="_viewRange"></param>
    public void CheckForPlayer(float _viewRange)
    {
        if (Health > 0)
        {
            isPlayerInView = Physics2D.Raycast(transform.position, Vector2.right, _viewRange, playerLayerMask);
            ChangeAnimationState("ANIM_IsMonsterAttacking", isPlayerInView);
        }
    }

    private void ChangeAnimationState(string _animBool, bool _boolState)
    {
        // Main animator
        if (TryGetComponent(out Animator _animator))
        {
            _animator.SetBool(_animBool, _boolState);
        }

        // Secondary animator
        if (transform.GetChild(0).gameObject.TryGetComponent(out Animator _cAnimator))
        {
            _cAnimator.SetBool(_animBool, _boolState);
        }
    }

    /// <summary>
    /// Makes monsters attack when player in vision + range
    /// </summary>
    /// <param name="_doesMonsterShoot"></param>
    public void Attack(bool _doesMonsterShoot)
    {
        if (_doesMonsterShoot)
        {
            GameObject _instantiatedBullet = Instantiate(bulletPrefab, transform);
            _instantiatedBullet.GetComponent<BulletScript>().Damage = Damage;
        }
    }

#region Monster Health Related

    private void UpdateHPBar(float _hp)
    {
        associatedHPBar.value = _hp;

        if (_hp <= 0)
        {
            Destroy(associatedHPBar.gameObject);
        }
    }

    /// <summary>
    /// Updates HP bar + Check if monster alive
    /// </summary>
    public void CheckMonsterHP()
    {
        UpdateHPBar(Health);

        if (Health <= 0)
        {
            // Clears sprite / layer | Updates stats | Starts death animaation
            gameObject.layer = 0;
            UIStatsScript.UpdateStat(ref Stats.MonstersLost, +1);
            GetComponent<SpriteRenderer>().sprite = null;
            ChangeAnimationState("ANIM_IsMonsterDead", true);
        }
    }

    /// <summary>
    /// Deletes monster.
    /// </summary>
    public void MonsterKilled()
    {
        Destroy(gameObject);
    }

#endregion
}
