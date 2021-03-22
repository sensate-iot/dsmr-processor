using System;
using System.Collections.Generic;
using System.Linq;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Logic
{
	public static class DataCalculator
	{
		private const string InstantaneousPowerProduction = "InstantaneousPowerProduction";
		private const string InstantaneousPowerUsage = "InstantaneousPowerUsage";
		private const string EnergyConsumptionTariff1 = "EnergyConsumptionTariff1";
		private const string EnergyConsumptionTariff2 = "EnergyConsumptionTariff2";
		private const string EnergyProductionTariff1 = "EnergyProductionTariff1";
		private const string EnergyProductionTariff2 = "EnergyProductionTariff2";
		private const string GasConsumption = "GasConsumption";
		private const string GasFlow = "GasFlow";
		private const string Temperature = "temperature";
		private const string Pressure = "pressure";
		private const string RelativeHumidity = "rh";

		public static void ComputeEnvironmentAverages(IDictionary<DateTime, DataPoint> output, IEnumerable<Measurement> measurements)
		{
			var groups = measurements.GroupBy(g => new {
				Timestamp = new DateTime(g.Timestamp.Year, g.Timestamp.Month, g.Timestamp.Day, g.Timestamp.Hour,
				                         g.Timestamp.Minute, 0, DateTimeKind.Utc)
			}).Select(x => new {
				x.Key,
				Measurements = x.ToList()
			});

			foreach(var group in groups) {
				if(!output.TryGetValue(group.Key.Timestamp, out var dp)) {
					continue;
				}

				var temperature = 0M;
				var rh = 0M;
				var pressure = 0M;

				foreach(var measurement in group.Measurements) {
					temperature += measurement.Data[Temperature].Value;
					rh += measurement.Data[RelativeHumidity].Value;
					pressure += measurement.Data[Pressure].Value;
				}

				temperature /= group.Measurements.Count;
				rh /= group.Measurements.Count;
				pressure /= group.Measurements.Count;

				dp.RelativeHumidity = rh;
				dp.Pressure = pressure;
				dp.Temperature = temperature;

				// TODO: remove hardcoded
				dp.OutsideAirTemperature = 7.2M;
			}
		}

		public static void ComputeGasAverages(IDictionary<DateTime, DataPoint> output, IEnumerable<Measurement> measurements)
		{
			var groups = measurements.GroupBy(g => new {
				Timestamp = new DateTime(g.Timestamp.Year, g.Timestamp.Month, g.Timestamp.Day, g.Timestamp.Hour,
				                         g.Timestamp.Minute, 0, DateTimeKind.Utc)
			}).Select(x => new {
				x.Key,
				Measurements = x.ToList()
			});

			foreach(var group in groups) {

				if(!output.TryGetValue(group.Key.Timestamp, out var dp)) {
					continue;
				}

				var gasUsage = group.Measurements[group.Measurements.Count - 1];
				var gasFlow = group.Measurements.Average(measurement => measurement.Data[GasFlow].Value);

				dp.GasFlow = gasFlow;
				dp.GasUsage = gasUsage.Data[GasConsumption].Value;
			}
		}

		public static IDictionary<DateTime, DataPoint> ComputePowerAverages(SensorMapping mapping, IEnumerable<Measurement> measurements)
		{
			var groups = measurements.GroupBy(g => new {
				Timestamp = new DateTime(g.Timestamp.Year, g.Timestamp.Month, g.Timestamp.Day, g.Timestamp.Hour,
				                         g.Timestamp.Minute, 0, DateTimeKind.Utc)
			}).Select(x => new {
				x.Key,
				Measurements = x.ToList()
			});

			var datapoints = new SortedDictionary<DateTime, DataPoint>();

			foreach(var group in groups) {
				var production = 0M;
				var consumption = 0M;
				var energyUsed = 0M;
				var energyProduced = 0M;

				foreach(var measurement in group.Measurements) {
					production += measurement.Data[InstantaneousPowerProduction].Value;
					consumption = measurement.Data[InstantaneousPowerUsage].Value;
					energyUsed = measurement.Data[EnergyConsumptionTariff1].Value +
					             measurement.Data[EnergyConsumptionTariff2].Value;
					energyProduced = measurement.Data[EnergyProductionTariff1].Value +
					                 measurement.Data[EnergyProductionTariff2].Value;
				}

				production /= group.Measurements.Count;
				consumption /= group.Measurements.Count;
				energyUsed /= group.Measurements.Count;
				energyProduced /= group.Measurements.Count;

				datapoints.Add(group.Key.Timestamp, new DataPoint {
					PowerProduction = production,
					PowerUsage = consumption,
					EnergyProduction = energyProduced,
					EnergyUsage = energyUsed,
					Timestamp = group.Key.Timestamp,
					SensorId = mapping.SensorId
				});
			}

			return datapoints;
		}
	}
}
