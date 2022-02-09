DECLARE @idTorneo INT
set @idtorneo = 2;

DECLARE @idDisciplina INT
set @idDisciplina = 3

declare @commandText varchar(MAX)
set @commandText = ''

declare @i int
set @i = 1

while (@i <= 8 )
begin

if (@i > 1) set @commandText = @commandText + ' UNION ';

set @commandText = @commandText + 'select rosso.*, blu.* from Qualificati16 rosso ,Qualificati16 blu where rosso.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and rosso.Posizione = '+CAST(@i as varchar(2))+' and blu.Posizione = (16 -('+CAST(@i as varchar(2))+'-1)) and rosso.Campo = 0 and blu.Campo = 0';
--print @commandText
set @i = @i +1;
end


set @i = 1
while (@i <= 4 )
begin

set @commandText = @commandText + ' UNION ';
set @commandText = @commandText + 'select rosso.*, blu.* from Qualificati16 rosso ,Qualificati16 blu where rosso.IdTorneo = '
+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))
+' and rosso.Posizione = 1 and blu.Posizione = 2 and rosso.Campo = blu.Campo and rosso.campo > 0';

set @commandText = @commandText + ' UNION ';
set @commandText = @commandText + 'select rosso.*, blu.* from Qualificati16 rosso ,Qualificati16 blu where rosso.IdTorneo = '
+CAST(@idTorneo as varchar(2))+' and rosso.IdDisciplina = '+CAST(@idDisciplina as varchar(2))+' and blu.IdTorneo = '+CAST(@idTorneo as varchar(2))+' and blu.IdDisciplina = '+CAST(@idDisciplina as varchar(2))
+' and rosso.Posizione = 3 and blu.Posizione = 4 and rosso.Campo = blu.Campo and rosso.campo > 0';
--print @commandText
set @i = @i +1;
end


EXEC (@commandText)

