using Grpc.Core;
using GrpcService;

namespace GrpcService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly UserDb _userDb;

        public GreeterService(ILogger<GreeterService> logger, UserDb userDb)
        {
            _logger = logger;
            _userDb = userDb;
        }

        public override Task<GetUsersReply> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var reply = new GetUsersReply();
            reply.Users.AddRange(_userDb.Users);

            return Task.FromResult(reply);
        }
    }
}
