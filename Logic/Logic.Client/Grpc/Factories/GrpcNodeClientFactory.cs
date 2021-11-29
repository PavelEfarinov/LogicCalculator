using Logic.Proto;

namespace Logic.Client.Grpc.Factories
{
	public class GrpcNodeClientFactory
	{
		public static NodesConnector.NodesConnectorClient GetNodesClient()
		{
			return new NodesConnector.NodesConnectorClient(GrpcChannelFactory.ServerChannel);
		}
	}
}
