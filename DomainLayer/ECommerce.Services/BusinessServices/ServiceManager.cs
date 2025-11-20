using AutoMapper;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.ServicesAbstraction.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.BusinessServices
{
    public class ServiceManager(IMapper _mapper, IUnitOfWork _unitofwork) : IServiceManager
    {
        public readonly Lazy<IProductServices> lazyProductServices = 
           new Lazy<IProductServices>(() => new ProductServices(mapper: _mapper, unitOfWork: _unitofwork));
        public IProductServices ProductServices => lazyProductServices.Value;
    }
}
