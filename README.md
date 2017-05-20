# character-sheet-importer
An Android application for importing and dynamically displaying characters created by Hero Lab and PCGen

The 'master' branch is to represent the production ready code - hence it's very empty.  Pull 'develop' and create feature branches against it, which can then be merged into the develop branch via pull request.

The application is written in C# using the Xamarin.Android framework, the solution should be opened in Visual Studio 2017 Community Edition or better

When building you will get the following warning (among others):

"Severity	Code	Description	Project	File	Line	Suppression State
Warning		The $(TargetFrameworkVersion) for CharacterSheet.UI.dll (v6.0) is greater than the $(TargetFrameworkVersion) for your project (v5.1). You need to increase the $(TargetFrameworkVersion) for your project."

The reason the project targeting v5.1 is because permissions for things such as storage/network access are asked for upon install.  If the target framework is updated, then prompts to grant permission for features as and when they are first used will need to be implemented.
