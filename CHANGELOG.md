[![Error Control System.png](https://raw.githubusercontent.com/Behzadkhosravifar/ErrorControlSystem/master/Images/Error%20Control%20System.png)](https://github.com/Behzadkhosravifar/ErrorControlSystem)

# Version history


## [v4.2.1](https://github.com/Behzadkhosravifar/ErrorControlSystem/archive/v4.2.1.zip)

Added

* Bug fixes
* StoredProcedure for catching Sql Errors created (sp_CatchError issue #54)



## [v4.1.5](https://github.com/Behzadkhosravifar/ErrorControlSystem/archive/v4.1.5.zip)

Added

* Some Bug fixes
* Developer UI designed by WPF user controls to show full info message
* Analyze StackTrace for convert to CodeMap #33
* Support Multiple Languages (English, Persian, German, Chinese, Hebrew, French, Azerbaijan, Turkish, Russian, Arabic) #52



## [v4.1.3](https://github.com/Behzadkhosravifar/ErrorControlSystem/archive/v4.1.3.zip)

Added

* Bug fixes
* Add Server Instance Username and Password Text boxes to Analyzer #40
* Solve Analyzer Throw an Exception to Can Not Found Database #38 issue
* Developer UI designed by WPF user controls to show full info message
* Analyze StackTrace for convert to CodeMap #33



## [v4.1.2](https://github.com/Behzadkhosravifar/ErrorControlSystem/archive/v4.1.2.zip)

Added

* Bug fixes
* Improved stability
* Add Server and Database chosen combo box to ErrorLogAnalyzer
* Add WaitSplash feature to ECS Cache Analyzer for known background workers
* Never do not Raise Self Exceptions by new Algorithm
* Cache Size Checked Only When an Error Inserted
* Exempted Exception from Some Code Scopes
* Just Raise Error Log in Special Code Scopes
* Replace Exception.StackTrace data by Call Method StackTrace for store completely data
* Convert Exception.StackTrace to customized format for use that in Analyzer easily
* Add Many Options to System
* Change and speedup ~4.3 SDF cache file update model
* Add an EventHandler to ErrorControlSystem for Alert any UnHandled Exception by ExceptionHandler.OnShowUnhandledError
* LineColumn text divided to Line and Column integer number in databases
* ErrorControlSystem WPF version combined by WinForms version
* Add Dependency Assemblies to Solve ClickOnce Problem



## [v3.0.2](https://github.com/Behzadkhosravifar/ErrorControlSystem/archive/v3.0.zip)

Added

* Bug fixes
* Improved stability
* Personalized Exception Handler
* Filter Exception Raiser and Snapshot Capturing
* Set Error Handler Options from Database
* Send Cache Data to Server at Periodic Times
* Capture Multi-Screen Snapshots
* Auto Create and Update Tables and StoredProcedures on Database
* Release ErrorControlSystem WPF version


## [v2.0.0](https://github.com/Behzadkhosravifar/ErrorControlSystem/archive/v2.0.zip)

Added

* Change Design Pattern
* Separate any layers
* Convert every part of layers to one different class library .dll
* Implement Adapter Pattern to Exception Handler layer's
* Implement Singleton Pattern to Kernel class
* Find another design patterns for this project
* Set all project within a design patterns
* Add Analyzer for read cache files



## [v1.0.2](https://github.com/Behzadkhosravifar/ErrorControlSystem/archive/v1.0.zip)

Added

* Fix some bugs
* Add some UnitTests
* 20% Performance boost



Have fun!

Behzad Khosravifar
