[![Error Control System.png](https://raw.githubusercontent.com/Behzadkhosravifar/ErrorControlSystem/master/Images/Error%20Control%20System.png)](https://github.com/Behzadkhosravifar/ErrorControlSystem)

# Version history

## 2.0.169

Added

* Added automatic unit test rewriting
* Added configuration of how line-endings are generated during state printing. This is to mitigate problems due to different operating systems uses different line-endings.
* Added assertion helper methods `Stateprinter.Assert.AreEqual`, `Stateprinter.Assert.IsSame`, `Stateprinter.Assert.PrintIsSame` and `Stateprinter.Assert.That`.  Improves the unit test experience by printing a suggested expected string as C# code.
* Added a `AllFieldsAndPropertiesHarvester` which is able to harvest properties and fields.
* `StringConverter` is now configurable with respect to quote character.
* BREAKING CHANGE: Projective harvester is now using the `AllFieldsAndPropertiesHarvester` rather instead of the `FieldHarvester`. This means both fields and properties are now harvested.


## [v3.0](https://github.com/Behzadkhosravifar/ErrorControlSystem/archive/v3.0.zip)

Added

* Executing stylecop on the build server.
* Made the `Configuration` class API a bit more fluent
* BUGFIX: Harvesting of types were cached across `Stateprinter` instances, which no longer makes sense since harvesting is configurable from instance to instance.
* BUGFIX: Changed how `ToString()` methods are harvested. Thanks to "Sjdirect".


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
