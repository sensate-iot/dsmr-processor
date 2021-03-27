# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 26-03-2021
### Added
- Improve error handling in the weather service

### Updated
- Weather service, pass cancellation tokens to API clients
- Energy average computation

## [1.0.0] - 26-03-2021
### Added
- Support for Windows services
- Improved error handling

### Updated
- Logging in error situations
- Various program sections based on code analysis feedback

### Removed
- The DSMR database project (migrated to its own solution)

## [0.0.1] - 24-03-2021
### Added
- Test data client
- Test clock
- API client for the Sensate IoT data client
- Measurement processor to group messages (averages) by the minute

### Updated
- The data models
- Various stored procedures
