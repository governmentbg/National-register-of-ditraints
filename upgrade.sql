-- 26.10.2020 Angel
alter table "public"."AspNetUsers"
add column "CHSINumber" integer null;

alter table "public"."AspNetUsers"
add column "CheckedInCHSIRegister" boolean null;

insert into "N"."UserType" ("Code", "Name", "NameEn", "Deactivated", "Sort")
values ('CHSIHelper', 'Помощник на ЧСИ', 'Private court enforcer hepler', false, 15)
--end

-- 28-10-2020

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "Location" text;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "Bailiff" text;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "Syndic" text;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "Insolvent" text;


ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "EnforcementDate" timestamp without time zone;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "EnforcedBy" text;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "EnforcedAt" timestamp without time zone;



UPDATE "N"."DistraintStatus"
set "Code" = 'ENFORCED', "NameEn" = 'enforced'
where "Code" = 'APPLIED'


-- 29-10-2020

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "RevocationDate" timestamp without time zone;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "RevokedBy" text;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "RevokedAt" timestamp without time zone;


insert into "N"."DistraintStatus"
values ('REVOKED', 'отменен', 'revoked', false);


ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "ExemptionDate" timestamp without time zone;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "ExemptedBy" text;

ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "ExemptedAt" timestamp without time zone;


insert into "N"."DistraintStatus"
values ('EXEMPTED', 'вдигнат', 'exempted', false);

-- 4-11-2020

ALTER TABLE "Distraint"
DROP COLUMN IF EXISTS "Bailiff",
DROP COLUMN IF EXISTS "Syndic";

-- 5-11-2020

ALTER TABLE "Distraint"
DROP COLUMN IF EXISTS "Insolvent";

update "Distraint"
set "EnforcementDate" = "ApplyDate"
where "ApplyDate" is not null
and "EnforcementDate" is null


ALTER TABLE "Distraint"
DROP COLUMN IF EXISTS "ApplyDate";

ALTER TABLE "Distraint"
DROP COLUMN IF EXISTS "CancelDate";


update "Distraint"
set "StatusCode" = 'ENFORCED', "EnforcedBy" = "CreatedBy", "EnforcedAt" = "CreatedOn"
where "StatusCode" = 'REGISTERED'


-- 9-11-2020


CREATE SEQUENCE public."RegixPerson_Id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."RegixPerson_Id_seq"
    OWNER TO postgres;



CREATE TABLE public."RegixPerson"
(
    "Id" integer NOT NULL DEFAULT nextval('"RegixPerson_Id_seq"'::regclass),
    "Identifier" text COLLATE pg_catalog."default" NOT NULL,
    "FirstName" text COLLATE pg_catalog."default",
    "MiddleName" text COLLATE pg_catalog."default",
    "LastName" text COLLATE pg_catalog."default",
    "DateOfBirth" timestamp without time zone,
    "DateOfDeath" timestamp without time zone,
    CONSTRAINT "RegixPerson_pkey" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."RegixPerson"
    OWNER to postgres;



ALTER TABLE "Distraint"
ADD COLUMN IF NOT EXISTS "InFavourOfPersonId" integer;

ALTER TABLE "Distraint" 
ADD CONSTRAINT "InFavourOfPerson_fkey" 
FOREIGN KEY ("InFavourOfPersonId") 
REFERENCES "RegixPerson" ("Id");

-- 17-11-2020

ALTER TABLE [NRZ].[dbo].RequestForCertificateOfDistraintOfProperty
ADD CreatedOn DATETIME2(7) NOT NULL;

ALTER TABLE [NRZ].[dbo].RequestForCertificateOfDistraintOfProperty
ADD IsPersonalIdentifierTypeLNCh BIT NULL;

ALTER TABLE [NRZ].[dbo].RequestForCertificateOfDistraintOfProperty ALTER COLUMN StreetAddress nvarchar(MAX) NULL

ALTER TABLE [NRZ].[dbo].RequestForCertificateOfDistraintOfProperty ALTER COLUMN StreetAddressOfLegalEntity nvarchar(MAX) NULL

-- ALTER TABLE [NRZ].[dbo].RequestForCertificateOfDistraintOfProperty DROP COLUMN DistraintId;