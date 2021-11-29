using System;

namespace Logic.Server.Models
{
	public class UserSession : BaseModel
	{
		public Guid SessionId { get; set; }
		public DateTimeOffset LastLoginTime { get; set; }
	}
}
