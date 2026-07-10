using UnityEngine;

public class RotateBlckHole : MonoBehaviour
{
    public float rotationSpeed = 20f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
