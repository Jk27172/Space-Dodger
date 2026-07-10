using UnityEngine;

public class BlackHoleCenter : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TriggerGameOver();
            return;
        }

        Obstacle obstacle = other.GetComponent<Obstacle>();

        if (obstacle != null)
        {
            other.transform.position = Random.insideUnitCircle * 10f;
        }
    }
}
