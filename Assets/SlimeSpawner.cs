using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    float spawnInterval = 5f;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating(nameof(CreateSlime), 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateSlime()
    {
        animator.SetTrigger("open_chest");
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Debug.Log("Trigger");
    }
}
