using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vaccine script
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>

namespace IndieMarc.TopDown
{

    public class Vaccine : MonoBehaviour
    {

        public int vaccine_index = 0;
        public int vaccine_value = 1;
        public Sprite vaccine_half;

        private string unique_id;
        private SpriteRenderer sprite_renderer;
        private CarryItem carry_item;

        private int vaccines_left = 3;

        void Start()
        {
            sprite_renderer = GetComponent<SpriteRenderer>();
            carry_item = GetComponent<CarryItem>();
            carry_item.OnTake += OnTake;
            carry_item.OnDrop += OnDrop;

        }

        private void OnTake(GameObject triggerer)
        {
            
        }

        private void OnDrop(GameObject triggerer)
        {
            
        }

        public bool VaccinatePatient()
        {
            vaccines_left = vaccines_left - 1;

            if (vaccines_left < 3)
            {
                sprite_renderer.sprite = vaccine_half;
            }

            if (vaccines_left == 0) {
                Destroy(gameObject);
            }

            return true;
        }
    }

}