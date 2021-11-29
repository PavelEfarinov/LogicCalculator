using System;
using System.Threading.Tasks;
using Logic.Client.Grpc.Factories;
using Logic.Dto.Models;
using Logic.Proto;

namespace Logic.Client.Grpc.Facades
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
