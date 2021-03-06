﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Door script
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>
/// 

namespace IndieMarc.TopDown
{

    public class Patient : MonoBehaviour
    {
        [Header("Patient")]
        public bool healed = false;
        public bool reset_on_death = true;

        [Header("Audio")]
        public AudioClip audio_patient_healed;

        [Header("Sprite")]
        public Sprite patient_healed_sprite;
        
        private Vector3 initialPos;
        private int nb_keys_inside;
        private int audio_last_played;
        private bool initial_opened;
        private Vector3 target_pos;
        private bool should_open;
        private SpriteRenderer sprite_renderer;
        private PlayerCharacter player;

        private AudioSource audio_source;
        
        private static List<Patient> patient_list = new List<Patient>();

        void Awake()
        {
            patient_list.Add(this);
        }

        void OnDestroy()
        {
            patient_list.Remove(this);
        }

        void Start()
        {
            initialPos = transform.position;
            initialPos.z = 0f;
            audio_source = GetComponent<AudioSource>();
            target_pos = transform.position;
            sprite_renderer = GetComponentInChildren<SpriteRenderer>();
            player = FindObjectOfType<PlayerCharacter>();

            ResetOne();
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.GetComponent<Vaccine>())
            {
                var vaccine = coll.gameObject.GetComponent<Vaccine>();
                TryHealPatientWithVaccine(vaccine);
            }

            if (coll.gameObject.GetComponent<PlayerCharacter>())
            {
                if (!player.IsWearingMouthCaper())
                {
                    player.Kill();
                }
            }
        }

        public void TryHealPatientWithVaccine(Vaccine vaccine)
        {
            if (healed)
            {
                return;
            }

            if (!player.IsWearingMouthCaper())
            {
                player.Kill();
                return;
            }

            vaccine.VaccinatePatient();
            healed = true;
            sprite_renderer.sprite = patient_healed_sprite;
            audio_source.PlayOneShot(audio_patient_healed);

            var everyone_healed = !patient_list.Exists(p => !p.healed);
            if (everyone_healed)
            {
                vaccine.OpenDoor();
            }
        }

        public void ResetOne()
        {
            healed = false;
        }
    }

}