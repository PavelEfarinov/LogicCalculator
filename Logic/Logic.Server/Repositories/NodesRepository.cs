using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Db.Connector;
using Logic.Server.Models;
using Microsoft.Extensions.Logging;

namespace Logic.Server.Repositories
{
	public class NodesRepository
	{
		private readonly ILogger<NodesRepository> _logger;
		private readonly DatabaseConnector _connector;
		private readonly string nodesTable = "logic_nodes";
		private readonly string nodesLinksTable = "logic_nodes_links";

		public NodesRepository(ILogger<NodesRepository> logger, DatabaseConnector connector)
		{
			_logger = logger;
			_connector = connector;
		}

		public void CreateNewNode(NodeModel node)
		{
			_logger.LogInformation("Creating new node {Node}", node.ToString());
			using var connection = _connector.GetConnection();
			var query = $@"INSERT INTO {nodesTable} (node_name, user_id, node_type) VALUES (@Name, @UserId, @Type)";
			connection.Open();
			connection.Execute(query,
				new {Name = node.NodeName, UserId = node.UserId, Type = node.NodeType.ToString()});
			connection.Close();
			_logger.LogInformation("Created new node with name {Name}", node.NodeName);
		}

		public void LinkNodes(string sourceNode, string destinationNode)
		{
			_logger.LogInformation("Linking nodes {Node1} -> {Node2}", sourceNode, destinationNode);
			using var connection = _connector.GetConnection();
			var query = $@"INSERT INTO {nodesLinksTable} (node_name_src, node_name_dest) VALUES (@SrcNode, @DestNode)";
			connection.Open();
			var result = connection.Execute(query,
				new {SrcNode = sourceNode, DestNode = destinationNode});
			connection.Close();
			_logger.LogInformation("Nodes linked");
		}

		public IReadOnlyCollection<NodeModel> GetUserNodes(Guid sessionId)
		{
			_logger.LogInformation("Getting all nodes for {User}", sessionId);
			using var connection = _connector.GetConnection();
			var query =
				$@"SELECT user_id UserId, node_name NodeName, node_type NodeType, node_value NodeValue FROM {nodesTable} WHERE user_id = @SessionId";
			connection.Open();
			var result = connection.Query<NodeModel>(query,
				new {SessionId = sessionId});
			var list = result.ToList();
			_logger.LogInformation("Got {Count} objects", list.Count);
			connection.Close();
			return list;
		}

		public IReadOnlyCollection<NodeLinkModel> GetUserNodeSources(string nodeName)
		{
			_logger.LogInformation("Getting all source nodes for {Node}", nodeName);
			using var connection = _connector.GetConnection();
			var query =
				$@"SELECT node_name_src SourceNodeName, node_name_dest DestinationNodeName FROM {nodesLinksTable} WHERE node_name_dest = @NodeName";
			connection.Open();
			var result = connection.Query<NodeLinkModel>(query,
				new {NodeName = nodeName});
			var list = result.ToList();
			_logger.LogInformation("Got {Count} objects", list.Count);
			connection.Close();
			return list;
		}

		public IReadOnlyCollection<NodeLinkModel> GetUserNodeDestinations(string nodeName)
		{
			_logger.LogInformation("Getting all destination nodes for {Node}", nodeName);
			using var connection = _connector.GetConnection();
			var query =
				$@"SELECT node_name_src SourceNodeName, node_name_dest DestinationNodeName FROM {nodesLinksTable} WHERE node_name_src = @NodeName";
			connection.Open();
			var result = connection.Query<NodeLinkModel>(query,
				new {NodeName = nodeName});
			var list = result.ToList();
			_logger.LogInformation("Got {Count} objects", list.Count);
			connection.Close();
			return list;
		}

		public NodeModel GetUserNode(string nodeName)
		{
			_logger.LogInformation("Getting {Node} node", nodeName);
			using var connection = _connector.GetConnection();
			var query =
				$@"SELECT user_id UserId, node_value NodeValue, node_name NodeName, node_type NodeType, node_value NodeValue FROM {nodesTable} WHERE node_name = @NodeName";
			connection.Open();
			var result = connection.QuerySingle<NodeModel>(query,
				new {NodeName = nodeName});
			_logger.LogInformation("Got {Node} node", result);
			connection.Close();
			return result;
		}

		public IReadOnlyCollection<NodeLinkModel> GetUserNodeLinks(string nodeName)
		{
			_logger.LogInformation("Getting {Node} node links", nodeName);
			using var connection = _connector.GetConnection();
			var query =
				$@"SELECT node_name_src SourceNodeName, node_name_dest DestinationNodeName FROM {nodesLinksTable} WHERE node_name_src = @NodeName OR node_name_dest = @NodeName";
			connection.Open();
			var result = connection.Query<NodeLinkModel>(query,
				new {NodeName = nodeName}).ToList();
			_logger.LogInformation("Got {Node} node links", string.Join(' ', result));
			connection.Close();
			return result;
		}

		public void UpdateNodeValue(string nodeName, bool? newValue)
		{
			_logger.LogInformation("Updating {Node} value to {Value}", nodeName, newValue);
			using var connection = _connector.GetConnection();
			var query = $@"UPDATE {nodesTable} SET node_value = @NewValue WHERE node_name = @NodeName";
			connection.Open();
			connection.Execute(query,
				new {NodeName = nodeName, NewValue = newValue});
			connection.Close();
			_logger.LogInformation("Updated");
		}
	}
}
