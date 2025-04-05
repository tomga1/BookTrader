using AutoMapper;
using BookTrader.Models;
using BookTrader.DTOs;

namespace BookTrader.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<InsertCategoriaDTO, Categorias>(); // Mapeo automático
            CreateMap<InsertSugerenciaDTO, Sugerencias>(); // Mapeo automático

            CreateMap<InsertLibroDTO, Libros>()
            .ForMember(dest => dest.Categoria, opt => opt.Ignore()) // Se obtiene de la BD
            .ForMember(dest => dest.Condicion, opt => opt.Ignore()) // Se obtiene de la BD
            .ForMember(dest => dest.Idioma, opt => opt.Ignore()) // Se obtiene de la BD
            .ForMember(dest => dest.ImagenPath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ImagenUrl) ? null : src.ImagenUrl))
            .ForMember(dest => dest.EstadoPublicacion, opt => opt.Ignore());

        }


    }
}
