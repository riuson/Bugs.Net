using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany("riuson")]
[assembly: AssemblyProduct("Bugs.Net")]
[assembly: AssemblyCopyright("© 2015 riuson")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyFileVersion("1.0.*")]

[AttributeUsage(AttributeTargets.Assembly)]
internal class AssemblyGitRevisionAttribute : Attribute
{
    public string RevisionHash { get; private set; }

    public AssemblyGitRevisionAttribute() : this(string.Empty) { }
    public AssemblyGitRevisionAttribute(string value) { this.RevisionHash = value; }
}

[AttributeUsage(AttributeTargets.Assembly)]
internal class AssemblyGitCommitAuthorDateAttribute : Attribute
{
    public DateTime CommitAuthorDate { get; private set; }

    public AssemblyGitCommitAuthorDateAttribute() : this(string.Empty) { }
    public AssemblyGitCommitAuthorDateAttribute(string value) { this.CommitAuthorDate = DateTime.Parse(value); }
}
