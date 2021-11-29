using System;
using System.Linq;
using Logic.Client.CommandLineOptions;
using Logic.Client.Grpc.Factories;
using Logic.Proto;
using Logic.Utils;

namespace Logic.Client.Grpc.Facades
{
	public class NodeServiceFacade
	{
		public string AddNode(AddOptions options, Guid sessionId)
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

			return "An error occured: " + response.ErrorMessage;
		}

		public string LinkNodes(LinkOptions options, Guid sessionId)
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

			return "An error occured: " + response.ErrorMessage;
		}

		public string SetNodeValue(SetOptions options, Guid sessionId)
		{
			var response = GrpcNodeClientFactory.GetNodesClient().SetNodeValue(new SetNodeValueRequest()
			{
				NodeName = options.NodeName,
				NodeValue = options.Value.ToNullable(),
				SessionId = sessionId.ToString(),
			});
			if (response.Success)
			{
				return "SUCCESS";
			}

			return "An error occured: " + response.ErrorMessage;
		}

		public string GetAllNodesInfo(Guid sessionId)
		{
			var response = GrpcNodeClientFactory.GetNodesClient().GetUserNodes(new GetNodesRequest()
			{
				SessionId = sessionId.ToString(),
			});
			if (response.Status.Success)
			{
				return string.Join(", ",
					response.Nodes.Select(node =>
						$"{node.NodeName}:{node.NodeType.ToString()}:{node.NodeValue.ToPrintString()}"));
			}

			return "An error occured: " + response.Status.ErrorMessage;
		}

		public string GetNodeInfo(ShowOptions options, Guid sessionId)
		{
			var response = GrpcNodeClientFactory.GetNodesClient().GetUserNode(new GetNodeRequest()
			{
				NodeName = options.NodeName,
				SessionId = sessionId.ToString(),
			});
			if (response.Status.Success)
			{
				var sources = string.Join(", ",
					response.SourceNodes.Select(node => $"{node.NodeName}:{node.NodeValue.ToPrintString()}"));
				var destinations = string.Join(", ",
					response.DestinationNodes.Select(node => $"{node.NodeName}:{node.NodeValue.ToPrintString()}"));
				return $"Name: {response.NodeName}\r\n" +
				       $"Type: {response.NodeType}\r\n" +
				       $"Value: {response.NodeValue.ToPrintString()}\r\n" +
				       (sources.Length > 0
					       ? $"{sources}\r\n" +
					         $"{"\\/".PadLeft(sources.Length / 2)}\r\n"
					       : "") +
				       $"{(response.NodeName + ":" + response.NodeValue.ToPrintString()).PadLeft(sources.Length / 2)}\r\n" +
				       (destinations.Length > 0
					       ? $"{"\\/".PadLeft(sources.Length / 2)}\r\n" +
					         $"{destinations}\r\n"
					       : "");
			}

			return "An error occured: " + response.Status.ErrorMessage;
		}
	}
}
