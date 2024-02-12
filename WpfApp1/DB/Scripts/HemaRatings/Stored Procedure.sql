CREATE PROCEDURE [dbo].[HemaRatingSemiFinal]
	@idTorneo int,
	@idDisciplina int
AS

	declare @commandText varchar(MAX)
set @commandText = ''

	set @commandText = 'select (rosso.Nome + '' '' + Rosso.Cognome) as Fighter1, (blu.Nome + '' '' + blu.Cognome) as Fighter2, '+
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 5 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 5 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 5 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 5 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''Semi Final'' as Round '+ 
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
RETURN 0


CREATE PROCEDURE [dbo].[HemaRatingFinal]
	@idTorneo int,
	@idDisciplina int
AS

	declare @commandText varchar(MAX)
set @commandText = ''

	set @commandText = 'select (rosso.Nome + '' '' + Rosso.Cognome) as Fighter1, (blu.Nome + '' '' + blu.Cognome) as Fighter2, '+
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 5 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 5 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 5 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 5 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''Final'' as Round '+ 
'from Atleti rosso, Atleti blu, '+
'(select top(1) rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu  '+
'from Finali rosso, Finali blu '+
'where rosso.Campo = blu.Campo and rosso.IdAtleta <> blu.IdAtleta '+
'and rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' '+
'and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' '+
'and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Campo = 1) as result '+
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;

EXEC (@commandText)
RETURN 0


CREATE PROCEDURE [dbo].[HemaRating8Final]
	@idTorneo int,
	@idDisciplina int
AS

	declare @commandText varchar(MAX)
set @commandText = ''

declare @i int
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
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 5 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 5 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 5 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 5 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''Eights Final'' as Round '+ 
'from Atleti rosso, Atleti blu, (' +@commandText+') as result '
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;

EXEC (@commandText)
RETURN 0

CREATE PROCEDURE [dbo].[HemaRating4Final]
	@idTorneo int,
	@idDisciplina int
AS

	declare @commandText varchar(MAX)
set @commandText = ''

declare @i int
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
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 5 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 5 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 5 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 5 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''Quarter Final'' as Round '+ 
'from Atleti rosso, Atleti blu, (' +@commandText+') as result '
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;

EXEC (@commandText)
RETURN 0


CREATE PROCEDURE [dbo].[HemaRating16Final]
	@idTorneo int,
	@idDisciplina int
AS
	declare @commandText varchar(MAX)
set @commandText = ''

declare @i int
set @i = 1

while (@i <= 16 )
begin

if (@i > 1) set @commandText = @commandText + ' UNION ';

set @commandText = @commandText + 
'select (rosso.Nome + '' '' + Rosso.Cognome) as Fighter1, (blu.Nome + '' '' + blu.Cognome) as Fighter2, '+
'case when result.PuntiAtletaRosso > result.PuntiAtletaBlu and result.PuntiAtletaBlu < 5 then ''Win'' when result.PuntiAtletaRosso < result.PuntiAtletaBlu and result.PuntiAtletaRosso < 5 then ''Loss'' 	when result.PuntiAtletaRosso = result.PuntiAtletaRosso then ''Draw'' 	else ''Draw'' END as Fighter_1_Result, '+
'case  when result.PuntiAtletaBlu > result.PuntiAtletaRosso and result.PuntiAtletaRosso < 5 then ''Win''when result.PuntiAtletaBlu < result.PuntiAtletaRosso and result.PuntiAtletaBlu < 5 then ''Loss'' when result.PuntiAtletaBlu = result.PuntiAtletaRosso then ''Draw'' else ''Draw''  END as Fighter_2_Result, ''16nd Final'' as Round '+ 
'from Atleti rosso, Atleti blu, ' +
'(select rosso.IdAtleta as idRosso, rosso.PuntiFatti as PuntiAtletaRosso, blu.IdAtleta as idBlu, blu.PuntiFatti as PuntiAtletaBlu from Qualificati32 rosso ,Qualificati32 blu where rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Posizione = '+CAST(@i as varchar(2))+' and blu.Posizione = (32 -('+CAST(@i as varchar(2))+'-1)) and rosso.Campo = 0 and blu.Campo = 0) as result '
+ ' where rosso.Id = result.idRosso and blu.Id = result.idBlu' ;
--print @commandText
set @i = @i +1;
end


--ottavi
EXEC (@commandText)

RETURN 0