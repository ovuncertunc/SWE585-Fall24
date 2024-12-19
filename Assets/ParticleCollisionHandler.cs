using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    public ParticleSystem water; // Water particle system
    public Renderer groundRenderer; // Renderer of the ground
    public float shineDuration = 5f; // Time the shine effect lasts
    public float maxSmoothness = 1f; // Maximum smoothness value for the wet effect
    public float fadeSpeed = 0.2f; // Speed at which the shine fades

    private Material groundMaterial; // The ground's material
    private float originalSmoothness; // Original smoothness value of the ground
    private Coroutine fadeCoroutine;

    private void Start()
    {
        // Get the material of the ground and its original smoothness value
        groundMaterial = groundRenderer.material;
        originalSmoothness = groundMaterial.GetFloat("_Smoothness");

        // Enable collision detection on the water particle system
        var collision = water.collision;
        collision.enabled = true;
        collision.type = ParticleSystemCollisionType.World;
        collision.sendCollisionMessages = true;
    }

    private void OnParticleCollision(GameObject other)
    {
        // When a water particle hits the ground, make it shiny
        if (other == groundRenderer.gameObject)
        {
            // Start the shine effect
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine); // Stop any ongoing fading
            }
            groundMaterial.SetFloat("_Smoothness", maxSmoothness);
            fadeCoroutine = StartCoroutine(FadeShineEffect());
        }
    }

    private IEnumerator FadeShineEffect()
    {
        // Wait for the shine duration
        yield return new WaitForSeconds(shineDuration);

        // Gradually fade the smoothness back to its original value
        float currentSmoothness = groundMaterial.GetFloat("_Smoothness");
        while (currentSmoothness > originalSmoothness)
        {
            currentSmoothness = Mathf.Lerp(currentSmoothness, originalSmoothness, fadeSpeed * Time.deltaTime);
            groundMaterial.SetFloat("_Smoothness", currentSmoothness);
            yield return null;
        }

        // Ensure it resets to the exact original smoothness
        groundMaterial.SetFloat("_Smoothness", originalSmoothness);
    }
}
