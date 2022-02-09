
DECLARE @idTorneo INT
set @idtorneo = 12;

DECLARE @idDisciplina INT
set @idDisciplina = 4


select (a.Nome + ' ' + a.Cognome) as Name, asd.Nome_ASD as Club, 'IT' as Nationality, a.Sesso as Gender, hr.IdHemaRatings as HemaRatingsId
from Atleti a join AtletiVsTorneoVsDiscipline atd on a.Id = atd.IdAtleta
join TorneoVsDiscipline td on td.Id = atd.IdTorneoVsDiscipline
join ASD asd on a.IdASD = asd.Id
left outer join HemaRatings hr on hr.IdAtleta = a.Id
where td.IdTorneo = @idTorneo
and td.IdDisciplina = @idDisciplina
order by a.Cognome

select 
--gi.IdAtletaRosso, 
--gi.PuntiAtletaRosso, 
(rosso.Nome + ' ' + Rosso.Cognome) as Fighter1,
(blu.Nome + ' ' + blu.Cognome) as Fighter2,
case 
	when gi.PuntiAtletaRosso > gi.PuntiAtletaBlu and gi.PuntiAtletaBlu < 3 then 'Win'
	when gi.PuntiAtletaRosso < gi.PuntiAtletaBlu and gi.PuntiAtletaRosso < 3 then 'Loss'
	when gi.PuntiAtletaRosso = gi.PuntiAtletaRosso then 'Draw'
	else 'Draw' 
	END as Fighter_1_Result,

--gi.IdAtletaBlu, 
--gi.PuntiAtletaBlu, 
case 
	when gi.PuntiAtletaBlu > gi.PuntiAtletaRosso and gi.PuntiAtletaRosso < 3 then 'Win'
	when gi.PuntiAtletaBlu < gi.PuntiAtletaRosso and gi.PuntiAtletaBlu < 3 then 'Loss'
	when gi.PuntiAtletaBlu = gi.PuntiAtletaRosso then 'Draw'
	else 'Draw' 
	END as Fighter_2_Result,

 ('Pool '+CAST(gi.NumeroGirone as varchar(2))) as Round 

from GironiIncontri gi
join Atleti rosso on rosso.id = gi.IdAtletaRosso
join Atleti blu on blu.id = gi.IdAtletaBlu
where gi.IdTorneo = @idTorneo
and gi.IdDisciplina = @idDisciplina
order by NumeroGirone


--sedicesimi
declare @commandText varchar(MAX)
set @commandText = ''

declare @i int
set @i = 1

while (@i <= 16 )
begin

if (@i > 1) set @commandText = @commandText + ' UNION ';

set @commandText = @commandText + 
'select (rosso.Nome + '' '' + Rosso.Cognome) as Fighter1, (blu.Nome + '' '' + blu.Cognome) as Fighter2, '+
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 3 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 3 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 3 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 3 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''16nd Final'' as Round '+ 
'from Atleti rosso, Atleti blu, ' +
'(select rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu from Qualificati32 rosso ,Qualificati32 blu where rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Posizione = '+CAST(@i as varchar(2))+' and blu.Posizione = (32 -('+CAST(@i as varchar(2))+'-1)) and rosso.Campo = 0 and blu.Campo = 0) as result '
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;
--print @commandText
set @i = @i +1;
end


--ottavi
EXEC (@commandText)

set @commandText = ''
set @i = 1

while (@i <= 8 )
begin

if (@i > 1) set @commandText = @commandText + ' UNION ';

set @commandText = @commandText + 'select rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  from Qualificati16 rosso ,Qualificati16 blu where rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Posizione = '+CAST(@i as varchar(2))+' and blu.Posizione = (16 -('+CAST(@i as varchar(2))+'-1)) and rosso.Campo = 0 and blu.Campo = 0';
set @i = @i +1;
end


set @i = 1
while (@i <= 4 )
begin

set @commandText = @commandText + ' UNION ';
set @commandText = @commandText + 'select rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu from Qualificati16 rosso ,Qualificati16 blu where rosso.IdTorneo = '
+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))
+' and rosso.Posizione = 1 and blu.Posizione = 2 and rosso.Campo = blu.Campo and rosso.campo > 0';

set @commandText = @commandText + ' UNION ';
set @commandText = @commandText + 'select rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  from Qualificati16 rosso ,Qualificati16 blu where rosso.IdTorneo = '
+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))
+' and rosso.Posizione = 3 and blu.Posizione = 4 and rosso.Campo = blu.Campo and rosso.campo > 0';
--print @commandText
set @i = @i +1;
end

set @commandText = 
'select (rosso.Nome + '' '' + Rosso.Cognome) as Fighter1, (blu.Nome + '' '' + blu.Cognome) as Fighter2, '+
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 3 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 3 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 3 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 3 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''Eights Final'' as Round '+ 
'from Atleti rosso, Atleti blu, (' +@commandText+') as result '
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;

EXEC (@commandText)


----- quarti


set @commandText = ''
set @i = 1

while (@i <= 4 )
begin

if (@i > 1) set @commandText = @commandText + ' UNION ';

set @commandText = @commandText + 'select rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  from Qualificati8 rosso ,Qualificati8 blu where rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Posizione = '+CAST(@i as varchar(2))+' and blu.Posizione = (8 -('+CAST(@i as varchar(2))+'-1)) and rosso.Campo = 0 and blu.Campo = 0';
--print @commandText
set @i = @i +1;
end


set @i = 1
while (@i <= 2 )
begin

set @commandText = @commandText + ' UNION ';
set @commandText = @commandText + 'select rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu from Qualificati8 rosso ,Qualificati8 blu where rosso.IdTorneo = '
+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))
+' and rosso.Posizione = 1 and blu.Posizione = 2 and rosso.Campo = blu.Campo and rosso.campo > 0';

set @commandText = @commandText + ' UNION ';
set @commandText = @commandText + 'select rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  from Qualificati8 rosso ,Qualificati8 blu where rosso.IdTorneo = '
+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))
+' and rosso.Posizione = 3 and blu.Posizione = 4 and rosso.Campo = blu.Campo and rosso.campo > 0';
--print @commandText
set @i = @i +1;
end

set @commandText = 
'select (rosso.Nome + '' '' + Rosso.Cognome) as Fighter1, (blu.Nome + '' '' + blu.Cognome) as Fighter2, '+
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 3 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 3 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 3 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 3 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''Quarter Final'' as Round '+ 
'from Atleti rosso, Atleti blu, (' +@commandText+') as result '
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;

EXEC (@commandText)

-- semifinali
set @commandText = 'select (rosso.Nome + '' '' + Rosso.Cognome) as Fighter1, (blu.Nome + '' '' + blu.Cognome) as Fighter2, '+
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 3 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 3 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 3 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 3 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''Semi Final'' as Round '+ 
'from Atleti rosso, Atleti blu, '+
'(select top(1) rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  '+
'from Semifinali rosso, Semifinali blu '+
'where rosso.Campo = blu.Campo and rosso.IdAtleta <> blu.IdAtleta '+
'and rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' '+
'and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Campo = 1 UNION '+
'select top(1) rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  '+
'from Semifinali rosso, Semifinali blu '+
'where rosso.Campo = blu.Campo and rosso.IdAtleta <> blu.IdAtleta '+
'and rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' '+
'and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Campo = 2) as result '+
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;

EXEC (@commandText)

--finali
set @commandText = 'select (rosso.Nome + '' '' + Rosso.Cognome) as Fighter1, (blu.Nome + '' '' + blu.Cognome) as Fighter2, '+
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 3 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 3 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 3 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 3 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''Final'' as Round '+ 
'from Atleti rosso, Atleti blu, '+
'(select top(1) rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  '+
'from Finali rosso, Finali blu '+
'where rosso.Campo = blu.Campo and rosso.IdAtleta <> blu.IdAtleta '+
'and rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' '+
'and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Campo = 1 UNION '+
'select top(1) rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  '+
'from Finali rosso, Finali blu '+
'where rosso.Campo = blu.Campo and rosso.IdAtleta <> blu.IdAtleta '+
'and rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' '+
'and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Campo = 2) as result '+
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;

EXEC (@commandText)





