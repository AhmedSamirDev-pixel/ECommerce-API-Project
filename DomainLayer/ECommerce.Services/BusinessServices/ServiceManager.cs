using AutoMapper;
using ECommerce.Domain.Contracts.BasketRepo;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.Domain.Models.Identity;
using ECommerce.ServicesAbstraction.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        // Lazy loading ensures that each service is created only when first accessed
        private readonly Lazy<IProductServices> lazyProductServices;
        private readonly Lazy<IBasketService> lazyBasketServices;
        private readonly Lazy<IAuthenticationServices> lazyAuthenticationServices;
        private readonly Lazy<IOrderServices> lazyOrderServices;
        private readonly Lazy<IPaymentServices> lazyPaymentServices;

        public ServiceManager(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IBasketRepository basketRepository,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            // Save injected dependencies
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _userManager = userManager;
            _configuration = configuration;

            // Lazy initialization of ProductServices
            // Created only when ProductServices is accessed
            lazyProductServices = new Lazy<IProductServices>(
                () => new ProductServices(_unitOfWork, _mapper)
            );

            // Lazy initialization of BasketServices
            lazyBasketServices = new Lazy<IBasketService>(
                () => new BasketServices(_basketRepository, _mapper)
            );

            // Lazy initialization of AuthenticationServices
            lazyAuthenticationServices = new Lazy<IAuthenticationServices>(
                () => new AuthenticationServices(_userManager, _configuration, _mapper)
            );

            // Lazy initialization of OrderServices
            lazyOrderServices = new Lazy<IOrderServices>(
                () => new OrderServices(_mapper, basketRepository, _unitOfWork)
            );

            // Lazy initialization of PaymentServices
            lazyPaymentServices = new Lazy<IPaymentServices>(
                () => new PaymentServices(_configuration, _basketRepository, _unitOfWork, _mapper)
            );
        }

        // Expose ProductServices instance via IServiceManager
        public IProductServices ProductServices => lazyProductServices.Value;

        // Expose BasketServices instance via IServiceManager
        public IBasketService BasketServices => lazyBasketServices.Value;

        // Expose AuthenticationServices instance
        public IAuthenticationServices AuthenticationServices => lazyAuthenticationServices.Value;

        // Expose OrderServices instance
        public IOrderServices OrderServices => lazyOrderServices.Value;

        // Expose PaymentServices instance
        public IPaymentServices PaymentServices => lazyPaymentServices.Value;
    }

}
