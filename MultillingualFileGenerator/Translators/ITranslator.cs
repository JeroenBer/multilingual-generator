using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Translators;
public interface ITranslator
{
    Task<string> Translate(string text);
}
