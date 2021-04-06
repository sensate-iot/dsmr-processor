# Sensate IoT - Smart Energy

The Sensate IoT Smart Energy project implements an IoT solution for (Dutch)
Smart Meters. The project consists of several repository's:

- DSMR parser ;
- DSMR web client (implementator of this service);
- DSMR processor (this repo).

## DSMR Processor

The DSMR processing service receives data from Sensate IoT using the Data API. This
data is processed into time buckets, to allow customer facing applications to use
smaller data sets.
