using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KFS : LandVehicle
{
	public KFS(int level = 0) : base(level)
	{
		this.durability = maxDurability;
		base.Advantages.Add(new SeaAdvantage(10));
		base.Advantages.Add(new AirAdvantage(20));
	}

	public override float maxDurability { get => 10; }
	public override float attack { get => 10; }
	public override string title { get => "Kara Füze Sistemi"; }

	protected override SubVehicleType subVehicleType => SubVehicleType.KFS;


}

