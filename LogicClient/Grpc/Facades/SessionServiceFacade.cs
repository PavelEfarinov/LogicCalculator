using System;
using System.Threading.Tasks;
using Logic.Dto.Models;
using LogicClient.Grpc.Factories;
using LogicServer;
using LogicServer.Proto;

namespace LogicClient.Grpc.Facades
{
	public class SessionServiceFacade
	{
		public async Task<UserSessionDto> InitConnectionWithGuidAsync(UserSessionDto sessionDto)
		{
			var reply = await GrpcSessionClientFactory.GetSessionClient().InitConnectionWithGuidAsync(
				new ConnectionRequest
				{
					UserId = sessionDto.SessionId.ToString()
				});
			return new UserSessionDto()
			{
				SessionId = Guid.Parse(reply.UserId)
			};
		}

		public async Task<UserSessionDto> InitConnectionAsync()
		{
			var response = await GrpcSessionClientFactory.GetSessionClient().InitConnectionAsync(new NewConnectionRequest());
			return new UserSessionDto {SessionId = Guid.Parse(response.UserId)};
		}
	}
}
