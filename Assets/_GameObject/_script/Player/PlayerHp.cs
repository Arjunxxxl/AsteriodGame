using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour, IDamagable
{
    [SerializeField] private bool canTakeDamage;
    [SerializeField] private int maxHp;
    [SerializeField] private int hpLeft;

    [Header("Collision Data")]
    [SerializeField] private float collisionRange;
    [SerializeField] private LayerMask collisionLayer;

    private Player player;

    private void Update()
    {
        CheckForCollision();
    }

    public void SetUp(Player player)
    {
        this.player = player;

        hpLeft = maxHp;
        canTakeDamage = false;

        StartCoroutine(EnableDamageTaking());
    }

    IEnumerator EnableDamageTaking()
    {
        yield return new WaitForSeconds(1f);
        canTakeDamage = true;
    }

    public void Damage(int damageAmt)
    {
        if(!canTakeDamage)
        {
            return;
        }

        hpLeft -= damageAmt;

        if(hpLeft <= 0)
        {
            hpLeft = 0;
            player.Died();
        }
    }

    private void CheckForCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRange, collisionLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != null)
            {
                AsteroisSpawner.DestroyAsteroids?.Invoke(hitCollider.gameObject);
            }
        }

        Damage(1 * hitColliders.Length);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);

        Gizmos.DrawSphere(transform.position, collisionRange);
    }
}
