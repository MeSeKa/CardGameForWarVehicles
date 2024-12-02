using UnityEngine;

public class BattleField : MonoBehaviour
{
    DropZone dropZone;
    Card card;

	private void Start()
	{
		dropZone = GetComponent<DropZone>();
	}

	public Card Card { get => card; set => card = value; }
	public DropZone DropZone { get => dropZone; set => dropZone = value; }
}
