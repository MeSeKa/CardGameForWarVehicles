using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static bool CanDrag { get => canDrag; }
	static bool canDrag = false;
	public static void SetCanDrag(bool set)
	{
		canDrag = set;
	}

	Transform previousParent = null;


	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!canDrag) return;
        
        previousParent = transform.parent;
		transform.SetParent(GameManager.instance.Canvas);

		GetComponent<CanvasGroup>().blocksRaycasts = false; // Kart diğer alanlarla düzgün etkileşime geçsin
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!canDrag) return;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			transform.parent as RectTransform, // Ana container'ın RectTransform'u
			eventData.position, // Fare pozisyonu
			eventData.pressEventCamera, // Oyun kamerası
			out Vector2 localPoint // Hesaplanan yerel pozisyon
		);

		transform.localPosition = localPoint; // Kartı yeni pozisyona taşı
	}


	public void OnEndDrag(PointerEventData eventData)
	{
		if (!canDrag) return;

		GetComponent<CanvasGroup>().blocksRaycasts = true;

		if (transform.parent == GameManager.instance.Canvas) // Alan bulunmazsa geri dön
		{
			transform.SetParent(previousParent);
			transform.localPosition = Vector3.zero;
		}
	}
}
