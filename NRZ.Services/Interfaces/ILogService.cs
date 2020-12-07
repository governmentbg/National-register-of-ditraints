using NRZ.Data;
using NRZ.Models.GridModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface ILogService
    {
        Task<long> Track(UserLogs log);
        Task UpdateDurationAndStatusCodeAsync(long auditId, TimeSpan duration, int statusCode);

        Task<GridResponseModel> SearchLogs(GridSearchModel searchModel);
    }
}
