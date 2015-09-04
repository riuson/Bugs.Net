@echo on
set destination=%1AssemblyInfoVersion.cs
 
echo copy version info:
git --git-dir %1/.git log --date=iso --pretty=tformat:"hash: %%H%%nauthor date: %%ad" -1 

echo using System.Reflection; > %destination%
echo.  >> %destination%
git --git-dir %1/.git log --date=iso --pretty=tformat:"[assembly: AssemblyGitRevisionAttribute(%%x22%%H%%x22)]" -1 >> %destination%
git --git-dir %1/.git log --date=iso --pretty=tformat:"[assembly: AssemblyGitCommitAuthorDateAttribute(%%x22%%ad%%x22)]" -1 >> %destination%
git --git-dir %1/.git log --date=iso --pretty=tformat:"[assembly: AssemblyInformationalVersionAttribute(%%x22#%%h from %%ad%%x22)]" -1 >> %destination%

if exist %destination% goto END

echo %destination% not generated.
echo generating empty stub...
echo "using System.Reflection;%%n%%n[assembly: AssemblyGitRevisionAttribute(\"???\")]%%n[assembly: AssemblyGitCommitAuthorDateAttribute(\"???\")]" > %destination%

:END
echo Done.