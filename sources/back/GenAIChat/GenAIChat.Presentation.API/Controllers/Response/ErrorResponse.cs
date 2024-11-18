using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenAIChat.Presentation.API.Controllers.Response
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public IEnumerable<ErrorMessage> Errors { get; set; } = new List<ErrorMessage>();

        public ErrorResponse(string message, IEnumerable<Exception>? exceptions = null)
        {
            Message = message;
            if (exceptions is not null)
                foreach (var exception in exceptions) AddError(exception);
        }
        public ErrorResponse(Exception exception) : this(new List<Exception> { exception }) { }
        public ErrorResponse(IEnumerable<Exception> exceptions)
        {
            foreach (var exception in exceptions) AddError(exception);
        }
        public ErrorResponse(ModelStateDictionary modelState)
        {
            Message = "Arguments are invalid";
            foreach (var modelStateEntry in modelState)
            {
                foreach (var modelError in modelStateEntry.Value.Errors)
                {
                    AddError(modelError.ErrorMessage, modelError.Exception);
                }
            }
        }

        public void AddError(Exception ex) => AddError(ex.Message, ex);
        public void AddError(string message, Exception? error) => AddError("Unknow", message, error);
        public void AddError(string code, string message, Exception? error) => ((List<ErrorMessage>)Errors).Add(new ErrorMessage
        {
            Code = code,
            Message = message,
            Error = error
        });
    }

    public class ErrorMessage
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Exception? Error { get; set; } = null;
    }
}
