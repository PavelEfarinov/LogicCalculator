using LogicServer.Proto;

namespace LogicClient.Grpc.Factories
{
	public class GrpcNodeClientFactory
	{
		public static NodesConnector.NodesConnectorClient GetNodesClient()
		{
			return new NodesConnector.NodesConnectorClient(GrpcChannelFactory.ServerChannel);
		}
	}
}
