using UnityEngine;
using System.Collections;

public class SprintGate : MonoBehaviour
{
    [Header("Assignments")]
    public Transform doorObject;      // The Block that moves
    public Transform openPosition;    // An empty object marking where it goes
    public SpriteRenderer triggerVisual;

    [Header("Settings")]
    public float openSpeed = 2.0f;    // Moves slowly
    public float closeSpeed = 15.0f;  // Snaps back FAST
    public float holdTime = 0.1f;

    [Header("Visual Feedback")]
    public Color activeColor = Color.red;
    public float shakeIntensity = 0.05f;
    public float shakeDuration = 0.2f;


    // Internal State
    private Vector3 closedPos;
    private enum DoorState { Closed, Opening, Open, Closing }
    private DoorState state = DoorState.Closed;
    private float timer = 0f;
    private Vector3 triggerOriginalPos;
    private Color originalColor;

    private void Start()
    {
        // Remember where the door started
        if (doorObject != null)
            closedPos = doorObject.position;

        if (triggerVisual != null)
        {
            triggerOriginalPos = triggerVisual.transform.localPosition;
            originalColor = triggerVisual.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only trigger if Player hits it AND door is currently shut
        if (other.CompareTag("Player") && state == DoorState.Closed)
        {
            state = DoorState.Opening;

            if (triggerVisual != null)
            {
                StopAllCoroutines(); // Por si se pisa varias veces
                StartCoroutine(TriggerFeedback());
            }
        }
    }

    private IEnumerator TriggerFeedback()
    {
        triggerVisual.color = activeColor;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Generar un offset aleatorio pequeño
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            triggerVisual.transform.localPosition = triggerOriginalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null; // Esperar al siguiente frame
        }

        // Volver a la posición original y color original
        triggerVisual.transform.localPosition = triggerOriginalPos;

        // Esperar a que la puerta se cierre para devolver el color original
        // o puedes devolverlo aquí mismo si quieres que sea rápido
        while (state != DoorState.Closed)
        {
            yield return null;
        }
        triggerVisual.color = originalColor;
    }

    private void Update()
    {
        if (doorObject == null || openPosition == null) return;

        switch (state)
        {
            case DoorState.Opening:
                // Move towards Open Position (Slowly)
                doorObject.position = Vector3.MoveTowards(
                    doorObject.position,
                    openPosition.position,
                    openSpeed * Time.deltaTime
                );

                // Check if we reached the target
                if (Vector3.Distance(doorObject.position, openPosition.position) < 0.01f)
                {
                    state = DoorState.Open;
                    timer = holdTime;
                }
                break;

            case DoorState.Open:
                // Wait for a tiny moment
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    state = DoorState.Closing;
                }
                break;

            case DoorState.Closing:
                // Snap back to Closed Position (Fast!)
                doorObject.position = Vector3.MoveTowards(
                    doorObject.position,
                    closedPos,
                    closeSpeed * Time.deltaTime
                );

                // Check if fully closed
                if (Vector3.Distance(doorObject.position, closedPos) < 0.01f)
                {
                    state = DoorState.Closed;
                }
                break;
        }
    }

    // Visualize the path in the Editor so it's easier to place
    private void OnDrawGizmos()
    {
        if (doorObject != null && openPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(doorObject.position, openPosition.position);
        }
    }
}