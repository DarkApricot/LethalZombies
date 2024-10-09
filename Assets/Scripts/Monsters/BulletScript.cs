using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int Damage;

    private float speed = 0.02f;
    private SpriteRenderer playerSpriteRenderer;

    void Update()
    {
        MoveBullet();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        BulletHit(_collision);
    }

    /// <summary>
    /// Bullet moves forward when game isn't paused.
    /// </summary>
    private void MoveBullet()
    {
        if (Time.timeScale == 1)
        {
            transform.position = new Vector2(transform.position.x + speed, transform.position.y);
        }
    }

    /// <summary>
    /// Update stats HitPlayer and calls Bullet destroy anim.
    /// </summary>
    /// <param name="_collision"></param>
    private void BulletHit(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
        {
            _collision.gameObject.GetComponent<PlayerScript>().Health -= Damage;

            // Makes player sprite red.
            playerSpriteRenderer = _collision.gameObject.GetComponent<SpriteRenderer>();
            playerSpriteRenderer.color = Color.red;

            // Updates player stats / Call Bullet destroy anim.
            speed = 0f;
            UIStatsScript.UpdateStat(ref Stats.DamageGiven, Damage);
            GetComponent<Animator>().enabled = true;
        }
    }

    /// <summary>
    /// Animation event.
    /// Resets sprite color and deletes bullet.
    /// </summary>
    public void DestroyBullet()
    {
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.color = Color.white;
        }

        Destroy(gameObject);
    }
}
