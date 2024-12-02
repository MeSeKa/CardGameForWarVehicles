using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	private bool haveFought = false;
	private bool atBattleField = false;
	public void SetAtBattleField(bool set)
	{
		atBattleField = set;
	}

	public static Vector2 normalCardSize = new Vector2(125, 175);
	static Color foughtColor = Color.gray;
	static Color notfoughtColor = Color.white;

	[SerializeField] private TextMeshProUGUI cardNameField;

	[SerializeField] private TextMeshProUGUI levelField;

	[SerializeField] private TextMeshProUGUI durabilityField;

	[SerializeField] private TextMeshProUGUI attackField;

	[SerializeField] Image background;

	[SerializeField] Image vehicleTypeIcon;

	[SerializeField] Image subVehicleTypeIcon;

	private Vehicle vehicle;


	public Vehicle Vehicle { get => vehicle; }
	public bool HaveFought { get => haveFought; set => haveFought = value; }
	public bool AtBattleField { get => atBattleField; set => atBattleField = value; }

	public void SetVehicle(Vehicle vehicle)
	{
		this.vehicle = vehicle;
		background.sprite = AssetHolder.Instance.GetVehicleTypeBackGround(vehicle.GetVehicleType());
		vehicleTypeIcon.sprite = AssetHolder.Instance.GetVehicleTypeIcon(vehicle.GetVehicleType());
		subVehicleTypeIcon.sprite = AssetHolder.Instance.GetVehicleTypeIcon(vehicle.GetSubVehicleType());
	}

	public void ShowCard()
	{
		cardNameField.text = vehicle.title;
		levelField.text = vehicle.Level.ToString();
		durabilityField.text = vehicle.durability.ToString();
		attackField.text = vehicle.attack.ToString();
	}

	public void SetFought(bool set)
	{
		haveFought = set;
		GetComponent<Image>().color = set ? foughtColor : notfoughtColor;
	}


}
