using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoLight : MonoBehaviour
{
    // Start is called before the first frame update
    private bool flicker_mode;
    private Light light_component;
    private IEnumerator coroutine;

    void Start()
    {
        flicker_mode = false;
        light_component = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn()
    {
        flicker_mode = true;
        coroutine = Flicker(0.48f);

        if (flicker_mode)
        {
            StartCoroutine(coroutine);
        }
    }

    IEnumerator Flicker(float seconds)
    {
        while (flicker_mode)
        {
            light_component.color = new Color(Random.value, Random.value, Random.value);
            yield return new WaitForSeconds(seconds);
        }
    }
}
