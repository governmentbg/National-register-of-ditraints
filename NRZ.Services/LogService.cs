using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Models.GridModels;
using NRZ.Services.Extensions;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class LogService : BaseService, ILogService
    {
        public LogService(NRZContext context, IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
        }

        public async Task<long> Track(UserLogs log)
        {
            if (log == null)
            {
                return 0;
            }
            await _context.UserLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            return log.Id;
        }

        public async Task UpdateDurationAndStatusCodeAsync(long auditId, TimeSpan duration, int statusCode)
        {
            var audit = await GetByIdAsync(auditId);
            if (audit != null)
            {
                audit.Duration = duration.Milliseconds;
                audit.ResponseStatusCode = statusCode;
                await _context.SaveChangesAsync();
            }
        }

        private async Task<UserLogs> GetByIdAsync(long id)
        {
            return await _context.UserLogs.FindAsync(id);
        }

        public async Task<GridResponseModel> SearchLogs(GridSearchModel searchModel)
        {
            GridResponseModel result = new GridResponseModel();

            IQueryable<UserLogs> userLogs = _context.UserLogs.OrderByDescending(x => x.Id).AsQueryable();

            string searchToLower = searchModel.Search.ToLower();

            if (searchModel.StartDate.HasValue && searchModel.EndDate.HasValue)
            {

                if (searchModel.StartDate > searchModel.EndDate)
                {
                    DateTime? temp = searchModel.StartDate;
                    searchModel.StartDate = searchModel.EndDate;
                    searchModel.EndDate = temp;
                }

                userLogs = userLogs.Where(
                ul => ul.ServerTimeUtc.Date >= searchModel.StartDate
                && ul.ServerTimeUtc.Date <= searchModel.EndDate);

            }
            if (!String.IsNullOrEmpty(searchToLower))
            {
                userLogs = userLogs.Where(ul => ul.ActionName.ToLower().Contains(searchToLower)
                                        || ul.Controller.ToLower().Contains(searchToLower)
                                        || ul.Data.ToLower().Contains(searchToLower)
                                        || ul.Id.ToString().ToLower().Contains(searchToLower)
                                        || ul.Message.ToLower().Contains(searchToLower)
                                        || ul.RequestMethod.ToLower().Contains(searchToLower)
                                        || ul.Result.ToLower().Contains(searchToLower)
                                        || ul.UserId.ToLower().Contains(searchToLower)
                                        || ul.Ip.ToLower().Contains(searchToLower)
                                        || ul.Duration.ToString().ToLower().Contains(searchToLower)
                                        || ul.ResponseStatusCode.ToString().ToLower().Contains(searchToLower));
            }

            result.Total = userLogs.Count();

            if (!String.IsNullOrEmpty(searchModel.SortBy))
            {
                userLogs = OrderByStringWithReflection.OrderBy(userLogs, searchModel.SortBy, searchModel.SortDesc);
            }

            userLogs = userLogs.Skip((searchModel.Page - 1) * searchModel.ItemsPerPage)
                 .Take(searchModel.ItemsPerPage);

            result.Items = userLogs;
            result.Page = searchModel.Page;
            result.PageSize = searchModel.ItemsPerPage;

            return result;
        }
    }
}
