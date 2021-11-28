using System;
using System.Threading.Tasks;
using Grpc.Core;
using Logic.Dto.Models;
using LogicServer.Proto;
using Microsoft.Extensions.Logging;
using NodeType = Logic.Dto.Models.NodeType;

namespace LogicServer.Services.Grpc
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
	}
}
