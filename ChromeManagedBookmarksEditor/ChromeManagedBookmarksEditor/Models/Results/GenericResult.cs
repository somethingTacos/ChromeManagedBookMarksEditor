using System;

namespace ChromeManagedBookmarksEditor.Models.Results
{
    public class GenericResult
    {
        public string? ErrorMessage { get; private set; } = null;

        public bool Succeeded => ErrorMessage == null;

        public string Message { get; private set; } = "";

        public object? Data { get; private set; } = null;

        public bool HasData => Data != null;

        protected GenericResult(string Message, object? Data = null, string? ErrorMessage = null)
        {
            this.Message = Message;
            this.ErrorMessage = ErrorMessage;
            this.Data = Data;
        }

        public static GenericResult FromSuccess(string OptionalMessage = "", object? OptionalData = null) => new GenericResult(OptionalMessage, OptionalData, null);

        public static GenericResult FromError(string ErrorMessage) => new GenericResult("", null, ErrorMessage);

        public static GenericResult FromException(Exception ex) => new GenericResult("", null, ex.Message);
    }
}
