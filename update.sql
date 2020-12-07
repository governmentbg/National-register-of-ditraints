-- 23.10.2020 Angel
alter table "N"."UserRegisterType"
add "IsPublic" boolean not null default true;

update "N"."UserRegisterType"
set "IsPublic"  = false
where "Code" = 'CHSI';

-- end