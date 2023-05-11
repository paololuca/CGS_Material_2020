--update in tabella ranking per disciplina ed atleta

update ranking set Punteggio = @punteggio, LastUdate = GETDATE(), Posizionamento = @posizionamento where IdAtleta = @atleta and IdDisciplina = @iddisciplina


--insert into storico con nuova fase
insert into RankingStorico 
select IdAtleta, Punteggio, GETDATE(), GETDATE(), Posizionamento, IdDisciplina, 2022, @newPhase from ranking