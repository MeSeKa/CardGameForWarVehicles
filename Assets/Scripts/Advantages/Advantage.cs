using System.Collections;
using UnityEngine;


public abstract class Advantage
{
	protected abstract VehicleType advantageOnVehicleType { get; }
	protected float amount;

	public bool hasAdvantageOn(VehicleType vehicleType)
	{
		return advantageOnVehicleType == vehicleType;
	}

	public float CalculateAdvantage(VehicleType vehicleType)
	{
		return hasAdvantageOn(vehicleType) ? amount : 0;
	}

	protected Advantage(float amount)
	{
		this.amount = amount;
	}
}
