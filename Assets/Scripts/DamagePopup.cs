
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class DamagePopup : MonoBehaviour
{
	// Create a Damage Popup
	public static DamagePopup Create(Vector3 position, string text, bool isCriticalHit)
	{
		Transform damagePopupTransform = Instantiate(GameManager.instance.PopUpText, position, Quaternion.identity, GameManager.instance.Canvas);

		DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
		damagePopup.Setup(text, isCriticalHit);

		return damagePopup;
	}

	private static int sortingOrder;

	private const float DISAPPEAR_TIMER_MAX = 1f;

	private TextMeshProUGUI textMesh;
	private float disappearTimer;
	private Color textColor;
	private Vector3 moveVector;

	private void Awake()
	{
		textMesh = transform.GetComponent<TextMeshProUGUI>();
	}

	public void Setup(string text, bool isCriticalHit)
	{
		textMesh.SetText(text);
		if (!isCriticalHit)
		{
			// Normal hit
			textMesh.fontSize = 36;
			textColor = Color.yellow;
		}
		else
		{
			// Critical hit
			textMesh.fontSize = 50;
			textColor = Color.red;
		}
		textMesh.color = textColor;
		disappearTimer = DISAPPEAR_TIMER_MAX;

		moveVector = new Vector3(.7f, 1) * 60f;

	}

	private void Update()
	{
		transform.position += moveVector * Time.deltaTime;
		moveVector -= moveVector * 8f * Time.deltaTime;

		if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
		{
			// First half of the popup lifetime
			float increaseScaleAmount = 1f;
			transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
		}
		else
		{
			// Second half of the popup lifetime
			float decreaseScaleAmount = 1f;
			transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
		}

		disappearTimer -= Time.deltaTime;
		if (disappearTimer < 0)
		{
			// Start disappearing
			float disappearSpeed = 3f;
			textColor.a -= disappearSpeed * Time.deltaTime;
			textMesh.color = textColor;
			if (textColor.a < 0)
			{
				Destroy(gameObject);
			}
		}
	}

}
