using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories
{
	public abstract class AbstractRepository : IDisposable
	{
		private readonly IDbConnection m_connection;
		private bool m_disposed;

		protected AbstractRepository(IDbConnection connection)
		{
			this.m_connection = connection;
			this.m_disposed = false;
		}

		protected async Task<TValue> QuerySingleAsync<TValue>(string sproc, params object[] args)
		{
			this.VerifyDisposed();
			this.VerifyConnection();

			var @params = this.buildParameterBag(args);
			return await this.m_connection.QueryFirstOrDefaultAsync<TValue>(sproc, @params, commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);
		}

		protected async Task ExecuteAsync(string sproc, params object[] args)
		{
			this.VerifyDisposed();
			this.VerifyConnection();

			var @params = this.buildParameterBag(args);
			var result = await this.m_connection.ExecuteAsync(sproc, @params, commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);

			if(result < 1) {
				throw new InvalidOperationException($"Unable to insert data point to sensor: {args[1]}");
			}
		}

		protected async Task<IEnumerable<TValue>> QueryAsync<TValue>(string sproc, params object[] args)
		{
			this.VerifyDisposed();
			this.VerifyConnection();

			var @params = this.buildParameterBag(args);
			var result = await this.m_connection.QueryAsync<TValue>(sproc, @params, commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);

			return result.ToList();
		}

		private DynamicParameters buildParameterBag(params object[] args)
		{
			var bag = new DynamicParameters();

			for(var idx = 0; idx < args.Length; idx += 2) {
				var key = args[idx] as string;
				var value = args[idx + 1];

				bag.Add(key, value);
			}

			return bag;
		}

		private void VerifyConnection()
		{
			if(this.m_connection.State == ConnectionState.Open) {
				return;
			}

			this.m_connection.Open();
		}

		private void VerifyDisposed()
		{
			if(this.m_disposed) {
				throw new ObjectDisposedException(nameof(AbstractRepository));
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing) {
				this.m_connection.Dispose();
			}

			this.m_disposed = true;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
