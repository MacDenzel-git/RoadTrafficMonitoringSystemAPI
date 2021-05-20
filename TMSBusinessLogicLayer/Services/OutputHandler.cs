namespace TMSBusinessLogicLayer.Services
{
    public class OutputHandler
    {
        public bool IsErrorOccured { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public int NumberOfCrimes { get; set; }
        public string Identifier { get; set; }
        public object other { get; set; }

        public bool IsErrorKnown { get; set; }

    }
}