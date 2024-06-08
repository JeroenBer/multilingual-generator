using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Xliff;

public static class TranslationStates
{
    public const string New = "new";
    public const string NeedsReview = "needs-review-translation";
    public const string Translated = "translated";
    public const string Final = "final";
    public const string SignedOff = "signed-off";
}
