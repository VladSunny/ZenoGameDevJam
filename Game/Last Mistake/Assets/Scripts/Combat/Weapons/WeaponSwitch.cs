using System;
using Scripts.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class WeaponSwitch : MonoBehaviour
    {
        [SerializeField] private GameObject _pistol;
        [SerializeField] private GameObject _rifle;

        private PlayerInput _playerInput;
        private InputAction _weaponSwitchAction;

        private void Awake() {
            _playerInput = GetComponentInParent<PlayerInput>();
            
            if (_playerInput != null) {
                _weaponSwitchAction = _playerInput.actions["WeaponSwitch"];
                _weaponSwitchAction.performed += SwitchWeapon;
            }

            _pistol.SetActive(true);
            _rifle.SetActive(false);
        }

        private void SwitchWeapon(InputAction.CallbackContext context)
        {
            _pistol.SetActive(!_pistol.activeSelf);
            _rifle.SetActive(!_rifle.activeSelf);

            if (_pistol.activeSelf) _pistol.GetComponent<GunBase>().UpdateUI();
            if (_rifle.activeSelf) _rifle.GetComponent<GunBase>().UpdateUI();
        }
    }
}
