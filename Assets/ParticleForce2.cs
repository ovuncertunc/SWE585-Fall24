using UnityEngine;

public class ParticleForce2 : MonoBehaviour
{
    public ParticleSystem water; // Reference to water particle system for gun1
    public ParticleSystem water2; // Reference to water particle system for gun2
    public Transform gun1; // Reference to gun1
    public Transform gun2; // Reference to gun2 (this will stay in front of gun1)
    public float forceMultiplier = 10f; // Force multiplier for applied forces
    public float moveSpeed = 5f; // Speed at which the guns move
    public float interactionRange = 2f; // Distance threshold for force application
    public KeyCode emitKey = KeyCode.Space; // Key to trigger particle emission
    public bool toggleMode = true; // Set to true for toggle, false for single burst
    public float distanceBetweenGuns = 13.5f; // Distance between gun1 and gun2 along the forward direction

    private bool isEmitting = false; // Tracks whether the particle system is currently emitting

    void Update()
    {
        // Handle particle system toggle or burst on key press
        if (Input.GetKeyDown(emitKey))
        {
            if (toggleMode)
            {
                isEmitting = !isEmitting;
                if (isEmitting)
                {
                    water.Play();
                    water2.Play();
                }
                else
                {
                    water.Stop();
                    water2.Stop();
                }
            }
            else
            {
                water.Emit(50); // Emit 50 particles from gun1
                water2.Emit(50); // Emit 50 particles from gun2
            }
        }

        // Handle movement for gun1
        HandleMovement();

        // Update gun2's position and rotation in front of gun1
        UpdateGun2Position();
    }

    void HandleMovement()
    {
        // Get input for movement (WASD or Arrow Keys)
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Move gun1 on the plane
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
        gun1.position += move;
    }

    void UpdateGun2Position()
    {
        if (gun1 != null && gun2 != null)
        {
            // Set gun2's position directly in front of gun1 based on gun1's forward direction
            gun2.position = gun1.position + gun1.forward * distanceBetweenGuns;

        }
    }
}
