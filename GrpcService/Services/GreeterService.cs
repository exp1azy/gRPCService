using Grpc.Core;
using GrpcService;

namespace GrpcService.Services
{
    public class GreeterService(ILogger<GreeterService> logger, UserDb userDb) : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger = logger;
        private readonly UserDb _userDb = userDb;

        public override Task<GetUsersReply> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var reply = new GetUsersReply();
            reply.Users.AddRange(_userDb.Users);

            return Task.FromResult(reply);
        }

        public override Task<CreateUserReply> CreateNewUser(CreateUserRequest request, ServerCallContext context)
        {
            var reply = new CreateUserReply();
            var result = _userDb.CreateUser(request.Username, request.Password);
            reply.Result = result;

            return Task.FromResult(reply);
        }

        public override Task<UpdateUserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var reply = new UpdateUserReply();
            var result = _userDb.UpdateUser(request.Id, request.HasUsername ? request.Username : null, request.HasPassword ? request.Password : null);
            reply.Result = result;

            return Task.FromResult(reply);
        }

        public override Task<DeleteUserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var reply = new DeleteUserReply();
            var result = _userDb.DeleteUser(request.Id);
            reply.Result = result;

            return Task.FromResult(reply);
        }
    }
}
