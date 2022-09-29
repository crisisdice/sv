using Netcode;

public class LocationWeather : INetObject<NetFields>
{
	public readonly NetInt weatherForTomorrow = new NetInt();

	public readonly NetBool isRaining = new NetBool();

	public readonly NetBool isSnowing = new NetBool();

	public readonly NetBool isLightning = new NetBool();

	public readonly NetBool isDebrisWeather = new NetBool();

	public NetFields NetFields { get; } = new NetFields();


	public LocationWeather()
	{
		NetFields.AddFields(isRaining, isSnowing, isLightning, isDebrisWeather, weatherForTomorrow);
	}

	public void InitializeDayWeather()
	{
		isRaining.Value = false;
		isSnowing.Value = false;
		isLightning.Value = false;
		isDebrisWeather.Value = false;
	}

	public void CopyFrom(LocationWeather other)
	{
		isRaining.Value = other.isRaining.Value;
		isSnowing.Value = other.isSnowing.Value;
		isLightning.Value = other.isLightning.Value;
		isDebrisWeather.Value = other.isDebrisWeather.Value;
		weatherForTomorrow.Value = other.weatherForTomorrow.Value;
	}
}
