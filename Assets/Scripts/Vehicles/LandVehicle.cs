using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class LandVehicle : Vehicle
{
	protected override VehicleType type { get => VehicleType.Land; }

	protected LandVehicle(int level = 0) : base(level)
	{
	}
}

