using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BrJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 Direction;
    public Image JoyesticImage;
    public string HorizontalAxis = "Horizontal";
    public string VerticalAxis = "Vertical";

    private float _max;
    private Vector2 _startPos;
    private Vector2 _currentPosition;
    private Vector3 _joyPosition;
    private bool _isDraging;
    public Vector3 Value3 => new Vector3(Direction.x, 0, Direction.y) / _max;


    public virtual void Start()
    {
        _joyPosition = JoyesticImage.rectTransform.position;
        _max = GetComponent<RectTransform>().rect.width * transform.lossyScale.x / 2;
    }

    public virtual void OnBeginDrag(PointerEventData data)
    {
        _startPos = data.position;
        JoyesticImage.rectTransform.SetPositionAndRotation(new Vector3(_startPos.x, _startPos.y, 0), Quaternion.identity);
    }

    public void OnDrag(PointerEventData data)
    {
        _isDraging = true;
        _currentPosition = data.position;
        CalculateDrag();
    }

    public virtual void CalculateDrag()
    {
        Direction = _currentPosition - _startPos;

        Vector2 dragLenth = Vector2.ClampMagnitude(Direction, _max);

        if (Direction.magnitude > _max)
        {
            _startPos += Direction - dragLenth;
            JoyesticImage.rectTransform.SetPositionAndRotation(_startPos, Quaternion.identity);
        }

        JoyesticImage.rectTransform.GetChild(0).
            SetPositionAndRotation(
            _startPos + dragLenth,
            Quaternion.identity
            );
    }

    public virtual void OnEndDrag(PointerEventData data)
    {
        _isDraging = false;
        Direction = Vector2.zero;
        JoyesticImage.rectTransform.SetPositionAndRotation(_joyPosition, Quaternion.identity);
        JoyesticImage.rectTransform.GetChild(0).SetPositionAndRotation(JoyesticImage.rectTransform.position, Quaternion.identity);
    }

    void Update()
    {
        if (_isDraging)
            return;

        Direction.x = Input.GetAxis(HorizontalAxis) * _max;
        Direction.y = Input.GetAxis(VerticalAxis) * _max;
        Direction = Direction.normalized * _max;
        JoyesticImage.rectTransform.GetChild(0).SetPositionAndRotation(JoyesticImage.rectTransform.position+(Vector3)Direction, Quaternion.identity);

    }


    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
