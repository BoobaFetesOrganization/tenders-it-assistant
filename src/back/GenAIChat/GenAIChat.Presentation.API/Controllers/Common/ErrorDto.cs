using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenAIChat.Presentation.API.Controllers.Common
{
    public class ErrorDto
    {
        public string Message { get; set; } = string.Empty;
        public IDictionary<string, ICollection<ErrorMessage>> Errors { get; set; } = new Dictionary<string, ICollection<ErrorMessage>>();

        public ErrorDto(string message, IEnumerable<Exception>? exceptions = null)
        {
            Message = message;
            if (exceptions is not null)
                foreach (var exception in exceptions) AddError(exception);
        }
        public ErrorDto(Exception exception) : this([exception]) { }
        public ErrorDto(IEnumerable<Exception> exceptions)
        {
            foreach (var exception in exceptions) AddError(exception);
        }
        public ErrorDto(ModelStateDictionary modelState)
        {
            Message = "Arguments are invalid";
            foreach (var modelStateEntry in modelState)
            {
                foreach (var modelError in modelStateEntry.Value.Errors)
                {
                    AddError(modelStateEntry.Key, modelError.ErrorMessage);
                }
            }
        }

        public void AddError(Exception ex)
        {
            if (ex is AggregateException aggregate)
                aggregate.InnerExceptions.ToList().ForEach(AddError);
            else
                AddError(ex.Message);
        }
        public void AddError(string message) => AddError("Unknow", message);
        public void AddError(string code, string message)
        {
            Errors.TryAdd(code, []);
            Errors[code].Add(new ErrorMessage
            {
                Code = code,
                Message = message
            });
        }
    }

    public class ErrorMessage
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
