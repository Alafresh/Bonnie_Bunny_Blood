using Unity.Mathematics;
using UnityEngine;

public class Levitacion : MonoBehaviour
{
    public float amplitud=0.5f;
    public float frecuencia = 0.5f;
    private Rigidbody rb;

   
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float y = math.sin(Time.time * frecuencia) * amplitud;
        Vector3 newPosition = rb.position;
        newPosition.y = y+1.5f;

        rb.MovePosition(newPosition);
    }
    
}
