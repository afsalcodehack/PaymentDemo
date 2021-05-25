using Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.IRepositories;
using Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripePaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeSampleController : ControllerBase
    {
        private readonly ILogger<StripeSampleController> _logger;
        private readonly ISampleRepository _sampleRepository;
        public StripeSampleController(ILogger<StripeSampleController> logger, ISampleRepository sampleRepository)
        {
            _logger = logger;
            _sampleRepository = sampleRepository;
        }

        [HttpGet]
        public Result<List<Sample>> Get()
        {
            return _sampleRepository.Get();
        }
    }
}
