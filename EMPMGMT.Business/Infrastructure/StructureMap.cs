using System;
using StructureMap;
using StructureMap.Configuration.DSL;
using EMPMGMT.Business;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;

namespace EMPMGMT.Business.Infrastructure
{
    public class BusinessRegistry : Registry
    {
        public BusinessRegistry()
        {
            For<IUnitOfWork>().Use<UnitOfWork>();
        }
    }
}
