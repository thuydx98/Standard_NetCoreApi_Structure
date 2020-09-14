using System.Linq;
using System.Net;

namespace StandardApi.CrossCutting.Commands
{
    public static class CommandResultExtensions
    {
        public static int GetStatusCode(this CommandResult commandResult)
        {
            return commandResult.Succeeded
                ? (int)HttpStatusCode.OK
                : (commandResult.GetFirstErrorCode() ?? (int)HttpStatusCode.InternalServerError);
        }

        public static object GetData(this CommandResult commandResult)
        {
            return new
            {
                Success = commandResult.Succeeded,
                Data = commandResult.Data,
                Message = commandResult.GetFirstErrorMessage()
            };
        }

        public static int? GetFirstErrorCode(this CommandResult commandResult)
        {
            return commandResult.Errors.FirstOrDefault()?.Code;
        }
        public static string GetFirstErrorMessage(this CommandResult commandResult)
        {
            return commandResult.Errors.FirstOrDefault()?.Message;
        }
    }
}
