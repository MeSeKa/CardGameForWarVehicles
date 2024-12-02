using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Firkateyn : SeaVehicle
{
	public Firkateyn(int level = 0) : base(level)
	{
		this.durability = maxDurability;
		base.Advantages.Add(new AirAdvantage(10));
		base.Advantages.Add(new LandAdvantage(10));
	}

	public override float maxDurability { get => 15; }
	public override float attack { get => 10; }
	public override string title { get => "Firkateyn"; }

	protected override SubVehicleType subVehicleType => SubVehicleType.Firkateyn;


}

