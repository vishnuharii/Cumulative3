using System;
using System.Reflection;

namespace HTTP5125_Cumulative_Project_Part_3.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}