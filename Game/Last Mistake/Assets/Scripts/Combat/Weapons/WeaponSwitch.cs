using System;
using Scripts.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class WeaponSwitch : MonoBehaviour
    {
        private int _selectedWeapon = 0;
        private bool _destroyed = false;

        private PlayerInput _playerInput;
        private InputAction _weaponSwitchAction;

        private void Awake() {
            SelectedWeapon();

            _playerInput = GetComponentInParent<PlayerInput>();

            if (_playerInput != null)
                _weaponSwitchAction = _playerInput.actions["WeaponSwitch"];

            SelectedWeapon();
        }

        private void OnEnable() {
            if (_weaponSwitchAction != null)
                _weaponSwitchAction.performed += ctx => SwitchWeapon();
        }

        private void OnDisable() {
            if (_weaponSwitchAction != null)
                _weaponSwitchAction.performed -= ctx => SwitchWeapon();
        }

        // private void Update() {
        //     if (Input.GetAxis("Mouse ScrollWheel") > 0f) SwitchWeapon();
        // }

        private void SwitchWeapon() {
            if (_destroyed) return;

            int i = 0;
            foreach (Transform weapon in transform) {
                if (i == _selectedWeapon) {
                    if (weapon.GetComponent<GunBase>().IsReloading()) return;
                    break;
                }
                i++;
            }

            _selectedWeapon++;
            _selectedWeapon %= transform.childCount;
            SelectedWeapon();
        }

        private void SelectedWeapon() {
            int i = 0;
            foreach (Transform weapon in transform) {
                if (i == _selectedWeapon) {
                    weapon.gameObject.SetActive(true);
                    weapon.GetComponent<GunBase>().UpdateUI();
                }
                else
                    weapon.gameObject.SetActive(false);
                i++;
            }
        }

        private void OnDestroy() {
            _destroyed = true;
            if (_weaponSwitchAction != null)
                _weaponSwitchAction.performed -= ctx => SwitchWeapon();
        }
    }
}
