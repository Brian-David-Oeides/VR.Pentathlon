using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Slider))]

public class SnapBackSlider : MonoBehaviour, IPointerUpHandler
{
    private Slider slider;
    [SerializeField] private float defaultValue = 0f; // set default value 0

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = defaultValue;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // snap back to default value slider handle released
        slider.value = defaultValue;
    }
}
