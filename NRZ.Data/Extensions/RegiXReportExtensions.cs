using NRZ.Models.Company;
using NRZ.Models.Person;
using NRZ.Models.Property;
using NRZ.Models.RegiX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class RegiXReportExtensions
    {
        public static RegiXReportModel ToModel(this RegiXreport report)
        {
            if (report == null)
            {
                return null;
            }

            RegiXReportModel model = new RegiXReportModel
            {
                Id = report.Id,
                ProviderName = report.ProviderName,
                RegisterName = report.RegisterName,
                ReportName = report.ReportName,
                AdapterSubdirectory = report.AdapterSubdirectory,
                OperationName = report.OperationName,
                RequestXsd = report.RequestXsd,
                ResponseXsd = report.ResponseXsd,
                Operation = report.Operation,
                IsDeleted = report.IsDeleted
            };

            return model;
        }


        public static Aircraft ToEntity(this RegiX.Client.ResponseModels.Aircraft model)
        {
            if (model == null)
            {
                return null;
            }

            Aircraft entity = new Aircraft
            {
                ProducerName = model.Producer?.Name,
                ProducerNameEn = model.Producer?.NameEn,
                ProducerCountryCode = model.Producer?.CountryCode,
                ProducerCountryName = model.Producer?.CountryName,
                AirCategoryCode = model.AirCategory?.Code,
                AirCategoryName = model.AirCategory?.Name,
                Icao = model.ICAO,
                MsnserialNumber = model.MSNSerialNumber,
                ModelName = model.BGModelName,
                ModelNameEn = model.ENModelName,
            };

            entity.AircraftRegistration = CreateAircraftRegistrations(entity.Id, model);
            entity.AircraftDebt = CreateAircraftDebts(entity.Id, model);

            return entity;
        }

        public static AircraftViewModel ToViewModel(this RegiX.Client.ResponseModels.Aircraft model)
        {
            if (model == null)
            {
                return null;
            }

            AircraftViewModel viewModel = new AircraftViewModel
            {
                Id = -1,
                ProducerName = model.Producer?.Name,
                ProducerNameEn = model.Producer?.NameEn,
                ProducerCountryCode = model.Producer?.CountryCode,
                ProducerCountryName = model.Producer?.CountryName,
                AirCategoryCode = model.AirCategory?.Code,
                AirCategoryName = model.AirCategory?.Name,
                Icao = model.ICAO,
                MsnserialNumber = model.MSNSerialNumber,
                ModelName = model.BGModelName,
                ModelNameEn = model.ENModelName,
            };

            viewModel.Registrations = CreateAircraftRegistrationViewModels(model);
            viewModel.Debts = CreateAircraftDebtViewNodels(model);

            return viewModel;
        }

        public static void UpdateEntity(this Data.Aircraft entity, RegiX.Client.ResponseModels.Aircraft model)
        {
            if (model == null || entity == null)
            {
                return;
            }

            entity.ProducerName = model.Producer?.Name;
            entity.ProducerNameEn = model.Producer?.NameEn;
            entity.ProducerCountryCode = model.Producer?.CountryCode;
            entity.ProducerCountryName = model.Producer?.CountryName;
            entity.AirCategoryCode = model.AirCategory?.Code;
            entity.AirCategoryName = model.AirCategory?.Name;
            entity.Icao = model.ICAO;
            entity.MsnserialNumber = model.MSNSerialNumber;
            entity.ModelName = model.BGModelName;
            entity.ModelNameEn = model.ENModelName;

            entity.AircraftRegistration = new List<AircraftRegistration>();
            entity.AircraftRegistration = CreateAircraftRegistrations(entity.Id, model);
            entity.AircraftDebt = CreateAircraftDebts(entity.Id, model);

            return;
        }

        private static List<AircraftRegistration> CreateAircraftRegistrations(int entityId, RegiX.Client.ResponseModels.Aircraft model)
        {
            List<AircraftRegistration> list = model.Registrations.Registration.Select(x => new AircraftRegistration
            {
                AircraftId = entityId,
                ActNumber = x.ActNumberSpecified ? x.ActNumber : default(int?),
                RegistrationNumber = x.RegistrationNumberSpecified ? x.RegistrationNumber : default(int?),
                RegistrationDate = x.RegistrationDateSpecified ? DateTime.SpecifyKind(x.RegistrationDate, DateTimeKind.Utc) : default(DateTime?),
                RegistrationMark = x.RegistrationMark,
                IsLastRegistration = x.IsLastRegistrationSpecified ? x.IsLastRegistration : default(bool?),
                RegistrationStatusIsActive = (x.RegistrationStatus != null && x.RegistrationStatus.IsActiveRegistrationSpecified) ? x.RegistrationStatus.IsActiveRegistration : default(bool?),
                RegistrationStatusCode = (x.RegistrationStatus != null && x.RegistrationStatus.StatusCodeSpecified) ? x.RegistrationStatus.StatusCode : default(int?),
                RegistrationStatusName = x.RegistrationStatus != null ? x.RegistrationStatus.StatusName : default(string),
                RegistrationDocumentNumber = x.RegistrationDocument != null ? x.RegistrationDocument.DocNumber : default(string),
                RegistrationDocumentDate = (x.RegistrationDocument != null && x.RegistrationDocument.DocDateSpecified) ? DateTime.SpecifyKind(x.RegistrationDocument.DocDate, DateTimeKind.Utc) : default(DateTime?),
                RegistrationDocumentDescription = x.RegistrationDocument != null ? x.RegistrationDocument.Description : default(string),
                DeregistrationDate = (x.DeregistrationInfo != null && x.DeregistrationInfo.DeregistrationDateSpecified) ? DateTime.SpecifyKind(x.DeregistrationInfo.DeregistrationDate, DateTimeKind.Utc) : default(DateTime?),
                DeregistrationReason = x.DeregistrationInfo != null ? x.DeregistrationInfo.DeregistrationReason : default(string),
                DeregistrationDescription = x.DeregistrationInfo != null ? x.DeregistrationInfo.DeregistrationDescription : default(string),
                DeregistrationCountryCode = x.DeregistrationInfo != null ? x.DeregistrationInfo.DeregistrationCountryCode : default(string),
                DeregistrationCountryName = x.DeregistrationInfo != null ? x.DeregistrationInfo.DeregistrationCountryName : default(string),
                LeasingDocumentNumber = x.LeasingInfo != null ? x.LeasingInfo.DocNumber : default(string),
                LeasingDocumentDate = (x.LeasingInfo != null && x.LeasingInfo.DocDateSpecified) ? DateTime.SpecifyKind(x.LeasingInfo.DocDate, DateTimeKind.Utc) : default(DateTime?),
                LeasingEndDate = (x.LeasingInfo != null && x.LeasingInfo.EndDateSpecified) ? DateTime.SpecifyKind(x.LeasingInfo.EndDate, DateTimeKind.Utc) : default(DateTime?),
                LeasingAgreement = x.LeasingInfo != null ? x.LeasingInfo.Agreement : default(string),
                LeasingLessorPersonIdentifier = (x.LeasingInfo != null && x.LeasingInfo.Lessor != null && x.LeasingInfo.Lessor.Person != null) ? x.LeasingInfo.Lessor.Person.Identifier : default(string),
                LeasingLessorPersonNames = (x.LeasingInfo != null && x.LeasingInfo.Lessor != null && x.LeasingInfo.Lessor.Person != null) ? x.LeasingInfo.Lessor.Person.Names : default(string),
                LeasingLessorEntityIdentifier = (x.LeasingInfo != null && x.LeasingInfo.Lessor != null && x.LeasingInfo.Lessor.Entity != null) ? x.LeasingInfo.Lessor.Entity.Identifier : default(string),
                LeasingLessorEntityName = (x.LeasingInfo != null && x.LeasingInfo.Lessor != null && x.LeasingInfo.Lessor.Entity != null) ? x.LeasingInfo.Lessor.Entity.Name : default(string),

                AircraftRegistrationOperatorPerson = (x.Operators != null && x.Operators.PersonList != null && x.Operators.PersonList.Person != null) ?
                x.Operators.PersonList.Person.Select((p, Id) => new AircraftRegistrationOperatorPerson
                {
                    RegistrationId = Id,
                    Identifier = p.Identifier,
                    Names = p.Names
                }).ToList()
                : null,

                AircraftRegistrationOperatorEntity = (x.Operators != null && x.Operators.EntitiesList != null && x.Operators.EntitiesList.Entity != null) ?
                x.Operators.EntitiesList.Entity.Select((e, Id) => new AircraftRegistrationOperatorEntity
                {
                    RegistrationId = Id,
                    Identifier = e.Identifier,
                    Name = e.Name
                }).ToList()
                : null,


                AircraftRegistrationOwnerPerson = (x.Owners != null && x.Owners.PersonList != null && x.Owners.PersonList.Person != null) ?
                x.Owners.PersonList.Person.Select((p, Id) => new AircraftRegistrationOwnerPerson
                {
                    RegistrationId = Id,
                    Identifier = p.Identifier,
                    Names = p.Names
                }).ToList()
                : null,

                AircraftRegistrationOwnerEntity = (x.Owners != null && x.Owners.EntitiesList != null && x.Owners.EntitiesList.Entity != null) ?
                x.Owners.EntitiesList.Entity.Select((e, Id) => new AircraftRegistrationOwnerEntity
                {
                    RegistrationId = Id,
                    Identifier = e.Identifier,
                    Name = e.Name
                }).ToList()
                : null,

            }).ToList();

            return list;
        }

        private static List<AircraftDebt> CreateAircraftDebts(int entityId, RegiX.Client.ResponseModels.Aircraft model)
        {
            List<AircraftDebt> list = model.Debts.Debt.Select(x => new AircraftDebt
            {
                AircraftId = entityId,
                InputDate = x.InputDateSpecified ? DateTime.SpecifyKind(x.InputDate, DateTimeKind.Utc) : default(DateTime?),
                DebtTypeCode = x.DebtTypeCodeSpecified ? x.DebtTypeCode : default(int?),
                DebtType = x.DebtType,
                IsActive = x.IsActiveSpecified ? x.IsActive : default(bool?),
                ApplicantIdentifier = x.Applicant != null ? x.Applicant.Identifier : default(string),
                ApplicantName = x.Applicant != null ? x.Applicant.Name : default(string),
                DocumentIncomingNumber = x.Document != null ? x.Document.IncomingNumber : default(string),
                DocumentIncomingDate = (x.Document != null && x.Document.IncomingDateSpecified) ? DateTime.SpecifyKind(x.Document.IncomingDate, DateTimeKind.Utc) : default(DateTime?),
                DocumentExternalNumber = x.Document != null ? x.Document.ExternalNumber : default(string),
                DocumentExternalDate = (x.Document != null && x.Document.ExternalDateSpecified) ? DateTime.SpecifyKind(x.Document.ExternalDate, DateTimeKind.Utc) : default(DateTime?),
                RepaymentDate = (x.Repayment != null && x.Repayment.RepaymentDateSpecified) ? DateTime.SpecifyKind(x.Repayment.RepaymentDate, DateTimeKind.Utc) : default(DateTime?),
                RepaymentDocumentIncomingNumber = (x.Repayment != null && x.Repayment.RepaymentDocument != null) ? x.Repayment.RepaymentDocument.IncomingNumber : default(string),
                RepaymentDocumentIncomingDate = (x.Repayment != null && x.Repayment.RepaymentDocument != null && x.Repayment.RepaymentDocument.IncomingDateSpecified) ? DateTime.SpecifyKind(x.Repayment.RepaymentDocument.IncomingDate, DateTimeKind.Utc) : default(DateTime?),
                RepaymentDocumentExternalNumber = (x.Repayment != null && x.Repayment.RepaymentDocument != null) ? x.Repayment.RepaymentDocument.ExternalNumber : default(string),
                RepaymentDocumentExternalDate = (x.Repayment != null && x.Repayment.RepaymentDocument != null && x.Repayment.RepaymentDocument.ExternalDateSpecified) ? DateTime.SpecifyKind(x.Repayment.RepaymentDocument.ExternalDate, DateTimeKind.Utc) : default(DateTime?),
                RepaymentNotes = x.Repayment != null ? x.Repayment.Notes : default(string),
                Notes = x.Notes,

            }).ToList();

            return list;
        }

        private static List<AircraftRegistrationViewModel> CreateAircraftRegistrationViewModels(RegiX.Client.ResponseModels.Aircraft model)
        {
            List<AircraftRegistrationViewModel> list = model.Registrations.Registration.Select(x => new AircraftRegistrationViewModel
            {
                AircraftId = -1,
                ActNumber = x.ActNumberSpecified ? x.ActNumber : default(int?),
                RegistrationNumber = x.RegistrationNumberSpecified ? x.RegistrationNumber : default(int?),
                RegistrationDate = x.RegistrationDateSpecified ? DateTime.SpecifyKind(x.RegistrationDate, DateTimeKind.Utc) : default(DateTime?),
                RegistrationMark = x.RegistrationMark,
                IsLastRegistration = x.IsLastRegistrationSpecified ? x.IsLastRegistration : default(bool?),
                RegistrationStatusIsActive = (x.RegistrationStatus != null && x.RegistrationStatus.IsActiveRegistrationSpecified) ? x.RegistrationStatus.IsActiveRegistration : default(bool?),
                RegistrationStatusCode = (x.RegistrationStatus != null && x.RegistrationStatus.StatusCodeSpecified) ? x.RegistrationStatus.StatusCode : default(int?),
                RegistrationStatusName = x.RegistrationStatus != null ? x.RegistrationStatus.StatusName : default(string),
                RegistrationDocumentNumber = x.RegistrationDocument != null ? x.RegistrationDocument.DocNumber : default(string),
                RegistrationDocumentDate = (x.RegistrationDocument != null && x.RegistrationDocument.DocDateSpecified) ? DateTime.SpecifyKind(x.RegistrationDocument.DocDate, DateTimeKind.Utc) : default(DateTime?),
                RegistrationDocumentDescription = x.RegistrationDocument != null ? x.RegistrationDocument.Description : default(string),
                DeregistrationDate = (x.DeregistrationInfo != null && x.DeregistrationInfo.DeregistrationDateSpecified) ? DateTime.SpecifyKind(x.DeregistrationInfo.DeregistrationDate, DateTimeKind.Utc) : default(DateTime?),
                DeregistrationReason = x.DeregistrationInfo != null ? x.DeregistrationInfo.DeregistrationReason : default(string),
                DeregistrationDescription = x.DeregistrationInfo != null ? x.DeregistrationInfo.DeregistrationDescription : default(string),
                DeregistrationCountryCode = x.DeregistrationInfo != null ? x.DeregistrationInfo.DeregistrationCountryCode : default(string),
                DeregistrationCountryName = x.DeregistrationInfo != null ? x.DeregistrationInfo.DeregistrationCountryName : default(string),
                LeasingDocumentNumber = x.LeasingInfo != null ? x.LeasingInfo.DocNumber : default(string),
                LeasingDocumentDate = (x.LeasingInfo != null && x.LeasingInfo.DocDateSpecified) ? DateTime.SpecifyKind(x.LeasingInfo.DocDate, DateTimeKind.Utc) : default(DateTime?),
                LeasingEndDate = (x.LeasingInfo != null && x.LeasingInfo.EndDateSpecified) ? DateTime.SpecifyKind(x.LeasingInfo.EndDate, DateTimeKind.Utc) : default(DateTime?),
                LeasingAgreement = x.LeasingInfo != null ? x.LeasingInfo.Agreement : default(string),
                LeasingLessorPersonIdentifier = (x.LeasingInfo != null && x.LeasingInfo.Lessor != null && x.LeasingInfo.Lessor.Person != null) ? x.LeasingInfo.Lessor.Person.Identifier : default(string),
                LeasingLessorPersonNames = (x.LeasingInfo != null && x.LeasingInfo.Lessor != null && x.LeasingInfo.Lessor.Person != null) ? x.LeasingInfo.Lessor.Person.Names : default(string),
                LeasingLessorEntityIdentifier = (x.LeasingInfo != null && x.LeasingInfo.Lessor != null && x.LeasingInfo.Lessor.Entity != null) ? x.LeasingInfo.Lessor.Entity.Identifier : default(string),
                LeasingLessorEntityName = (x.LeasingInfo != null && x.LeasingInfo.Lessor != null && x.LeasingInfo.Lessor.Entity != null) ? x.LeasingInfo.Lessor.Entity.Name : default(string),

                OperatorPeople = (x.Operators != null && x.Operators.PersonList != null && x.Operators.PersonList.Person != null) ?
                x.Operators.PersonList.Person.Select((p, Id) => new AircraftRegistrationOperatorPersonViewModel
                {
                    RegistrationId = Id,
                    Identifier = p.Identifier,
                    Names = p.Names
                }).ToList()
                : null,

                OperatorEntities = (x.Operators != null && x.Operators.EntitiesList != null && x.Operators.EntitiesList.Entity != null) ?
                x.Operators.EntitiesList.Entity.Select((e, Id) => new AircraftRegistrationOperatorEntityViewModel
                {
                    RegistrationId = Id,
                    Identifier = e.Identifier,
                    Name = e.Name
                }).ToList()
                : null,


                OwnerPeople = (x.Owners != null && x.Owners.PersonList != null && x.Owners.PersonList.Person != null) ?
                x.Owners.PersonList.Person.Select((p, Id) => new AircraftRegistrationOwnerPersonViewModel
                {
                    RegistrationId = Id,
                    Identifier = p.Identifier,
                    Names = p.Names
                }).ToList()
                : null,

                OwnerEntities = (x.Owners != null && x.Owners.EntitiesList != null && x.Owners.EntitiesList.Entity != null) ?
                x.Owners.EntitiesList.Entity.Select((e, Id) => new AircraftRegistrationOwnerEntityViewModel
                {
                    RegistrationId = Id,
                    Identifier = e.Identifier,
                    Name = e.Name
                }).ToList()
                : null,

            }).ToList();

            return list;
        }

        private static List<AircraftDebtViewModel> CreateAircraftDebtViewNodels(RegiX.Client.ResponseModels.Aircraft model)
        {
            List<AircraftDebtViewModel> list = model.Debts.Debt.Select(x => new AircraftDebtViewModel
            {
                AircraftId = -1,
                InputDate = x.InputDateSpecified ? DateTime.SpecifyKind(x.InputDate, DateTimeKind.Utc) : default(DateTime?),
                DebtTypeCode = x.DebtTypeCodeSpecified ? x.DebtTypeCode : default(int?),
                DebtType = x.DebtType,
                IsActive = x.IsActiveSpecified ? x.IsActive : default(bool?),
                ApplicantIdentifier = x.Applicant != null ? x.Applicant.Identifier : default(string),
                ApplicantName = x.Applicant != null ? x.Applicant.Name : default(string),
                DocumentIncomingNumber = x.Document != null ? x.Document.IncomingNumber : default(string),
                DocumentIncomingDate = (x.Document != null && x.Document.IncomingDateSpecified) ? DateTime.SpecifyKind(x.Document.IncomingDate, DateTimeKind.Utc) : default(DateTime?),
                DocumentExternalNumber = x.Document != null ? x.Document.ExternalNumber : default(string),
                DocumentExternalDate = (x.Document != null && x.Document.ExternalDateSpecified) ? DateTime.SpecifyKind(x.Document.ExternalDate, DateTimeKind.Utc) : default(DateTime?),
                RepaymentDate = (x.Repayment != null && x.Repayment.RepaymentDateSpecified) ? DateTime.SpecifyKind(x.Repayment.RepaymentDate, DateTimeKind.Utc) : default(DateTime?),
                RepaymentDocumentIncomingNumber = (x.Repayment != null && x.Repayment.RepaymentDocument != null) ? x.Repayment.RepaymentDocument.IncomingNumber : default(string),
                RepaymentDocumentIncomingDate = (x.Repayment != null && x.Repayment.RepaymentDocument != null && x.Repayment.RepaymentDocument.IncomingDateSpecified) ? DateTime.SpecifyKind(x.Repayment.RepaymentDocument.IncomingDate, DateTimeKind.Utc) : default(DateTime?),
                RepaymentDocumentExternalNumber = (x.Repayment != null && x.Repayment.RepaymentDocument != null) ? x.Repayment.RepaymentDocument.ExternalNumber : default(string),
                RepaymentDocumentExternalDate = (x.Repayment != null && x.Repayment.RepaymentDocument != null && x.Repayment.RepaymentDocument.ExternalDateSpecified) ? DateTime.SpecifyKind(x.Repayment.RepaymentDocument.ExternalDate, DateTimeKind.Utc) : default(DateTime?),
                RepaymentNotes = x.Repayment != null ? x.Repayment.Notes : default(string),
                Notes = x.Notes,

            }).ToList();

            return list;
        }



        public static Vehicle ToEntity(this RegiX.Client.ResponseModels.Vehicle model)
        {
            if (model == null)
            {
                return null;
            }

            Vehicle entity = new Vehicle
            {
                RegistrationNumber = model.VehicleRegistrationNumber,
                FirstRegistrationDate = model.FirstRegistrationDateSpecified ? DateTime.SpecifyKind(model.FirstRegistrationDate, DateTimeKind.Utc) : default(DateTime?),
                Vin = model.VINNumber,
                EngineNumber = model.EngineNumber,
                VehicleType = model.MotorVehicleType,
                Model = model.MotorVehicleModel,
                TradeDescription = model.TradeDescription,
                Color = model.Color,
                Category = model.Category,
            };

            entity.VehicleOwner = new List<VehicleOwner>();
            entity.VehicleOwner.Add(new VehicleOwner
            {
                BulgarianCitizenPin = model.OwnerPersonData?.EGN,
                BulgarianCitizenFirstName = model.OwnerPersonData?.FirstName,
                BulgarianCitizenMiddleName = model.OwnerPersonData?.Surname,
                BulgarianCitizenLastName = model.OwnerPersonData?.FamilyName,
                ForeignCitizenPin = model.OwnerForeignerPersonData?.EGN,
                ForeignCitizenPn = model.OwnerForeignerPersonData?.LNCh,
                ForeignCitizenNamesCyrillic = model.OwnerForeignerPersonData?.Names,
                ForeignCitizenNamesLatin = model.OwnerForeignerPersonData?.NamesLatin,
                CompanyId = model.OwnerEntityData?.Identifier,
                CompanyName = model.OwnerEntityData?.Name,
                CompanyNameLatin = model.OwnerEntityData?.NameLatin,
            });

            return entity;
        }

        public static VehicleViewModel ToViewModel(this RegiX.Client.ResponseModels.Vehicle model)
        {
            if (model == null)
            {
                return null;
            }

            VehicleViewModel viewModel = new VehicleViewModel
            {
                Id = -1,
                RegistrationNumber = model.VehicleRegistrationNumber,
                FirstRegistrationDate = model.FirstRegistrationDateSpecified ? DateTime.SpecifyKind(model.FirstRegistrationDate, DateTimeKind.Utc) : default(DateTime?),
                Vin = model.VINNumber,
                EngineNumber = model.EngineNumber,
                VehicleType = model.MotorVehicleType,
                Model = model.MotorVehicleModel,
                TradeDescription = model.TradeDescription,
                Color = model.Color,
                Category = model.Category,
            };

            viewModel.Owners = CreateVehicleOwnerViewNodels(model);

            return viewModel;
        }

        private static List<VehicleOwnerViewModel> CreateVehicleOwnerViewNodels(RegiX.Client.ResponseModels.Vehicle model)
        {
            VehicleOwnerViewModel owner = new VehicleOwnerViewModel
            {
                BulgarianCitizenPin = model.OwnerPersonData?.EGN,
                BulgarianCitizenFirstName = model.OwnerPersonData?.FirstName,
                BulgarianCitizenMiddleName = model.OwnerPersonData?.Surname,
                BulgarianCitizenLastName = model.OwnerPersonData?.FamilyName,
                ForeignCitizenPin = model.OwnerForeignerPersonData?.EGN,
                ForeignCitizenPn = model.OwnerForeignerPersonData?.LNCh,
                ForeignCitizenNamesCyrillic = model.OwnerForeignerPersonData?.Names,
                ForeignCitizenNamesLatin = model.OwnerForeignerPersonData?.NamesLatin,
                CompanyId = model.OwnerEntityData?.Identifier,
                CompanyName = model.OwnerEntityData?.Name,
                CompanyNameLatin = model.OwnerEntityData?.NameLatin,
            };

            List<VehicleOwnerViewModel> list = new List<VehicleOwnerViewModel>();
            list.Add(owner);
            return list;
        }


        public static void UpdateEntity(this Data.Vehicle entity, RegiX.Client.ResponseModels.Vehicle model)
        {
            if (model == null || entity == null)
            {
                return;
            }

            entity.RegistrationNumber = model.VehicleRegistrationNumber;
            entity.FirstRegistrationDate = model.FirstRegistrationDateSpecified ? DateTime.SpecifyKind(model.FirstRegistrationDate, DateTimeKind.Utc) : default(DateTime?);
            entity.Vin = model.VINNumber;
            entity.EngineNumber = model.EngineNumber;
            entity.VehicleType = model.MotorVehicleType;
            entity.Model = model.MotorVehicleModel;
            entity.TradeDescription = model.TradeDescription;
            entity.Color = model.Color;
            entity.Category = model.Category;

            entity.VehicleOwner = new List<VehicleOwner>();
            entity.VehicleOwner.Add(new VehicleOwner
            {
                BulgarianCitizenPin = model.OwnerPersonData?.EGN,
                BulgarianCitizenFirstName = model.OwnerPersonData?.FirstName,
                BulgarianCitizenMiddleName = model.OwnerPersonData?.Surname,
                BulgarianCitizenLastName = model.OwnerPersonData?.FamilyName,
                ForeignCitizenPin = model.OwnerForeignerPersonData?.EGN,
                ForeignCitizenPn = model.OwnerForeignerPersonData?.LNCh,
                ForeignCitizenNamesCyrillic = model.OwnerForeignerPersonData?.Names,
                ForeignCitizenNamesLatin = model.OwnerForeignerPersonData?.NamesLatin,
                CompanyId = model.OwnerEntityData?.Identifier,
                CompanyName = model.OwnerEntityData?.Name,
                CompanyNameLatin = model.OwnerEntityData?.NameLatin,
            });

            return;
        }


        public static Vehicle ToEntity(this RegiX.Client.ResponseModels.GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult model)
        {
            if (model == null)
            {
                return null;
            }

            Vehicle entity = new Vehicle
            {
                RegistrationNumber = model.VehicleData?.RegistrationNumber,
                FirstRegistrationDate = (model.VehicleData != null && model.VehicleData.FirstRegistrationDateSpecified) ? DateTime.SpecifyKind(model.VehicleData.FirstRegistrationDate, DateTimeKind.Utc) : default(DateTime?),
                Vin = model.VehicleData?.VIN,
                EngineNumber = model.VehicleData?.EngineNumber,
                VehicleType = model.VehicleData?.VehicleType,
                Model = model.VehicleData?.Model,
                TypeApprovalNumber = model.VehicleData?.TypeApprovalNumber,
                ApprovalType = model.VehicleData?.ApprovalType,
                TradeDescription = model.VehicleData?.TradeDescription,
                Color = model.VehicleData?.Color,
                Category = model.VehicleData?.Category,
                OffRoadSymbols = model.VehicleData?.OffRoadSymbols,
                MassG = model.VehicleData?.MassG,
                MassF1 = model.VehicleData?.MassF1,
                MassF2 = model.VehicleData?.MassF2,
                MassF3 = model.VehicleData?.MassF3,
                VehicleNumOfAxles = model.VehicleData?.VehNumOfAxles,
                VehicleMassO1 = model.VehicleData?.VehMassO1,
                VehicleMassO2 = model.VehicleData?.VehMassO2,
                Capacity = model.VehicleData?.Capacity,
                MaxPower = model.VehicleData?.MaxPower,
                Fuel = model.VehicleData?.Fuel,
                EnvironmentalCategory = model.VehicleData?.EnvironmentalCategory,
                VehicleDocumentNumber = model.VehicleData?.VehicleDocument?.VehDocumentNumber,
                VehicleDocumentDate = (model.VehicleData != null && model.VehicleData.VehicleDocument != null && model.VehicleData.VehicleDocument.VehDocumentDateSpecified) ?
                    DateTime.SpecifyKind(model.VehicleData.VehicleDocument.VehDocumentDate, DateTimeKind.Utc)
                    : default(DateTime?),

            };

            entity.VehicleOwner = CreateVehicleOwners(entity.Id, model);
            entity.VehicleUser = CreateVehicleUsers(entity.Id, model);

            return entity;
        }

        public static void UpdateEntity(this Data.Vehicle entity, RegiX.Client.ResponseModels.GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult model)
        {
            if (model == null || entity == null)
            {
                return;
            }

            entity.RegistrationNumber = model.VehicleData?.RegistrationNumber;
            entity.FirstRegistrationDate = (model.VehicleData != null && model.VehicleData.FirstRegistrationDateSpecified) ? DateTime.SpecifyKind(model.VehicleData.FirstRegistrationDate, DateTimeKind.Utc) : default(DateTime?);
            entity.Vin = model.VehicleData?.VIN;
            entity.EngineNumber = model.VehicleData?.EngineNumber;
            entity.VehicleType = model.VehicleData?.VehicleType;
            entity.Model = model.VehicleData?.Model;
            entity.TypeApprovalNumber = model.VehicleData?.TypeApprovalNumber;
            entity.ApprovalType = model.VehicleData?.ApprovalType;
            entity.TradeDescription = model.VehicleData?.TradeDescription;
            entity.Color = model.VehicleData?.Color;
            entity.Category = model.VehicleData?.Category;
            entity.OffRoadSymbols = model.VehicleData?.OffRoadSymbols;
            entity.MassG = model.VehicleData?.MassG;
            entity.MassF1 = model.VehicleData?.MassF1;
            entity.MassF2 = model.VehicleData?.MassF2;
            entity.MassF3 = model.VehicleData?.MassF3;
            entity.VehicleNumOfAxles = model.VehicleData?.VehNumOfAxles;
            entity.VehicleMassO1 = model.VehicleData?.VehMassO1;
            entity.VehicleMassO2 = model.VehicleData?.VehMassO2;
            entity.Capacity = model.VehicleData?.Capacity;
            entity.MaxPower = model.VehicleData?.MaxPower;
            entity.Fuel = model.VehicleData?.Fuel;
            entity.EnvironmentalCategory = model.VehicleData?.EnvironmentalCategory;
            entity.VehicleDocumentNumber = model.VehicleData?.VehicleDocument?.VehDocumentNumber;
            entity.VehicleDocumentDate = (model.VehicleData != null && model.VehicleData.VehicleDocument != null && model.VehicleData.VehicleDocument.VehDocumentDateSpecified) ?
                    DateTime.SpecifyKind(model.VehicleData.VehicleDocument.VehDocumentDate, DateTimeKind.Utc)
                    : default(DateTime?);

            entity.VehicleOwner = CreateVehicleOwners(entity.Id, model);
            entity.VehicleUser = CreateVehicleUsers(entity.Id, model);

            return;
        }

        private static List<VehicleOwner> CreateVehicleOwners(long entityId, RegiX.Client.ResponseModels.GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult model)
        {
            List<VehicleOwner> list = model.OwnersData.Owner.Select(x => new VehicleOwner
            {
                VehicleId = entityId,
                BulgarianCitizenPin = x.BulgarianCitizen?.PIN,
                BulgarianCitizenFirstName = x.BulgarianCitizen?.Names?.First,
                BulgarianCitizenMiddleName = x.BulgarianCitizen?.Names?.Surname,
                BulgarianCitizenLastName = x.BulgarianCitizen?.Names?.Family,

                ForeignCitizenPin = x.ForeignCitizen?.PIN,
                ForeignCitizenPn = x.ForeignCitizen?.PN,
                ForeignCitizenNamesCyrillic = x.ForeignCitizen?.NamesCyrillic,
                ForeignCitizenNamesLatin = x.ForeignCitizen?.NamesLatin,
                ForeignCitizenNationality = x.ForeignCitizen?.Nationality,

                CompanyId = x.Company?.ID,
                CompanyName = x.Company?.Name,
                CompanyNameLatin = x.Company?.NameLatin,

            }).ToList();

            return list;
        }

        private static List<VehicleUser> CreateVehicleUsers(long entityId, RegiX.Client.ResponseModels.GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult model)
        {
            List<VehicleUser> list = model.UsersData.Select(x => new VehicleUser
            {
                VehicleId = entityId,
                BulgarianCitizenPin = x.User?.BulgarianCitizen?.PIN,
                BulgarianCitizenFirstName = x.User?.BulgarianCitizen?.Names?.First,
                BulgarianCitizenMiddleName = x.User?.BulgarianCitizen?.Names?.Surname,
                BulgarianCitizenLastName = x.User?.BulgarianCitizen?.Names?.Family,

                ForeignCitizenPin = x.User?.ForeignCitizen?.PIN,
                ForeignCitizenPn = x.User?.ForeignCitizen?.PN,
                ForeignCitizenNamesCyrillic = x.User?.ForeignCitizen?.NamesCyrillic,
                ForeignCitizenNamesLatin = x.User?.ForeignCitizen?.NamesLatin,
                ForeignCitizenNationality = x.User?.ForeignCitizen?.Nationality,

                CompanyId = x.User?.Company?.ID,
                CompanyName = x.User?.Company?.Name,
                CompanyNameLatin = x.User?.Company?.NameLatin,

            }).ToList();

            return list;
        }


        public static VehicleViewModel ToViewModel(this RegiX.Client.ResponseModels.GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult model)
        {
            if (model == null)
            {
                return null;
            }

            VehicleViewModel viewModel = new VehicleViewModel
            {
                Id = -1,
                RegistrationNumber = model.VehicleData?.RegistrationNumber,
                FirstRegistrationDate = (model.VehicleData != null && model.VehicleData.FirstRegistrationDateSpecified) ? DateTime.SpecifyKind(model.VehicleData.FirstRegistrationDate, DateTimeKind.Utc) : default(DateTime?),
                Vin = model.VehicleData?.VIN,
                EngineNumber = model.VehicleData?.EngineNumber,
                VehicleType = model.VehicleData?.VehicleType,
                Model = model.VehicleData?.Model,
                TypeApprovalNumber = model.VehicleData?.TypeApprovalNumber,
                ApprovalType = model.VehicleData?.ApprovalType,
                TradeDescription = model.VehicleData?.TradeDescription,
                Color = model.VehicleData?.Color,
                Category = model.VehicleData?.Category,
                OffRoadSymbols = model.VehicleData?.OffRoadSymbols,
                MassG = model.VehicleData?.MassG,
                MassF1 = model.VehicleData?.MassF1,
                MassF2 = model.VehicleData?.MassF2,
                MassF3 = model.VehicleData?.MassF3,
                VehicleNumOfAxles = model.VehicleData?.VehNumOfAxles,
                VehicleMassO1 = model.VehicleData?.VehMassO1,
                VehicleMassO2 = model.VehicleData?.VehMassO2,
                Capacity = model.VehicleData?.Capacity,
                MaxPower = model.VehicleData?.MaxPower,
                Fuel = model.VehicleData?.Fuel,
                EnvironmentalCategory = model.VehicleData?.EnvironmentalCategory,
                VehicleDocumentNumber = model.VehicleData?.VehicleDocument?.VehDocumentNumber,
                VehicleDocumentDate = (model.VehicleData != null && model.VehicleData.VehicleDocument != null && model.VehicleData.VehicleDocument.VehDocumentDateSpecified) ? DateTime.SpecifyKind(model.VehicleData.VehicleDocument.VehDocumentDate, DateTimeKind.Utc) : default(DateTime?),
            };

            if(model.OwnersData != null)
            {
                viewModel.Owners = CreateVehicleOwnerViewModels(model.OwnersData);
            }

            if (model.UsersData != null)
            {
                viewModel.Users = CreateVehicleUserViewModels(model.UsersData);
            }


            return viewModel;
        }

        private static List<VehicleOwnerViewModel> CreateVehicleOwnerViewModels(RegiX.Client.ResponseModels.OwnersDataTypeV3 model)
        {
            List<VehicleOwnerViewModel> list = new List<VehicleOwnerViewModel>();

            foreach (RegiX.Client.ResponseModels.OwnerTypeV3 ownerV3 in model.Owner)
            {
                VehicleOwnerViewModel owner = new VehicleOwnerViewModel
                {
                    BulgarianCitizenPin = ownerV3.BulgarianCitizen?.PIN,
                    BulgarianCitizenFirstName = ownerV3.BulgarianCitizen?.Names?.First,
                    BulgarianCitizenMiddleName = ownerV3.BulgarianCitizen?.Names?.Surname,
                    BulgarianCitizenLastName = ownerV3.BulgarianCitizen?.Names?.Family,
                    ForeignCitizenPin = ownerV3.ForeignCitizen?.PIN,
                    ForeignCitizenPn = ownerV3.ForeignCitizen?.PN,
                    ForeignCitizenNamesCyrillic = ownerV3.ForeignCitizen?.NamesCyrillic,
                    ForeignCitizenNamesLatin = ownerV3.ForeignCitizen?.NamesLatin,
                    CompanyId = ownerV3.Company?.ID,
                    CompanyName = ownerV3.Company?.Name,
                    CompanyNameLatin = ownerV3.Company?.NameLatin,
                };


                list.Add(owner);
            }
            
            return list;
        }

        private static List<VehicleUserViewModel> CreateVehicleUserViewModels(RegiX.Client.ResponseModels.UsersDataTypeV3[] model)
        {
            List<VehicleUserViewModel> list = new List<VehicleUserViewModel>();

            foreach (RegiX.Client.ResponseModels.UsersDataTypeV3 userV3 in model)
            {
                VehicleUserViewModel user = new VehicleUserViewModel
                {
                    BulgarianCitizenPin = userV3.User?.BulgarianCitizen?.PIN,
                    BulgarianCitizenFirstName = userV3.User?.BulgarianCitizen?.Names?.First,
                    BulgarianCitizenMiddleName = userV3.User?.BulgarianCitizen?.Names?.Surname,
                    BulgarianCitizenLastName = userV3.User?.BulgarianCitizen?.Names?.Family,
                    ForeignCitizenPin = userV3.User?.ForeignCitizen?.PIN,
                    ForeignCitizenPn = userV3.User?.ForeignCitizen?.PN,
                    ForeignCitizenNamesCyrillic = userV3.User?.ForeignCitizen?.NamesCyrillic,
                    ForeignCitizenNamesLatin = userV3.User?.ForeignCitizen?.NamesLatin,
                    CompanyId = userV3.User?.Company?.ID,
                    CompanyName = userV3.User?.Company?.Name,
                    CompanyNameLatin = userV3.User?.Company?.NameLatin,
                };


                list.Add(user);
            }

            return list;
        }



        public static RegixPersonModel ToViewModel(this RegiX.Client.ResponseModels.ValidPersonResponse model)
        {
            if (model == null)
            {
                return null;
            }

            RegixPersonModel viewModel = new RegixPersonModel
            {
                Id = -1,
                FirstName = model.FirstName,
                MiddleName = model.SurName,
                LastName = model.FamilyName,
                DateOfBirth = model.BirthDateSpecified ? DateTime.SpecifyKind(model.BirthDate, DateTimeKind.Utc) : default(DateTime?),
                DateOfDeath = model.DeathDateSpecified ? DateTime.SpecifyKind(model.DeathDate, DateTimeKind.Utc) : default(DateTime?),
            };

            return viewModel;
        }


        public static RegixCompanyModel ToViewModel(this RegiX.Client.ResponseModels.ValidUICResponse model)
        {
            if (model == null)
            {
                return null;
            }

            Shared.Enums.RegixCompanyStatus status = model.StatusSpecified ? GetCompanyStatus(model.Status) : Shared.Enums.RegixCompanyStatus.UNKNOWN;
            RegixCompanyModel viewModel = new RegixCompanyModel
            {
                Id = -1,
                Name = model.Company,
                LegalFormAbbr = model.LegalForm.LegalFormAbbr,
                LegalFormName = model.LegalForm.LegalFormName,
                StatusCode = status.ToString(),
            };

            return viewModel;
        }

        public static Shared.Enums.RegixCompanyStatus GetCompanyStatus(RegiX.Client.ResponseModels.StatusType regixStatus)
        {
            Shared.Enums.RegixCompanyStatus companyStatus = Shared.Enums.RegixCompanyStatus.UNKNOWN;
            switch (regixStatus)
            {
                case RegiX.Client.ResponseModels.StatusType.Новапартида:
                    companyStatus = Shared.Enums.RegixCompanyStatus.NEW;
                    break;
                case RegiX.Client.ResponseModels.StatusType.Новазакритапартида:
                    companyStatus = Shared.Enums.RegixCompanyStatus.NEWCLOSED;
                    break;
                case RegiX.Client.ResponseModels.StatusType.Пререгистриранапартида:
                    companyStatus = Shared.Enums.RegixCompanyStatus.REREGISTERED;
                    break;
                case RegiX.Client.ResponseModels.StatusType.Пререгистрираназакритапартида:
                    companyStatus = Shared.Enums.RegixCompanyStatus.REREGISTEREDCLOSED;
                    break;

                default:
                    break;
            }

            return companyStatus;

        }



        public static VesselViewModel ToViewModel(this RegiX.Client.ResponseModels.VesselInfoType model)
        {
            if (model == null)
            {
                return null;
            }

            VesselViewModel viewModel = new VesselViewModel
            {
                Id = -1,
                Bt = model.MainFeatures?.BTSpecified == true ? model.MainFeatures?.BT : default,
                Nt = model.MainFeatures?.NTSpecified == true ? model.MainFeatures?.NT : default,
                MaxLength = model.MainFeatures?.MaxLengthSpecified == true ? model.MainFeatures?.MaxLength : default,
                LengthBetweenPerpendiculars = model.MainFeatures?.LengthBetweenPerpendicularsSpecified == true ? model.MainFeatures?.LengthBetweenPerpendiculars : default,
                MaxWidth = model.MainFeatures?.MaxWidthSpecified == true ? model.MainFeatures?.MaxWidth : default,
                Waterplane = model.MainFeatures?.WaterplaneSpecified == true ? model.MainFeatures?.Waterplane : default,
                ShipboardHeight = model.MainFeatures?.ShipboardHeightSpecified == true ? model.MainFeatures?.ShipboardHeight : default,
                Deadweight = model.MainFeatures?.DeadweightSpecified == true ? model.MainFeatures?.Deadweight : default,
                NumberOfEngines = model.MainFeatures?.NumberOfEnginesSpecified == true ? model.MainFeatures?.NumberOfEngines : default,
                EnginesFuel = model.MainFeatures?.EnginesFuel,
                SumEnginePower = model.MainFeatures?.SumEnginePowerSpecified == true ? (decimal?)model.MainFeatures?.SumEnginePower : default,
                BodyNumber = model.MainFeatures?.BodyNumber,
            };

            viewModel.Engines = CreateVesselEngineViewModels(model);
            viewModel.Owners = CreateVesselOwnerViewModels(model);
            viewModel.RegistrationData = CreateVesselRegistrationDataViewModel(model);

            return viewModel;
        }

        private static List<VesselEngineViewModel> CreateVesselEngineViewModels(RegiX.Client.ResponseModels.VesselInfoType model)
        {
            List<VesselEngineViewModel> list = model.MainFeatures?.Engines.Select(x => new VesselEngineViewModel
            {
                VesselId = -1,
                SystemModification = x.SystemModification,
                EngineNumber = x.EngineNumber,
                Power = x.PowerSpecified == true ? x.Power : default,
                Type = x.Type,
            }).ToList();

            return list;
        }

        private static List<VesselOwnerViewModel> CreateVesselOwnerViewModels(RegiX.Client.ResponseModels.VesselInfoType model)
        {
            List<VesselOwnerViewModel> list = model.Owners?.Select(x => new VesselOwnerViewModel
            {
                VesselId = -1,
                IsCompany = x.IsCompany,
                CompanyName = x.Company?.CompanyName,
                Eik = x.Company?.EIK,
                PersonFirstName = x.Person?.Names?.FirstName,
                PersonMiddleName = x.Person?.Names?.SurName,
                PersonLastName = x.Person?.Names?.FamilyName,
                Egn = x.Person?.EGN,
                ImoNumber = x.ImoNumber,
                IsUser = x.IsUser,
                Address = x.Address,
            }).ToList();

            return list;
        }

        private static VesselRegistrationDataViewData CreateVesselRegistrationDataViewModel(RegiX.Client.ResponseModels.VesselInfoType model)
        {
            Shared.Enums.VesselStatus status = model.RegistrationData?.StatusSpecified == true ? GetVesselStatus(model.RegistrationData.Status) : Shared.Enums.VesselStatus.Unknown;
            VesselRegistrationDataViewData data = new VesselRegistrationDataViewData
            {
                VesselId = -1,
                ShipName = model.RegistrationData?.ShipName,
                ShipNameLatin = model.RegistrationData?.ShipNameLatin,
                RegistrationPort = model.RegistrationData?.RegistrationPort,
                RegistrationNumber = model.RegistrationData?.RegistrationNumber,
                Tom = model.RegistrationData?.Tom,
                Page = model.RegistrationData?.Page,
                Type = model.RegistrationData?.Type,
                Status = status.ToString(),
            };

            return data;
        }

        public static Shared.Enums.VesselStatus GetVesselStatus(RegiX.Client.ResponseModels.StatusEnum regixStatus)
        {
            Shared.Enums.VesselStatus vesselStatus = Shared.Enums.VesselStatus.Unknown;
            switch (regixStatus)
            {
                case RegiX.Client.ResponseModels.StatusEnum.Active:
                    vesselStatus = Shared.Enums.VesselStatus.Active;
                    break;
                case RegiX.Client.ResponseModels.StatusEnum.Inactive:
                    vesselStatus = Shared.Enums.VesselStatus.Inactive;
                    break;
                case RegiX.Client.ResponseModels.StatusEnum.Erased:
                    vesselStatus = Shared.Enums.VesselStatus.Erased;
                    break;
                default:
                    break;
            }

            return vesselStatus;

        }



    }
}
