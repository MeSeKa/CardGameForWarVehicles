using System.Collections;
using UnityEngine;

public class SeaAdvantage : Advantage
{
	public SeaAdvantage(float amount) : base(amount)
	{
	}

	protected override VehicleType advantageOnVehicleType { get { return VehicleType.Sea; } }

}