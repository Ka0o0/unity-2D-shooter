using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class HealthBarHelper : MonoBehaviour
{

	public float CurrentHpPercentage = 100;
	
	// Images
	public Sprite FullSprite;
	public Sprite ThreeQuaterSprite;
	public Sprite HalfSprite;
	public Sprite OneQuaterSprite;

	void Update ()
	{
		var spriteRenderer = GetComponent<SpriteRenderer>();

		if (CurrentHpPercentage > 95)
		{
			spriteRenderer.sprite = FullSprite;
		} else if (CurrentHpPercentage > 70)
		{
			spriteRenderer.sprite = ThreeQuaterSprite;
		} else if (CurrentHpPercentage > 35)
		{
			spriteRenderer.sprite = HalfSprite;
		}
		else
		{
			spriteRenderer.sprite = OneQuaterSprite;
		}
	}
}
