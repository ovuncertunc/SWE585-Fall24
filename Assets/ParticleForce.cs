using UnityEngine;

public class ParticleForce : MonoBehaviour
{
    public ParticleSystem water; // Reference to the water particle system
    public Transform bucket; // Reference to the bucket GameObject
    public float forceMultiplier = 1f; // Force multiplier for applied forces
    public float moveSpeed = 5f; // Speed at which the bucket moves
    public KeyCode emitKey = KeyCode.Space; // Key to trigger particle emission
    public bool toggleMode = true; // Set to true for toggle, false for single burst

    private bool isEmitting = false; // Tracks whether the particle system is currently emitting
    private Rigidbody[] aboveGroundObjects; // Array to store all rigidbodies above ground

    void Start()
    {
        // Find all objects above ground by checking for a specific tag or rigidbody
        GameObject[] objects = GameObject.FindGameObjectsWithTag("AboveGround");
        aboveGroundObjects = new Rigidbody[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            aboveGroundObjects[i] = objects[i].GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Handle particle system toggle or burst on key press
        if (Input.GetKeyDown(emitKey))
        {
            if (toggleMode)
            {
                // Toggle the particle system on or off
                if (isEmitting)
                {
                    water.Stop();
                    isEmitting = false;
                }
                else
                {
                    water.Play();
                    isEmitting = true;
                }
            }
            else
            {
                // Emit particles as a single burst
                water.Emit(50); // Emit 50 particles (adjust as needed)
            }
        }
    }

    void FixedUpdate()
    {
        // Handle bucket movement with arrow keys or WASD
        if (bucket != null)
        {
            HandleMovement();
        }

        if (water == null || aboveGroundObjects == null || aboveGroundObjects.Length == 0) return;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[water.particleCount];
        int count = water.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 particlePosition = particles[i].position + water.transform.position;

            // Loop through all rigidbodies above ground and apply force
            foreach (Rigidbody obj in aboveGroundObjects)
            {
                if (obj == null) continue;

                Vector3 directionToObj = obj.position - particlePosition;
                float distanceToObj = directionToObj.magnitude;

                if (distanceToObj < 2f) // Adjust this range as needed
                {
                    Vector3 forceToObj = directionToObj.normalized / (distanceToObj * distanceToObj) * forceMultiplier;
                    obj.AddForce(forceToObj, ForceMode.Force);
                }
            }
        }

        // Update particles in the particle system
        water.SetParticles(particles, count);
    }

    void HandleMovement()
    {
        // Get input for movement (WASD or Arrow Keys)
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Move the bucket on the plane
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        // Apply movement to the bucket's transform
        bucket.position += move;
    }
}
