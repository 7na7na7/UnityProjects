using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{ 

[SerializeField] Enemy enemyPrefab;

Enemy enemy;

public void Spawn()
    {
    if (enemy == null) {
        enemy =Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}
}