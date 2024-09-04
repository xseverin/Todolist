namespace UseCases
{


    public class Result<T>
    {
        public bool IsSucceed { get; private set; } = true;
        public Dictionary<string, string[]> Messages { get; private set; } = [];

        public T? Data { get; private set; }

        public Result<T> SetSuccess(T data)
        {
            Data = data;
            return this;
        }

        public Result<T> SetSuccess(T data, string key, string value)
        {
            Data = data;
            Messages.Add(key, [value]);
            return this;
        }

        public Result<T> SetSuccess(T data, Dictionary<string, string[]> message)
        {
            Data = data;
            Messages = message;
            return this;
        }

        public Result<T> SetSuccess(T data, string key, string[] value)
        {
            Data = data;
            Messages.Add(key, value);
            return this;
        }

        public Result<T> SetError(string key, string value)
        {
            IsSucceed = false;
            Messages.Add(key, [value]);
            return this;
        }

        public Result<T> SetError(string key, string[] value)
        {
            IsSucceed = false;
            Messages.Add(key, value);
            return this;
        }

        public Result<T> SetError(Dictionary<string, string[]> message)
        {
            IsSucceed = false;
            Messages = message;
            return this;
        }
    }
}