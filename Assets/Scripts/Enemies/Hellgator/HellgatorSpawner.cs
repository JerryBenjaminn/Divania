using UnityEngine;

public class HellgatorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hellgatorPrefab;
    [SerializeField] private float spawnDelay = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("SpawnHellgator", spawnDelay);
        }
    }

    private void SpawnHellgator()
    {
        Instantiate(hellgatorPrefab, transform.position, Quaternion.identity);
    }
}

