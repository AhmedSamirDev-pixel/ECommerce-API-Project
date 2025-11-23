using AutoMapper;
using ECommerce.Domain.Contracts.BasketRepo;
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
    public class ServiceManager : IServiceManager
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Lazy<IProductServices> lazyProductServices;
        private readonly IBasketRepository _basketRepository;
        private readonly Lazy<IBasketService> lazyBasketServices;

        public ServiceManager(IMapper mapper, IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;

            // Initialize lazy ProductServices
            lazyProductServices = new Lazy<IProductServices>(
                () => new ProductServices(_unitOfWork, _mapper)
            );

            // Initialize lazy BasketServices using _basketRepository
            lazyBasketServices = new Lazy<IBasketService>(
                () => new BasketServices(_basketRepository, _mapper)
            );
        }

        // Expose ProductServices via IServiceManager
        public IProductServices ProductServices => lazyProductServices.Value;

        // Expose BasketServices via IServiceManager
        public IBasketService BasketServices => lazyBasketServices.Value; 
    }
}
