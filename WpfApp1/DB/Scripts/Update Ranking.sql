--update in tabella ranking per disciplina ed atleta

update ranking set Punteggio = @punteggio, LastUdate = GETDATE(), Posizionamento = @posizionamento where IdAtleta = @atleta and IdDisciplina = @iddisciplina


--insert into storico con nuova fase
insert into RankingStorico 
select IdAtleta, Punteggio, GETDATE(), GETDATE(), Posizionamento, IdDisciplina, 2023, @newPhase from ranking

insert

--vanno azzerati, se sto facendo l'update del reanking per gli assoluti, tutti quelli ceh non sono stati aggiornati
--update RankingStorico set Punteggio = 0, LastUdate = GETDATE(), Posizionamento = 999 where LastUdate < convert(datetime, 11/05/2023 ) and Anno = 2023 and Fase = 10

