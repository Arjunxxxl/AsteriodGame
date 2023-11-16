using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroisSpawner : MonoBehaviour
{
    [Header("Active Asteroid Data")]
    [SerializeField] private int maxAsteroidCanActiveAtSameTime;
    [SerializeField] private List<Asteriod> asteriods;

    public static Action<GameObject> DestroyAsteroids;

    private int index;

    private void OnEnable()
    {
        DestroyAsteroids += OnDestroyAsteroids;
    }

    private void OnDisable()
    {
        DestroyAsteroids -= OnDestroyAsteroids;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp()
    {
        if (asteriods == null)
        {
            asteriods = new List<Asteriod>();
        }
        else
        {
            foreach (var item in asteriods)
            {
                item.gameObject.SetActive(false);
            }

            asteriods.Clear();
        }

        index = 0;

        SpawnAsteroids();
    }

    private void SpawnAsteroids()
    {
        int asteroidsToSpwn = maxAsteroidCanActiveAtSameTime - asteriods.Count;

        if(asteroidsToSpwn <= 0)
        {
            return;
        }

        for (int i = 0; i < asteroidsToSpwn; i++)
        {
            string tag = "Asteroid" + (int)Random.Range(1, 4);

            GameObject obj = ObjectPooler.Instance.SpawnFormPool(tag, transform.position, Quaternion.identity);
            obj.SetActive(true);

            obj.name += index + "";

            obj.GetComponent<Asteriod>().SetUp();

            asteriods.Add(obj.GetComponent<Asteriod>());

            index++;
        }
    }

    public void OnDestroyAsteroids(GameObject asteriodObj)
    {
        if(asteriodObj.activeSelf && asteriods.Contains(asteriodObj.GetComponent<Asteriod>()))
        {
            asteriodObj.GetComponent<Asteriod>().SetActiveStatus(false);
                        
            string tag = "Explosion" + (int)Random.Range(1, 3);
            ObjectPooler.Instance.SpawnFormPool(tag, asteriodObj.transform.position, Quaternion.identity).GetComponent<ParticleSystem>().Play();

            asteriods.Remove(asteriodObj.GetComponent<Asteriod>());

            asteriodObj.gameObject.SetActive(false);

            SpawnAsteroids();
        }
    }
}
