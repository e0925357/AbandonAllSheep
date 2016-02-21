using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float random;

	// Use this for initialization
	void Start ()
	{
	    spriteRenderer = GetComponent<SpriteRenderer>();
        random = Random.Range(0.0f, 65535.0f);
    }
	
	// Update is called once per frame
	void Update ()
	{
        Color color = spriteRenderer.color;
        float noise = Mathf.PerlinNoise(random, Time.time);
        color.a = Mathf.Lerp(0.1f, 0.3f, noise); 
        spriteRenderer.color = color;
	}
}
