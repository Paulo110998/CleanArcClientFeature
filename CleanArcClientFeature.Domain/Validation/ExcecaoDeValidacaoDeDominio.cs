using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArcClientFeature.Domain.Validation;

public class ExcecaoDeValidacaoDeDominio : Exception
{
    public ExcecaoDeValidacaoDeDominio(string error) : base(error)
    {
        
    }

    public static void ExcecaoDeErro(bool hasError, string error)
    {
        if(hasError)
        {
            throw new ExcecaoDeValidacaoDeDominio(error);
        }
    }
}
