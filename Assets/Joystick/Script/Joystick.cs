using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Joystick : MonoBehaviour
{
    public static Joystick current;

    public RectTransform center;
    public RectTransform knob;
    public float range;
    public bool fixedJoystick;
    public bool visibleJoystick;

    [SerializeField]
    private Vector2 direction;


    public float knobDistance;

    Vector2 start;

    public bool isReady = true;

    [SerializeField] private CanvasGroup joystickCanvasGroup;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        ShowHide(false);

        joystickCanvasGroup.DOFade(0, 0.001f);
        joystickCanvasGroup.interactable = false;
        joystickCanvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (!isReady)
            return;

        Vector2 pos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (visibleJoystick)
            {
                ShowHide(true);
            }
            start = pos;

            knob.position = pos;
            center.position = pos;
        }
        else if (Input.GetMouseButton(0))
        {
            knob.position = pos;
            knob.position = center.position + Vector3.ClampMagnitude(knob.position - center.position, center.sizeDelta.x * range);

            if (knob.position != Input.mousePosition && !fixedJoystick)
            {
                Vector3 outsideBoundsVector = Input.mousePosition - knob.position;
                center.position += outsideBoundsVector;
            }

            knobDistance = Vector3.Distance(knob.position, center.position) / (center.sizeDelta.x * range);
            direction = (knob.position - center.position).normalized;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ShowHide(false);
            direction = Vector2.zero;

            EventSystem.CallJoystickButtonUp();
        }
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += EnableJoystick;
        EventSystem.OnGameOver += DisableJoystick;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= EnableJoystick;
        EventSystem.OnGameOver -= DisableJoystick;
    }

    private void EnableJoystick()
    {
        joystickCanvasGroup.DOFade(1, 0.01f);
        joystickCanvasGroup.interactable = true;
        joystickCanvasGroup.blocksRaycasts = true;
    }

    private void DisableJoystick(GameResult gameResult)
    {
        joystickCanvasGroup.DOFade(0, 0.001f);
        joystickCanvasGroup.interactable = false;
        joystickCanvasGroup.blocksRaycasts = false;
    }

    public float GetAxis(string _direction)
    {
        switch (_direction)
        {
            case PlayerPrefKeys.horizontal: return direction.x;
            case PlayerPrefKeys.vertical: return direction.y;
            default: return 0;
        }
    }

    public float GetAxisRaw(string _direction)
    {
        switch (_direction)
        {
            case PlayerPrefKeys.horizontal:
                if (direction.x > 0.0f) return 1f;
                if (direction.x < 0.0f) return -1f;
                return 0;
            case PlayerPrefKeys.vertical:
                if (direction.y > 0.0f) return 1f;
                if (direction.y < 0.0f) return -1f;
                return 0;
            default: return 0;
        }
    }

    void ShowHide(bool state)
    {
        center.gameObject.SetActive(state);
        knob.gameObject.SetActive(state);
    }
}


