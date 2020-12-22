using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audio_source;

    void Start()
    {
        gameObject.SetActive(false);
        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pop()
    {
        gameObject.SetActive(true);
        audio_source.Play();
    }
}
