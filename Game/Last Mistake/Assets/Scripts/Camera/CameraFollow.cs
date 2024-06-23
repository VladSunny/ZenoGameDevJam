using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        private void Update() {
            transform.position = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);
        }
    }
}
