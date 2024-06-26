using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Config
{
    [CreateAssetMenu(fileName = "PistolConfig", menuName = "ScriptableObjects/PistolConfig")]
    public class PistolConfig : ScriptableObject
    {
        public int bulletsInClip = 7;
        public int maxBullets = 35;
        public float damage = 10f;
        public float spread = 0.01f;
        public float force = 2f;
        public float concussion = 0.5f;
        public float shootCooldown = 0.5f;
    }
}
