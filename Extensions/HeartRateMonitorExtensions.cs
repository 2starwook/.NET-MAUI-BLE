namespace BluetoothCourse.Extensions;

public static class HeartRateMonitorExtensions {
	// Analyze the BLE data
	const byte HEART_RATE_VALUE_FORMAT = 0x01;
	const byte ENERGY_EXPANDED_STATUS = 0x08;

	public static uint DecodeHeartRate(this byte[] data) {
		byte currentOffset = 0;
		byte flags = data[currentOffset];
		bool isHeartRateValueSizeLong = (flags & HEART_RATE_VALUE_FORMAT) != 0;
		bool hasEnergyExpended = (flags & ENERGY_EXPANDED_STATUS) != 0;

		currentOffset++;

		ushort heartRateMeasurementValue = 0;

		if (isHeartRateValueSizeLong)
		{
			heartRateMeasurementValue = (ushort)((data[currentOffset + 1] << 8) + data[currentOffset]);
			currentOffset += 2;
		}
		else
		{
		heartRateMeasurementValue = data[currentOffset];
		currentOffset++;
		}

		ushort expendedEnergyValue = 0;

		if (hasEnergyExpended)
		{
		expendedEnergyValue = (ushort)((data[currentOffset + 1] << 8) + data[currentOffset]);
		currentOffset += 2;
		}

		return heartRateMeasurementValue;
	}
}