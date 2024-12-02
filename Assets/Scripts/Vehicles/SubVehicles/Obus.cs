using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Obus : LandVehicle
{
	public Obus(int level = 0) : base(level)
	{
		this.durability = maxDurability;
		base.Advantages.Add(new SeaAdvantage(5));
	}

	public override float maxDurability { get => 20; }
	public override float attack { get => 10; }
	public override string title { get => "Obüs"; }

	protected override SubVehicleType subVehicleType => SubVehicleType.Obus;


}

