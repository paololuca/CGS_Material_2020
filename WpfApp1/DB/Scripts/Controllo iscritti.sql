declare @IdTorneo int;
set @IdTorneo = 4;

select t.NomeTorneo,d.Nome, td.Categoria, Fighters from 
Torneo t 
join TorneoVsDiscipline td on t.Id = td.IdTorneo
join Discipline d on td.IdDisciplina = d.Id
join 
(select atd.IdTorneoVsDiscipline, count(*) as Fighters from AtletiVsTorneoVsDiscipline atd 
join TorneoVsDiscipline td2 on atd.IdTorneoVsDiscipline = td2.Id 

group by atd.IdTorneoVsDiscipline) c on c.IdTorneoVsDiscipline = td.Id
where t.id > @IdTorneo
order by t.Id, d.Id

