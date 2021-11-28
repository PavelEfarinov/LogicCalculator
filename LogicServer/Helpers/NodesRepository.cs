using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using DbConnector;
using Microsoft.Extensions.Logging;
using LogicServer.Models;

namespace LogicServer.Helpers
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
			var result = connection.Execute(query,
				new {Name = node.NodeName, UserId = node.UserId, Type = node.NodeType.ToString()});
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
			_logger.LogInformation("Nodes linked");
		}

		public IReadOnlyCollection<NodeModel> GetUserNodes(Guid sessionId)
		{
			_logger.LogInformation("Getting all nodes for {User}", sessionId);
			using var connection = _connector.GetConnection();
			var query = $@"SELECT user_id UserId, node_name NodeName, node_type NodeType FROM {nodesTable} WHERE user_id = @SessionId";
			connection.Open();
			var result = connection.Query<NodeModel>(query,
				new {SessionId = sessionId});
			var list = result.ToList();
			_logger.LogInformation("Got {Count} objects", list.Count);
			return list;
		}

		public NodeModel GetUserNode(string nodeName)
		{
			_logger.LogInformation("Getting {Node} node", nodeName);
			using var connection = _connector.GetConnection();
			var query = $@"SELECT user_id UserId, node_name NodeName, node_type NodeType FROM {nodesTable} WHERE node_name = @NodeName";
			connection.Open();
			var result = connection.QuerySingle<NodeModel>(query,
				new {NodeName = nodeName});
			_logger.LogInformation("Got {Node} node", result);
			return result;
		}

		public IReadOnlyCollection<NodeLinkModel> GetUserNodeLinks(string nodeName)
		{
			_logger.LogInformation("Getting {Node} node", nodeName);
			using var connection = _connector.GetConnection();
			var query = $@"SELECT node_name_src SourceNodeName, node_name_dest DestinationNodeName FROM {nodesLinksTable} WHERE node_name_src = @NodeName OR node_name_dest = @NodeName";
			connection.Open();
			var result = connection.Query<NodeLinkModel>(query,
				new {NodeName = nodeName}).ToList();
			_logger.LogInformation("Got {Node} node", string.Join(' ', result));
			return result;
		}
	}
}
