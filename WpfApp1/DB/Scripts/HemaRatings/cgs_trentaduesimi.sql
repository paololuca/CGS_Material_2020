DECLARE @idTorneo INT
set @idtorneo = 1;

DECLARE @idDisciplina INT
set @idDisciplina = 1

declare @commandText varchar(MAX)
set @commandText = ''

declare @i int
set @i = 1

while (@i <= 16 )
begin

if (@i > 1) set @commandText = @commandText + ' UNION ';

set @commandText = @commandText + 'select rosso.*, blu.* from Qualificati32 rosso ,Qualificati32 blu where rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Posizione = '+CAST(@i as varchar(2))+' and blu.Posizione = (32 -('+CAST(@i as varchar(2))+'-1)) and rosso.Campo = 0 and blu.Campo = 0';
--print @commandText
set @i = @i +1;
end

EXEC (@commandText)
