using System.Net.Http;
using System.Threading.Tasks;

namespace WebAppDemo.Common
{
    public interface IRESTAPIHandler
    {
        Task<object> PostAsync(string endpoint, StringContent content);

        Task<object> GetAsync(string endpoint);

        Task<object> PostAsync(string endpoint, MultipartFormDataContent content);
    }
}