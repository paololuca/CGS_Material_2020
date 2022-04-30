/*
select * from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.GironiIncontri where IdTorneo  = 3
select * from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.GironiIncontri where IdTorneo  = 3
select * from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.Qualificati16  where IdTorneo  = 3
select * from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Qualificati16 where IdTorneo  = 3 
*/

declare @IdTorneo int
--set @IdTorneo = --id torneo femminile

delete from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Gironi where IdTorneo  = @IdTorneo
delete from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.GironiIncontri where IdTorneo  = @IdTorneo

insert into [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Gironi 
select * from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.Gironi where IdTorneo  = @IdTorneo

insert into [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.GironiIncontri  (IdTorneo, IdDisciplina, IdAtletaRosso, PuntiAtletaRosso, IdAtletaBlu, PuntiAtletaBlu, NumeroGirone, DoppiaMorte)
select IdTorneo, IdDisciplina, IdAtletaRosso, PuntiAtletaRosso, IdAtletaBlu, PuntiAtletaBlu, NumeroGirone, DoppiaMorte 
from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.GironiIncontri where IdTorneo  = @IdTorneo


delete [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Qualificati32 where IdTorneo  = @IdTorneo 
delete [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Qualificati16 where IdTorneo  = @IdTorneo 
delete [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Qualificati8 where IdTorneo  = @IdTorneo 
delete [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Semifinali where IdTorneo  = @IdTorneo 
delete [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Finali where IdTorneo  = @IdTorneo 

insert into [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Qualificati32 (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.Qualificati32 where IdTorneo  = @IdTorneo

insert into [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Qualificati16 (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.Qualificati16 where IdTorneo  = @IdTorneo

insert into [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Qualificati8 (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.Qualificati8 where IdTorneo  = @IdTorneo

insert into [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Semifinali (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.Semifinali where IdTorneo  = @IdTorneo

insert into [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEI.MDF].dbo.Finali (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\USERS\PAOLO\SOURCE\GITHUB\CGS_MATERIAL_2020\WPFAPP1\DB\TORNEIFEMMINILE.MDF].dbo.Finali where IdTorneo  = @IdTorneo

