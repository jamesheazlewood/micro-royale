using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D phys;
    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Capture player input
        movement.x = Input.GetAxisRaw("Horizontal");  // A and D for left and right
        movement.y = Input.GetAxisRaw("Vertical");    // W and S for up and down
    }

    void FixedUpdate()
    {
        // Move the player based on input and speed
        phys.MovePosition(phys.position + movement * 5.0f * Time.fixedDeltaTime);
    }
}
