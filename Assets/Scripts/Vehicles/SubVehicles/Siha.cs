using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Siha : AirVehicle
{
	public Siha(int level = 0) : base(level)
	{
		this.durability = maxDurability;
		base.Advantages.Add(new LandAdvantage(10));
		base.Advantages.Add(new SeaAdvantage(10));
	}

	public override float maxDurability { get => 15; }
	public override float attack { get => 10; }
	public override string title { get => "Siha"; }

	protected override SubVehicleType subVehicleType => SubVehicleType.Siha;


}

