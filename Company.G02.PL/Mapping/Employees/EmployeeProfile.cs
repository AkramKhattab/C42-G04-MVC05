using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.viewModels.Employees;

namespace Company.G02.PL.Mapping.Employees
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            //CreateMap<EmployeeViewModel, Employee>();
        }
    }
}
