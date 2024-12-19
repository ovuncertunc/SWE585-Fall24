using UnityEngine;

public class ParticleForce : MonoBehaviour
{
    public ParticleSystem water; // Reference to the water particle system
    public Transform bucket; // Reference to the bucket GameObject
    public float forceMultiplier = 10f; // Force multiplier for applied forces
    public float moveSpeed = 5f; // Speed at which the bucket moves
    public float interactionRange = 2f; // Distance threshold for force application
    public KeyCode emitKey = KeyCode.Space; // Key to trigger particle emission
    public bool toggleMode = true; // Set to true for toggle, false for single burst

    private bool isEmitting = false; // Tracks whether the particle system is currently emitting
    private Rigidbody[] aboveGroundObjects; // Array to store all rigidbodies above ground

    void Start()
    {
        // Find all rigidbodies with a specific tag
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
                water.Emit(50); // Emit 50 particles (adjust as needed)
            }
        }
    }

    void FixedUpdate()
    {
        // Handle bucket movement
        if (bucket != null)
        {
            HandleMovement();
        }

        // if (water == null || aboveGroundObjects == null) return;

        // // Get particles
        // ParticleSystem.Particle[] particles = new ParticleSystem.Particle[water.particleCount];
        // int count = water.GetParticles(particles);

        // for (int i = 0; i < count; i++)
        // {
        //     // Convert particle data to world space
        //     Vector3 particleWorldPosition = water.transform.TransformPoint(particles[i].position);
        //     Vector3 particleWorldVelocity = water.transform.TransformVector(particles[i].velocity);

        //     foreach (Rigidbody obj in aboveGroundObjects)
        //     {
        //         if (obj == null) continue;

        //         float distanceToParticle = Vector3.Distance(obj.position, particleWorldPosition);

        //         if (distanceToParticle < interactionRange)
        //         {
        //             // Apply force in the direction of the particle's velocity
        //             Vector3 forceDirection = particleWorldVelocity.normalized;
        //             Vector3 force = forceDirection * forceMultiplier;

        //             obj.AddForce(force, ForceMode.Force);
        //             Debug.Log($"Force applied to {obj.gameObject.name}: {force}");
        //         }
        //     }
        // }
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
