using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public abstract class Vehicle : ICloneable
{
	protected int level = 0;
	public int Level => level;
	public abstract float maxDurability { get; }
	public abstract float attack { get; }
	public abstract string title { get; }

	protected abstract VehicleType type { get; }

	protected abstract SubVehicleType subVehicleType { get; }
	public List<Advantage> Advantages { get => advantages; private set => advantages = value; }

	private List<Advantage> advantages;

	public float durability { get; protected set; }

	protected Vehicle(int level = 0)
	{
		this.level = level;
		advantages = new List<Advantage>();
	}

	public string ShowDetails()
	{
		if (durability == 0) return this.title + " is dead.";
		return "Durability:" + durability + "\nLevel:" + level + "\n";
	}
	public float GiveDamage(Vehicle damageable)
	{
		float damage = attack;

		foreach (var advantage in advantages)
		{
			damage += advantage.CalculateAdvantage(damageable.type);
		}

		damageable.TakeDamage(damage);  //Give damage
		Debug.Log(damageable.title + " took " + damage + " damage.");

		if (damageable.IsDead())// if kill add level
		{
			if (damageable.level <= 10) level += 10;
			else level += damageable.level;
		}

		return damage;//Return the total damage if necessary
	}
	public void TakeDamage(float amount)
	{
		this.durability -= amount;
		durability = durability < 0f ? 0f : durability;
		ShowDetails();
	}

	public bool IsDead()
	{
		return durability == 0f;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public VehicleType GetVehicleType() { return type; }

	public SubVehicleType GetSubVehicleType() { return subVehicleType; }
}


public enum VehicleType
{
	NONE,
	Land,
	Air,
	Sea,
}

public enum SubVehicleType
{
	Ucak,
	Siha,
	Obus,
	KFS,
	Firkateyn,
	Sida
}
