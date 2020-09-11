using AutoMapper;

namespace StandardApi.Framework.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            RegisterTaskSuiteMapping();
        }

        private void RegisterTaskSuiteMapping()
        {
            //CreateMap<HrmTaskSuite, TaskSuiteViewModel>()
            //    .ForMember(dest => dest.NoOfTasks,
            //        opt => opt.MapFrom(src => src.HrmDefinitionTask.Count(dt => dt.Enabled != false)))
            //    .ForMember(dest => dest.TaskSuiteType,
            //        opt => opt.MapFrom(src => src.TaskSuiteType.Name))
            //    .ForMember(dest => dest.TaskSuiteTypeId,
            //        opt => opt.MapFrom(src => src.TaskSuiteType.Id));
            //CreateMap<HrmTaskSuite, DefaultTaskSuiteViewModel>()
            //    .ForMember(dest => dest.Tasks,
            //        opt => opt.MapFrom(src => src.HrmDefinitionTask.Where(dt => dt.Enabled != false)));

            //CreateMap<CreateTaskSuiteViewModel, HrmTaskSuite>();
            //CreateMap<UpdateTaskSuiteViewModel, HrmTaskSuite>();
        }
    }
}
