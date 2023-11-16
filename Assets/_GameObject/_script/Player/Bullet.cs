using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool isBulletActive;

    [Header("Bullet Movement Data")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDir;

    [Header("Bullet Active Duration")]
    [SerializeField] private float bulletMaxActiveDuration;
    [SerializeField] private float bulletActiveTimeElapsed;

    [Header("Collision Data")]
    [SerializeField] private float collisionRange;
    [SerializeField] private LayerMask collisionLayer;

    // Update is called once per frame
    void Update()
    {
        if(!isBulletActive)
        {
            return;
        }

        MoveBullet();
        CheckForCollision();

        UpdateBulletActiveDuration();
    }

    public void ActivateBullet(Vector3 moveDir)
    {
        this.moveDir = moveDir;

        isBulletActive = true;
        bulletActiveTimeElapsed = 0;
    }

    private void DeactivateBullet()
    {
        bulletActiveTimeElapsed = 0;
        isBulletActive = false;

        gameObject.SetActive(false);
    }

    private void MoveBullet()
    {
        if (!isBulletActive)
        {
            return;
        }

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }

    private void UpdateBulletActiveDuration()
    {
        if (!isBulletActive)
        {
            return;
        }

        bulletActiveTimeElapsed += Time.unscaledDeltaTime;

        if (bulletActiveTimeElapsed >= bulletMaxActiveDuration)
        {
            DeactivateBullet();
        }
    }

    private void CheckForCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRange, collisionLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != null)
            {
                Debug.Log(hitCollider.gameObject.name);
                AsteroisSpawner.DestroyAsteroids?.Invoke(hitCollider.gameObject);

                DeactivateBullet();
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);

        Gizmos.DrawSphere(transform.position, collisionRange);
    }
}
