using IndieMarc.TopDown;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkWithFriends : MonoBehaviour
{
    // Start is called before the first frame update
    private Confetti confetti;
    void Start()
    {
        confetti = transform.Find("Confetti").GetComponent<Confetti>();
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

        var disco_lights = FindObjectsOfType<DiscoLight>();

        foreach (var light in disco_lights)
        {
            light.TurnOn();
        }
    }
}
