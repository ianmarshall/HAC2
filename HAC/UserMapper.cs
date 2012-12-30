namespace HAC
{
    using System;
    using AutoMapper;
    using HAC.Domain;
    using HAC.Models;

    public class UserMapper 
    {
       static UserMapper()
        {
            Mapper.CreateMap<Member, ViewModelMember>();
            Mapper.CreateMap<ViewModelMember, Member>();
           // Mapper.AssertConfigurationIsValid();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}