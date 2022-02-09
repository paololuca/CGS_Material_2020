
DECLARE @idTorneo INT
set @idtorneo = 1;

DECLARE @idDisciplina INT
set @idDisciplina = 1

select (a.Cognome + ' ' + a.Nome) as Name, 'IT' as Nationality, a.Sesso as Gender  
from Atleti a join AtletiVsTorneoVsDiscipline atd on a.Id = atd.IdAtleta
join TorneoVsDiscipline td on td.Id = atd.IdTorneoVsDiscipline
where td.IdTorneo = @idTorneo
and td.IdDisciplina = @idDisciplina
order by a.Cognome

select * from Gironi
select * from GironiIncontri

select 
--gi.IdAtletaRosso, 
gi.PuntiAtletaRosso, 
(rosso.Cognome + ' ' + Rosso.Nome) as Fighter1,
(blu.Cognome + ' ' + blu.Nome) as Fighter2,
case 
	when gi.PuntiAtletaRosso > gi.PuntiAtletaBlu and gi.PuntiAtletaBlu < 3 then 'Win'
	when gi.PuntiAtletaRosso < gi.PuntiAtletaBlu and gi.PuntiAtletaRosso < 3 then 'Loss'
	when gi.PuntiAtletaRosso = gi.PuntiAtletaRosso then 'Draw'
	else 'Draw' 
	END as Fighter_1_Result,

--gi.IdAtletaBlu, 
gi.PuntiAtletaBlu, 
case 
	when gi.PuntiAtletaBlu > gi.PuntiAtletaRosso and gi.PuntiAtletaRosso < 3 then 'Win'
	when gi.PuntiAtletaBlu < gi.PuntiAtletaRosso and gi.PuntiAtletaBlu < 3 then 'Loss'
	when gi.PuntiAtletaBlu = gi.PuntiAtletaRosso then 'Draw'
	else 'Draw' 
	END as Fighter_2_Result,

gi.NumeroGirone as Pool 

from GironiIncontri gi
join Atleti rosso on rosso.id = gi.IdAtletaRosso
join Atleti blu on blu.id = gi.IdAtletaBlu
where gi.IdTorneo = @idTorneo
and gi.IdDisciplina = @idDisciplina
order by NumeroGirone

