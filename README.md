# Universe.Net.CueSheet


## Description
This is a project is working with CUE sheets and using in any audioplayers.


# Version control    
For convenience, the project should be cloned to the local path C:\P\Universe.Lastfm.API\ (but this is not at all necessary)
* master is the main working branch, changes are made to it only through MergeRequests
* develop - the second main working branch, changes are also included in it through MergeRequests

A separate branch is created for each development/bug fix task.

*It is created directly from the task, it is necessary to include errors in the name of the task; before creating it, indicate in parentheses in English the not very long name of the branch.*

## Rules for generating messages to a commit
The message also can be multi-line, for example:
#89 [WebApp Auth]: Added authorization in the web application.
#89 [Common]: Changed links to projects.
#89 [Universe.Algorithm, Tests]: Removed artifact. 

where:
* #89 - issue number (usually the same as the branch number) - in gitlab it will turn into a link to the issue,
   and when you hover the mouse it will show the name of the task
* "[WebApp Auth]:"" name of the functionality within which the commit is made. Must be indicated with a colon at the end.
* "Authorization has been added to the web application." - text describing what was done. Be sure to have a period at the end to complete the sentence.
* Several actions can be listed that are recorded in this manner
   e.g. "#89 WebApp Auth: The Unity container is connected in the WebApp project. The Unity container is connected in the Core project.""

* [\~] - we indicate it at the beginning of the commit line if we are merging files manually (git automatically generates a message). The message should look like:
     [\~] Merge from develop to 20-build-ef-data-access-layer
     or
     [\~] Merge from develop to #20

# Development tools
For a development, you should use VS 2017, VS2019, VS2022; also must additionally be installed:
* support for PowerShell projects (this is installed when installing VS)
* git integrated into studio (this is installed when installing VS)
* If, after updating from the develop branch, the projects' References are gone, and the error shows "NuGet", you can do the following:
    on Solution, right-click and select and select Restore NuGet Packages. Then Clean solution, build solution.
* To have NuGet restore assemblies automatically, you need to do the following:
    in the Tools - Options - NuGet Package Manager menu, select 2 checkboxes:
    - Allow NuGet to download missing packages;
    - Automaticall check for missing packages during build in Visual Studio.

# Note
* File encoding (especially *.ps1) must be UTF-8

# CodingStyle
* When formatting the code, follow the resharper settings in the ReSharper.DotSettings file
* !Before committing, be sure to reformat the changed code according to the ResharperSln scheme (exceptions are codogen and code
  generated by T4 template)
* When reformating, select the ResharperSln profile for a full reformat, ResharperSln NoSort for classes in which the order cannot be changed
* Catch block, if there is no `throw ...;` in the catch block, then you need to indicate a comment why it is not there, for example
  as in the example below
  If a new error instance is created in the catch block, then it is necessary to indicate the original error or comment
  why the original error should not be stated.
```c#
catch (Exception ex) {
    _log.Unexpected(ex);
    //throw; Whatever happens here should not affect the execution of everything else
}
````