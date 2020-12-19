﻿using IndieMarc.TopDown;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthCaper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<PlayerCharacter>())
        {
            var player = coll.gameObject.GetComponent<PlayerCharacter>();
            player.StartWearMouthCaper();
            Destroy(gameObject);
        }
    }
}
