using System.Collections;
using UnityEngine;

public class LandAdvantage : Advantage
{
	public LandAdvantage(float amount) : base(amount)
	{
	}

	protected override VehicleType advantageOnVehicleType { get { return VehicleType.Land; } }

}