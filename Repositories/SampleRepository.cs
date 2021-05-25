using Entity.Entities;
using Repositories.IRepositories;
using Result;
using Result.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
   public class SampleRepository : ISampleRepository
    {
        stripeContext _stripeContext;
        public SampleRepository(stripeContext stripeContext)
        {
            _stripeContext = stripeContext;
        }

        public Result<List<Sample>> Get()
        {
            List<Sample> samples = new List<Sample>();
            try
            {
                var getSamples = _stripeContext.Samples.ToList();
                samples = getSamples;
                return Result<List<Sample>>.Success(samples);
            }
            catch (Exception ex)
            {
                return Result<List<Sample>>.Failure(FailureError.Service(("Service Failed")));
            }
        }
    }
}
