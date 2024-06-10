using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class ExaminationService : IExaminationService
    {
        private readonly IExaminationService examinationService;
        public ExaminationService(IExaminationService examinationService) 
        { 
            this.examinationService = examinationService;
        }



    }
}
