using NRZ.Models.EPayment;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class EServicesSettingsExtensions
    {
        public static EServicesSettingsModel ToModel(this EservicesSettings entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new EServicesSettingsModel
            {
                ServiceProviderName = entity.ServiceProviderName,
                ServiceProviderBank = entity.ServiceProviderBank,
                ServiceProviderBic = entity.ServiceProviderBic,
                ServiceProviderIban = entity.ServiceProviderIban,
                Currency = entity.Currency,
                SeizedPropertyCertificateRequestExpirationDays = entity.SeizedPropertyCertificateRequestExpirationDays,
                SeizedPropertyCertificateFee = entity.SeizedPropertyCertificateFee,
                SeizedPropertyCertificateReason = entity.SeizedPropertyCertificateReason,
                SeizedPropertyReportRequestExpirationDays = entity.SeizedPropertyReportRequestExpirationDays,
                SeizedPropertyByOwnerReportFee = entity.SeizedPropertyByOwnerReportFee,
                SeizedPropertyReportReason = entity.SeizedPropertyReportReason,
                AdministrativeServiceNotificationUrl = entity.AdministrativeServiceNotificationUrl,
            };

            return model;
        }

        public static void Update(this EservicesSettings entity, EServicesSettingsModel model)
        {
            if (entity == null || model == null)
            {
                return;
            }

            entity.ServiceProviderName = model.ServiceProviderName;
            entity.ServiceProviderBank = model.ServiceProviderBank;
            entity.ServiceProviderBic = model.ServiceProviderBic;
            entity.ServiceProviderIban = model.ServiceProviderIban;
            entity.Currency = model.Currency;
            entity.SeizedPropertyCertificateRequestExpirationDays = model.SeizedPropertyCertificateRequestExpirationDays;
            entity.SeizedPropertyCertificateFee = model.SeizedPropertyCertificateFee;
            entity.SeizedPropertyCertificateReason = model.SeizedPropertyCertificateReason;
            entity.SeizedPropertyReportRequestExpirationDays = model.SeizedPropertyReportRequestExpirationDays;
            entity.SeizedPropertyByOwnerReportFee = model.SeizedPropertyByOwnerReportFee;
            entity.SeizedPropertyReportReason = model.SeizedPropertyReportReason;
            entity.AdministrativeServiceNotificationUrl = model.AdministrativeServiceNotificationUrl;

            return;
        }
    }
}
