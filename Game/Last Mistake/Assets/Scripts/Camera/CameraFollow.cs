using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.PlayerCamera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Vector3 _cameraOffset;

        private void Update() {
            transform.position = new Vector3(
                _playerTransform.position.x + _cameraOffset.x,
                transform.position.y + _cameraOffset.y,
                _playerTransform.position.z + _cameraOffset.z
            );
        }
    }
}
