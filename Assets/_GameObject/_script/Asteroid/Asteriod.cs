using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
    [SerializeField] private bool isActive;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Vector3 rotationDir;

    [Header("Bounds")]
    [SerializeField] private Vector3 minRange;
    [SerializeField] private Vector3 maxRange;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        Move();
        Rotate();
    }

    public void SetUp()
    {
        Vector3 currentPos = new Vector3(Random.Range(minRange.x, maxRange.x),
                                        Random.Range(minRange.y, maxRange.y),
                                        Random.Range(minRange.z, maxRange.z));

        transform.position = currentPos;

        moveSpeed = Random.Range(5, 15);
        moveDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

        rotationSpeed = Random.Range(-200, 200);
        rotationDir = Vector3.one * Random.Range(-1f, 1f);

        SetActiveStatus(true);
    }

    public void SetActiveStatus(bool isActive)
    {
        this.isActive = isActive;
    }

    private void Move()
    {
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

        if(Vector3.Distance(transform.position, Vector3.zero) > 50f)
        {
            AsteroisSpawner.DestroyAsteroids?.Invoke(gameObject);
        }
    }

    private void Rotate()
    {
        transform.Rotate(rotationDir * rotationSpeed * Time.deltaTime, Space.World);
    }
}
