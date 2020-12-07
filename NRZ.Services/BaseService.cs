using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Data.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public abstract class BaseService
    {
        protected readonly IStringLocalizer<SharedResources> _localizer;
        private bool disposed;

        protected NRZContext _context { get; private set; }

        protected BaseService(NRZContext context,
            IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetContext(NRZContext context)
        {
            _context = context;
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any other managed objects here.
                _context.Dispose();
            }

            // Free any unmanaged objects here.

            disposed = true;
        }

        ~BaseService()
        {
            Dispose(false);
        }

        //protected async Task<List<AttachmentCreateModel>> ParseAttachmentsAsync(IEnumerable<IFormFile> files)
        //{
        //    List<AttachmentCreateModel> docs = new List<AttachmentCreateModel>();

        //    foreach (var file in files)
        //    {
        //        docs.Add(await ParseAttachmentAsync(file));
        //    }

        //    return docs;
        //}

        //protected async Task<AttachmentCreateModel> ParseAttachmentAsync(IFormFile file)
        //{
        //    var result = new AttachmentCreateModel();

        //    using (var stream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(stream);


        //        result.FileName = file.FileName;
        //        result.Type = file.ContentType;
        //        result.Content = stream.ToArray();
        //    }

        //    return result;
        //}

        protected AspNetUsers GetAspNetUserById(string id)
        {
            return _context.AspNetUsers.Find(id);
        }

        protected async Task<AspNetUsers> GetAspNetUserByIdAsync(string id)
        {
            return await _context.AspNetUsers.FindAsync(id);
        }

        protected void SetCreateStamp(ICreatable model, string userId)
        {
            model.CreatedBy = userId;
            model.CreatedOn = DateTime.UtcNow;
        }

        protected void SetUpdateStamp(IEditable model, string userId)
        {
            model.UpdatedBy = userId;
            model.UpdatedOn = DateTime.UtcNow;
        }

        protected void SetDeleteStamp(IDeletable model, string userId)
        {
            model.Deleted = true;
            model.DeletedBy = userId;
            model.DeletedOn = DateTime.UtcNow;
        }
    }
}
