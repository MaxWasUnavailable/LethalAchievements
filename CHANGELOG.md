# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

### Fixed

### Changed

### Removed

## [Unreleased] -- TODO

### Added
- Method to get PluginInfo of an achievement
- Method to get dictionary of PluginInfo of all achievements
- Cache PluginInfo of achievements to avoid repeated lookups

### Fixed

### Changed
- Throw if plugins aren't loaded yet when trying to get methods that require them

### Removed

## [1.0.0] - 29/03/2024

### Added

- IAchievement interface
- BaseAchievement abstract class that implements IAchievement
- AchievementManager class that manages achievement registration, initialisation, uninitalisation, and popups
- AchievementRegistry class that stores all achievements, and is used by the Achievement Manager
- AchievementPopupStyle enum that defines the style of the popup, and is used in the config file
- A number of helpers related to the popups and achievements
