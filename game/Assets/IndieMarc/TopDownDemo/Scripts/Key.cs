using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Key script
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>

namespace IndieMarc.TopDown
{

    public class Key : MonoBehaviour
    {

        public int key_index = 0;
        public int key_value = 1;

        [Header("Audio")]
        public AudioClip audio_take;
        public AudioClip audio_drop;

        private string unique_id;
        private CarryItem carry_item;
        private AudioSource audio_source;


        void Start()
        {
            carry_item = GetComponent<CarryItem>();
            carry_item.OnTake += OnTake;
            carry_item.OnDrop += OnDrop;
            audio_source = GetComponent<AudioSource>();
        }

        private void OnTake(GameObject triggerer)
        {
            audio_source.PlayOneShot(audio_take);
        }

        private void OnDrop(GameObject triggerer)
        {
            audio_source.PlayOneShot(audio_drop);
        }

        public bool TryOpenDoor(GameObject door)
        {
            if (door.GetComponent<Door>() && door.GetComponent<Door>().CanKeyUnlock(this) && !door.GetComponent<Door>().IsOpened())
            {
                door.GetComponent<Door>().UnlockWithKey(key_value);
                Destroy(gameObject);
                return true;
            }
            return false;
        }
    }

}