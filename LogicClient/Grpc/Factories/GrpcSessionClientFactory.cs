using LogicServer.Proto;

namespace LogicClient.Grpc.Factories
{
	public class GrpcSessionClientFactory
	{
		public static SessionConnector.SessionConnectorClient GetSessionClient()
		{
			return new SessionConnector.SessionConnectorClient(GrpcChannelFactory.ServerChannel);
		}
	}
}
