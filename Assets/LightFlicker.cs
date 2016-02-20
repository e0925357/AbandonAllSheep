using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float initialAlpha;
    private float random;

	// Use this for initialization
	void Start ()
	{
	    spriteRenderer = GetComponent<SpriteRenderer>();
	    initialAlpha = spriteRenderer.color.a;
        random = Random.Range(0.0f, 65535.0f);
    }
	
	// Update is called once per frame
	void Update ()
	{
        Color color = spriteRenderer.color;
        float noise = Mathf.PerlinNoise(random, Time.time);
        color.a = initialAlpha * noise;
        spriteRenderer.color = color;
	}
}
