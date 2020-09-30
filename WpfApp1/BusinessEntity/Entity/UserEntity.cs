using System;

namespace BusinessEntity.Entity
{
    public class UserEntity
    {
        public int Id { get; set; }
        public String Satrapia { get; set; }
        public String Cognome { get; set; }
        public String Nome { get; set; }
        public int Ranking { get; set; }
        public UserEntity(String satrapia, String cognome, String nome)
        {
            Satrapia = satrapia;
            Cognome = cognome;
            Nome = nome;
            Ranking = 0;        //default value
        }
        public UserEntity()
        { }
    }
}
