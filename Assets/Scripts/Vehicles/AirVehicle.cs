using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class AirVehicle : Vehicle
{
	protected AirVehicle(int level = 0) : base(level)
	{
	}

	protected override VehicleType type { get => VehicleType.Air; }

}

