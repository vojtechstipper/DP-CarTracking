using CarTracking.MobileApp.Models;
using Refit;

namespace CarTracking.MobileApp.API;

public class ApiWrapper<T>(T api)
{
    public async Task<Result<TResult>> ExecuteAsync<TResult>(Func<T, Task<ApiResponse<TResult>>> apiCall)
    {
        try
        {
            var result = await apiCall(api);
            if (result.IsSuccessStatusCode)
            {
                return new Result<TResult>(result.Content, true);
            }

            return new Result<TResult>(false);
        }
        catch (Exception ex)
        {
            // Handle unexpected errors
            return new Result<TResult>(false);
        }
    }
}