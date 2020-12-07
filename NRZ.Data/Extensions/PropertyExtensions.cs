using NRZ.Models.Address;
using NRZ.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class PropertyExtensions
    {
        #region Other property
        public static OtherPropertyModel ToViewModel(this OtherProperty entity)
        {
            if (entity == null)
            {
                return null;
            }

            OtherPropertyModel model = new OtherPropertyModel
            {
                Id = entity.Id,
                Identifier = entity.Identifier,
                Description = entity.Description,
                Type = entity.Type,
                IsManuallyAdded = entity.IsManuallyAdded
            };

            return model;
        }

        #endregion

        #region Vehicle
        public static VehicleViewModel ToViewModel(this Vehicle entity)
        {
            if (entity == null)
            {
                return null;
            }

            VehicleViewModel model = new VehicleViewModel
            {
                Id = entity.Id,
                RegistrationNumber = entity.RegistrationNumber,
                FirstRegistrationDate = entity.FirstRegistrationDate.HasValue ? DateTime.SpecifyKind(entity.FirstRegistrationDate.Value, DateTimeKind.Utc) : entity.FirstRegistrationDate,
                Vin = entity.Vin,
                EngineNumber = entity.EngineNumber,
                VehicleType = entity.VehicleType,
                Model = entity.Model,
                TypeApprovalNumber = entity.TypeApprovalNumber,
                ApprovalType = entity.ApprovalType,
                TradeDescription = entity.TradeDescription,
                Color = entity.Color,
                Category = entity.Category,
                OffRoadSymbols = entity.OffRoadSymbols,
                MassG = entity.MassG,
                MassF1 = entity.MassF1,
                MassF2 = entity.MassF2,
                MassF3 = entity.MassF3,
                VehicleNumOfAxles = entity.VehicleNumOfAxles,
                VehicleMassO1 = entity.VehicleMassO1,
                VehicleMassO2 = entity.VehicleMassO2,
                Capacity = entity.Capacity,
                MaxPower = entity.MaxPower,
                Fuel = entity.Fuel,
                EnvironmentalCategory = entity.EnvironmentalCategory,
                VehicleDocumentNumber = entity.VehicleDocumentNumber,
                VehicleDocumentDate = entity.VehicleDocumentDate.HasValue ? DateTime.SpecifyKind(entity.VehicleDocumentDate.Value, DateTimeKind.Utc) : entity.VehicleDocumentDate,

                Owners = entity.VehicleOwner
                .Select(x => x.ToViewModel()).ToList(),

                Users = entity.VehicleUser
                .Select(x => x.ToViewModel()).ToList(),
            };

            return model;
        }

        public static VehicleOwnerViewModel ToViewModel(this VehicleOwner entity)
        {
            if (entity == null)
            {
                return null;
            }

            VehicleOwnerViewModel model = new VehicleOwnerViewModel
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                BulgarianCitizenPin = entity.BulgarianCitizenPin,
                BulgarianCitizenFirstName = entity.BulgarianCitizenFirstName,
                BulgarianCitizenMiddleName = entity.BulgarianCitizenMiddleName,
                BulgarianCitizenLastName = entity.BulgarianCitizenLastName,
                ForeignCitizenPin = entity.ForeignCitizenPin,
                ForeignCitizenPn = entity.ForeignCitizenPn,
                ForeignCitizenNamesCyrillic = entity.ForeignCitizenNamesCyrillic,
                ForeignCitizenNamesLatin = entity.ForeignCitizenNamesLatin,
                ForeignCitizenNationality = entity.ForeignCitizenNationality,
                CompanyId = entity.CompanyId,
                CompanyName = entity.CompanyName,
                CompanyNameLatin = entity.CompanyNameLatin,

            };

            return model;
        }

        public static VehicleUserViewModel ToViewModel(this VehicleUser entity)
        {
            if (entity == null)
            {
                return null;
            }

            VehicleUserViewModel model = new VehicleUserViewModel
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                BulgarianCitizenPin = entity.BulgarianCitizenPin,
                BulgarianCitizenFirstName = entity.BulgarianCitizenFirstName,
                BulgarianCitizenMiddleName = entity.BulgarianCitizenMiddleName,
                BulgarianCitizenLastName = entity.BulgarianCitizenLastName,
                ForeignCitizenPin = entity.ForeignCitizenPin,
                ForeignCitizenPn = entity.ForeignCitizenPn,
                ForeignCitizenNamesCyrillic = entity.ForeignCitizenNamesCyrillic,
                ForeignCitizenNamesLatin = entity.ForeignCitizenNamesLatin,
                ForeignCitizenNationality = entity.ForeignCitizenNationality,
                CompanyId = entity.CompanyId,
                CompanyName = entity.CompanyName,
                CompanyNameLatin = entity.CompanyNameLatin,

            };

            return model;
        }

        public static Vehicle ToEntity(this VehicleViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            Vehicle entity = new Vehicle
            {
                Id = model.Id,
                RegistrationNumber = model.RegistrationNumber,
                FirstRegistrationDate = model.FirstRegistrationDate.HasValue ? DateTime.SpecifyKind(model.FirstRegistrationDate.Value, DateTimeKind.Utc) : model.FirstRegistrationDate,
                Vin = model.Vin,
                EngineNumber = model.EngineNumber,
                VehicleType = model.VehicleType,
                Model = model.Model,
                TypeApprovalNumber = model.TypeApprovalNumber,
                ApprovalType = model.ApprovalType,
                TradeDescription = model.TradeDescription,
                Color = model.Color,
                Category = model.Category,
                OffRoadSymbols = model.OffRoadSymbols,
                MassG = model.MassG,
                MassF1 = model.MassF1,
                MassF2 = model.MassF2,
                MassF3 = model.MassF3,
                VehicleNumOfAxles = model.VehicleNumOfAxles,
                VehicleMassO1 = model.VehicleMassO1,
                VehicleMassO2 = model.VehicleMassO2,
                Capacity = model.Capacity,
                MaxPower = model.MaxPower,
                Fuel = model.Fuel,
                EnvironmentalCategory = model.EnvironmentalCategory,
                VehicleDocumentNumber = model.VehicleDocumentNumber,
                VehicleDocumentDate = model.VehicleDocumentDate.HasValue ? DateTime.SpecifyKind(model.VehicleDocumentDate.Value, DateTimeKind.Utc) : model.VehicleDocumentDate,

            };

            if (model.Owners != null && model.Owners.Any())
                entity.VehicleOwner = model.Owners
                .Select(x => x.ToEntity()).ToList();

            if (model.Users != null && model.Users.Any())
            {
                entity.VehicleUser = model.Users
                .Select(x => x.ToEntity()).ToList();
            }

            entity.VehicleExtension = new VehicleExtension
            {
                VehicleId = entity.Id,
                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                RequestId = model.ExtensionRequestId,
                Deactivated = false
            };

            return entity;
        }

        public static VehicleOwner ToEntity(this VehicleOwnerViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            VehicleOwner entity = new VehicleOwner
            {
                Id = model.Id,
                VehicleId = model.VehicleId,
                BulgarianCitizenPin = model.BulgarianCitizenPin,
                BulgarianCitizenFirstName = model.BulgarianCitizenFirstName,
                BulgarianCitizenMiddleName = model.BulgarianCitizenMiddleName,
                BulgarianCitizenLastName = model.BulgarianCitizenLastName,
                ForeignCitizenPin = model.ForeignCitizenPin,
                ForeignCitizenPn = model.ForeignCitizenPn,
                ForeignCitizenNamesCyrillic = model.ForeignCitizenNamesCyrillic,
                ForeignCitizenNamesLatin = model.ForeignCitizenNamesLatin,
                ForeignCitizenNationality = model.ForeignCitizenNationality,
                CompanyId = model.CompanyId,
                CompanyName = model.CompanyName,
                CompanyNameLatin = model.CompanyNameLatin,
            };


            return entity;
        }

        public static VehicleUser ToEntity(this VehicleUserViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            VehicleUser entity = new VehicleUser
            {
                Id = model.Id,
                VehicleId = model.VehicleId,
                BulgarianCitizenPin = model.BulgarianCitizenPin,
                BulgarianCitizenFirstName = model.BulgarianCitizenFirstName,
                BulgarianCitizenMiddleName = model.BulgarianCitizenMiddleName,
                BulgarianCitizenLastName = model.BulgarianCitizenLastName,
                ForeignCitizenPin = model.ForeignCitizenPin,
                ForeignCitizenPn = model.ForeignCitizenPn,
                ForeignCitizenNamesCyrillic = model.ForeignCitizenNamesCyrillic,
                ForeignCitizenNamesLatin = model.ForeignCitizenNamesLatin,
                ForeignCitizenNationality = model.ForeignCitizenNationality,
                CompanyId = model.CompanyId,
                CompanyName = model.CompanyName,
                CompanyNameLatin = model.CompanyNameLatin,
            };


            return entity;
        }

        public static void UpdateEntity(this Data.Vehicle entity, VehicleViewModel model)
        {
            if (model == null || entity == null)
            {
                return;
            }

            entity.Id = model.Id;
            entity.RegistrationNumber = model.RegistrationNumber;
            entity.FirstRegistrationDate = model.FirstRegistrationDate.HasValue ? DateTime.SpecifyKind(model.FirstRegistrationDate.Value, DateTimeKind.Utc) : model.FirstRegistrationDate;
            entity.Vin = model.Vin;
            entity.EngineNumber = model.EngineNumber;
            entity.VehicleType = model.VehicleType;
            entity.Model = model.Model;
            entity.TypeApprovalNumber = model.TypeApprovalNumber;
            entity.ApprovalType = model.ApprovalType;
            entity.TradeDescription = model.TradeDescription;
            entity.Color = model.Color;
            entity.Category = model.Category;
            entity.OffRoadSymbols = model.OffRoadSymbols;
            entity.MassG = model.MassG;
            entity.MassF1 = model.MassF1;
            entity.MassF2 = model.MassF2;
            entity.MassF3 = model.MassF3;
            entity.VehicleNumOfAxles = model.VehicleNumOfAxles;
            entity.VehicleMassO1 = model.VehicleMassO1;
            entity.VehicleMassO2 = model.VehicleMassO2;
            entity.Capacity = model.Capacity;
            entity.MaxPower = model.MaxPower;
            entity.Fuel = model.Fuel;
            entity.EnvironmentalCategory = model.EnvironmentalCategory;
            entity.VehicleDocumentNumber = model.VehicleDocumentNumber;
            entity.VehicleDocumentDate = model.VehicleDocumentDate.HasValue ? DateTime.SpecifyKind(model.VehicleDocumentDate.Value, DateTimeKind.Utc) : model.VehicleDocumentDate;


            entity.VehicleOwner = new List<VehicleOwner>();
            if (model.Owners != null && model.Owners.Any())
            {
                entity.VehicleOwner = model.Owners
                    .Select(x => x.ToEntity()).ToList();
            }

            entity.VehicleUser = new List<VehicleUser>();
            entity.VehicleUser = model.Users
                .Select(x => x.ToEntity()).ToList();

            return;
        }

        #endregion

        #region Aircraft
        public static AircraftViewModel ToViewModel(this Aircraft entity)
        {
            if (entity == null)
            {
                return null;
            }

            AircraftViewModel model = new AircraftViewModel
            {
                Id = entity.Id,
                ProducerName = entity.ProducerName,
                ProducerNameEn = entity.ProducerNameEn,
                ProducerCountryCode = entity.ProducerCountryCode,
                ProducerCountryName = entity.ProducerCountryName,
                AirCategoryCode = entity.AirCategoryCode,
                AirCategoryName = entity.AirCategoryName,
                Icao = entity.Icao,
                MsnserialNumber = entity.MsnserialNumber,
                ModelName = entity.ModelName,
                ModelNameEn = entity.ModelNameEn,

                Debts = entity.AircraftDebt
                .Select(x => x.ToViewModel()).ToList(),

                Registrations = entity.AircraftRegistration
                .Select(x => x.ToViewModel()).ToList(),
            };

            return model;
        }

        public static AircraftDebtViewModel ToViewModel(this AircraftDebt entity)
        {
            if (entity == null)
            {
                return null;
            }

            AircraftDebtViewModel model = new AircraftDebtViewModel
            {
                Id = entity.Id,
                AircraftId = entity.AircraftId,
                InputDate = entity.InputDate.HasValue ? DateTime.SpecifyKind(entity.InputDate.Value, DateTimeKind.Utc) : entity.InputDate,
                DebtTypeCode = entity.DebtTypeCode,
                DebtType = entity.DebtType,
                IsActive = entity.IsActive,
                ApplicantIdentifier = entity.ApplicantIdentifier,
                ApplicantName = entity.ApplicantName,
                DocumentIncomingNumber = entity.DocumentIncomingNumber,
                DocumentIncomingDate = entity.DocumentIncomingDate.HasValue ? DateTime.SpecifyKind(entity.DocumentIncomingDate.Value, DateTimeKind.Utc) : entity.DocumentIncomingDate,
                DocumentExternalNumber = entity.DocumentExternalNumber,
                DocumentExternalDate = entity.DocumentExternalDate.HasValue ? DateTime.SpecifyKind(entity.DocumentExternalDate.Value, DateTimeKind.Utc) : entity.DocumentExternalDate,
                RepaymentDate = entity.RepaymentDate.HasValue ? DateTime.SpecifyKind(entity.RepaymentDate.Value, DateTimeKind.Utc) : entity.RepaymentDate,
                RepaymentDocumentIncomingNumber = entity.RepaymentDocumentIncomingNumber,
                RepaymentDocumentIncomingDate = entity.RepaymentDocumentIncomingDate.HasValue ? DateTime.SpecifyKind(entity.RepaymentDocumentIncomingDate.Value, DateTimeKind.Utc) : entity.RepaymentDocumentIncomingDate,
                RepaymentDocumentExternalNumber = entity.RepaymentDocumentExternalNumber,
                RepaymentDocumentExternalDate = entity.RepaymentDocumentExternalDate.HasValue ? DateTime.SpecifyKind(entity.RepaymentDocumentExternalDate.Value, DateTimeKind.Utc) : entity.RepaymentDocumentExternalDate,
                RepaymentNotes = entity.RepaymentNotes,
                Notes = entity.Notes,

            };

            return model;
        }

        public static AircraftRegistrationOwnerPersonViewModel ToViewModel(this AircraftRegistrationOwnerPerson entity)
        {
            if (entity == null)
            {
                return null;
            }

            AircraftRegistrationOwnerPersonViewModel model = new AircraftRegistrationOwnerPersonViewModel
            {
                Id = entity.Id,
                RegistrationId = entity.RegistrationId,
                Identifier = entity.Identifier,
                Names = entity.Names,
            };

            return model;
        }

        public static AircraftRegistrationOwnerEntityViewModel ToViewModel(this AircraftRegistrationOwnerEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            AircraftRegistrationOwnerEntityViewModel model = new AircraftRegistrationOwnerEntityViewModel
            {
                Id = entity.Id,
                RegistrationId = entity.RegistrationId,
                Identifier = entity.Identifier,
                Name = entity.Name,
            };

            return model;
        }

        public static AircraftRegistrationOperatorPersonViewModel ToViewModel(this AircraftRegistrationOperatorPerson entity)
        {
            if (entity == null)
            {
                return null;
            }

            AircraftRegistrationOperatorPersonViewModel model = new AircraftRegistrationOperatorPersonViewModel
            {
                Id = entity.Id,
                RegistrationId = entity.RegistrationId,
                Identifier = entity.Identifier,
                Names = entity.Names,
            };

            return model;
        }

        public static AircraftRegistrationOperatorEntityViewModel ToViewModel(this AircraftRegistrationOperatorEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            AircraftRegistrationOperatorEntityViewModel model = new AircraftRegistrationOperatorEntityViewModel
            {
                Id = entity.Id,
                RegistrationId = entity.RegistrationId,
                Identifier = entity.Identifier,
                Name = entity.Name,
            };

            return model;
        }

        public static AircraftRegistrationViewModel ToViewModel(this AircraftRegistration entity)
        {
            if (entity == null)
            {
                return null;
            }

            AircraftRegistrationViewModel model = new AircraftRegistrationViewModel
            {
                Id = entity.Id,
                AircraftId = entity.AircraftId,
                ActNumber = entity.ActNumber,
                RegistrationNumber = entity.RegistrationNumber,
                RegistrationDate = entity.RegistrationDate.HasValue ? DateTime.SpecifyKind(entity.RegistrationDate.Value, DateTimeKind.Utc) : entity.RegistrationDate,
                RegistrationMark = entity.RegistrationMark,
                IsLastRegistration = entity.IsLastRegistration,
                RegistrationStatusIsActive = entity.RegistrationStatusIsActive,
                RegistrationStatusCode = entity.RegistrationStatusCode,
                RegistrationStatusName = entity.RegistrationStatusName,
                RegistrationDocumentNumber = entity.RegistrationDocumentNumber,
                RegistrationDocumentDate = entity.RegistrationDocumentDate.HasValue ? DateTime.SpecifyKind(entity.RegistrationDocumentDate.Value, DateTimeKind.Utc) : entity.RegistrationDocumentDate,
                RegistrationDocumentDescription = entity.RegistrationDocumentDescription,
                DeregistrationDate = entity.DeregistrationDate.HasValue ? DateTime.SpecifyKind(entity.DeregistrationDate.Value, DateTimeKind.Utc) : entity.DeregistrationDate,
                DeregistrationReason = entity.DeregistrationReason,
                DeregistrationDescription = entity.DeregistrationDescription,
                DeregistrationCountryCode = entity.DeregistrationCountryCode,
                DeregistrationCountryName = entity.DeregistrationCountryName,
                LeasingDocumentNumber = entity.LeasingDocumentNumber,
                LeasingDocumentDate = entity.LeasingDocumentDate.HasValue ? DateTime.SpecifyKind(entity.LeasingDocumentDate.Value, DateTimeKind.Utc) : entity.LeasingDocumentDate,
                LeasingEndDate = entity.LeasingEndDate.HasValue ? DateTime.SpecifyKind(entity.LeasingEndDate.Value, DateTimeKind.Utc) : entity.LeasingEndDate,
                LeasingAgreement = entity.LeasingAgreement,
                LeasingLessorPersonIdentifier = entity.LeasingLessorPersonIdentifier,
                LeasingLessorPersonNames = entity.LeasingLessorPersonNames,
                LeasingLessorEntityIdentifier = entity.LeasingLessorEntityIdentifier,
                LeasingLessorEntityName = entity.LeasingLessorEntityName,

                OperatorEntities = entity.AircraftRegistrationOperatorEntity
                        .Select(op => op.ToViewModel()).ToList(),

                OperatorPeople = entity.AircraftRegistrationOperatorPerson
                        .Select(op => op.ToViewModel()).ToList(),

                OwnerEntities = entity.AircraftRegistrationOwnerEntity
                        .Select(op => op.ToViewModel()).ToList(),

                OwnerPeople = entity.AircraftRegistrationOwnerPerson
                        .Select(op => op.ToViewModel()).ToList(),
            };

            return model;
        }



        public static Aircraft ToEntity(this AircraftViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            Aircraft entity = new Aircraft
            {
                Id = model.Id,
                ProducerName = model.ProducerName,
                ProducerNameEn = model.ProducerNameEn,
                ProducerCountryCode = model.ProducerCountryCode,
                ProducerCountryName = model.ProducerCountryName,
                AirCategoryCode = model.AirCategoryCode,
                AirCategoryName = model.AirCategoryName,
                Icao = model.Icao,
                MsnserialNumber = model.MsnserialNumber,
                ModelName = model.ModelName,
                ModelNameEn = model.ModelNameEn,
            };

            if (model.Debts != null && model.Debts.Any())
                entity.AircraftDebt = model.Debts
                .Select(x => x.ToEntity()).ToList();

            if (model.Registrations != null && model.Registrations.Any())
                entity.AircraftRegistration = model.Registrations
                .Select(x => x.ToEntity()).ToList();

            entity.AircraftExtension = new AircraftExtension
            {
                AircraftId = entity.Id,
                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                RequestId = model.ExtensionRequestId,
                Deactivated = false
            };

            return entity;
        }

        public static AircraftRegistrationOwnerPerson ToEntity(this AircraftRegistrationOwnerPersonViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            AircraftRegistrationOwnerPerson entity = new AircraftRegistrationOwnerPerson
            {
                Id = model.Id,
                RegistrationId = model.RegistrationId,
                Identifier = model.Identifier,
                Names = model.Names,
            };

            return entity;
        }

        public static AircraftRegistrationOwnerEntity ToEntity(this AircraftRegistrationOwnerEntityViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            AircraftRegistrationOwnerEntity entity = new AircraftRegistrationOwnerEntity
            {
                Id = model.Id,
                RegistrationId = model.RegistrationId,
                Identifier = model.Identifier,
                Name = model.Name,
            };

            return entity;
        }

        public static AircraftRegistrationOperatorPerson ToEntity(this AircraftRegistrationOperatorPersonViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            AircraftRegistrationOperatorPerson entity = new AircraftRegistrationOperatorPerson
            {
                Id = model.Id,
                RegistrationId = model.RegistrationId,
                Identifier = model.Identifier,
                Names = model.Names,
            };

            return entity;
        }

        public static AircraftRegistrationOperatorEntity ToEntity(this AircraftRegistrationOperatorEntityViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            AircraftRegistrationOperatorEntity entity = new AircraftRegistrationOperatorEntity
            {
                Id = model.Id,
                RegistrationId = model.RegistrationId,
                Identifier = model.Identifier,
                Name = model.Name,
            };

            return entity;
        }

        public static AircraftRegistration ToEntity(this AircraftRegistrationViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            AircraftRegistration entity = new AircraftRegistration
            {
                Id = model.Id,
                AircraftId = model.AircraftId,
                ActNumber = model.ActNumber,
                RegistrationNumber = model.RegistrationNumber,
                RegistrationDate = model.RegistrationDate.HasValue ? DateTime.SpecifyKind(model.RegistrationDate.Value, DateTimeKind.Utc) : model.RegistrationDate,
                RegistrationMark = model.RegistrationMark,
                IsLastRegistration = model.IsLastRegistration,
                RegistrationStatusIsActive = model.RegistrationStatusIsActive,
                RegistrationStatusCode = model.RegistrationStatusCode,
                RegistrationStatusName = model.RegistrationStatusName,
                RegistrationDocumentNumber = model.RegistrationDocumentNumber,
                RegistrationDocumentDate = model.RegistrationDocumentDate.HasValue ? DateTime.SpecifyKind(model.RegistrationDocumentDate.Value, DateTimeKind.Utc) : model.RegistrationDocumentDate,
                RegistrationDocumentDescription = model.RegistrationDocumentDescription,
                DeregistrationDate = model.DeregistrationDate.HasValue ? DateTime.SpecifyKind(model.DeregistrationDate.Value, DateTimeKind.Utc) : model.DeregistrationDate,
                DeregistrationReason = model.DeregistrationReason,
                DeregistrationDescription = model.DeregistrationDescription,
                DeregistrationCountryCode = model.DeregistrationCountryCode,
                DeregistrationCountryName = model.DeregistrationCountryName,
                LeasingDocumentNumber = model.LeasingDocumentNumber,
                LeasingDocumentDate = model.LeasingDocumentDate.HasValue ? DateTime.SpecifyKind(model.LeasingDocumentDate.Value, DateTimeKind.Utc) : model.LeasingDocumentDate,
                LeasingEndDate = model.LeasingEndDate.HasValue ? DateTime.SpecifyKind(model.LeasingEndDate.Value, DateTimeKind.Utc) : model.LeasingEndDate,
                LeasingAgreement = model.LeasingAgreement,
                LeasingLessorPersonIdentifier = model.LeasingLessorPersonIdentifier,
                LeasingLessorPersonNames = model.LeasingLessorPersonNames,
                LeasingLessorEntityIdentifier = model.LeasingLessorEntityIdentifier,
                LeasingLessorEntityName = model.LeasingLessorEntityName,

                AircraftRegistrationOperatorEntity = (model.OperatorEntities != null && model.OperatorEntities.Any()) ?
                        model.OperatorEntities.Select(op => op.ToEntity()).ToList() : null,

                AircraftRegistrationOperatorPerson = (model.OperatorPeople != null && model.OperatorPeople.Any()) ?
                        model.OperatorPeople.Select(op => op.ToEntity()).ToList() : null,

                AircraftRegistrationOwnerEntity = (model.OwnerEntities != null && model.OwnerEntities.Any()) ?
                        model.OwnerEntities.Select(op => op.ToEntity()).ToList() : null,

                AircraftRegistrationOwnerPerson = (model.OwnerPeople != null && model.OwnerPeople.Any()) ?
                        model.OwnerPeople.Select(op => op.ToEntity()).ToList() : null,
            };

            return entity;
        }

        public static AircraftDebt ToEntity(this AircraftDebtViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            AircraftDebt entity = new AircraftDebt
            {
                Id = model.Id,
                AircraftId = model.AircraftId,
                InputDate = model.InputDate.HasValue ? DateTime.SpecifyKind(model.InputDate.Value, DateTimeKind.Utc) : model.InputDate,
                DebtTypeCode = model.DebtTypeCode,
                DebtType = model.DebtType,
                IsActive = model.IsActive,
                ApplicantIdentifier = model.ApplicantIdentifier,
                ApplicantName = model.ApplicantName,
                DocumentIncomingNumber = model.DocumentIncomingNumber,
                DocumentIncomingDate = model.DocumentIncomingDate.HasValue ? DateTime.SpecifyKind(model.DocumentIncomingDate.Value, DateTimeKind.Utc) : model.DocumentIncomingDate,
                DocumentExternalNumber = model.DocumentExternalNumber,
                DocumentExternalDate = model.DocumentExternalDate.HasValue ? DateTime.SpecifyKind(model.DocumentExternalDate.Value, DateTimeKind.Utc) : model.DocumentExternalDate,
                RepaymentDate = model.RepaymentDate.HasValue ? DateTime.SpecifyKind(model.RepaymentDate.Value, DateTimeKind.Utc) : model.RepaymentDate,
                RepaymentDocumentIncomingNumber = model.RepaymentDocumentIncomingNumber,
                RepaymentDocumentIncomingDate = model.RepaymentDocumentIncomingDate.HasValue ? DateTime.SpecifyKind(model.RepaymentDocumentIncomingDate.Value, DateTimeKind.Utc) : model.RepaymentDocumentIncomingDate,
                RepaymentDocumentExternalNumber = model.RepaymentDocumentExternalNumber,
                RepaymentDocumentExternalDate = model.RepaymentDocumentExternalDate.HasValue ? DateTime.SpecifyKind(model.RepaymentDocumentExternalDate.Value, DateTimeKind.Utc) : model.RepaymentDocumentExternalDate,
                RepaymentNotes = model.RepaymentNotes,
                Notes = model.Notes,
            };

            return entity;
        }


        public static void UpdateEntity(this Data.Aircraft entity, AircraftViewModel model)
        {
            if (model == null || entity == null)
            {
                return;
            }

            entity.Id = model.Id;
            entity.ProducerName = model.ProducerName;
            entity.ProducerNameEn = model.ProducerNameEn;
            entity.ProducerCountryCode = model.ProducerCountryCode;
            entity.ProducerCountryName = model.ProducerCountryName;
            entity.AirCategoryCode = model.AirCategoryCode;
            entity.AirCategoryName = model.AirCategoryName;
            entity.Icao = model.Icao;
            entity.MsnserialNumber = model.MsnserialNumber;
            entity.ModelName = model.ModelName;
            entity.ModelNameEn = model.ModelNameEn;

            entity.AircraftDebt = new List<AircraftDebt>();
            if (model.Debts != null && model.Debts.Any())
            {
                entity.AircraftDebt = model.Debts
                .Select(x => x.ToEntity()).ToList();
            }

            entity.AircraftRegistration = new List<AircraftRegistration>();
            if (model.Registrations != null && model.Registrations.Any())
            {
                entity.AircraftRegistration = model.Registrations
                .Select(x => x.ToEntity()).ToList();
            }

            return;
        }

        #endregion

        #region Vessel
        public static VesselViewModel ToViewModel(this Vessel entity)
        {
            if (entity == null)
            {
                return null;
            }

            VesselViewModel model = new VesselViewModel
            {
                Id = entity.Id,
                Bt = entity.Bt,
                Nt = entity.Nt,
                MaxLength = entity.MaxLength,
                LengthBetweenPerpendiculars = entity.LengthBetweenPerpendiculars,
                MaxWidth = entity.MaxWidth,
                Waterplane = entity.Waterplane,
                ShipboardHeight = entity.ShipboardHeight,
                Deadweight = entity.Deadweight,
                NumberOfEngines = entity.NumberOfEngines,
                EnginesFuel = entity.EnginesFuel,
                SumEnginePower = entity.SumEnginePower,
                BodyNumber = entity.BodyNumber,
                ExtensionRequestId = entity.VesselExtension?.RequestId,

                Engines = entity.VesselEngine
                .Select(x => x.ToViewModel()).ToList(),

                Owners = entity.VesselOwner
                .Select(x => x.ToViewModel()).ToList(),

                RegistrationData = entity.VesselRegistrationData
                .Select(x => x.ToViewModel()).FirstOrDefault()
            };

            return model;
        }

        public static VesselEngineViewModel ToViewModel(this VesselEngine entity)
        {
            if (entity == null)
            {
                return null;
            }

            VesselEngineViewModel model = new VesselEngineViewModel
            {
                Id = entity.Id,
                VesselId = entity.VesselId,
                SystemModification = entity.SystemModification,
                EngineNumber = entity.EngineNumber,
                Power = entity.Power,
                Type = entity.Type
            };

            return model;
        }

        public static VesselOwnerViewModel ToViewModel(this VesselOwner entity)
        {
            if (entity == null)
            {
                return null;
            }

            VesselOwnerViewModel model = new VesselOwnerViewModel
            {
                Id = entity.Id,
                VesselId = entity.VesselId,
                IsCompany = entity.IsCompany,
                CompanyName = entity.CompanyName,
                Eik = entity.Eik,
                PersonFirstName = entity.PersonFirstName,
                PersonMiddleName = entity.PersonMiddleName,
                PersonLastName = entity.PersonLastName,
                Egn = entity.Egn,
                ImoNumber = entity.ImoNumber,
                IsUser = entity.IsUser,
                Address = entity.Address,
            };

            return model;
        }

        public static VesselRegistrationDataViewData ToViewModel(this VesselRegistrationData entity)
        {
            if (entity == null)
            {
                return null;
            }

            VesselRegistrationDataViewData model = new VesselRegistrationDataViewData
            {
                Id = entity.Id,
                VesselId = entity.VesselId,
                ShipName = entity.ShipName,
                ShipNameLatin = entity.ShipNameLatin,
                RegistrationPort = entity.RegistrationPort,
                RegistrationNumber = entity.RegistrationNumber,
                Tom = entity.Tom,
                Page = entity.Page,
                Type = entity.Type,
                Status = entity.Status,
                StatusName = entity.StatusNavigation?.Name,
                StatusNameEn = entity.StatusNavigation?.NameEn,
            };

            return model;
        }


        public static Vessel ToEntity(this VesselViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            Vessel entity = new Vessel
            {
                Id = model.Id,
                Bt = model.Bt,
                Nt = model.Nt,
                MaxLength = model.MaxLength,
                LengthBetweenPerpendiculars = model.LengthBetweenPerpendiculars,
                MaxWidth = model.MaxWidth,
                Waterplane = model.Waterplane,
                ShipboardHeight = model.ShipboardHeight,
                Deadweight = model.Deadweight,
                NumberOfEngines = model.NumberOfEngines,
                EnginesFuel = model.EnginesFuel,
                SumEnginePower = model.SumEnginePower,
                BodyNumber = model.BodyNumber
            };

            if (model.Engines != null && model.Engines.Any())
                entity.VesselEngine = model.Engines
                .Select(x => x.ToEntity()).ToList();

            if (model.Owners != null && model.Owners.Any())
                entity.VesselOwner = model.Owners
                .Select(x => x.ToEntity()).ToList();

            if (model.RegistrationData != null)
            {
                entity.VesselRegistrationData = new List<VesselRegistrationData>();
                entity.VesselRegistrationData.Add(model.RegistrationData.ToEntity());
            }

            entity.VesselExtension = new VesselExtension
            {
                VesselId = entity.Id,
                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                RequestId = model.ExtensionRequestId,
                Deactivated = false
            };

            return entity;
        }

        public static VesselEngine ToEntity(this VesselEngineViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            VesselEngine entity = new VesselEngine
            {
                Id = model.Id,
                VesselId = model.VesselId,
                SystemModification = model.SystemModification,
                EngineNumber = model.EngineNumber,
                Power = model.Power,
                Type = model.Type
            };

            return entity;
        }

        public static VesselOwner ToEntity(this VesselOwnerViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            VesselOwner entity = new VesselOwner
            {
                Id = model.Id,
                VesselId = model.VesselId,
                IsCompany = model.IsCompany,
                CompanyName = model.CompanyName,
                Eik = model.Eik,
                PersonFirstName = model.PersonFirstName,
                PersonMiddleName = model.PersonMiddleName,
                PersonLastName = model.PersonLastName,
                Egn = model.Egn,
                ImoNumber = model.ImoNumber,
                IsUser = model.IsUser,
                Address = model.Address,
            };

            return entity;
        }

        public static VesselRegistrationData ToEntity(this VesselRegistrationDataViewData model)
        {
            if (model == null)
            {
                return null;
            }

            VesselRegistrationData entity = new VesselRegistrationData
            {
                Id = model.Id,
                VesselId = model.VesselId,
                ShipName = model.ShipName,
                ShipNameLatin = model.ShipNameLatin,
                RegistrationPort = model.RegistrationPort,
                RegistrationNumber = model.RegistrationNumber,
                Tom = model.Tom,
                Page = model.Page,
                Type = model.Type,
                Status = model.Status,
            };

            return entity;
        }


        public static void UpdateEntity(this Data.Vessel entity, VesselViewModel model)
        {
            if (model == null || entity == null)
            {
                return;
            }

            entity.Id = model.Id;
            entity.Bt = model.Bt;
            entity.Nt = model.Nt;
            entity.MaxLength = model.MaxLength;
            entity.LengthBetweenPerpendiculars = model.LengthBetweenPerpendiculars;
            entity.MaxWidth = model.MaxWidth;
            entity.Waterplane = model.Waterplane;
            entity.ShipboardHeight = model.ShipboardHeight;
            entity.Deadweight = model.Deadweight;
            entity.NumberOfEngines = model.NumberOfEngines;
            entity.EnginesFuel = model.EnginesFuel;
            entity.SumEnginePower = model.SumEnginePower;
            entity.BodyNumber = model.BodyNumber;

            entity.VesselEngine = new List<VesselEngine>();
            if (model.Engines != null && model.Engines.Any())
            {
                entity.VesselEngine = model.Engines
                .Select(x => x.ToEntity()).ToList();
            }

            entity.VesselOwner = new List<VesselOwner>();
            if (model.Owners != null && model.Owners.Any())
            {
                entity.VesselOwner = model.Owners
                .Select(x => x.ToEntity()).ToList();
            }

            entity.VesselRegistrationData = new List<VesselRegistrationData>();
            if (model.RegistrationData != null)
            {
                entity.VesselRegistrationData.Add(model.RegistrationData.ToEntity());
            }

            return;
        }

        #endregion

        #region Agricultural machinery

        public static AgriculturalMachineryViewModel ToViewModel(this AgriculturalMachinery entity)
        {
            if (entity == null)
            {
                return null;
            }

            AgriculturalMachineryViewModel model = new AgriculturalMachineryViewModel
            {
                Id = entity.Id,
                RegistrationNumber = entity.RegistrationNumber,
                FrameNumber = entity.FrameNumber,
                Type = entity.Type,
            };

            return model;
        }

        #endregion

        public static PropertyModel ToViewModel(this Property e)
        {
            return e == null
                ? null
                : new PropertyModel
                {
                    Id = e.Id,
                    Type = e.Type,
                    Floor = e.Floor,
                    Area = e.Area,
                    PropertyConstructionTypeId = e.PropertyConstructionTypeId,
                    Identifier = e.Identifier,
                    IdentifierType = e.IdentifierType,
                    Description = e.Description,
                    IsManuallyAdded = e.IsManuallyAdded,
                    Address = e.Address != null 
                        ? new AddressModel
                        {
                            Id = e.Address.Id,
                            RegionId = e.Address.RegionId,
                            MunicipalityId = e.Address.MunicipalityId,
                            CityId = e.Address.CityId,
                            StreetAddress = e.Address.StreetAddress
                        }
                        : null
                };
        }
    }
}
