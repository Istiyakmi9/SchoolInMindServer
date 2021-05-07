using System;

namespace BottomhalfCore.Exceptions
{
    [Serializable]
    public class BeanException : Exception
    {
        private new string Message { set; get; }
        private string ExceptionPath { set; get; }
        private string LocationTrackedPath { set; get; }
        public void SetMessage(string ErrorMessage)
        {
            this.Message = ErrorMessage;
        }

        public void SetExceptionPath(string ExceptionPath)
        {
            this.ExceptionPath = ExceptionPath;
        }

        public string GetMessage()
        {
            return this.Message;
        }

        public string GetLocationTrack()
        {
            return this.LocationTrackedPath;
        }

        public void LocationTrack(string TracedName)
        {
            if (string.IsNullOrEmpty(this.LocationTrackedPath))
                this.LocationTrackedPath = TracedName;
            else
                this.LocationTrackedPath += " -> " + TracedName;
        }

        public string GetFullMessage()
        {
            string FullMessage = null;
            FullMessage = "Error messaeg: " + this.Message;
            FullMessage += "\nLocation or Path where error occured: " + this.ExceptionPath;
            return FullMessage;
        }

        public string GetExceptionPath()
        {
            return this.ExceptionPath;
        }
    }
}
