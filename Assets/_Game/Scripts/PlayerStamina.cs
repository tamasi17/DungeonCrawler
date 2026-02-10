using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerStamina : MonoBehaviour
{
    [Header("Settings")]
    public float maxSprintTime = 3.0f;
    public float recoverySpeed = 1.0f;

    [Header("Visuals")]
    public Light2D sprintLight;
    public Gradient heatGradient;

    [Header("Circle Shape")]
    public float minRadius = 0.5f;
    public float maxRadius = 2.5f;
    [Range(0f, 1f)]
    public float hardness = 0.95f; // 1.0 = Perfect hard circle, 0.0 = Very soft

    [Header("Intensity")]
    public float baseIntensity = 2.0f;

    // State
    private float currentHeat = 0f;
    private bool mustCooldown = false; // The lock for "Mid Tank" logic

    // We can only sprint if the cooldown lock is OFF
    public bool CanSprint => !mustCooldown;

    private void Update()
    {
        HandleHeatLogic();
        UpdateVisuals();
    }

    private void HandleHeatLogic()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool tryingToSprint = Input.GetKey(KeyCode.LeftShift);

        // LOGIC: If we are trying to sprint, moving, and NOT locked out
        if (tryingToSprint && isMoving && !mustCooldown)
        {
            // HEATING UP
            currentHeat += Time.deltaTime;

            // Did we hit the limit?
            if (currentHeat >= maxSprintTime)
            {
                currentHeat = maxSprintTime;
                mustCooldown = true; // Lock it because we hit max
                Debug.Log("Overheated! Full cooldown required.");
            }
        }
        else
        {
            // COOLING DOWN

            // If we were sprinting but stopped (let go of key), lock it immediately!
            if (currentHeat > 0 && !mustCooldown && !tryingToSprint)
            {
                mustCooldown = true;
            }

            currentHeat -= Time.deltaTime * recoverySpeed;

            // Only unlock when we hit absolute zero
            if (currentHeat <= 0)
            {
                currentHeat = 0;
                mustCooldown = false; // UNLOCK
            }
        }
    }

    private void UpdateVisuals()
    {
        if (sprintLight == null) return;

        float heatPercent = currentHeat / maxSprintTime;

        // 1. COLOR
        sprintLight.color = heatGradient.Evaluate(heatPercent);

        // 2. SIZE & HARD EDGES
        float targetRadius = Mathf.Lerp(minRadius, maxRadius, heatPercent);

        // Set Outer Radius
        sprintLight.pointLightOuterRadius = targetRadius;
        // Set Inner Radius close to Outer to create a "Hard Edge"
        sprintLight.pointLightInnerRadius = targetRadius * hardness;

        // 3. INTENSITY & FLICKER
        if (mustCooldown)
        {
            // If we are locked out (cooling down), pulse slowly
            // Use 'PingPong' for a sharp linear bounce instead of smooth Sine wave
            float pulse = Mathf.PingPong(Time.time * 2.0f, 1.0f); // 0 to 1
            sprintLight.intensity = baseIntensity * (0.5f + pulse);
        }
        else if (heatPercent > 0.8f) // Warning Phase (80%+)
        {
            // Aggressive Strobe Flicker
            // Randomly snapping between 0.5x and 1.5x brightness
            float strobe = (Random.value > 0.5f) ? 1.5f : 0.5f;
            sprintLight.intensity = baseIntensity * strobe;
        }
        else
        {
            // Normal Growth (Fade in from 0)
            sprintLight.intensity = Mathf.Lerp(0f, baseIntensity, heatPercent);
        }
    }

    public void UpgradeStamina(float extraTime)
    {
        maxSprintTime += extraTime;
    }
}