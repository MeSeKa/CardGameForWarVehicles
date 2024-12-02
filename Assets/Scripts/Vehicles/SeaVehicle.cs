using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class SeaVehicle : Vehicle
{
	protected override VehicleType type { get => VehicleType.Sea; }

	protected SeaVehicle(int level = 0) : base(level)
	{

	}
}

