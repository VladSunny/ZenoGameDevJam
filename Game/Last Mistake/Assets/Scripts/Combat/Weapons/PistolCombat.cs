using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Combat;
using Scripts.Movement;
using TMPro;
using System.Collections;
using Scripts.Config;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Scripts
{
    public class PistolCombat : GunBase
    {
        private PlayerInput _playerInput;
        private InputAction _shootAction;
        private InputAction _reloadAction;

        protected override void Awake() {
            base.Awake();

            _playerInput = GetComponentInParent<PlayerInput>();

            if (_playerInput != null) {
                _shootAction = _playerInput.actions["Shoot"];
                _reloadAction = _playerInput.actions["Reload"];
            }
        }

        private void OnEnable() {
            if (_shootAction != null)
                _shootAction.performed += ctx => Shoot();

            if (_reloadAction != null)
                _reloadAction.performed += ctx => StartReload();
        }

        private void OnDisable() {
            if (_shootAction != null)
                _shootAction.performed -= ctx => Shoot();

            if (_reloadAction != null)
                _reloadAction.performed -= ctx => StartReload();
        }
    }
}
