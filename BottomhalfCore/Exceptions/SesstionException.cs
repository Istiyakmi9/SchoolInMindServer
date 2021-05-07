using BottomhalfCore.Flags;
using System;
using System.Configuration;

namespace BottomhalfCore.Exceptions
{
    public class SessionException : Exception
    {

        public SessionException()
        {
        }
        public SessionException(EFlags ExceptionCode)
        {
            this.ExceptionCode = ExceptionCode;
            this.Message = GenerateMessage(ExceptionCode);
        }

        public string GenerateMessage(EFlags FlagName)
        {
            string Message = string.Empty;
            switch (FlagName)
            {
                case EFlags.TokenExpired:
                    Message = "Your token get expired.";
                    break;
                case EFlags.TokenNotFound:
                    Message = "Your request does't contain authentication token.";
                    break;
                case EFlags.InvalidToken:
                    Message = "Invalid requested token.";
                    break;
                default: Message = "Getting unknown error"; break;
            }
            return Message;
        }

        public SessionException(string Message)
        {
            this.Message = Message;
        }

        public SessionException(string Message, string Token)
        {
            this.Message = Message;
            this.Token = Token;
        }

        public void BindExceptionDetail(string Message, string Url)
        {
            this.Message = Message;
        }

        public void SetMessage(string Message)
        {
            if (string.IsNullOrEmpty(this.Message))
                this.Message = Message;
            else
            {
                this.Message += ".\n" + Message;
            }
        }
        private new string Message { set; get; }
        public EFlags ExceptionCode { set; get; }
        public string Token { set; get; }
    }
}
