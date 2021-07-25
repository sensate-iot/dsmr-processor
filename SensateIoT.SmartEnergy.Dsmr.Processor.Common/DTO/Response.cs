using System;
using System.Collections.Generic;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.DTO
{
	public class Response<TValue>
	{
		public Guid ResponseId { get; set; }
		public IList<string> Errors { get; set; }
		public TValue Data { get; set; }

		public Response()
		{
			this.ResponseId = Guid.NewGuid();
		}

		public Response(TValue value) : this()
		{
			this.Data = value;
		}

		public void AddError(string error)
		{
			if(this.Errors == null) {
				this.Errors = new List<string>();
			}

			this.Errors.Add(error);
		}
	}
}