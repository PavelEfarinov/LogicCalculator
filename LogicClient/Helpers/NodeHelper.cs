using System;
using LogicClient.CommandLineOptions;
using LogicClient.Grpc.Factories;
using LogicServer.Proto;

namespace LogicClient.Helpers
{
	public class NodeHelper
	{
		public static string AddNode(AddOptions options, Guid sessionId)
		{
			var response = GrpcNodeClientFactory.GetNodesClient().AddNewNode(new AddNodeRequest()
			{
				NodeName = options.NodeName,
				NodeType = (NodeType) options.NodeType,
				SessionId = sessionId.ToString(),
			});
			if (response.Success)
			{
				return "SUCCESS";
			}
			else
			{
				return "An error occured: " + response.ErrorMessage;
			}
		}
		public static string LinkNodes(LinkOptions options, Guid sessionId)
		{
			var response = GrpcNodeClientFactory.GetNodesClient().LinkNodes(new LinkNodeRequest()
			{
				SourceNodeName = options.SourceNodeName,
				DestinationNodeName = options.DestinationNodeName,
				SessionId = sessionId.ToString(),
			});
			if (response.Success)
			{
				return "SUCCESS";
			}
			else
			{
				return "An error occured: " + response.ErrorMessage;
			}
		}
	}
}
