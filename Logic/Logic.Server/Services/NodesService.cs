using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Dto.Models;
using Logic.Server.Helpers;
using Logic.Server.Models;
using Logic.Server.Repositories;
using Microsoft.Extensions.Logging;

namespace Logic.Server.Services
{
	public class NodesService
	{
		private readonly NodesRepository _nodesRepository;
		private readonly NodesValidator _validator;
		private readonly NodeUpdater _nodeUpdater;
		private readonly ILogger<NodesService> _logger;

		public NodesService(ILogger<NodesService> logger, NodesRepository nodesRepository, NodesValidator validator,
			NodeUpdater nodeUpdater)
		{
			_logger = logger;
			_nodesRepository = nodesRepository;
			_validator = validator;
			_nodeUpdater = nodeUpdater;
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
			_validator.ValidateLinkNodes(srcNode, destNode, userId);
			_logger.LogInformation("Linking nodes {Node1} -> {Node2}", srcNode, destNode);
			_nodesRepository.LinkNodes(srcNode, destNode);
			_nodeUpdater.UpdateChildNodeValue(destNode);
		}

		public IReadOnlyCollection<NodeModel> GetAllUserNodes(string sessionId)
		{
			return _nodesRepository.GetUserNodes(Guid.Parse(sessionId));
		}

		public NodeModel GetUserNode(string nodeName, string sessionId)
		{
			_validator.CheckUserHasNode(nodeName, sessionId);
			return _nodesRepository.GetUserNode(nodeName);
		}

		public IReadOnlyCollection<NodeModel> GetUserNodeSources(string nodeName, string sessionId)
		{
			_validator.CheckUserHasNode(nodeName, sessionId);
			var sources = _nodesRepository.GetUserNodeSources(nodeName);
			return sources.Select(nodeLink => _nodesRepository.GetUserNode(nodeLink.SourceNodeName)).ToList();
		}

		public IReadOnlyCollection<NodeModel> GetUserNodeDestinations(string nodeName, string sessionId)
		{
			_validator.CheckUserHasNode(nodeName, sessionId);
			var destinations = _nodesRepository.GetUserNodeDestinations(nodeName);
			return destinations.Select(nodeLink => _nodesRepository.GetUserNode(nodeLink.DestinationNodeName)).ToList();
		}

		public void UpdateNodeValue(string nodeName, bool? value, string sessionId)
		{
			_validator.CheckUserHasNode(nodeName, sessionId);
			_validator.CheckNodeHasType(nodeName, NodeType.CONST);
			_nodesRepository.UpdateNodeValue(nodeName, value);

			var childNodes = _nodesRepository.GetUserNodeDestinations(nodeName);
			foreach (var node in childNodes)
			{
				_nodeUpdater.UpdateChildNodeValue(node.DestinationNodeName);
			}
		}
	}
}
