using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float minSize = 0.05f;
    public float maxSize = 0.2f;

    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    public float maxSpinSpeed = 10f;

    public GameObject bounceEffect;

    Rigidbody2D rb;
    public Rigidbody2D RB => rb;

    
    void Start()
    {

        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        
        Vector2 randomDirection = Random.insideUnitCircle;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);
        rb.AddTorque(randomTorque);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 viewPos = Camera.main.WorldToViewportPoint(pos);

        if (viewPos.x < 0)
        {
            viewPos.x = 1;
        }
        else if (viewPos.x > 1)
        {
            viewPos.x = 0;
        }

        if (viewPos.y < 0)
        {
            viewPos.y = 1;
        }
        else if (viewPos.y > 1)
        {
            viewPos.y = 0;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Obstacle otherObstacle = collision.gameObject.GetComponent<Obstacle>();

        if (otherObstacle != null)
        {
            if (GetEntityId() > otherObstacle.GetEntityId())
            {
                return;
            }
        }

        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject effect = Instantiate(bounceEffect, contactPoint, Quaternion.identity);

        Destroy(effect, 1f);
    }
}
