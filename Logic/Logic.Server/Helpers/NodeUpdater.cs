using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Dto.Models;
using Logic.Server.Models;
using Logic.Server.Repositories;

namespace Logic.Server.Helpers
{
	public class NodeUpdater
	{
		private readonly NodesRepository _nodesRepository;

		public NodeUpdater(NodesRepository nodesRepository)
		{
			_nodesRepository = nodesRepository;
		}

		public void UpdateChildNodeValue(string nodeName)
		{
			var childNode = _nodesRepository.GetUserNode(nodeName);
			List<NodeModel> nodeParents;
			switch (childNode.NodeType)
			{
				case NodeType.OR:
					nodeParents = _nodesRepository.GetUserNodeSources(nodeName)
						.Select(node => _nodesRepository.GetUserNode(node.SourceNodeName)).ToList();
					_nodesRepository.UpdateNodeValue(nodeName, CalculateOrNodeValue(nodeParents));
					break;
				case NodeType.AND:
					nodeParents = _nodesRepository.GetUserNodeSources(nodeName)
						.Select(node => _nodesRepository.GetUserNode(node.SourceNodeName)).ToList();
					_nodesRepository.UpdateNodeValue(nodeName, CalculateAndNodeValue(nodeParents));
					break;
				case NodeType.NOT:
					var nodeParent = _nodesRepository.GetUserNodeSources(nodeName)
						.Select(node => _nodesRepository.GetUserNode(node.SourceNodeName)).First();
					_nodesRepository.UpdateNodeValue(nodeName, CalculateNotNodeValue(nodeParent));
					break;
				case NodeType.CONST:
					throw new Exception("Could not update CONST node value through parent");
			}
		}

		private bool? CalculateOrNodeValue(List<NodeModel> parentNodes)
		{
			if (parentNodes.Count < 2)
			{
				return null;
			}

			if (!parentNodes[0].NodeValue.HasValue)
			{
				return null;
			}

			if (!parentNodes[1].NodeValue.HasValue)
			{
				return null;
			}

			return parentNodes[0].NodeValue.Value || parentNodes[1].NodeValue.Value;
		}

		private bool? CalculateAndNodeValue(List<NodeModel> parentNodes)
		{
			if (parentNodes.Count < 2)
			{
				return null;
			}

			if (!parentNodes[0].NodeValue.HasValue)
			{
				return null;
			}

			if (!parentNodes[1].NodeValue.HasValue)
			{
				return null;
			}

			return parentNodes[0].NodeValue.Value && parentNodes[1].NodeValue.Value;
		}

		private bool? CalculateNotNodeValue(NodeModel parentNode)
		{
			if (!parentNode.NodeValue.HasValue)
			{
				return null;
			}

			return !parentNode.NodeValue.Value;
		}
	}
}
