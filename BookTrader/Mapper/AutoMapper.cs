using AutoMapper;
using BookTrader.Models;
using BookTrader.DTOs; 

namespace BookTrader.Mapper
{
    public class AutoMapperProfile :  Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<InsertCategoriaDTO, Categorias>(); // Mapeo automático

        }


    }
}
