using Logic.Proto;

namespace Logic.Client.Grpc.Factories
{
	public class GrpcSessionClientFactory
	{
		public static SessionConnector.SessionConnectorClient GetSessionClient()
		{
			return new SessionConnector.SessionConnectorClient(GrpcChannelFactory.ServerChannel);
		}
	}
}
