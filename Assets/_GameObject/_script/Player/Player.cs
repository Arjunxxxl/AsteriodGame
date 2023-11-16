using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isDead;

    [Header("Mouse Input")]
    [SerializeField] private Vector3 mouseStartPos;
    [SerializeField] private Vector3 mouseEndPos;
    [SerializeField] private Vector3 mouseDir;

    [Header("Move Data")]
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float moveSpeed;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;

    [Header("Bounds")]
    [SerializeField] private Vector3 minRange;
    [SerializeField] private Vector3 maxRange;

    PlayerHp playerHp;
    PlayerShooting playerShooting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            return;
        }

        GetMouseInput();
        Move();
        Rotate();
    }

    public void SetUp()
    {
        if(playerHp == null)
        {
            playerHp = GetComponent<PlayerHp>();
        }

        if(playerShooting == null)
        {
            playerShooting = GetComponent<PlayerShooting>();
        }

        transform.position = Vector3.up * -10;
        transform.rotation = Quaternion.identity;

        playerHp.SetUp(this);
        playerShooting.ActivateShooting(true);

        isDead = false;
    }

    #region Input
    private void GetMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            mouseEndPos = Input.mousePosition;
            mouseDir = mouseEndPos - mouseStartPos;

            mouseDir.Normalize();
        }

        if(Input.GetMouseButtonUp(0))
        {
            mouseStartPos = Input.mousePosition;
            mouseEndPos = Input.mousePosition;
            mouseDir = mouseEndPos - mouseStartPos;

            mouseDir.Normalize();
        }
    }
    #endregion

    #region Move / Rotate
    private void Move()
    {
        moveDir = mouseDir;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        Vector3 currentPos = transform.position;
        currentPos.z = maxRange.z;

        if (currentPos.x < minRange.x || currentPos.x > maxRange.x)
        {
            currentPos.x *= -1;

            if (Mathf.Abs(moveDir.y) > 0.5f)
            {
                currentPos.y *= -1;
            }

            transform.position = currentPos;
        }

        if (currentPos.y < minRange.y || currentPos.y > maxRange.y)
        {
            currentPos.y *= -1;

            if (Mathf.Abs(moveDir.x) > 0.5f)
            {
                currentPos.x *= -1;
            }

            transform.position = currentPos;
        }
    }

    private void Rotate()
    {
        if(moveDir != Vector3.zero)
        {
            Vector2 rotateDir = new Vector2(moveDir.x, moveDir.y);

            Quaternion targetRot = Quaternion.LookRotation(transform.forward, rotateDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }
    #endregion

    public void Died()
    {
        isDead = true;

        ObjectPooler.Instance.SpawnFormPool("PlayerDeathExplosion", transform.position, Quaternion.Euler(-90, 0, 0)).GetComponent<ParticleSystem>().Play();

        playerShooting.ActivateShooting(false);

        gameObject.SetActive(false);
    }
}
