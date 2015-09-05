@echo on
set destination=%1AssemblyInfoVersion.cs
 
echo copy version info:
git --git-dir %1/.git log --date=iso --pretty=tformat:"hash: %%H%%nauthor date: %%ad" -1 

echo using System.Reflection; > %destination%
echo.  >> %destination%
git --git-dir %1/.git log --date=iso --pretty=tformat:"[assembly: AssemblyGitRevisionAttribute(%%x22%%H%%x22)] %%n[assembly: AssemblyGitCommitAuthorDateAttribute(%%x22%%ad%%x22)] %%n[assembly: AssemblyInformationalVersionAttribute(%%x22#%%h from %%ad%%x22)]" -1 >> %destination%

echo Generated version file:
echo --------------------
type %destination%
echo --------------------

if exist %destination% goto END

echo %destination% not generated.
echo generating empty stub...

( echo using System.Reflection; & echo. & echo [assembly: AssemblyGitRevisionAttribute^("???"^)] & echo. & echo [assembly: AssemblyGitCommitAuthorDateAttribute^("???"^)] & echo. & echo [assembly: AssemblyInformationalVersionAttribute^("???"^)]) > %destination%
echo Generated version file:
echo --------------------
type %destination%
echo --------------------

:END
echo Done.