using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{
    public bool isOn=false;
    public bool IsOn { get; set; }
    public ToggleSwitch toggleSFX;
    public ToggleSwitch toggle;
    [SerializeField]
    private RectTransform ToggleIndicator;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Color onColor;
    [SerializeField]
    private Color offColor;
    private float onX;
    private float offX;
    [SerializeField]
    private float tweenTime = 0.25f;
    public delegate void ValueChanged(ToggleSwitch Toggle,bool value);
    public event ValueChanged valueChanged;
    [SerializeField] AudioSource audioSource;
    void Start()
    {
        toggle = this;
        offX = ToggleIndicator.anchoredPosition.x;
        onX = backgroundImage.rectTransform.rect.width - ToggleIndicator.rect.width;
        if (IsOn)
         StartToggle();
    }
    public void StartToggle(bool value=true)
    {
        backgroundImage.color = onColor;
         Vector2 pos = ToggleIndicator.gameObject.transform.localPosition;
        ToggleIndicator.gameObject.transform.localPosition = pos + new Vector2(ToggleIndicator.rect.width, 0);
        isOn = value;
    }
    public void Toggle(bool value, bool playSFX = true)
    {
        Debug.Log("Toggle");
        isOn = value;
            IsOn = value;
        if (toggleSFX.isOn)
            audioSource.Play();
        ToggleColor(isOn);
            MoveIndicator(isOn);
            valueChanged?.Invoke(toggle,isOn);
    }

    private void MoveIndicator(bool value)
    {
        if (value)
        {
            ToggleIndicator.DOAnchorPosX(onX, tweenTime);
        }
        else ToggleIndicator.DOAnchorPosX(offX, tweenTime);
    }

    private void ToggleColor(bool value)
    {
        if(value)
        {
            backgroundImage.DOColor(onColor, tweenTime);
        }
        else backgroundImage.DOColor(offColor, tweenTime);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!IsOn);//меняем значение на противоположное при нажатии
    }
}
