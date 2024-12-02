using TMPro;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
	private Player player;

	[SerializeField] private TextMeshProUGUI IDField;

	[SerializeField] private TextMeshProUGUI nameField;

	[SerializeField] private TextMeshProUGUI scoreField;

	[SerializeField] private Transform possibleCardsTranformParent;

	[SerializeField] private Transform handTransformParent;

	[SerializeField] private Transform tableTransform;

	public Player Player { get => player; set => player = value; }
	public Transform HandTransformParent { get => handTransformParent; set => handTransformParent = value; }
	public Transform TableTransform { get => tableTransform; set => tableTransform = value; }
	public Transform PossibleCardsTranformParent { get => possibleCardsTranformParent; set => possibleCardsTranformParent = value; }

	public void SetPlayer(Player player)
	{
		this.player = player;
		ShowPlayerStats();
	}

	public void ShowPlayerStats()
	{
		IDField.text = player.PlayerID.ToString();
		nameField.text = player.PlayerName;
		UpdateScore();
	}

	public void UpdateScore()
	{
		scoreField.text = "Score: " + player.Score.ToString();
	}

}
