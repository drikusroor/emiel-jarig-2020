using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    private AudioSource audio_source;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReallyPlayMusic()
    {
        coroutine = Dance(0.48f);
        StartCoroutine(coroutine);
        audio_source.Play();
    }

    public void PlayMusic()
    {
        gameObject.SetActive(true);
        Invoke("ReallyPlayMusic", .5f);
    }

    IEnumerator Dance(float seconds)
    {
        while (true)
        {
            Vector3 scale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), 0);
            transform.localScale = scale;
            yield return new WaitForSeconds(seconds);
        }
    }
}
