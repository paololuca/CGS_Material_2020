select Nome + ' ' + Cognome as NomeCompleto, a.Id as IdAtleta, Cognome, Nome, IdASD, Nome_ASD 
from Atleti a join ASD as asd on a.IdASD = ASD.Id