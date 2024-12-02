using System.Collections;
using UnityEngine;

public class AirAdvantage : Advantage
{
	public AirAdvantage(float amount) : base(amount)
	{
	}

	protected override VehicleType advantageOnVehicleType { get { return VehicleType.Air; } }
}