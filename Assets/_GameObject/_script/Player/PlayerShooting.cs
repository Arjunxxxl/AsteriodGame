using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Data")]
    [SerializeField] private bool isShootingActive;
    [SerializeField] private float shootingDelay;
    [SerializeField] private float shootingTimeElapsed;
    [SerializeField] private Transform buttleSpawnT;

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    public void ActivateShooting(bool isShootingActive)
    {
        this.isShootingActive = isShootingActive;
    }

    private void Shoot()
    {
        if(!isShootingActive)
        {
            return;
        }

        shootingTimeElapsed += Time.deltaTime;

        if(shootingTimeElapsed >= shootingDelay) 
        {
            shootingTimeElapsed = 0;

            GameObject bullet = ObjectPooler.Instance.SpawnFormPool("Bullet", buttleSpawnT.position, Quaternion.identity);

            Vector2 rotateDir = new Vector2(transform.up.x, transform.up.y);

            Quaternion targetRot = Quaternion.LookRotation(bullet.transform.forward, transform.up);
            bullet.transform.rotation = targetRot;

            bullet.GetComponent<Bullet>().ActivateBullet(transform.up);
        }
    }
}
