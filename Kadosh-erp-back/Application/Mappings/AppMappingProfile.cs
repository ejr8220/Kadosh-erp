using Application.Dtos.Request.General;
using Application.Dtos.Request.Security;
using Application.Dtos.Request.Tax;
using Application.Dtos.Response.General;
using Application.Dtos.Response.Security;
using Application.Dtos.Response.Tax;
using AutoMapper;
using Domain.Common.Enums;
using Domain.Entities.General;
using Domain.Entities.Security;
using Domain.Entities.Tax;

namespace Application.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CountryRequestDto, Country>();
            CreateMap<Country, CountryResponseDto>();

            CreateMap<ProvinceRequestDto, Province>();
            CreateMap<Province, ProvinceResponseDto>();

            CreateMap<CityRequestDto, City>();
            CreateMap<City, CityResponseDto>();

            CreateMap<ParishRequestDto, Parish>();
            CreateMap<Parish, ParishResponseDto>();

            CreateMap<ZoneRequestDto, Zone>();
            CreateMap<Zone, ZoneResponseDto>();

            CreateMap<BranchRequestDto, Branch>();
            CreateMap<Branch, BranchResponseDto>();

            CreateMap<PersonRequestDto, Person>();
            CreateMap<Person, PersonResponseDto>();

            CreateMap<CompanyRequestDto, Company>()
                .ForMember(dest => dest.CompanyTypeId, opt => opt.MapFrom(src => src.CompanyTypeId ?? 1));
            CreateMap<Company, CompanyResponseDto>()
                .ForMember(dest => dest.IdentificationTypeId, opt => opt.MapFrom(src => src.Person.IdentificationTypeId))
                .ForMember(dest => dest.CompanyTypeId, opt => opt.MapFrom(src => src.CompanyTypeId))
                .ForMember(dest => dest.CompanyTypeName, opt => opt.MapFrom(src => src.CompanyType != null ? src.CompanyType.Name : string.Empty))
                .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Person.IdentificationNumber))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.Person.LastName))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => (int?)src.Person.CountryId))
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Person.Country != null ? src.Person.Country.Name : string.Empty))
                .ForMember(dest => dest.ProvinceId, opt => opt.MapFrom(src => (int?)src.Person.ProvinceId))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Person.Province != null ? src.Person.Province.Name : string.Empty))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => (int?)src.Person.CityId))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Person.City != null ? src.Person.City.Name : string.Empty))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Person.Address))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Person.ContactFormPersons
                    .Where(cfp => cfp.ContextType == ContactContextType.Company && cfp.ContactForm.Name == "Telefono")
                    .OrderByDescending(cfp => cfp.IsPrimary)
                    .Select(cfp => cfp.Value)
                    .FirstOrDefault() ?? string.Empty))
                .ForMember(dest => dest.ContactForms, opt => opt.MapFrom(src => src.Person.ContactFormPersons
                    .Where(cfp => cfp.ContextType == ContactContextType.Company)
                    .OrderByDescending(cfp => cfp.IsPrimary)
                    .ThenBy(cfp => cfp.ContactForm.Name)
                    .Select(cfp => new CompanyContactFormResponseDto
                    {
                        Name = cfp.ContactForm.Name,
                        Value = cfp.Value,
                        IsPrimary = cfp.IsPrimary
                    })))
                .ForMember(dest => dest.PersonIdentificationNumber, opt => opt.MapFrom(src => src.Person.IdentificationNumber))
                .ForMember(dest => dest.PersonFirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.PersonLastName, opt => opt.MapFrom(src => src.Person.LastName))
                .ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
                .ForMember(dest => dest.LegalpresentativeIdentification, opt => opt.MapFrom(src => src.LegalpresentativeIdentification))
                .ForMember(dest => dest.LegalRepresentativeName, opt => opt.MapFrom(src => src.LegalRepresentative))
                .ForMember(dest => dest.AccountantIdentification, opt => opt.MapFrom(src => src.AccountantIdentification))
                .ForMember(dest => dest.AccountantName, opt => opt.MapFrom(src => src.Accountant));

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            CreateMap<PermissionRequestDto, Permission>();
            CreateMap<Permission, PermissionResponseDto>();

            CreateMap<UserRoleRequestDto, UserRole>();
            CreateMap<UserRole, UserRoleResponseDto>();

            CreateMap<CompanyUserRequestDto, CompanyUser>();
            CreateMap<CompanyUser, CompanyUserResponseDto>();

            CreateMap<RolePermissionRequestDto, RolePermission>();
            CreateMap<RolePermission, RolePermissionResponseDto>();

            CreateMap<UserRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Companies, opt => opt.Ignore());

            CreateMap<IdentificationTypeRequestDto, IdentificationType>();
            CreateMap<IdentificationType, IdentificationTypeResponseDto>();

            CreateMap<CompanyTypeRequestDto, CompanyType>();
            CreateMap<CompanyType, CompanyTypeResponseDto>();

            CreateMap<ParameterDetail, ParameterDetailResponseDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value1));
            CreateMap<ParameterHeader, ParameterHeaderResponseDto>();
        }
    }
}
