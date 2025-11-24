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
        private readonly Lazy<IProductServices> lazyProductServices;
        private readonly Lazy<IBasketService> lazyBasketServices;
        private readonly Lazy<IAuthenticationServices> lazyAuthenticationServices;

        public ServiceManager(IMapper mapper, IUnitOfWork unitOfWork, IBasketRepository basketRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _userManager = userManager;
            _configuration = configuration;

            // Initialize lazy ProductServices
            lazyProductServices = new Lazy<IProductServices>(
                () => new ProductServices(_unitOfWork, _mapper)
            );

            // Initialize lazy BasketServices using _basketRepository
            lazyBasketServices = new Lazy<IBasketService>(
                () => new BasketServices(_basketRepository, _mapper)
            );

            lazyAuthenticationServices = new Lazy<IAuthenticationServices>(
                () => new AuthenticationServices(_userManager, _configuration, _mapper)
            );
        }

        // Expose ProductServices via IServiceManager
        public IProductServices ProductServices => lazyProductServices.Value;

        // Expose BasketServices via IServiceManager
        public IBasketService BasketServices => lazyBasketServices.Value;

        // Expose AuthenticationServices via IServiceManager
        public IAuthenticationServices AuthenticationServices => lazyAuthenticationServices.Value; 
    }
}
