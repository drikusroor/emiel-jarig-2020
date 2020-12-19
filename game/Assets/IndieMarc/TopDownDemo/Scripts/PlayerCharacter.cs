using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Top down character movement
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>

namespace IndieMarc.TopDown
{
    public class PlayerCharacter : MonoBehaviour
    {
        public int player_id;

        [Header("Stats")]
        public float max_hp = 100f;

        [Header("Status")]
        public bool invulnerable = false;

        [Header("Movement")]
        public float move_accel = 1f;
        public float move_deccel = 1f;
        public float move_max = 1f;

        [Header("Audio")]
        public AudioClip audio_die;
        public AudioClip audio_wear_mouth_cape;

        public UnityAction onDeath;
        public UnityAction onHit;

        private Rigidbody2D rigid;
        private Transform head;
        private Transform mouth_caper;
        private Animator animator;
        private AutoOrderLayer auto_order;
        private ContactFilter2D contact_filter;
        private AudioSource audio_source;
        private AudioSource footsteps_audio_source;

        private float hp;
        private bool is_dead = false;
        private bool is_wearing_mouth_caper;
        private Vector2 start_pos;
        private Vector2 move;
        private Vector2 move_input;
        private Vector2 lookat = Vector2.zero;
        private float side = 1f;
        private bool disable_controls = false;
        private float hit_timer = 0f;

        private static Dictionary<int, PlayerCharacter> character_list = new Dictionary<int, PlayerCharacter>();

        void Awake()
        {
            character_list[player_id] = this;
            rigid = GetComponent<Rigidbody2D>();
            head = transform.Find("Head");
            mouth_caper = transform.Find("MouthCaper");
            animator = GetComponent<Animator>();
            auto_order = GetComponent<AutoOrderLayer>();
            hp = max_hp;
            start_pos = transform.position;
            audio_source = GetComponent<AudioSource>();
            footsteps_audio_source = transform.Find("Shadow").GetComponent<AudioSource>();
        }

        void OnDestroy()
        {
            character_list.Remove(player_id);
        }

        void Start()
        {

        }

        //Handle physics
        void FixedUpdate()
        {
            //Movement velocity
            float desiredSpeedX = Mathf.Abs(move_input.x) > 0.1f ? move_input.x * move_max : 0f;
            float accelerationX = Mathf.Abs(move_input.x) > 0.1f ? move_accel : move_deccel;
            move.x = Mathf.MoveTowards(move.x, desiredSpeedX, accelerationX * Time.fixedDeltaTime);
            float desiredSpeedY = Mathf.Abs(move_input.y) > 0.1f ? move_input.y * move_max : 0f;
            float accelerationY = Mathf.Abs(move_input.y) > 0.1f ? move_accel : move_deccel;
            move.y = Mathf.MoveTowards(move.y, desiredSpeedY, accelerationY * Time.fixedDeltaTime);

            //Move
            rigid.velocity = move;
            
            if (move.x == 0 && move.y == 0)
            {
                if (footsteps_audio_source.isPlaying)
                {
                    footsteps_audio_source.Stop();
                }
            }
            else
            {
                if (!footsteps_audio_source.isPlaying)
                {
                    footsteps_audio_source.Play();
                }
            }
        }

        //Handle render and controls
        void Update()
        {
            hit_timer += Time.deltaTime;
            move_input = Vector2.zero;

            //Controls
            if (!disable_controls)
            {
                //Controls
                PlayerControls controls = PlayerControls.Get(player_id);
                move_input = controls.GetMove();
                
            }

            //Update lookat side
            if (move.magnitude > 0.1f)
                lookat = move.normalized;
            if (Mathf.Abs(lookat.x) > 0.02)
                side = Mathf.Sign(lookat.x);
                // head.localScale.x = side;
                head.localRotation = Quaternion.Euler(0, side == 1 ? 0 : 180, 0);
                mouth_caper.localRotation = Quaternion.Euler(0, side == 1 ? 0 : 180, 0);
        }

        public void HealDamage(float heal)
        {
            if (!is_dead)
            {
                hp += heal;
                hp = Mathf.Min(hp, max_hp);
            }
        }

        public void StartWearMouthCaper()
        {
            is_wearing_mouth_caper = true;
            mouth_caper.gameObject.SetActive(true);
            audio_source.PlayOneShot(audio_wear_mouth_cape);
        }

        public bool IsWearingMouthCaper()
        {
            return is_wearing_mouth_caper;
        }

        public void TakeDamage(float damage)
        {
            if (!is_dead && !invulnerable && hit_timer > 0f)
            {
                hp -= damage;
                hit_timer = -1f;

                if (hp <= 0f)
                {
                    Kill();
                }
                else
                {
                    if (onHit != null)
                        onHit.Invoke();
                }
            }
        }

        public void Kill()
        {
            if (!is_dead)
            {
                is_dead = true;
                rigid.velocity = Vector2.zero;
                move = Vector2.zero;
                move_input = Vector2.zero;

                if (onDeath != null)
                    onDeath.Invoke();

                audio_source.PlayOneShot(audio_die);

                Teleport(start_pos);
            }
        }
        
        public void Teleport(Vector3 pos)
        {
            transform.position = pos;
            move = Vector2.zero;
        }

        public Vector2 GetMove()
        {
            return move;
        }

        public Vector2 GetFacing()
        {
            return lookat;
        }

        public int GetSortOrder()
        {
            return auto_order.GetSortOrder();
        }

        //Get Character side
        public float GetSide()
        {
            return side; //Return 1 frame before to let anim do transitions
        }

        public int GetSideAnim()
        {
            return (side >= 0) ? 1 : 3;
        }

        public bool IsDead()
        {
            return is_dead;
        }

        public void DisableControls() { disable_controls = true; }
        public void EnableControls() { disable_controls = false; }
        
        public static PlayerCharacter GetNearest(Vector3 pos, float range = 999f, bool alive_only=true)
        {
            PlayerCharacter nearest = null;
            float min_dist = range;
            foreach (PlayerCharacter character in character_list.Values)
            {
                if (!alive_only || !character.IsDead())
                {
                    float dist = (pos - character.transform.position).magnitude;
                    if (dist < min_dist)
                    {
                        min_dist = dist;
                        nearest = character;
                    }
                }
            }
            return nearest;
        }

        public static PlayerCharacter Get(int player_id)
        {
            foreach (PlayerCharacter character in character_list.Values)
            {
                if (character.player_id == player_id)
                {
                    return character;
                }
            }
            return null;
        }

        public static PlayerCharacter[] GetAll()
        {
            PlayerCharacter[] list = new PlayerCharacter[character_list.Count];
            character_list.Values.CopyTo(list, 0);
            return list;
        }
    }
}
