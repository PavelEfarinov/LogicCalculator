using System;
using Logic.Dto.Models;

namespace Logic.Server.Models
{
	public class NodeModel : BaseModel
	{
		public string NodeName { get; set; }
		public bool? NodeValue { get; set; }
		public Guid UserId { get; set; }
		public NodeType NodeType { get; set; }
	}
}
