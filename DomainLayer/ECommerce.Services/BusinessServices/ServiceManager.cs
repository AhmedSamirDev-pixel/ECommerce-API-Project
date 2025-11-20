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
    // Service Manager to manage all services
    public class ServiceManager: IServiceManager
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Lazy<IProductServices> lazyProductServices;
        public ServiceManager(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            // Initialize lazy ProductServices
            lazyProductServices = new Lazy<IProductServices>(
                 () => new ProductServices(_unitOfWork, _mapper)
            );
        }
        // Expose ProductServices via IServiceManager
        public IProductServices ProductServices => lazyProductServices.Value;
    }
}
