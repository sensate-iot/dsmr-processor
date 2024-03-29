# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.3.4] - 25-07-2021
- Update Data API response type

## [1.3.3] - 07-06-2021
### Updated 
- Weather processing
- Logging; add problem suggestion logs to warning logs

## [1.3.2] - 07-06-2021
### Added
- System.Runtime dependency
- Improved logging in the weather client

### Updated
- Project dependencies
- Platform queueing

## [1.3.1] - 17-04-2021
### Added
- Additional logging

### Updated
- Fix average computation of power consumption
- Data calculator

## [1.3.0] - 09-04-2021
### Added
- Product database integration
- Sensor mapping model/dto

### Updated
- Sensor mapping data layer

## [1.2.0] - 06-04-2021
### Added
- Implement data platform data deletion
- Implement data deletion in the test client

### Updated
- The processing flow: every processed hour is deleted from the data platform

## [1.1.0] - 01-04-2021
### Added
- Support for the tariff column

### Updated
- Stored procedures execution statements
- Data point model files

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
