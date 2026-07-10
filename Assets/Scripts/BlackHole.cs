using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float pullRadius = 3f;
    public float pullForce = 3f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, pullRadius);

        foreach (Collider2D obj in objects)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

            if (rb == null)
                continue;

            Vector2 direction = (Vector2)transform.position - rb.position;

            float distance = direction.magnitude;

            if (distance < 0.1f)
            {
                distance = 0.1f;
            }

            float force = pullForce / distance;

            rb.AddForce(direction.normalized * force);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
