using NRZ.Models.EPayment;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class PaymentRequestExtensions
    {
        public static PaymentRequestModel ToModel(this PaymentRequest entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new PaymentRequestModel
            {
                AisPaymentId = entity.AisPaymentId,
                ServiceProviderName = entity.ServiceProviderName,
                ServiceProviderBank = entity.ServiceProviderBank,
                ServiceProviderBic = entity.ServiceProviderBic,
                ServiceProviderIban = entity.ServiceProviderIban,
                Currency = entity.Currency,
                PaymentTypeCode = entity.PaymentTypeCode,
                PaymentAmount = entity.PaymentAmount,
                PaymentReason = entity.PaymentReason,
                ApplicantUinTypeId = entity.ApplicantUinTypeId,
                ApplicantUin = entity.ApplicantUin,
                ApplicantName = entity.ApplicantName,
                PaymentReferenceType = entity.PaymentReferenceType,
                PaymentReferenceNumber = entity.PaymentReferenceNumber,
                PaymentReferenceDate = DateTime.SpecifyKind(entity.PaymentReferenceDate, DateTimeKind.Utc),
                ExpirationDate = DateTime.SpecifyKind(entity.ExpirationDate, DateTimeKind.Utc),
                AdditionalInformation = entity.AdditionalInformation,
                AdministrativeServiceUri = entity.AdministrativeServiceUri,
                AdministrativeServiceSupplierUri = entity.AdministrativeServiceSupplierUri,
                AdministrativeServiceNotificationUrl = entity.AdministrativeServiceNotificationUrl,
            };

            return model;
        }

        public static PaymentRequest ToEntity(this PaymentRequestModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = new PaymentRequest
            {
                AisPaymentId = model.AisPaymentId,
                ServiceProviderName = model.ServiceProviderName,
                ServiceProviderBank = model.ServiceProviderBank,
                ServiceProviderBic = model.ServiceProviderBic,
                ServiceProviderIban = model.ServiceProviderIban,
                Currency = model.Currency,
                PaymentTypeCode = model.PaymentTypeCode,
                PaymentAmount = model.PaymentAmount,
                PaymentReason = model.PaymentReason,
                ApplicantUinTypeId = model.ApplicantUinTypeId,
                ApplicantUin = model.ApplicantUin,
                ApplicantName = model.ApplicantName,
                PaymentReferenceType = model.PaymentReferenceType,
                PaymentReferenceNumber = model.PaymentReferenceNumber,
                PaymentReferenceDate = DateTime.SpecifyKind(model.PaymentReferenceDate, DateTimeKind.Utc),
                ExpirationDate = DateTime.SpecifyKind(model.ExpirationDate, DateTimeKind.Utc),
                AdditionalInformation = model.AdditionalInformation,
                AdministrativeServiceUri = model.AdministrativeServiceUri,
                AdministrativeServiceSupplierUri = model.AdministrativeServiceSupplierUri,
                AdministrativeServiceNotificationUrl = model.AdministrativeServiceNotificationUrl,
            };

            return entity;
        }



        public static EservicePaymentRequest ToEntity(this EServicePaymentRequestCreateModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = new EservicePaymentRequest
            {
                EserviceTypeCode = model.EserviceTypeCode,
                SeizedPropertyCertificateRequestId = model.SeizedPropertyCertificateRequestId,
                SeizedPropertyReportRequestId = model.SeizedPropertyReportRequestId,
                PaymentRequestId = model.PaymentRequestId,
                CreatedAt = DateTime.Now,
                StatusCode = model.StatusCode,
            };

            return entity;
        }

        public static EServicePaymentRequestModel ToModel(this EservicePaymentRequest entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new EServicePaymentRequestModel
            {
                Id = entity.Id,
                EserviceTypeCode = entity.EserviceTypeCode,
                EserviceTypeName = entity.EserviceTypeCodeNavigation?.Name,
                EserviceTypeNameEn = entity.EserviceTypeCodeNavigation?.NameEn,
                SeizedPropertyCertificateRequestId = entity.SeizedPropertyCertificateRequestId,
                SeizedPropertyReportRequestId = entity.SeizedPropertyReportRequestId,
                ServiceRequestId = entity.SeizedPropertyCertificateRequestId.HasValue 
                ? entity.SeizedPropertyCertificateRequestId.Value 
                : (entity.SeizedPropertyReportRequestId.HasValue 
                    ? entity.SeizedPropertyReportRequestId.Value 
                    : 0),
                PaymentRequestId = entity.PaymentRequestId,
                CreatedAt = entity.CreatedAt,
                StatusCode = entity.StatusCode,
                StatusName = entity.StatusCodeNavigation?.Name,
                StatusNameEn = entity.StatusCodeNavigation?.NameEn,
                UpdatedAt = entity.UpdatedAt.HasValue ? entity.UpdatedAt.Value : default(DateTime),
            };

            return model;
        }


        public static EServicePaymentStatusHistoryModel ToModel(this EservicePaymentRequestStatusHistory entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new EServicePaymentStatusHistoryModel
            {
                Id = entity.Id,
                RequestId = entity.RequestId,
                StatusCode = entity.StatusCode,
                StatusCodeName = entity.StatusCodeNavigation?.Name,
                StatusCodeNameEn = entity.StatusCodeNavigation?.NameEn,
                UpdatedAt = entity.UpdatedAt,
                EServiceTime = entity.EserviceTime.HasValue ? entity.EserviceTime.Value : default(DateTime),
                Errors = entity.Errors,
            };

            return model;
        }

        public static EPaymentRequestModel ToEpaymentModel(this PaymentRequest e)
        {
            return e == null
                ? null
                : new EPaymentRequestModel
                {
                    ServiceProviderName = e.ServiceProviderName,
                    ServiceProviderBank = e.ServiceProviderBank,
                    ServiceProviderBic = e.ServiceProviderBic,
                    ServiceProviderIban = e.ServiceProviderIban,
                    Currency = e.Currency,
                    PaymentTypeCode = int.TryParse(e.PaymentTypeCode, out int pCode) ? pCode : 1,
                    PaymentAmount = e.PaymentAmount,
                    PaymentReason = e.PaymentReason,
                    ApplicantUinTypeId = e.ApplicantUinTypeId,
                    ApplicantUin = e.ApplicantUin,
                    ApplicantName = e.ApplicantName,
                    ExpirationDate = e.ExpirationDate,
                    AdditionalInformation = e.AdditionalInformation,
                    AdministrativeServiceSupplierUri = e.AdministrativeServiceSupplierUri,
                    AdministrativeServiceNotificationUrl = e.AdministrativeServiceNotificationUrl,
                    PaymentReferenceType = int.TryParse(e.PaymentReferenceType, out int rType) ? rType : 1,
                    PaymentReferenceNumber = e.PaymentReferenceNumber,
                    PaymentReferenceDate = e.PaymentReferenceDate
                };
        }
    }
}
