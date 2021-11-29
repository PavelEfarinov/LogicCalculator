using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Logic.Dto.Models;
using Logic.Proto;
using Logic.Utils;
using Microsoft.Extensions.Logging;
using NodeType = Logic.Dto.Models.NodeType;

namespace Logic.Server.Services.Grpc
{
	public class NodesConnectorService : NodesConnector.NodesConnectorBase
	{
		private readonly ILogger<NodesConnectorService> _logger;
		private readonly NodesService _nodesService;

		public NodesConnectorService(ILogger<NodesConnectorService> logger, NodesService nodesService)
		{
			_logger = logger;
			_nodesService = nodesService;
		}

		public override Task<NodeConnectionReply> LinkNodes(LinkNodeRequest request, ServerCallContext context)
		{
			try
			{
				_nodesService.LinkNodes(request.SourceNodeName, request.DestinationNodeName, request.SessionId);
				return Task.FromResult(new NodeConnectionReply()
					{
						Success = true,
						ErrorMessage = ""
					}
				);
			}
			catch (Exception e)
			{
				_logger.LogError(message: e.ToString());
				return Task.FromResult(new NodeConnectionReply()
					{
						Success = false,
						ErrorMessage = e.ToString()
					}
				);
			}
		}

		public override Task<NodeConnectionReply> AddNewNode(AddNodeRequest request, ServerCallContext context)
		{
			try
			{
				_nodesService.AddNewNode(new NodeDto()
				{
					NodeName = request.NodeName,
					NodeType = (NodeType) request.NodeType,
					SessionId = request.SessionId
				});
				return Task.FromResult(new NodeConnectionReply()
					{
						Success = true,
						ErrorMessage = ""
					}
				);
			}
			catch (Exception e)
			{
				_logger.LogError(message: e.ToString());
				return Task.FromResult(new NodeConnectionReply()
					{
						Success = false,
						ErrorMessage = e.ToString()
					}
				);
			}
		}

		public override Task<GetNodeConnectionReply> GetUserNode(GetNodeRequest request, ServerCallContext context)
		{
			try
			{
				var userNode = _nodesService.GetUserNode(request.NodeName, request.SessionId);

				return Task.FromResult(new GetNodeConnectionReply()
				{
					Status = new NodeConnectionReply()
					{
						Success = true,
						ErrorMessage = ""
					},
					NodeName = userNode.NodeName,
					NodeType = (Logic.Proto.NodeType) userNode.NodeType,
					DestinationNodes =
					{
						_nodesService.GetUserNodeDestinations(userNode.NodeName, request.SessionId).Select(node =>
							new SimpleNode()
							{
								NodeName = node.NodeName,
								NodeType = (Logic.Proto.NodeType) node.NodeType,
								NodeValue = node.NodeValue.ToNullable()
							})
					},
					SourceNodes =
					{
						_nodesService.GetUserNodeSources(userNode.NodeName, request.SessionId).Select(node =>
							new SimpleNode()
							{
								NodeName = node.NodeName,
								NodeType = (Logic.Proto.NodeType) node.NodeType,
								NodeValue = node.NodeValue.ToNullable()
							})
					},
					NodeValue = userNode.NodeValue.ToNullable(),
				});
			}
			catch (Exception e)
			{
				_logger.LogError(message: e.ToString());
				return Task.FromResult(new GetNodeConnectionReply()
					{
						Status = new NodeConnectionReply()
						{
							Success = false,
							ErrorMessage = e.ToString()
						},
					}
				);
			}
		}

		public override Task<NodeConnectionReply> SetNodeValue(SetNodeValueRequest request, ServerCallContext context)
		{
			try
			{
				_nodesService.UpdateNodeValue(request.NodeName, request.NodeValue.ToBool(), request.SessionId);
				return Task.FromResult(new NodeConnectionReply()
					{
						Success = true,
						ErrorMessage = ""
					}
				);
			}
			catch (Exception e)
			{
				_logger.LogError(message: e.ToString());
				return Task.FromResult(new NodeConnectionReply()
					{
						Success = false,
						ErrorMessage = e.ToString()
					}
				);
			}
		}
	}
}
