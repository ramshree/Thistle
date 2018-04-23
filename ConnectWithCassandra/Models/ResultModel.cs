
namespace ConnectWithCassandra.Models
{
    /// <summary>
    /// Generic result model to communicate between classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T> where T : class
    {
        public ResultStatus resultCode { get; set; }
        public T resultContent { get; set; }
    }

    public enum ResultStatus
    {
        success = 1,
        failure = 2
    }
}
