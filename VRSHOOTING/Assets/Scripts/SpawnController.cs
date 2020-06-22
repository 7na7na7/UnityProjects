using UnityEngine;


public class SpawnController : MonoBehaviour
{

    [SerializeField] float spwanlnterval = 3f;

    EnemySpawner[] spawners;

    float timer = 0f;


    void Start()
    {
        spawners = GetComponentsInChildren<EnemySpawner>();
    }
    void Update()
    {

        timer += Time.deltaTime;

        if (spwanlnterval < timer)
        {
            var index = Random.Range(0, spawners.Length); spawners[index].Spawn(); timer = 0f;
        }
    }
}