using System.Collections.Generic;
using UnityEngine;

public class AssetHolder : MonoBehaviour
{
	public static AssetHolder Instance { get { return instance; } }
	static AssetHolder instance;
	private void Awake()
	{
		instance = this;
	}

	[SerializeField] private List<SubVehicleIcon> subVehicleTypes;
	[SerializeField] private List<VehicleTypeIcon> vehicleTypes;

	public Sprite GetVehicleTypeIcon(VehicleType type)
	{
		foreach (var item in vehicleTypes)
		{
			if (item.Type == type)
			{
				return item.Icon;
			}
		}

		return null;
	}

	public Sprite GetVehicleTypeBackGround(VehicleType type)
	{
		foreach (var item in vehicleTypes)
		{
			if (item.Type == type)
			{
				return item.BackGround;
			}
		}
		return null;
	}

	public Sprite GetVehicleTypeIcon(SubVehicleType type)
	{
		foreach (var item in subVehicleTypes)
		{
			if (item.Type == type)
			{
				return item.Icon;
			}
		}

		return null;
	}

	[System.Serializable]
	public class SubVehicleIcon
	{
		public SubVehicleType Type;
		public Sprite Icon;
	}

	[System.Serializable]
	public class VehicleTypeIcon
	{
		public VehicleType Type;
		public Sprite Icon;
		public Sprite BackGround;
	}
}
