using System;
using UnityEngine;
using DG.Tweening;

namespace Camera_Stuff
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] public Camera cam;
        [SerializeField] public Vector3 maxValue, minValue;
        private Vector3 _origin, _difference;
        private bool _drag, _move;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 worldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hitData = Physics2D.Raycast(worldPosition, Vector2.zero, 0);

                if (hitData)
                {
                    return;
                }
                else
                {
                    _difference = (cam.ScreenToWorldPoint(Input.mousePosition) - cam.transform.position);
                    if (_drag == false)
                    {
                        _drag = true;
                        _origin = cam.ScreenToWorldPoint(Input.mousePosition);
                    }
                }
            }
            else
            {
                _drag = false;
            }

            if (_drag)
            {
                cam.transform.position = _origin - _difference;
                ReBound();
            }
        }

        private void ReBound()
        {
            Vector3 targetPosition = cam.transform.position;
            Vector3 boundPosition = new Vector3(Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x)
                , Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y)
                , Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z));

            if (_move)
            {
                cam.transform.position = boundPosition;
            }
        }

        private void OnEnable()
        {
            _move = true;
        }

        private void OnDisable()
        {
            _move = false;
        }
    }
}
