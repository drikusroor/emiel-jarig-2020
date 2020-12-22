using IndieMarc.TopDown;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkWithFriends : MonoBehaviour
{
    // Start is called before the first frame update
    private Confetti confetti;
    private Speaker speaker;
    private IEnumerator coroutine;

    void Start()
    {
        confetti = transform.Find("Confetti").GetComponent<Confetti>();
        speaker = transform.Find("Speaker").GetComponent<Speaker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var hold_item = coll.GetComponent<CharacterHoldItem>();
        if (hold_item != null && hold_item.GetHeldItem() != null && hold_item.GetHeldItem().name == "Beer")
        {
            var held_item = hold_item.GetHeldItem();
            Destroy(held_item.gameObject);
            ActivateParty();
        }
    }

    void ActivateParty()
    {
        confetti.Pop();
        speaker.PlayMusic();

        var disco_lights = FindObjectsOfType<DiscoLight>();

        foreach (var light in disco_lights)
        {
            light.TurnOn();
        }

        Invoke("StartDancing", 0.5f);
    }

    void StartDancing()
    {
        coroutine = Dance(0.48f);
        StartCoroutine(coroutine);
    }

    IEnumerator Dance(float seconds)
    {
        while (true)
        {
            Vector3 scale = new Vector3(Random.Range(0.3f, 0.4f), Random.Range(0.3f, 0.4f), 0);
            transform.localScale = scale;
            yield return new WaitForSeconds(seconds);
        }
    }
}
