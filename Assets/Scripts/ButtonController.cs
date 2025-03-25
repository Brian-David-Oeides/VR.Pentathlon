using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]

public class ButtonController : MonoBehaviour
{
    [Header("Background Image Settings")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float fadeMinAlpha = 0.3f;

    [Header("Text Stutter Settings")]
    [SerializeField] private TextMeshProUGUI[] textElements;
    [SerializeField] private float stutterDuration = 0.3f;
    [SerializeField] private float stutterIntensity = 0.03f;
    [SerializeField] private int stutterIterations = 8;
    [SerializeField] private Color stutterColor = new Color(0.9f, 0.9f, 1f, 1f);

    [Header("Sprite Glow Settings")]
    [SerializeField] private Image glowSprite; // Reference to the glow sprite behind the UI
    [SerializeField] private float minGlowScale = 1.05f;
    [SerializeField] private float maxGlowScale = 1.15f;
    [SerializeField] private float glowPulseSpeed = 1.0f;
    [SerializeField] private float minGlowAlpha = 0.5f;
    [SerializeField] private float maxGlowAlpha = 0.8f;
    [SerializeField] private Color glowColor = new Color(0.7f, 0.3f, 0.9f, 0.7f);

    private Button button;
    private Color[] originalTextColors;
    private bool isGlowing = true;
    private Vector3 originalGlowScale;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);

        // Store original text colors
        originalTextColors = new Color[textElements.Length];
        for (int i = 0; i < textElements.Length; i++)
        {
            originalTextColors[i] = textElements[i].color;
        }

        // Set up glow sprite if it exists
        if (glowSprite == null)
        {
            Debug.LogWarning("Glow sprite not assigned! Please assign a sprite to the glowSprite field.");
        }
        else
        {
            // Set glow sprite initial properties
            originalGlowScale = glowSprite.transform.localScale;
            glowSprite.color = glowColor;

            // Start the constant glow effect
            StartGlowEffect();
        }
    }

    private void OnEnable()
    {
        isGlowing = true;
        if (glowSprite != null && gameObject.activeInHierarchy)
        {
            StartGlowEffect();
        }
    }

    private void OnDisable()
    {
        isGlowing = false;
        if (glowSprite != null)
        {
            glowSprite.transform.localScale = originalGlowScale;
        }
    }

    private void StartGlowEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ConstantGlowEffect());
    }

    private void OnButtonClick()
    {
        StartCoroutine(FadeBackground());
        StartCoroutine(GlitchTextElements());
    }

    private IEnumerator ConstantGlowEffect()
    {
        if (glowSprite == null) yield break;

        while (isGlowing)
        {
            float pulseFactor = (Mathf.Sin(Time.time * glowPulseSpeed) + 1f) / 2f; // Range 0 to 1

            // Scale pulsing
            float currentScale = Mathf.Lerp(minGlowScale, maxGlowScale, pulseFactor);
            glowSprite.transform.localScale = originalGlowScale * currentScale;

            // Alpha pulsing
            Color tempColor = glowSprite.color;
            tempColor.a = Mathf.Lerp(minGlowAlpha, maxGlowAlpha, pulseFactor);
            glowSprite.color = tempColor;

            yield return null;
        }
    }

    private IEnumerator FadeBackground()
    {
        if (backgroundImage == null) yield break;

        Color originalColor = backgroundImage.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, fadeMinAlpha);

        float elapsedTime = 0f;

        // Fade out
        while (elapsedTime < fadeDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            backgroundImage.color = Color.Lerp(originalColor, targetColor, elapsedTime / (fadeDuration / 2));
            yield return null;
        }

        elapsedTime = 0f;

        // Fade in
        while (elapsedTime < fadeDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            backgroundImage.color = Color.Lerp(targetColor, originalColor, elapsedTime / (fadeDuration / 2));
            yield return null;
        }

        backgroundImage.color = originalColor;
    }

    private IEnumerator GlitchTextElements()
    {
        if (textElements.Length == 0) yield break;

        float timePerIteration = stutterDuration / stutterIterations;
        Vector3[] originalPositions = new Vector3[textElements.Length];

        // Store all original positions
        for (int j = 0; j < textElements.Length; j++)
        {
            originalPositions[j] = textElements[j].transform.localPosition;
        }

        for (int i = 0; i < stutterIterations; i++)
        {
            // Apply subtle stutter effect
            for (int j = 0; j < textElements.Length; j++)
            {
                // Small horizontal jitter only
                Vector3 stutterOffset = new Vector3(
                    Random.Range(-1f, 1f) * stutterIntensity,
                    0f,
                    0f
                );

                textElements[j].transform.localPosition = originalPositions[j] + stutterOffset;

                // Subtle color variation but no dramatic change
                if (i % 4 == 0)
                {
                    textElements[j].color = Color.Lerp(originalTextColors[j], stutterColor, 0.3f);
                }
                else
                {
                    textElements[j].color = originalTextColors[j];
                }

                // Very minimal character spacing change
                textElements[j].characterSpacing = Random.Range(-2f, 2f) * stutterIntensity;
            }

            yield return new WaitForSeconds(timePerIteration / 2);

            // Reset positions
            for (int j = 0; j < textElements.Length; j++)
            {
                textElements[j].transform.localPosition = originalPositions[j];
                textElements[j].color = originalTextColors[j];
                textElements[j].characterSpacing = 0f;
            }

            yield return new WaitForSeconds(timePerIteration / 2);
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
}