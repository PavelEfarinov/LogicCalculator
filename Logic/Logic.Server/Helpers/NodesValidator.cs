using System;
using Logic.Dto.Models;
using Logic.Server.Repositories;
using Logic.Server.Services;
using Microsoft.Extensions.Logging;

namespace Logic.Server.Helpers
{
	public class NodesValidator
	{
		private readonly NodesRepository _nodesRepository;
		private readonly ILogger<NodesService> _logger;

		public NodesValidator(ILogger<NodesService> logger, NodesRepository nodesRepository)
		{
			_logger = logger;
			_nodesRepository = nodesRepository;
		}

		public void CheckUserHasNode(string nodeName, string sessionId)
		{
			try
			{
				_nodesRepository.GetUserNode(nodeName);
			}
			catch
			{
				throw new Exception($"{sessionId} has no node named {nodeName}");
			}
		}

		public void CheckNodeHasType(string nodeName, NodeType type)
		{
			var node = _nodesRepository.GetUserNode(nodeName);
			if (node.NodeType != type)
			{
				throw new Exception($"{node} has has type different from {type.ToString()}.");
			}
		}


		public void ValidateLinkNodes(string srcNode, string destNode, string userId)
		{
			CheckUserHasNode(srcNode, userId);
			CheckUserHasNode(destNode, userId);

			if (srcNode.Equals(destNode))
			{
				throw new Exception("Source and destination node should be different");
			}

			var destNodeModel = _nodesRepository.GetUserNode(destNode);

			switch (destNodeModel.NodeType)
			{
				case NodeType.CONST:
					throw new Exception("Count not link any node into CONST node");
				case NodeType.OR:
					if (_nodesRepository.GetUserNodeSources(destNode).Count >= 2)
					{
						throw new Exception("Count not add new link to OR node");
					}

					break;
				case NodeType.AND:
					if (_nodesRepository.GetUserNodeSources(destNode).Count >= 2)
					{
						throw new Exception("Count not add new link to AND node");
					}

					break;
				case NodeType.NOT:
					if (_nodesRepository.GetUserNodeSources(destNode).Count >= 1)
					{
						throw new Exception("Count not add new link to NOT node");
					}

					break;
				default:
					throw new InvalidCastException(
						$"Could not get proper NodeType from {destNodeModel.NodeType.ToString()}");
			}
		}
	}
}
