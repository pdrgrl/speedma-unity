using UnityEngine;

public class ConveyorBeltPhysical : MonoBehaviour
{
    public float speed = 2.0f;
    public Vector3 direction = Vector3.forward; // Direction toward the second machine

    void OnCollisionStay(Collision collision)
    {
        // Check if the object on the belt has a Rigidbody
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            // Move the object along the belt surface
            Vector3 movement = direction.normalized * speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}
