using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeOutManger : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image glowSpriteImage;
    [SerializeField] private GameObject[] uiElementsToDisable;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool disableAfterFade = true;

    private Color backgroundOriginalColor;
    private Color glowOriginalColor;

    private void Awake()
    {
        if (backgroundImage != null)
        {
            backgroundOriginalColor = backgroundImage.color;
        }

        if (glowSpriteImage != null)
        {
            glowOriginalColor = glowSpriteImage.color;
        }
    }

    public void FadeOutUI()
    {
        StartCoroutine(FadeOutUICoroutine());
    }

    public void FadeInUI()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeInUICoroutine());
    }

    private IEnumerator FadeOutUICoroutine()
    {
        float elapsedTime = 0f;

        // Store initial colors
        Color bgStartColor = backgroundImage != null ? backgroundImage.color : Color.white;
        Color glowStartColor = glowSpriteImage != null ? glowSpriteImage.color : Color.white;

        // Target colors (fully transparent)
        Color bgTargetColor = new Color(bgStartColor.r, bgStartColor.g, bgStartColor.b, 0f);
        Color glowTargetColor = new Color(glowStartColor.r, glowStartColor.g, glowStartColor.b, 0f);

        // Fade out
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration;

            if (backgroundImage != null)
            {
                backgroundImage.color = Color.Lerp(bgStartColor, bgTargetColor, normalizedTime);
            }

            if (glowSpriteImage != null)
            {
                glowSpriteImage.color = Color.Lerp(glowStartColor, glowTargetColor, normalizedTime);
            }

            yield return null;
        }

        // Ensure the final state is reached
        if (backgroundImage != null)
        {
            backgroundImage.color = bgTargetColor;
        }

        if (glowSpriteImage != null)
        {
            glowSpriteImage.color = glowTargetColor;
        }

        // Disable the UI if needed
        if (disableAfterFade)
        {
            // Disable additional UI elements if specified
            foreach (GameObject element in uiElementsToDisable)
            {
                if (element != null)
                {
                    element.SetActive(false);
                }
            }

            // Disable this GameObject last
            gameObject.SetActive(false);

            // Reset colors for when the UI is enabled again
            if (backgroundImage != null)
            {
                backgroundImage.color = backgroundOriginalColor;
            }

            if (glowSpriteImage != null)
            {
                glowSpriteImage.color = glowOriginalColor;
            }
        }
    }

    private IEnumerator FadeInUICoroutine()
    {
        float elapsedTime = 0f;

        // Ensure all UI elements are active
        foreach (GameObject element in uiElementsToDisable)
        {
            if (element != null)
            {
                element.SetActive(true);
            }
        }

        // Start with transparent colors
        Color bgTransparentColor = new Color(backgroundOriginalColor.r, backgroundOriginalColor.g, backgroundOriginalColor.b, 0f);
        Color glowTransparentColor = new Color(glowOriginalColor.r, glowOriginalColor.g, glowOriginalColor.b, 0f);

        if (backgroundImage != null)
        {
            backgroundImage.color = bgTransparentColor;
        }

        if (glowSpriteImage != null)
        {
            glowSpriteImage.color = glowTransparentColor;
        }

        // Fade in
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration;

            if (backgroundImage != null)
            {
                backgroundImage.color = Color.Lerp(bgTransparentColor, backgroundOriginalColor, normalizedTime);
            }

            if (glowSpriteImage != null)
            {
                glowSpriteImage.color = Color.Lerp(glowTransparentColor, glowOriginalColor, normalizedTime);
            }

            yield return null;
        }

        // Ensure the final state is reached
        if (backgroundImage != null)
        {
            backgroundImage.color = backgroundOriginalColor;
        }

        if (glowSpriteImage != null)
        {
            glowSpriteImage.color = glowOriginalColor;
        }
    }
}