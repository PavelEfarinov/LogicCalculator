using System;
using Logic.Dto.Models;

namespace LogicServer.Models
{
	public class NodeModel : BaseModel
	{
		public string NodeName { get; set; }
		public Guid UserId { get; set; }
		public NodeType NodeType { get; set; }
	}
}
