using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IControllerSystem
{
    public float Range = 50;
    public bool StaticPosition = false;

    [Header("Movement Direction")]
    public bool IsVertical = true;
    public bool IsHorizontal = true;

    [Header("Axis Names")]
    public string VerticalAxis = "Vertical";
    public string HorizontalAxis = "Horizontal";

    [Header("Hiding When Touch Ended")]
    public bool HideFrame = false;
    public bool HideHandle = false;

    [Header("Joystick Handle")]
    public RectTransform JoystickHandle;
    public Color JoystickHandleHideColor = Color.clear;
    Image JoystickHandleImage;
    Color JoystickHandleFirstColor;

    [Header("Joystick Frame")]
    public RectTransform JoystickFrame;
    public Color JoystickFrameHideColor = Color.clear;
    Image JoystickFrameImage;
    Color JoystickFrameFirstColor;

    RectTransform Canvas;

    float ScreenWidth;
    float ScreenHeight;
    float ScreenWidthRatio;
    float ScreenHeightRatio;
    bool IsActivated = false;


    void Awake()
    {

        Canvas = transform.GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Init()
    {
        
        JoystickHandleImage = JoystickHandle.transform.GetComponent<Image>();
        JoystickHandleFirstColor = JoystickHandleImage.color;
        if(HideHandle)
        {
            JoystickHandleImage.color = JoystickHandleHideColor;
        }

        JoystickFrameImage = JoystickFrame.transform.GetComponent<Image>();
        JoystickFrameFirstColor = JoystickFrameImage.color;
        if(HideFrame)
        {
            JoystickFrameImage.color = JoystickFrameHideColor;
        }

        ScreenHeight = Canvas.rect.height;
        ScreenHeightRatio = ScreenHeight / Screen.height;

        ScreenWidth = Canvas.rect.width;
        ScreenWidthRatio = ScreenWidth / Screen.width;
        
        ControllerSystem.Instance.CreateAxis(VerticalAxis);
        ControllerSystem.Instance.CreateAxis(HorizontalAxis);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        dragPosition = Vector2.zero;
        JoystickHandle.anchoredPosition = dragPosition;
        
        ControllerSystem.Instance.SetAxisValue(HorizontalAxis,dragPosition.x / Range);
        ControllerSystem.Instance.SetAxisValue(VerticalAxis,dragPosition.y / Range);
        SetColors(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 startPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickFrame,eventData.position,null,out startPos);

        if(StaticPosition)
        {
            if(startPos.magnitude < Range)
            {
                IsActivated = true;
                SetColors(true);
            }
            else
            {
                IsActivated = false;
            }
        }
        else
        {
            Vector2 joystickPos = new Vector2(eventData.position.x * ScreenWidthRatio, eventData.position.y * ScreenHeightRatio);
            JoystickFrame.anchoredPosition = joystickPos;
            IsActivated = true;
            SetColors(true);
        }
        
    }

    Vector2 dragPosition;
    public void OnDrag(PointerEventData eventData)
    {
        if(IsActivated)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickFrame,eventData.position,null,out dragPosition);

            if(!IsHorizontal)
            {
                dragPosition.x = 0;
            }

            if(!IsVertical)
            {
                dragPosition.y = 0;
            }

            if(dragPosition.magnitude > Range)
            {
                dragPosition = dragPosition.normalized * Range;
            }
            JoystickHandle.anchoredPosition = dragPosition;

            ControllerSystem.Instance.SetAxisValue(HorizontalAxis,dragPosition.x / Range);
            ControllerSystem.Instance.SetAxisValue(VerticalAxis,dragPosition.y / Range);
        }

        
    }


    void SetColors(bool IsDrag)
    {
        Color handleColor = JoystickHandleFirstColor;
        Color frameColor = JoystickFrameFirstColor;

        if(!IsDrag)
        {
            if(HideFrame)
            {
                frameColor = JoystickFrameHideColor;
            }
            if(HideHandle)
            {
                handleColor = JoystickHandleHideColor;
            }
        }

        JoystickFrameImage.color = frameColor;
        JoystickHandleImage.color = handleColor;

    }


}

