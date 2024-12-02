using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
	BattleField battleField;
	private void Start()
	{
		battleField = GetComponent<BattleField>();
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (transform.childCount == 0) // Alan boşsa kartı yerleştir
		{
			Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
			if (draggable != null && canPlay(draggable))
			{
				battleField.Card = draggable.GetComponent<Card>();
				battleField.Card.SetAtBattleField(true);

				draggable.transform.SetParent(transform);
				draggable.transform.localPosition = Vector3.zero;
				((RectTransform)draggable.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Card.normalCardSize.y);
				((RectTransform)draggable.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Card.normalCardSize.x);
			}
		}
		else
		{
			Debug.Log("Bu alan dolu!");
		}
	}

	bool canPlay(Draggable draggable)
	{
		Card card = draggable.GetComponent<Card>();
		return GameManager.instance.PlayerHolder.Player.CanPlayThisCard(card);
	}
}
