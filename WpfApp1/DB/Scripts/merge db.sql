/*
select * from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.GironiIncontri where IdTorneo  = 29
select * from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.GironiIncontri where IdTorneo  = 29
select * from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.Qualificati16  where IdTorneo  = 29
select * from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Finali where IdTorneo  = 29
*/

declare @IdTorneo int
--set @IdTorneo = --id torneo femminile

delete from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Gironi where IdTorneo  = @IdTorneo
delete from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.GironiIncontri where IdTorneo  = @IdTorneo

insert into [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Gironi 
select * from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.Gironi where IdTorneo  = @IdTorneo

insert into [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.GironiIncontri  (IdTorneo, IdDisciplina, IdAtletaRosso, PuntiAtletaRosso, IdAtletaBlu, PuntiAtletaBlu, NumeroGirone, DoppiaMorte)
select IdTorneo, IdDisciplina, IdAtletaRosso, PuntiAtletaRosso, IdAtletaBlu, PuntiAtletaBlu, NumeroGirone, DoppiaMorte 
from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.GironiIncontri where IdTorneo  = @IdTorneo


delete [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Qualificati32 where IdTorneo  = @IdTorneo 
delete [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Qualificati16 where IdTorneo  = @IdTorneo 
delete [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Qualificati8 where IdTorneo  = @IdTorneo 
delete [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Semifinali where IdTorneo  = @IdTorneo 
delete [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Finali where IdTorneo  = @IdTorneo 

insert into [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Qualificati32 (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.Qualificati32 where IdTorneo  = @IdTorneo

insert into [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Qualificati16 (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.Qualificati16 where IdTorneo  = @IdTorneo

insert into [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Qualificati8 (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.Qualificati8 where IdTorneo  = @IdTorneo

insert into [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Semifinali (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo 
from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.Semifinali where IdTorneo  = @IdTorneo

insert into [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEI.MDF].dbo.Finali (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo, Round) 
select IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo, Round
from [C:\Users\PaoloLuca\Documents\source\GitHub\CGS_Material_2020\WpfApp1\DB\TORNEIFEMMINILE.MDF].dbo.Finali where IdTorneo  = @IdTorneo

