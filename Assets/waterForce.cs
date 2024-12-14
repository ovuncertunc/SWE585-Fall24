using UnityEngine;

public class waterForce : MonoBehaviour
{
    public ParticleSystem water; // Reference to the water particle system
    public float particleForceMultiplier = 1f; // Multiplier for the force applied by particles
    public float particleImpactRadius = 0.5f; // Radius of effect for each particle

    private ParticleSystem.Particle[] particles; // Array to store particles

    void FixedUpdate()
    {
        if (water == null) return;

        // Get all active particles in the system
        particles = new ParticleSystem.Particle[water.particleCount];
        int particleCount = water.GetParticles(particles);

        for (int i = 0; i < particleCount; i++)
        {
            // Convert the particle's position from local to world space
            Vector3 particleWorldPosition = water.transform.TransformPoint(particles[i].position);

            // Find objects within the particle's impact radius
            Collider[] hitColliders = Physics.OverlapSphere(particleWorldPosition, particleImpactRadius);

            foreach (Collider collider in hitColliders)
            {
                Rigidbody rb = collider.attachedRigidbody;

                // Apply force only if the object has a Rigidbody
                if (rb != null)
                {
                    // Calculate the particle velocity in world space
                    Vector3 particleVelocity = water.transform.TransformVector(particles[i].velocity);

                    // Apply force to the Rigidbody based on particle velocity
                    Vector3 force = particleVelocity * particleForceMultiplier;
                    rb.AddForce(force, ForceMode.Impulse);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Visualize the particle impact radius in the scene (for debugging)
        if (particles == null) return;

        Gizmos.color = Color.blue;
        foreach (ParticleSystem.Particle particle in particles)
        {
            Vector3 worldPosition = water.transform.TransformPoint(particle.position);
            Gizmos.DrawWireSphere(worldPosition, particleImpactRadius);
        }
    }
}
