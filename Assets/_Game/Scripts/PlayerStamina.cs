using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light2D

public class PlayerStamina : MonoBehaviour
{
    [Header("Settings")]
    public float maxSprintTime = 3.0f;
    public float recoverySpeed = 1.0f;

    [Header("Visuals - The Combo")]
    public Light2D sprintLight;       // The Glow
    public SpriteRenderer playerSprite; // The Body
    public Gradient heatGradient;     // White -> Orange -> Red

    [Header("Light Shape")]
    public float minRadius = 0.5f;
    public float maxRadius = 3.0f;
    [Range(0f, 1f)] public float hardness = 0.9f; // 1 = Solid Circle, 0 = Fuzzy
    public float baseIntensity = 1.0f; // Keep this low (1.0) to avoid "white blowout"

    // State
    private float currentHeat = 0f;
    private bool mustCooldown = false; // The Lock

    public bool CanSprint => !mustCooldown;

    private void Start()
    {
        if (playerSprite == null) playerSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleHeatLogic();
        UpdateVisuals();
    }

    private void HandleHeatLogic()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool tryingToSprint = Input.GetKey(KeyCode.LeftShift);

        // LOGIC: Heat up only if moving, holding shift, and NOT locked out
        if (tryingToSprint && isMoving && !mustCooldown)
        {
            currentHeat += Time.deltaTime;

            // Hit the limit?
            if (currentHeat >= maxSprintTime)
            {
                currentHeat = maxSprintTime;
                mustCooldown = true; // Lock immediately
                Debug.Log("Overheat! Cooldown locked.");
            }
        }
        else
        {
            // LOCK LOGIC: If we have ANY heat and let go of Shift, lock it.
            if (currentHeat > 0 && !mustCooldown && !tryingToSprint)
            {
                mustCooldown = true;
            }

            // Cool down
            currentHeat -= Time.deltaTime * recoverySpeed;

            // Unlock only at absolute zero
            if (currentHeat <= 0)
            {
                currentHeat = 0;
                mustCooldown = false;
            }
        }
    }

    private void UpdateVisuals()
    {
        float heatPercent = currentHeat / maxSprintTime;
        Color targetColor = heatGradient.Evaluate(heatPercent);

        // --- PART 1: THE BODY (SPRITE) ---
        if (playerSprite != null)
        {
            // Safety: Always blend from WHITE (Original Sprite) to the Target Color
            // If we just used targetColor, and your gradient started Black, you'd disappear!
            playerSprite.color = Color.Lerp(Color.white, targetColor, heatPercent);
        }

        // --- PART 2: THE GLOW (LIGHT) ---
        if (sprintLight != null)
        {
            // A. Color & Shape
            sprintLight.color = targetColor;

            float currentRad = Mathf.Lerp(minRadius, maxRadius, heatPercent);
            sprintLight.pointLightOuterRadius = currentRad;
            sprintLight.pointLightInnerRadius = currentRad * hardness; // Hard Edge Logic

            // B. Intensity & Flicker
            if (mustCooldown)
            {
                // Cooldown: Pulse the light slowly (Breathing effect)
                float pulse = Mathf.PingPong(Time.time, 0.5f) + 0.5f; // 0.5 to 1.0
                sprintLight.intensity = baseIntensity * pulse;
            }
            else if (heatPercent > 0.8f)
            {
                // Warning (80%+): Strobe flicker (Square wave)
                float strobe = (Random.value > 0.5f) ? 1.2f : 0.8f;
                sprintLight.intensity = baseIntensity * strobe;
            }
            else
            {
                // Normal: Fade in from 0 intensity
                sprintLight.intensity = Mathf.Lerp(0f, baseIntensity, heatPercent);
            }
        }
    }

    public void UpgradeStamina(float extraTime)
    {
        maxSprintTime += extraTime;
    }
}