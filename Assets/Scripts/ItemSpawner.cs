using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private bool respawnOverTime = false;
    [SerializeField] private float respawnInterval = 10f;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gizmoColor = Color.cyan;

    private float timer;

    private void Start()
    {
        if (spawnOnStart)
            SpawnItems();
    }

    private void Update()
    {
        if (!respawnOverTime) return;

        timer += Time.deltaTime;
        if (timer >= respawnInterval)
        {
            timer = 0f;
            SpawnItems();
        }
    }

    private void SpawnItems()
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("ItemSpawner: No prefab assigned!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector2 spawnPos = (Vector2)transform.position + offset;
            Instantiate(itemPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
