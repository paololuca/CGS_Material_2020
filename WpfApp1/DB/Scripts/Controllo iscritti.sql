﻿select t.NomeTorneo,d.Nome, td.Categoria, fighter from 
Torneo t 
join TorneoVsDiscipline td on t.Id = td.IdTorneo
join Discipline d on td.IdDisciplina = d.Id
join 
(select atd.IdTorneoVsDiscipline, count(*) as fighter from AtletiVsTorneoVsDiscipline atd 
join TorneoVsDiscipline td2 on atd.IdTorneoVsDiscipline = td2.Id 

group by atd.IdTorneoVsDiscipline) c on c.IdTorneoVsDiscipline = td.Id
