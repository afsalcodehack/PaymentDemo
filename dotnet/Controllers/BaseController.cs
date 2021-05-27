using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderInformationControllerBase" /> class.
        /// </summary>
        /// <param name="logger">Reference to ILogger.</param>
        public BaseController(ILogger<BaseController> logger) => Logger = logger;

        /// <summary>
        ///     Gets Logger member.
        /// </summary>
        protected ILogger<BaseController> Logger { get; }
    }
}
