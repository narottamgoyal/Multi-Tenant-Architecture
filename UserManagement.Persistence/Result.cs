namespace UserManagement.Domain
{
    public class Result<T>
    {
        public T Value { get; private set; }
        public string Error { get; private set; }
        public bool IsSuccess { get; private set; }

        private Result() { }
        public static Result<T> Fail(string error)
        {

            return new Result<T>
            {
                Error = error
            };
        }

        public static Result<T> Success(T t)
        {
            return new Result<T>
            {
                Value = t,
                IsSuccess = true
            };
        }
    }
}
