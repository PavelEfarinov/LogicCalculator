using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Dto.Models;
using LogicServer.Helpers;
using LogicServer.Models;
using Microsoft.Extensions.Logging;

namespace LogicServer.Services
{
	public class NodesService
	{
		private readonly NodesRepository _nodesRepository;
		private readonly ILogger<NodesService> _logger;

		public NodesService(ILogger<NodesService> logger, NodesRepository nodesRepository)
		{
			_logger = logger;
			_nodesRepository = nodesRepository;
		}

		private void ValidateLinkNodes(string srcNode, string destNode, string userId)
		{
			var userNodeNames = GetAllUserNodes(userId).Select(node => node.NodeName).ToList();
			if (!userNodeNames.Contains(srcNode) || !userNodeNames.Contains(destNode))
			{
				throw new Exception($"{userId} has no nodes named {srcNode} or {destNode}");
			}

			if (srcNode.Equals(destNode))
			{
				throw new Exception("Source and destination node should be different");
			}

			var destNodeModel = _nodesRepository.GetUserNode(destNode);

			switch (destNodeModel.NodeType)
			{
				case NodeType.CONST:
					throw new Exception("Count not link node int CONST node");
				case NodeType.OR:
					if (_nodesRepository.GetUserNodeLinks(destNode).Count >= 2)
					{
						throw new Exception("Count not add new link to OR node");
					}

					break;
				case NodeType.AND:
					if (_nodesRepository.GetUserNodeLinks(destNode).Count >= 2)
					{
						throw new Exception("Count not add new link to AND node");
					}

					break;
				case NodeType.NOT:
					if (_nodesRepository.GetUserNodeLinks(destNode).Count >= 1)
					{
						throw new Exception("Count not add new link to NOT node");
					}

					break;
				default:
					throw new InvalidCastException(
						$"Could not get proper NodeType from {destNodeModel.NodeType.ToString()}");
			}
		}


		public void AddNewNode(NodeDto node)
		{
			_logger.LogInformation("Creating new node for user {SessionId}", node.SessionId);
			_nodesRepository.CreateNewNode(new NodeModel()
			{
				NodeName = node.NodeName,
				UserId = Guid.Parse(node.SessionId),
				NodeType = node.NodeType,
			});
		}

		public void LinkNodes(string srcNode, string destNode, string userId)
		{
			ValidateLinkNodes(srcNode, destNode, userId);
			_logger.LogInformation("Linking nodes {Node1} -> {Node2}", srcNode, destNode);
			_nodesRepository.LinkNodes(srcNode, destNode);
		}

		public IReadOnlyCollection<NodeModel> GetAllUserNodes(string sessionId)
		{
			return _nodesRepository.GetUserNodes(Guid.Parse(sessionId));
		}
	}
}
